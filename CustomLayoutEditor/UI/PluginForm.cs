using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using RTCV.Common;
using RTCV.UI.Modular;

namespace CustomLayoutEditor.UI;

public partial class PluginForm : ComponentForm
{
    public volatile bool HideOnClose = true;

    public PluginForm()
    {
        InitializeComponent();
        FormClosing += PluginForm_FormClosing;
        lbVersion.Text = CustomLayoutEditor.Instance.Version.ToString();
    }

    private void PluginForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (HideOnClose)
        {
            e.Cancel = true;
            Hide();
        }
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
        if (!S.ISNULL<EditorForm>())
        {
            S.GET<EditorForm>().Show();
            return;
        }
        S.SET(new EditorForm());
        S.GET<EditorForm>().Show();
    }
}