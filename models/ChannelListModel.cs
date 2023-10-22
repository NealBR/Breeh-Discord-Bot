namespace CSharp_Discord_Bot.models
{
    public class ChannelListModel
    {
        public string Mode { get; set; }

        public List<ChannelModel> Values { get; set; } = new();
    }
}