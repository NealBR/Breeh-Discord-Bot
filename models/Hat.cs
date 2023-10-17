namespace CSharp_Discord_Bot.models
{
    public class Hat
    {
        public string? caption { get; set; }
        public string? filename { get; set; }
        public bool enabled { get; set; } = true;
        public int monthFrom { get; set; } = 1;
        public int dayFrom { get; set; } = 1;
        public int monthTo { get; set; } = 12;
        public int dayTo { get; set; } = 31;

        /// <summary>
        ///     Weight that determines the chance of this hat getting chosen.
        ///     Increase the number to have this hat chosen more common, decrease it to make it more rare.
        /// </summary>
        /// <value>A positive number representing the rarity of the hat. Defaults to 1.</value>
        public double weight { get; set; } = 1;
    }
}
