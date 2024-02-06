using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace SmartModSwitch.Data {
    [Serializable]
    public class Config : IPluginConfiguration {
        public int Version { get; set; } = 0;

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        public bool OverlayActive { get; set; } = false;

        public List<Asg> AssignmentGroups { get; } = new List<Asg>();

        // boilerplate code to save config
        [NonSerialized]
        private SmartModSwitch? smsw;
        public void Initialize(SmartModSwitch smsw) {
            this.smsw = smsw;
            //AssignmentGroups.Add(new AssignmentGroup("Default1"));
            //AssignmentGroups.Add(new AssignmentGroup("Default2"));
            //AssignmentGroups.Add(new AssignmentGroup("Default3"));
        }
        public void Save() {
            smsw!.PluginInterface.SavePluginConfig(this);
        }
    }
}
