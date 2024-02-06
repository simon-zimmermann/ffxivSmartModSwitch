using System;
using System.Collections.Generic;

namespace SmartModSwitch.Data;


/// <summary>
/// = Assignment Group
/// </summary>
[Serializable]
public class Asg {
	public string Name { get; set; }
	public bool Enabled { get; set; } = false;
	public AsgType Type { get; set; } = AsgType.NOTHING;
	public int EmoteIdx { get; set; } = 0;
	public AsgReset Reset { get; set; } = AsgReset.NEXT;
	public float ResetTime { get; set; } = 0.0f;

	public Asg(string name) {
		Name = name;
	}
}


public enum AsgType : int {
	NOTHING,
	EMOTE,
	COMMAND
}
public enum AsgReset : int {
	NEXT,
	TIMER
}