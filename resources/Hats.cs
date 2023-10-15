using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Discord_Bot.resources
{
    public static class Hats
    {
        public static (string, string)[] HatList = new (string Caption, string Filepath)[]
        {
            ("Sleepy", "Sleepy Al.png"),
            ("Fancy", "Tophat Al.png"),
            ("Magical", "Wizard Al.png"),
            ("Busy", "Hard Hat Al.png")
        };
    }
}
