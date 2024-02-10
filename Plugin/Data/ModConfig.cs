using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace SmartModSwitch.Data {
    [Serializable]
    public class ModConfig : IPluginConfiguration {
        public int Version { get; set; } = 0;

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        public bool OverlayActive { get; set; } = false;

        public List<Asg> AssignmentGroups { get; } = new List<Asg>();

        public void Initialize() {
        }
        public void Save() {
            SMSW.PluginInterface.SavePluginConfig(this);
        }
    }
}
