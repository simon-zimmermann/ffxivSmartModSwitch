using System;
using System.Collections.Generic;
using Lumina.Excel.GeneratedSheets;

namespace SmartModSwitch.Interop;

public class GameData : IDisposable {
    public readonly List<Emote> emotes = [];
    public readonly List<string> emoteNames = [];
    public readonly List<EmoteCategory> emoteCategories = [];
    public readonly List<string> emoteCategoryNames = [];

    public GameData(SmartModSwitch smsw) {
        var emoteSheet = smsw.DataManager.GetExcelSheet<Emote>();
        if (emoteSheet != null)
            foreach (var item in emoteSheet)
                if (item.Name != null && item.Name != "") {
                    emotes.Add(item);
                    emoteNames.Add(item.Name);
                }
        var emoteCategorySheet = smsw.DataManager.GetExcelSheet<EmoteCategory>();
        if (emoteCategorySheet != null)
            foreach (var item in emoteCategorySheet)
                if (item.Name != null && item.Name != "") {
                    emoteCategories.Add(item);
                    emoteCategoryNames.Add(item.Name);
                }
    }

    public void Dispose() {
    }
}