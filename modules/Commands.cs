using Discord.Commands;
using Discord.WebSocket;

using CSharp_Discord_Bot.models;
using CSharp_Discord_Bot.models.singletons;
using CSharp_Discord_Bot.constants;

namespace CSharp_Discord_Bot.modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command(CommandNames.AlExists, true)]
        public async Task Exists()
        {
            await ReplyAsync("<:alStare:1162845434264436901>");
        }

        #region AlStatus Handlers

        private static bool TestHatConditions(Hat hat, SocketGuildUser user)
        {
            if (hat.conditions.Count == 0)
                return true;
            return hat.conditions.Any(condition => TestHatCondition(condition, user));
        }

        private static bool TestHatCondition(HatCondition condition, SocketGuildUser user)
        {
            switch (condition.type)
            {
                case HatConditionType.role:
                    if (ulong.TryParse(condition.value, out ulong roleId))
                        return user.Roles.Any((role) => role.Id == roleId);
                    // If the roleId cannot be parsed to a ulong value, this condition is invalid.
                    break;
                case HatConditionType.user:
                    if (ulong.TryParse(condition.value, out ulong userId))
                        return user.Id == userId;
                    // If the userId cannot be parsed to a ulong value, this condition is invalid.
                    break;
                case HatConditionType.none:
                default:
                    break;
            }
            return false;
        }

        private static Hat PickHat(DataSingleton data, SocketGuildUser user)
        {
            Random rnd = new();
            DateTime date = DateTime.Now;

            // Hats can have specific conditions that need to be met to show up, eg roles / users.
            // We will walk through this collection multiple times, so make it a list to prevent evaluating the
            // conditions multiple times.
            List<Hat> filteredHats = (from hat in data.hats
                                      where hat.enabled
                                         && hat.monthFrom <= date.Month && hat.monthTo >= date.Month
                                         && hat.dayFrom <= date.Day && hat.dayTo >= date.Day
                                         && TestHatConditions(hat, user)
                                      select hat).ToList();

            List<Hat> priorityHats = (from hat in filteredHats
                                      where hat.priority == HatPriority.top
                                      select hat).ToList();
            if (priorityHats.Count > 0)
            {
                var result = priorityHats[0];
                Console.WriteLine($"Matching priority hats: {priorityHats.Count}, first hat is '{result.caption}'.");
                return result;
            }

            IEnumerable<Hat> normalHats = from hat in filteredHats
                                          where hat.priority == HatPriority.normal
                                          select hat;

            double totalWeight = normalHats.Sum((hat) => hat.weight);
            double targetWeight = rnd.NextDouble() * totalWeight;

            double currentWeight = 0;
            // Increment the currentWeight with the weight of each hat that is encountered,
            // until we've reached the target weight.
            return normalHats.First(hat => (currentWeight += hat.weight) >= targetWeight);
        }

        #endregion AlStatus Handlers

        [Command(CommandNames.AlStatus, true)]
        public async Task Status()
        {
            if (!IsValidChannel())
                return;

            DataSingleton data = DataSingleton.GetInstance();

            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);
            // Purging the user cache so that the next time a user calls this command, the roles are updated.
            Context.Guild.PurgeUserCache(user => user.Id == Context.User.Id);

            Hat selectedHat = PickHat(data, user);

            string workingDirectory = Environment.CurrentDirectory;
            string resourceDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName + "\\resources\\";

            if (string.IsNullOrWhiteSpace(selectedHat.filename))
                await Context.Channel.SendMessageAsync(selectedHat.caption);
            else
                await Context.Channel.SendFileAsync(resourceDirectory + selectedHat.filename, selectedHat.caption);
        }

        #region Dev Commands

        [Command(CommandNames.Version)]
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
