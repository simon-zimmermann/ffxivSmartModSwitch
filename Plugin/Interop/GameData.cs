using System;
using System.Collections.Generic;
using Lumina.Excel.GeneratedSheets;

namespace SmartModSwitch.Interop;

public class GameData : IDisposable {
	public readonly List<Emote> emotes = [];
	public readonly List<EmoteCategory> emoteCategories = [];

	public GameData() {
		var emoteSheet = SMSW.DataManager.GetExcelSheet<Emote>();
		if (emoteSheet != null)
			foreach (var item in emoteSheet)
				if (item.Name != null && item.Name != "") {
					emotes.Add(item);
				}
		var emoteCategorySheet = SMSW.DataManager.GetExcelSheet<EmoteCategory>();
		if (emoteCategorySheet != null)
			foreach (var item in emoteCategorySheet)
				if (item.Name != null && item.Name != "") {
					emoteCategories.Add(item);
				}

	}

	public void Dispose() {
	}
}