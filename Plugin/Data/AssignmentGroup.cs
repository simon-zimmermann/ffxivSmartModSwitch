using System;
using System.Collections.Generic;
using SmartModSwitch.Interop;
using SmartModSwitch.UI;

namespace SmartModSwitch.Data;


/// <summary>
/// = Assignment Group
/// </summary>
[Serializable]
public class Asg {
	//public string Name { get; set; }
	public bool Enabled { get; set; } = false;
	public AsgType Type { get; set; } = AsgType.NOTHING;
	public int EmoteIdx { get; set; } = 0;
	public AsgReset Reset { get; set; } = AsgReset.NEXT;
	public float ResetTime { get; set; } = 0.0f;
	public List<AsgModsEntry> Mods { get; } = [];

	public Asg() {
	}
	public override string ToString() {
		return Strings.UIAsgType[Type] + ": TODO EMOTE NAME";
	}
}
public class AsgModsEntry {
	public PenumbraMod Mod { get; set; }
	public bool Enabled { get; set; } = false;
	public AsgModsEntry(PenumbraMod mod) {
		Mod = mod;
	}
	public override string ToString() {
		return Mod.ModName;
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