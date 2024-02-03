using System;
using System.Linq;
using Penumbra.Api;
using Penumbra.Api.Enums;
namespace SmartModSwitch;

public sealed class ExternalConfigReader : IDisposable
{
    private readonly SmartModSwitch smsw;

    public ExternalConfigReader(SmartModSwitch smsw)
    {
        this.smsw = smsw;

        smsw.Logger.Information("Config dictionary: {0}", smsw.PluginInterface.ConfigDirectory);
        var penumbraAvailable = (smsw.PluginInterface.InstalledPlugins.FirstOrDefault(p => string.Equals(p.InternalName, "Penumbra", StringComparison.OrdinalIgnoreCase))
                ?.Version ?? new Version(0, 0, 0, 0)) >= new Version(0, 8, 1, 6);

        smsw.Logger.Information("Penumbra available: {0}", penumbraAvailable);
        var penumbraRedraw = Penumbra.Api.Ipc.RedrawObjectByName.Subscriber(smsw.PluginInterface);
        penumbraRedraw!.Invoke("self", RedrawType.Redraw);
    }
    public void Dispose()
    {
    }
}