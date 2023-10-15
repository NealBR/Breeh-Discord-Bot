using CSharp_Discord_Bot.resources;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Discord_Bot.modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("al")]
        public async Task Exists()
        {
            await ReplyAsync("<:alStare:1162845434264436901>");
        }
        [Command("breehtest")]
        public async Task TestExample()
        {
            await ReplyAsync("Test Successful");
        }
        [Command("alstatus")]
        public async Task Status()
        {
            Random rnd = new Random();
            int index = rnd.Next(0, Hats.HatList.Length);
            (string Caption, string Filepath) hat = Hats.HatList[index];

            await Context.Channel.SendFileAsync(hat.Filepath, hat.Caption);
        }
    }
}
