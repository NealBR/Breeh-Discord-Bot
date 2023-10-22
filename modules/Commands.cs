using Discord;
using Discord.Commands;
using Discord.WebSocket;

using CSharp_Discord_Bot.models;
using CSharp_Discord_Bot.models.singletons;

namespace CSharp_Discord_Bot.modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("al", true)]
        public async Task Exists()
        {
            await ReplyAsync("<:alStare:1162845434264436901>");
        }
        [Command("almood", true)]
        public async Task Status()
        {
            if (!IsValidChannel())
                return;

            Random rnd = new();
            DateTime date = DateTime.Now;
            List<Hat> hats = DataSingleton.GetInstance().hats;

            IEnumerable<Hat> filteredHats = from hat in hats
                                            where hat.enabled
                                               && hat.monthFrom <= date.Month && hat.monthTo >= date.Month
                                               && hat.dayFrom <= date.Day && hat.dayTo >= date.Day
                                            select hat;

            double totalWeight = filteredHats.Sum((hat) => hat.weight);
            double targetWeight = rnd.NextDouble() * totalWeight;

            double currentWeight = 0;
            // Increment the currentWeight with the weight of each hat that is encountered,
            // until we've reached the target weight.
            Hat selectedHat = filteredHats.First(hat => (currentWeight += hat.weight) >= targetWeight);

            string workingDirectory = Environment.CurrentDirectory;
            string resourceDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName + "\\resources\\";

            await Context.Channel.SendFileAsync(resourceDirectory + selectedHat.filename, selectedHat.caption);
        }

        #region Dev Commands
        
        [Command("alversion")]
        public async Task Version()
        {
            if (!IsValidChannel(true))
                return;

            var config = ConfigSingleton.GetInstance();
            await ReplyAsync($"Version: `{config.Version.Version}`.");
        }

        #endregion Dev Commands

        private bool IsValidChannel(bool isDevCommand = false)
        {
            ulong serverID = Context.Guild.Id;
            ulong channelID = Context.Channel.Id;

            var config = ConfigSingleton.GetInstance();
            var server = config.Config.Servers.Find(server => server.ID == serverID);

            // If the server is not in the config, any channel is valid for normal commands.
            if (server == null) return !isDevCommand;
            
            var channel = server.ChannelList.Values.Find(channel => channel.ID == channelID);
            if (server.ChannelList.Mode == "Whitelist")
            {
                if (channel == null) return false;
                // If this is a dev channel, allow any
                return channel.DevCommands || !isDevCommand;
            }

            if (server.ChannelList.Mode == "Blacklist" && channel != null)
                return false;

            return true;
        }
    }
}
