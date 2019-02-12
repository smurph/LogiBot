using Discord.Commands;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace LogiBot.Commands
{
    public class VitalsCommands : ModuleBase<SocketCommandContext>
    {
        [Command("Version")]
        public async Task Version()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            await Context.Channel.SendMessageAsync(version);
        }

        [Command("Ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("pong!");
        }
    }
}
