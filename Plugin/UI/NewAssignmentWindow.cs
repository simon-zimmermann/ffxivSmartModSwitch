using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System.Linq;
using Penumbra.Api;
using System.Threading;
using System.Threading.Tasks;
using SmartModSwitch.Interop;

namespace SmartModSwitch.UI;

public class NewAssignmentWindow : Window, IDisposable {
    private readonly SmartModSwitch smsw;
    private List<PenumbraMod> modList = new List<PenumbraMod>();
    private List<Emote> emoteList = new List<Emote>();
    private int selectedModIdx = 0;
    private int selectedEmoteIdx = 0;

    public NewAssignmentWindow(SmartModSwitch smsw) : base(
        "NewAssignmentWindow", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoTitleBar) {
        //this.SizeConstraints = new WindowSizeConstraints
        //{
        //    MinimumSize = new Vector2(375, 330),
        //    MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        //};

        this.smsw = smsw;


    }

    public void Dispose() {
    }

    public void Show() {
        IsOpen = true;
        modList = smsw.PenumbraIPC.GetModList();

        var emoteSheet = smsw.DataManager.GetExcelSheet<Emote>();
        if (emoteSheet == null) return;
        foreach (var item in emoteSheet) {
            //var emote = new EmoteRow(item);
            //if (emote.Keys.Count > 0) Items.Add(emote);
            if (item.Name != null && item.Name != "") {
                emoteList.Add(item);
            }
        }
    }

    private async Task asyncTask() {
        //var sw = new Stopwatch();
        //sw.Start();
        smsw.Logger.Info("async: Starting");
        Task delay = Task.Delay(2000);
        //Console.WriteLine("async: Running for {0} seconds", sw.Elapsed.TotalSeconds);
        await delay;
        //Console.WriteLine("async: Running for {0} seconds", sw.Elapsed.TotalSeconds);
        smsw.Logger.Info("async: Done");

        var success = Ipc.TrySetMod.Subscriber(smsw.PluginInterface).Invoke("Zfox Serious Base", modList[selectedModIdx].PenumbraPath, modList[selectedModIdx].ModName, false);
        smsw.Logger.Info("TrySetMod success: {0}", success);
    }
    public override void Draw() {
        {
            ImGui.BeginChild("ModSelect", new Vector2(ImGui.GetContentRegionAvail().X * 0.33f, -1));
            ImGui.Text("Select Mod: ");
            ImGui.BeginListBox("", new Vector2(-1, -1));
            for (int n = 0; n < modList.Count; n++) {
                bool isSelected = selectedModIdx == n;
                if (ImGui.Selectable(modList[n].PenumbraPath, isSelected)) {
                    selectedModIdx = n;
                }
            }
            ImGui.EndListBox();
            ImGui.EndChild();
        }
        ImGui.SameLine();
        {
            ImGui.BeginChild("EmoteSelect", new Vector2(ImGui.GetContentRegionAvail().X * 0.5f, -1));
            ImGui.Text("Select Emote");
            ImGui.BeginListBox("", new Vector2(-1, -1));
            for (int n = 0; n < emoteList.Count; n++) {
                bool isSelected = selectedEmoteIdx == n;
                if (ImGui.Selectable(emoteList[n].Name, isSelected)) {
                    selectedEmoteIdx = n;
                }
            }
            ImGui.EndListBox();
            ImGui.EndChild();
        }
        ImGui.SameLine();
        {
            ImGui.BeginChild("Summary", new Vector2(ImGui.GetContentRegionAvail().X * 1, -1));
            ImGui.Text("Summary");
            ImGui.TextWrapped("Selected Mod Name: " + modList[selectedModIdx].ModName);
            ImGui.TextWrapped("Selected Mod FS path: " + modList[selectedModIdx].FileSystemPath);
            ImGui.TextWrapped("Selected Mod Penumbra path: " + modList[selectedModIdx].PenumbraPath);
            ImGui.Spacing();
            ImGui.TextWrapped("Selected Emote Name: " + emoteList[selectedEmoteIdx].Name);
            ImGui.TextWrapped("Selected Emote TextCommand: " + emoteList[selectedEmoteIdx].TextCommand.Value?.Command.ToString() ?? "");
            ImGui.Spacing();


            // var success = Ipc.TrySetMod.Subscriber(smsw.PluginInterface).Invoke("Zfox Serious Base", "dances/memes/[OCN] Drop the bass","[OCN] Drop the bass",false);
            // smsw.Logger.Info("TrySetMod success: {0}", success);
            if (ImGui.Button("Execute")) {
                var success = Ipc.TrySetMod.Subscriber(smsw.PluginInterface).Invoke("Zfox Serious Base", modList[selectedModIdx].PenumbraPath, modList[selectedModIdx].ModName, true);
                smsw.Logger.Info("TrySetMod success: {0}", success);

                smsw.ChatHelper.SendSanitizedChatMessage(emoteList[selectedEmoteIdx].TextCommand.Value?.Command.ToString() ?? "");

                Task delay = asyncTask();
                //delay.Wait();
                //success = Ipc.TrySetMod.Subscriber(smsw.PluginInterface).Invoke("Zfox Serious Base", modList[selectedModIdx].PenumbraPath, modList[selectedModIdx].ModName, false);
                //smsw.Logger.Info("TrySetMod success: {0}", success);
            }

            ImGui.EndChild();
        }
    }
}