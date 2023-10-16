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
            Random rnd = new Random();
            DateTime date = DateTime.Now;
            HatList hats = DataSingleton.GetInstance().hats;

            Hat hat = null;

            while(hat == null)
            {
                int index = rnd.Next(0, hats.hats.Count);
                Hat tempHat = hats.hats[index];

                if (tempHat.enabled && 
                    (tempHat.monthFrom <= date.Month && tempHat.monthTo >= date.Month) && 
                    (tempHat.dayFrom <= date.Day && tempHat.dayTo >= date.Day))
                {
                    hat = tempHat;
                }
            }

            string workingDirectory = Environment.CurrentDirectory;
            string resourceDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\resources\\";

            await Context.Channel.SendFileAsync(resourceDirectory + hat.filename, hat.caption);
        }
    }
}
