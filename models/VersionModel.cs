using System.ComponentModel.DataAnnotations;

namespace CSharp_Discord_Bot.models
{
    public class VersionModel
    {
        [Required]
        public string Version { get; set; }
    }
}