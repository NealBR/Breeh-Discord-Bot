using System.ComponentModel.DataAnnotations;

namespace CSharp_Discord_Bot.models
{
    public class ServerModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public ulong ID { get; set; }

        public ChannelListModel ChannelList { get; set; } = new();
    }
}