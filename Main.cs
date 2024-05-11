using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CrashPlayerSharpApi;



namespace CrashPlayerCmd;
public class CrashPlayerCmd : BasePlugin
{

    public override string ModuleName => "CrashPlayer CSharp Cmd [Module]";
    public override string ModuleVersion => "0.1";

    private ICrashPlayerApi? cpc;
    private readonly PluginCapability<ICrashPlayerApi> pluginCPC = new("crashplayer:nfcore");


    public override void OnAllPluginsLoaded(bool hotReload)
    {
        cpc = pluginCPC.Get();
    }

    [RequiresPermissions("@css/root")]
    [ConsoleCommand("css_crashp", "Crash choosen player!")]
    //[CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void OnCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (command.ArgCount > 2)
            command.ReplyToCommand("Too many arguments!");
        else if (command.ArgCount == 2)
        {
            var targets = command.GetArgTargetResult(1);
            var res = "";
            foreach (var target in targets)
            {
                if (CheckPlayer(target) && target != player)
                {
                    if (res == "")
                        res = target.PlayerName;
                    else
                        res = $"{res}, {target.PlayerName}";
                    cpc.CPC_CrashPlayer(target);
                }
            }
            if (res == "")
                command.ReplyToCommand("There is no one to crash...");
            else
                command.ReplyToCommand($"Crashing players: {res}");
        }
        else
            command.ReplyToCommand("Usage: css_crash < user_id >");
    }

    private bool CheckPlayer(CCSPlayerController player)
    {
        if (player.IsValid && player.Connected == PlayerConnectedState.PlayerConnected && !player.IsBot) return true;
        else return false;
    }
}