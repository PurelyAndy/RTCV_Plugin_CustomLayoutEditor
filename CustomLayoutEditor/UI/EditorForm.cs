using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RTCV.Common;
using RTCV.CorruptCore;
using RTCV.UI;
using RTCV.UI.Modular;

namespace CustomLayoutEditor.UI;

public partial class EditorForm : ColorizedForm
{
    internal BidirectionalDictionary<ResizablePanel, ComponentForm> AttachedForms = [];
    private string _filePath;
    private Panel _root;

    private class TileInfo
    {
        public string TypeName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public AnchorStyles Anchor { get; set; }
        public string OriginalLine { get; set; }
    }
    
    public EditorForm()
    {
        _root = new()
        {
            Dock = DockStyle.Fill
        };
        _root.AllowDrop = true;
        _root.DragEnter += RootDragEnter;
        _root.DragDrop += RootDragDrop;
        
        InitializeComponent();
        pnGridContainer.Padding = new(16 - ResizablePanel.BorderSize);
        
        nmMinimumWidth_ValueChanged(null, null);
        nmMinimumHeight_ValueChanged(null, null);
        
        OnResizeEnd(null);
        
        flpComponents.Controls.Add(new MiniComponent()); // MemoryTools (UICore.mtForm)
        foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()))
        {
            if (type.IsSubclassOf(typeof(ComponentForm)))
            {
                try
                {
                    MiniComponent component = new(type);
                    flpComponents.Controls.Add(component);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        
        ResetGrid();
        pnGridContainer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        pnGridContainer.Dock = DockStyle.Fill;
    }

    private void RootDragDrop(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.StringFormat))
            return;
        
        ResizablePanel panel = new(_root);
        _root.Controls.Add(panel);
        panel.Location = _root.PointToClient(Cursor.Position);
        panel.Location = ResizablePanel.RoundToSize(panel.Location) + new Size(ResizablePanel.BorderSize, ResizablePanel.BorderSize);
        string typeName = (string)e.Data.GetData(DataFormats.StringFormat);
        ComponentForm droppedForm = null;
        if (typeName == "MemoryTools")
        {
            droppedForm = UICore.mtForm;
        }
        else
        {
            bool found = false;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(typeName);
                if (type != null && type.IsSubclassOf(typeof(ComponentForm)))
                {
                    droppedForm = (ComponentForm)S.GET(type);
                    found = true;
                    break;
                }
            }
            if (!found)
                return;
        }
        
        panel.ApplyWithTile(droppedForm);
        // y + 24 to account for the title bar height
        Size roundedMin = ResizablePanel.CeilToTileUnits(droppedForm.MinimumSize + new Size(0, 24));
        int oneUnit = ResizablePanel.UnitsToTileSize(1);
        int fourUnits = ResizablePanel.UnitsToTileSize(4);
        if (roundedMin.Width < oneUnit)
            roundedMin.Width = fourUnits;
        if (roundedMin.Height < oneUnit)
            roundedMin.Height = fourUnits;
        panel.Size = roundedMin;
    }

    private static void RootDragEnter(object sender, DragEventArgs e)
    {
         if (e.Data.GetDataPresent(DataFormats.StringFormat))
         {
             e.Effect = DragDropEffects.Copy;
         }
         else
         {
             e.Effect = DragDropEffects.None;
         }
    }

    private void ResetGrid()
    {
        foreach (var panel in AttachedForms.Keys.ToList())
        {
            panel.Delete();
        }

        if (AttachedForms.Count > 0)
        {
            MessageBox.Show("Something went wrong, and I think there are still attached forms. You may have to restart RTC to fix this.",
                "Custom Layout Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        if (pnGridContainer.Controls.Count > 0 && pnGridContainer.Controls[0] is ResizablePanel rootPanel)
        {
            foreach (Control control in rootPanel.Controls)
            {
                if (control is not ResizablePanel panel)
                    continue;
                panel.Delete();
            }
        }

        pnGridContainer.Controls.Clear();

        _root.Controls.Clear();
        pnGridContainer.Controls.Add(_root);
    }

    private void nmDefaultWidth_ValueChanged(object sender, EventArgs e)
    {
        int targetWidth = UnitsToSize((int)nmDefaultWidth.Value);
        
        int widthWithoutGrid = this.Size.Width - pnGridContainer.Size.Width;
        Size = new(targetWidth + widthWithoutGrid, this.Size.Height);
        
        pnGridContainer.Size = pnGridContainer.Size with { Width = targetWidth };
    }

    private void nmDefaultHeight_ValueChanged(object sender, EventArgs e)
    {
        int targetHeight = UnitsToSize((int)nmDefaultHeight.Value);
        
        int heightWithoutGrid = this.Size.Height - pnGridContainer.Size.Height;
        Size = new(this.Size.Width, targetHeight + heightWithoutGrid);
        
        pnGridContainer.Size = pnGridContainer.Size with { Height = targetHeight };
    }

    private void nmMinimumWidth_ValueChanged(object sender, EventArgs e)
    {
        int targetWidth = UnitsToSize((int)nmMinimumWidth.Value);
        
        int widthWithoutGrid = this.Size.Width - pnGridContainer.Size.Width;
        MinimumSize = new(targetWidth + widthWithoutGrid, this.MinimumSize.Height);
    }

    private void nmMinimumHeight_ValueChanged(object sender, EventArgs e)
    {
        int targetHeight = UnitsToSize((int)nmMinimumHeight.Value);
        
        int heightWithoutGrid = this.Size.Height - pnGridContainer.Size.Height;
        MinimumSize = new(this.MinimumSize.Width, targetHeight + heightWithoutGrid);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_filePath))
        {
            btnSaveAs_Click(sender, e);
            return;
        }
        
        SaveLayout(_filePath);
    }

    private void btnSaveAs_Click(object sender, EventArgs e)
    {
        SaveFileDialog fileDialog = new()
        {
            InitialDirectory = Path.Combine(RtcCore.RtcDir, "LAYOUTS"),
            FileName = tbName.Text + ".txt",
            Filter = "Custom Layouts (*.txt)|*.txt|All files (*.*)|*.*"
        };
        
        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            _filePath = fileDialog.FileName;
            SaveLayout(_filePath);
        }
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
        OpenFileDialog fileDialog = new()
        {
            InitialDirectory = Path.Combine(RtcCore.RtcDir, "LAYOUTS"),
            Filter = "Custom Layouts (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (fileDialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        _filePath = fileDialog.FileName;
        ResetGrid();
        tbName.Text = "Untitled";

        List<TileInfo> tiles = [];
        string layoutName = Path.GetFileNameWithoutExtension(_filePath);
        int gridWidth = 0, gridHeight = 0;
        int minWidth = 0, minHeight = 0;
        bool isResizable = false;
        bool loadToMain = false;

        string[] lines = File.ReadAllLines(_filePath);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                continue;
            
            int commentIndex = line.IndexOf("//", StringComparison.Ordinal);
            string uncommented = line;
            if (commentIndex >= 0)
                uncommented = line[..commentIndex].Trim();

            string[] parts = uncommented.Split(':');

            string command = parts[0].Trim();
            string value = null;

            if (parts.Length > 1)
                value = parts[1].Trim();

            switch (command)
            {
                case "GridName":
                    layoutName = value;
                    break;
                case "GridSize":
                    string[] sizeParts = value.Split(',');
                    if (sizeParts.Length == 2 && int.TryParse(sizeParts[0], out gridWidth) &&
                        int.TryParse(sizeParts[1], out gridHeight))
                    {
                        // Values will be set later
                    }
                    else
                    {
                        MessageBox.Show($"Invalid GridSize format: {uncommented}", "Layout Load Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    break;
                case "MinimumSize":
                    string[] minSizeParts = value.Split(',');
                    if (minSizeParts.Length == 2 && int.TryParse(minSizeParts[0], out minWidth) &&
                        int.TryParse(minSizeParts[1], out minHeight))
                    {
                        // Values will be set later
                    }
                    else
                    {
                        MessageBox.Show($"Invalid MinimumSize format: {uncommented}", "Layout Load Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    break;
                case "IsResizable":
                    isResizable = true;
                    break;
                case "LoadTo":
                    loadToMain = value == "Main";
                    break;
                case "SetTileForm":
                    string[] tileParts = value.Split(',');
                    if (tileParts.Length is 5 or 6 &&
                        int.TryParse(tileParts[1].Trim(), out int x) &&
                        int.TryParse(tileParts[2].Trim(), out int y) &&
                        int.TryParse(tileParts[3].Trim(), out int w) &&
                        int.TryParse(tileParts[4].Trim(), out int h))
                    {
                        TileInfo tile = new()
                        {
                            TypeName = tileParts[0].Trim(),
                            X = x,
                            Y = y,
                            Width = w,
                            Height = h,
                            OriginalLine = uncommented
                        };
                        if (tileParts.Length == 6 && int.TryParse(tileParts[5].Trim(), out int anchor))
                        {
                            tile.Anchor = (AnchorStyles)anchor;
                        }
                        tiles.Add(tile);
                    }
                    else
                    {
                        MessageBox.Show($"Invalid SetTileForm format (expected 5 or 6 comma-separated parts): {uncommented}",
                            "Layout Load Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    break;
                // Ignore CreateGrid, it's implicit
            }

            if (!lines.Any(l => l.StartsWith("CreateGrid")))
            {
                MessageBox.Show(
                    "Warning: CreateGrid command not found. This is necessary for some layout engines and will automatically be added upon saving.",
                    "Layout Load Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        tbName.Text = layoutName;
        cbResizable.Checked = isResizable;
        cbLoadToMain.Checked = loadToMain;

        nmDefaultWidth.ValueChanged -= nmDefaultWidth_ValueChanged;
        nmDefaultHeight.ValueChanged -= nmDefaultHeight_ValueChanged;
        nmMinimumWidth.ValueChanged -= nmMinimumWidth_ValueChanged;
        nmMinimumHeight.ValueChanged -= nmMinimumHeight_ValueChanged;

        nmDefaultWidth.Value = Math.Max(nmDefaultWidth.Minimum, gridWidth);
        nmDefaultHeight.Value = Math.Max(nmDefaultHeight.Minimum, gridHeight);
        nmMinimumWidth.Value = Math.Max(nmMinimumWidth.Minimum, minWidth);
        nmMinimumHeight.Value = Math.Max(nmMinimumHeight.Minimum, minHeight);

        UpdateFormSizeFromGridUnits((int)nmDefaultWidth.Value, (int)nmDefaultHeight.Value);

        nmDefaultWidth.ValueChanged += nmDefaultWidth_ValueChanged;
        nmDefaultHeight.ValueChanged += nmDefaultHeight_ValueChanged;
        nmMinimumWidth.ValueChanged += nmMinimumWidth_ValueChanged;
        nmMinimumHeight.ValueChanged += nmMinimumHeight_ValueChanged;

        ResetGrid();

        if (tiles.Any() && pnGridContainer.Controls.Count > 0 &&
            pnGridContainer.Controls[0] is Panel rootPanel)
        {
            ReconstructLayout(rootPanel, tiles);
        }
        else if (tiles.Any())
        {
            MessageBox.Show("Could not find the root panel to start layout reconstruction. Please re-open the custom layout editor.", "Layout Load Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void ReconstructLayout(Panel root, List<TileInfo> tiles)
    {
        foreach (TileInfo tile in tiles)
        {
            int x = ResizablePanel.UnitsToSize(tile.X);
            int y = ResizablePanel.UnitsToSize(tile.Y);
            int width = ResizablePanel.UnitsToTileSize(tile.Width);
            int height = ResizablePanel.UnitsToTileSize(tile.Height);
            
            ResizablePanel panel = new(root);
            root.Controls.Add(panel);
            panel.Location = new Point(x, y) + new Size(ResizablePanel.BorderSize, ResizablePanel.BorderSize);
            panel.Size = new(width, height);
            panel.Anchor = tile.Anchor;
            PlaceTile(panel, tile);
        }
    }
    
    private static void PlaceTile(ResizablePanel panel, TileInfo tileInfo)
    {
        ComponentForm form = null;
        Type formType;

        if (tileInfo.TypeName == "MemoryTools")
        {
            form = UICore.mtForm;
            formType = form?.GetType();
        }
        else
        {
            formType = AppDomain.CurrentDomain.GetAssemblies()
                              .SelectMany(a => a.GetTypes())
                              .FirstOrDefault(t => t.Name == tileInfo.TypeName && t.IsSubclassOf(typeof(ComponentForm)));

            if (formType != null)
            {
                form = (ComponentForm)S.GET(formType);
            }
        }


        if (form == null || formType == null)
        {
            throw new($"Could not find or instantiate ComponentForm '{tileInfo.TypeName}'.\nLine: {tileInfo.OriginalLine}");
        }

        panel.Anchor = tileInfo.Anchor;

        panel.ApplyWithTile(form);
    }

    private void UpdateFormSizeFromGridUnits(int widthUnits, int heightUnits)
    {
        int targetGridWidth = UnitsToSize(widthUnits);
        int targetGridHeight = UnitsToSize(heightUnits);

        int widthWithoutGrid = this.Width - pnGridContainer.Width;
        int heightWithoutGrid = this.Height - pnGridContainer.Height;

        int newFormWidth = Math.Max(this.MinimumSize.Width, targetGridWidth + widthWithoutGrid);
        int newFormHeight = Math.Max(this.MinimumSize.Height, targetGridHeight + heightWithoutGrid);

        this.Size = new(newFormWidth, newFormHeight);
    }

    private void SaveLayout(string path)
    {
        StringBuilder sb = new();
        sb.AppendLine($"GridName:{tbName.Text}");
        sb.AppendLine($"GridSize:{nmDefaultWidth.Value},{nmDefaultHeight.Value}");
        sb.AppendLine($"MinimumSize:{nmMinimumWidth.Value},{nmMinimumHeight.Value}");
        sb.AppendLine("CreateGrid");
        if (cbResizable.Checked)
            sb.AppendLine("IsResizable");
        
        foreach (ResizablePanel panel in _root.Controls)
        {
            int unitsX = (int)Math.Round(ResizablePanel.SizeToUnits(panel.Location.X - ResizablePanel.BorderSize));
            int unitsY = (int)Math.Round(ResizablePanel.SizeToUnits(panel.Location.Y - ResizablePanel.BorderSize));
            int widthUnits = (int)Math.Round(ResizablePanel.SizeToTileUnits(panel.Size.Width));
            int heightUnits = (int)Math.Round(ResizablePanel.SizeToTileUnits(panel.Size.Height));

            AnchorStyles anchor = panel.Anchor;
            if (anchor == AnchorStyles.None && panel.Controls.Count > 0 &&
                panel.Controls[0] is ComponentFormTile tile)
            {
                anchor = tile.Anchor;
            }

            if (!AttachedForms.TryGetValue(panel, out ComponentForm form))
            {
                MessageBox.Show("One or more panels lack contents. You must delete or fill them before saving.",
                    "Custom Layout Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string typeName = form.GetType().Name;
            if (typeName == "SelectBoxForm")
                typeName = "MemoryTools";
            sb.AppendLine($"SetTileForm:{typeName},{unitsX},{unitsY},{widthUnits},{heightUnits},{(int)anchor}");
        }

        sb.AppendLine($"LoadTo:{(cbLoadToMain.Checked ? "Main" : "External")}");

        File.WriteAllText(path, sb.ToString());
    }
    
    protected override void OnResizeEnd(EventArgs e)
    {
        Size size = Size;

        int widthUnits = (int)Math.Round(SizeToUnits(pnGridContainer.Size.Width));
        int targetWidth = UnitsToSize(widthUnits);
        
        int widthWithoutGrid = this.Size.Width - pnGridContainer.Size.Width;
        size.Width = targetWidth + widthWithoutGrid;
        
        int heightUnits = (int)Math.Round(SizeToUnits(pnGridContainer.Size.Height));
        int targetHeight = UnitsToSize(heightUnits);
        
        int heightWithoutGrid = this.Size.Height - pnGridContainer.Size.Height;
        size.Height = targetHeight + heightWithoutGrid;
    
        Size = size;
        nmDefaultWidth.Value = widthUnits;
        nmDefaultHeight.Value = heightUnits;
        nmMinimumWidth.Value = Math.Min(nmMinimumWidth.Value, widthUnits);
        nmMinimumHeight.Value = Math.Min(nmMinimumHeight.Value, heightUnits);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        foreach (var panel in AttachedForms.Keys.ToList())
        {
            panel.Delete();
        }
        S.SET<EditorForm>(null);
        base.OnFormClosing(e);
    }

    private static float SizeToUnits(int size)
    {
        return (size - 16) / 48f;
    }

    private static int UnitsToSize(int units)
    {
        return units * 32 + units * 16 + 16;
    }
}