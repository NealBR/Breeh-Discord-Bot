using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

using CSharp_Discord_Bot.models;
using CSharp_Discord_Bot.resources;

namespace CSharp_Discord_Bot.modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("al")]
        public async Task Exists()
        {
            await ReplyAsync("<:alStare:1162845434264436901>");
        }
        [Command("almood")]
        public async Task Status()
        {
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
            //Console.WriteLine($"Rolled a {targetWeight} out of {totalWeight}.");

            double currentWeight = 0;
            // Increment the currentWeight with the weight of each hat that is encountered,
            // until we've reached the target weight.
            Hat selectedHat = filteredHats.First(hat => (currentWeight += hat.weight) >= targetWeight);
            //Console.WriteLine($"CurrentWeight after selecting: {currentWeight}");
            //Console.WriteLine($"Selected Hat: '{selectedHat.filename}', weight {selectedHat.weight}.");

            string workingDirectory = Environment.CurrentDirectory;
            string resourceDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName + "\\resources\\";

            await Context.Channel.SendFileAsync(resourceDirectory + selectedHat.filename, selectedHat.caption);
        }
    }
}
