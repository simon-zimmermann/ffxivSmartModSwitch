using Dalamud.Configuration;
using System;
using System.Collections.Generic;

namespace SmartModSwitch.Data {
    [Serializable]
    public class ModConfig : IPluginConfiguration {
        public int Version { get; set; } = 0;

        public List<Asg> AssignmentGroups { get; } = new List<Asg>();

        public void Initialize() {
        }
        public void Save() {
            SMSW.PluginInterface.SavePluginConfig(this);
        }
    }
}
