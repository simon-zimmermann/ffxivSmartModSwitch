using System;

namespace SmartModSwitch.Data;

[Serializable]
public class AssignmentGroup {
    public string Name { get; set; }
    public bool Enabled { get; set; } = false;

    public AssignmentGroup(string name) {
        Name = name;
    }
}