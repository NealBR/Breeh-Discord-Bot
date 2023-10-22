using System.ComponentModel.DataAnnotations;

namespace CSharp_Discord_Bot.models
{
    public class ChannelModel
    {
        [Required]
        public ulong ID { get; set; }

        public bool DevCommands { get; set; } = false;
    }
}