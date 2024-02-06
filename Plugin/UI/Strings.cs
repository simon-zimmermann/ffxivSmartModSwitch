
using System.Collections.Generic;
using System.Linq;
using SmartModSwitch.Data;

namespace SmartModSwitch.UI;

public static class Strings {
    public static readonly Dictionary<AsgType, string> UIAsgType = new() {
        { AsgType.NOTHING, "Nothing" },
        { AsgType.EMOTE, "Emote" },
        { AsgType.COMMAND, "Command" }
    };
    public static readonly string[] UIAsgTypeStr = [.. UIAsgType.Values];
    public static readonly Dictionary<AsgReset, string> UIAsgReset = new() {
        { AsgReset.NEXT, "Next" },
        { AsgReset.TIMER, "Timer" }
    };
    public static readonly string[] UIAsgResetStr = [.. UIAsgReset.Values];
}

