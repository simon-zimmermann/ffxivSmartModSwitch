using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace SmartModSwitch
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        public bool OverlayActive { get; set; } = false;

        // boilerplate code to save config (copied from plugin template)
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;
        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }
        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
