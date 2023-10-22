using System.ComponentModel.DataAnnotations;

namespace CSharp_Discord_Bot.models
{
    public class SecretsModel
    {
        [Required]
        public string DISCORD_BOT_TOKEN { get; set; }
    }
}
