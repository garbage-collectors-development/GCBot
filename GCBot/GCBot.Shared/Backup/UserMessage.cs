using System;

namespace GCBot.Shared.Backup
{
    public class UserMessage
    {
        public uint Id { get; set; }
        public DateTime DateSent { get; set; }
        public uint SenderId { get; set; }
        public uint ChannelId { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
    }
}
