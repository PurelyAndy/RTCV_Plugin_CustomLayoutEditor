using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.Common;
using RTCV.CorruptCore;
using RTCV.UI;
using RTCV.UI.Extensions;
using RTCV.UI.Modular;

namespace CustomLayoutEditor.UI;

public partial class MiniComponent : UserControl
{
    private string ComponentName { get; }
    public MiniComponent() : this(UICore.mtForm, "MemoryTools")
    {
    }
        
    public MiniComponent(Type type) : this((ComponentForm)S.GET(type), type.FullName)
    {
    }

    private MiniComponent(ComponentForm form, string name)
    {
        InitializeComponent();
        ComponentName = name;
            
        string cachePath = Path.Combine(RtcCore.PluginDir, "CustomLayoutEditor", "Cache");
        if (!Directory.Exists(cachePath))
        {
            Directory.CreateDirectory(cachePath);
        }
        string imageLocation = Path.Combine(cachePath, $"{name}.png");
            
        Bitmap componentScreenshot;
        if (!File.Exists(imageLocation))
        {
            form.SwitchToWindow();
            form.Show();
            form.Update();
            
            Type t = typeof(ControlExtensions);
            componentScreenshot = (Bitmap)t.GetMethod("getFormScreenShot", BindingFlags.NonPublic | BindingFlags.Static)!.Invoke(null, [form]);
            componentScreenshot.Save(imageLocation);
            
            try
            {
                form.RestoreToPreviousPanel();
            }
            catch (Exception)
            {
                form.Hide();
            }
        }
        else
        {
            componentScreenshot = new(imageLocation);
        }
            
        this.Width = panel1.Height * componentScreenshot.Width / componentScreenshot.Height;
            
        panel1.BackgroundImageLayout = ImageLayout.Stretch;
        panel1.BackgroundImage = componentScreenshot;
        panel1.Width = panel1.Height * componentScreenshot.Width / componentScreenshot.Height;
            
        label1.Text = name[(name.LastIndexOf('.') + 1)..];

        panel1.MouseDown += (_, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(ComponentName, DragDropEffects.Copy);
            }
        };
    }
}