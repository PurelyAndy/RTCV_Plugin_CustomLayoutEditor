using System;
using System.ComponentModel.Composition;
using CustomLayoutEditor.UI;
using RTCV.Common;
using RTCV.PluginHost;
using RTCV.UI;
using RTCV.UI.Modular;

namespace CustomLayoutEditor;

[Export(typeof(IPlugin))]
public class CustomLayoutEditor : IPlugin
{
    public string Name => "Custom Layout Editor";
    public string Description => "Allows you to edit the layout of the RTC UI.";
    public string Author => "PurelyAndy";
    public Version Version => new(1, 0, 0);
    public RTCSide SupportedSide => RTCSide.Server;
    public static PluginForm PluginForm;
    public static CustomLayoutEditor Instance { get; private set; }

    public bool Start(RTCSide side)
    {
        Instance = this;
        PluginForm = new();

        UICore.mtForm.cbSelectBox.Items.Add(new Entry { text = PluginForm.Text, value = PluginForm });

        return true;
    }

    public bool StopPlugin()
    {
        if (!S.ISNULL<PluginForm>() && !S.GET<PluginForm>().IsDisposed)
        {
            S.GET<PluginForm>().HideOnClose = false;
            S.GET<PluginForm>().Close();
        }
        return true;
    }

    public void Dispose()
    {
    }
    
    // ReSharper disable All
    public class Entry
    {
        public string text { get; set; }
        public ComponentForm value { get; set; }
    }
    // ReSharper restore All
}