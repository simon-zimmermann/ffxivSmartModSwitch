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
		return Strings.UIAsgType[Type] + ": " + SMSW.GameData.emotes[EmoteIdx].Name;
	}
}
public class AsgModsEntry {
	public PenumbraMod PenumbraMod { get; set; }
	public bool Enabled { get; set; } = false;
	public string Command { get; set; } = "";
	public AsgModsEntry(PenumbraMod mod) {
		PenumbraMod = mod;
	}
	public override string ToString() {
		return PenumbraMod.ModName;
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