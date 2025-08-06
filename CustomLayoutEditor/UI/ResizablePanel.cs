using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using RTCV.Common;
using RTCV.UI;
using RTCV.UI.Modular;

namespace CustomLayoutEditor.UI;

internal class ResizablePanel : Panel
{
    private readonly Control _parent;
    private ComponentFormTile _tile;
    private ComponentForm _form;
    private readonly bool _topLevel;
    private Point _mouseDownAt = new(int.MinValue, int.MinValue);
    private Direction _direction;
    internal const int BorderSize = 4;

    public new Size Size
    {
        get => base.Size;
        set => base.Size = value + new Size(BorderSize * 2, BorderSize * 2);
    }

    public new Point Location
    {
        get => base.Location;
        set => base.Location = value - new Size(BorderSize, BorderSize);
    }

    private enum Direction
    {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Left,
        Right,
        Top,
        Bottom
    }

    private Direction ResizeDirection(Point p)
    {
        if (p is { X: < 12, Y: < 4 } or { X: < 4, Y: < 12 })
            return Direction.TopLeft;
        if (p.X > Width - 12 && p.Y > Height - 4 || p.X > Width - 4 && p.Y > Height - 12)
            return Direction.BottomRight;
        if (p.X < 12 && p.Y > Height - 4 || p.X < 4 && p.Y > Height - 12)
            return Direction.BottomLeft;
        if (p.X > Width - 12 && p.Y < 4 || p.X > Width - 4 && p.Y < 12)
            return Direction.TopRight;
        if (p.X < 4)
            return Direction.Left;
        if (p.X > Width - 4)
            return Direction.Right;
        if (p.Y < 4)
            return Direction.Top;
        if (p.Y > Height - 4)
            return Direction.Bottom;
        return Direction.None;
    }

    public ResizablePanel(Control parent, bool topLevel = false)
    {
        _parent = parent;
        _topLevel = topLevel;

        AllowDrop = true;
        Padding = new(BorderSize);
        
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

        MouseClick += ShowContextMenu;
        MouseDown += (_, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDownAt = e.Location;
                _direction = ResizeDirection(_mouseDownAt);
            }
        };
        MouseMove += DragOrResize;
        MouseLeave += (_, _) =>
        {
            this.Cursor = Cursors.Default;
            _mouseDownAt = new(int.MinValue, int.MinValue);
        };
        bool dont = false;
        Paint += DrawTextAndBorders;

        return;

        void DrawTextAndBorders(object _, PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, "Drag and drop a menu to here from the top", Font, ClientRectangle,
                Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);

            e.Graphics.DrawRectangle(new Pen(Color.White, 2) { DashStyle = DashStyle.Dash },
                new Rectangle(1, 1, Width - 2, Height - 2));
            if (!dont)
            {
                dont = true;
                Invalidate();
                Update();
            }

