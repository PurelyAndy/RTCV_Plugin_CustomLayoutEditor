using CustomLayoutEditor.UI;
using RTCV.Common;
using RTCV.NetCore;

namespace CustomLayoutEditor
{
    public class PluginConnectorRTC : IRoutable
    {
        public PluginConnectorRTC()
        {
            LocalNetCoreRouter.registerEndpoint(this, "Custom_Layout_EditorRTC");
        }

        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            return null;
        }
    }
}