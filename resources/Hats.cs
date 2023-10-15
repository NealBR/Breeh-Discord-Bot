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
            ("Busy", "alHardHat.3x.png"),
            ("Piraty", "alPirateHat.3x.png"),
            ("_**Extra**_ Piraty", "alPirateHat.Extra.3x.png"),
            ("Halloweeny", "alPumpkinHat.3x.png"),
            ("Christmassy", "alSanta.3x.png"),
            ("Fancy", "alTopHat.3x.png"),
            ("_**Extra**_ Fancy", "alTopHat.Extra.3x.png"),
            ("Magical", "alWizardHat.3x.png")
        };
    }
}