            dont = false;
        }
    }

    private void DragOrResize(object sender, MouseEventArgs e)
    {
        this.Cursor = ResizeDirection(e.Location) switch
        {
            Direction.TopLeft or Direction.BottomRight => Cursors.SizeNWSE,
            Direction.TopRight or Direction.BottomLeft => Cursors.SizeNESW,
            Direction.Left or Direction.Right => Cursors.SizeWE,
            Direction.Top or Direction.Bottom => Cursors.SizeNS,
            _ => Cursors.SizeAll
        };

        if (e.Button != MouseButtons.Left) return;

        if (_mouseDownAt.X == int.MinValue || _mouseDownAt.Y == int.MinValue)
        {
            return;
        }

        if (_direction == Direction.None)
        {
            if (Math.Abs(e.X - _mouseDownAt.X) > 5 || Math.Abs(e.Y - _mouseDownAt.Y) > 5)
            {
                this.Location = RoundToSize(new(
                    Math.Min(Math.Max(this.Location.X + e.X - _mouseDownAt.X, 0), _parent.Width - Width - BorderSize * 2),
                    Math.Min(Math.Max(this.Location.Y + e.Y - _mouseDownAt.Y, 0), _parent.Height - Height - BorderSize * 2)
                )) + new Size(BorderSize, BorderSize);
            }

            return;
        }

        int xDiff = e.X - _mouseDownAt.X;
        int yDiff = e.Y - _mouseDownAt.Y;

        Point oldLocation = Location;

        Point newLocation = Location;
        Size newSize = base.Size;
        bool baseSize = false;
        bool locationChanged = false;
        SuspendLayout();
        switch (_direction)
        {
            case Direction.TopLeft:
                newLocation = RoundToSize(new(Location.X + xDiff, Location.Y + yDiff));
                newSize -= new Size(newLocation.X - oldLocation.X, newLocation.Y - oldLocation.Y);
                baseSize = true;
                locationChanged = true;
                break;
            case Direction.Top:
                newLocation = RoundToSize(new(Location.X, Location.Y + yDiff));
                newSize -= new Size(0, newLocation.Y - oldLocation.Y);
                baseSize = true;
                locationChanged = true;
                break;
            case Direction.TopRight:
                newLocation = RoundToSize(new(Location.X, Location.Y + yDiff));
                newSize = RoundToTileUnits(new(e.X, base.Size.Height - (newLocation.Y - oldLocation.Y)));
                locationChanged = true;
                break;
            case Direction.Right:
                newSize = RoundToTileUnits(Size with { Width = e.X });
                break;
            case Direction.BottomRight:
                newSize = RoundToTileUnits(new(e.X, e.Y));
                break;
            case Direction.Bottom:
                newSize = RoundToTileUnits(new(base.Size.Width, e.Y));
                break;
            case Direction.BottomLeft:
                newLocation = RoundToSize(new(Location.X + xDiff, Location.Y));
                newSize = RoundToTileUnits(new(base.Size.Width - (newLocation.X - oldLocation.X), e.Y));
                locationChanged = true;
                break;
            case Direction.Left:
                newLocation = RoundToSize(new(Location.X + xDiff, Location.Y));
                newSize = RoundToTileUnits(new(base.Size.Width - (newLocation.X - oldLocation.X), base.Size.Height));
                locationChanged = true;
                break;
        }
        
        if (newSize.Width <= 0 || newSize.Height <= 0)
        {
            ResumeLayout();
            return;
        }

        if (newLocation.X < -BorderSize || newLocation.Y < -BorderSize ||
            newLocation.X + newSize.Width > _parent.Width + BorderSize * 2 ||
            newLocation.Y + newSize.Height > _parent.Height + BorderSize * 2)
        {
            ResumeLayout();
            return;
        }
        
        if (locationChanged)
            Location = newLocation + new Size(BorderSize, BorderSize);
        if (baseSize)
            base.Size = newSize;
        else
            Size = newSize;
        
        ResumeLayout();
    }

    private void ShowContextMenu(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        if (sender is ResizablePanel panel)
        {
            if (panel._form is not null)
            {
                MakeContextMenu(panel, e);
                return;
            }
            new ContextMenuBuilder()
                .If(!panel._topLevel).AddItem("Delete", Remove).EndIf()
                .BeginSubMenu("Edit Anchor Values")
                .AddItem("Left", (_, _) => Anchor ^= AnchorStyles.Left,
                    isChecked: Anchor.Contains(AnchorStyles.Left))
                .AddItem("Right", (_, _) => Anchor ^= AnchorStyles.Right,
                    isChecked: Anchor.Contains(AnchorStyles.Right))
                .AddItem("Top", (_, _) => Anchor ^= AnchorStyles.Top,
                    isChecked: Anchor.Contains(AnchorStyles.Top))
                .AddItem("Bottom", (_, _) => Anchor ^= AnchorStyles.Bottom,
                    isChecked: Anchor.Contains(AnchorStyles.Bottom))
                .EndSubMenu()
                .Build().Show(panel, e.Location);
        }
    }

    public void Delete()
    {
        Remove(null, null);
        if (!IsDisposed)
        {
            Remove(null, null);
        }
    }

    private void Remove(object sender, EventArgs e)
    {
        if (Controls.Count > 0)
        {
            _form.SwitchToWindow();
            _tile.Dispose();
            _form.RestoreToPreviousPanel();
            Controls.Clear();
            S.GET<EditorForm>().AttachedForms.Remove(this);
            _form = null;
            _tile = null;
            return;
        }

        if (Parent != null)
        {
            Parent.Controls.Remove(this);
            Dispose();
        }
    }

    internal static float SizeToUnits(int size)
    {
        return size / 48f;
    }

    internal static float SizeToTileUnits(int size)
    {
        return size / 48f + 1f / 3f;
    }

    internal static Size RoundToTileUnits(Size size)
    {
        return new()
        {
            Width = UnitsToTileSize((int)Math.Round(SizeToTileUnits(size.Width))),
            Height = UnitsToTileSize((int)Math.Round(SizeToTileUnits(size.Height)))
        };
    }
    internal static Size CeilToTileUnits(Size size)
    {
        return new()
        {
            Width = UnitsToTileSize((int)Math.Ceiling(SizeToTileUnits(size.Width))),
            Height = UnitsToTileSize((int)Math.Ceiling(SizeToTileUnits(size.Height)))
        };
    }

    internal static Point RoundToSize(Point point)
    {
        return new()
        {
            X = UnitsToSize((int)Math.Round(SizeToUnits(point.X))),
            Y = UnitsToSize((int)Math.Round(SizeToUnits(point.Y)))
        };
    }

    internal static int UnitsToSize(int units)
    {
        return units * 32 + units * 16;
    }

    internal static int UnitsToTileSize(int units)
    {
        return units * 48 - 16;
    }

    protected override void OnDragEnter(DragEventArgs e)
    {
        e.Effect = e.Data.GetDataPresent(DataFormats.StringFormat) ? DragDropEffects.Copy : DragDropEffects.None;
    }

    protected override void OnDragDrop(DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.StringFormat))
        {
            string typeName = (string)e.Data.GetData(DataFormats.StringFormat);
            if (typeName == "MemoryTools")
            {
                ApplyWithTile(UICore.mtForm);
                return;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(typeName);
                if (type != null && type.IsSubclassOf(typeof(ComponentForm)))
                {
                    ApplyWithTile((ComponentForm)S.GET(type));
                    return;
                }
            }
        }
    }

    internal void ApplyWithTile(ComponentForm componentForm)
    {
        if (Controls.Count > 0)
        {
            Remove(null, null);
        }

        if (S.GET<EditorForm>().AttachedForms.TryGetValue(componentForm, out ResizablePanel panel))
        {
            panel.Remove(null, null);
        }

        _tile = new();
        _form = componentForm;
        _tile.SetComponentForm(_form, (int)SizeToTileUnits(Width), (int)SizeToTileUnits(Height), true);
        _tile.TopLevel = false;
        _tile.Visible = true;
        _tile.Anchor = Anchor;
        Controls.Add(_tile);
        _tile.Dock = DockStyle.Fill;
        _form.Dock = DockStyle.Fill;

        S.GET<EditorForm>().AttachedForms[this] = _form;

        // By default, the tile is popped out to a window when dragging, so we have to remove these handlers
        var privateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        var cft = typeof(ComponentFormTile);
        var meh = typeof(MouseEventHandler);
        MouseEventHandler onFormTileMouseDown =
                (MouseEventHandler)cft.GetMethod("OnFormTileMouseDown", privateInstance)!.CreateDelegate(meh, _tile),
            onFormTileMouseUp =
                (MouseEventHandler)cft.GetMethod("OnFormTileMouseUp", privateInstance)!.CreateDelegate(meh, _tile),
            onFormTileMouseMove =
                (MouseEventHandler)cft.GetMethod("OnFormTileMouseMove", privateInstance)!.CreateDelegate(meh, _tile);

        _tile.MouseDown -= onFormTileMouseDown;
        _tile.MouseUp -= onFormTileMouseUp;
        _tile.MouseMove -= onFormTileMouseMove;
        _tile.lbComponentFormName.MouseDown -= onFormTileMouseDown;
        _tile.lbComponentFormName.MouseUp -= onFormTileMouseUp;
        _tile.lbComponentFormName.MouseMove -= onFormTileMouseMove;


        _tile.MouseDown += MakeContextMenu;
        _tile.lbComponentFormName.MouseDown += MakeContextMenu;

        Point mouseDownAt = new(int.MinValue, int.MinValue);

        _tile.MouseDown += MouseDown;
        _tile.MouseMove += MouseMove;
        _tile.lbComponentFormName.MouseDown += MouseDown;
        _tile.lbComponentFormName.MouseMove += MouseMove;

        return;

        void MouseDown(object o, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            mouseDownAt = e.Location;
        }

        void MouseMove(object o, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.Location = RoundToSize(new(
                Math.Min(Math.Max(this.Location.X + e.X - mouseDownAt.X, 0), _parent.Width - Width - BorderSize * 2),
                Math.Min(Math.Max(this.Location.Y + e.Y - mouseDownAt.Y, 0), _parent.Height - Height - BorderSize * 2)
            )) + new Size(BorderSize, BorderSize);
        }
    }

    private void MakeContextMenu(object o, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        new ContextMenuBuilder().AddItem("Detach to Window", (_, _) => _form.SwitchToWindow())
            .AddItem("Remove", Remove)
            .AddItem("Delete", (_, _) => Delete())
            .BeginSubMenu("Edit Anchor Values")
            .AddItem("Left", (_, _) => Anchor ^= AnchorStyles.Left,
                isChecked: Anchor.Contains(AnchorStyles.Left))
            .AddItem("Right", (_, _) => Anchor ^= AnchorStyles.Right,
                isChecked: Anchor.Contains(AnchorStyles.Right))
            .AddItem("Top", (_, _) => Anchor ^= AnchorStyles.Top,
                isChecked: Anchor.Contains(AnchorStyles.Top))
            .AddItem("Bottom", (_, _) => Anchor ^= AnchorStyles.Bottom,
                isChecked: Anchor.Contains(AnchorStyles.Bottom))
            .EndSubMenu()
            .Build()
            .Show((Control)o, this.PointToClient(Cursor.Position));
    }
}