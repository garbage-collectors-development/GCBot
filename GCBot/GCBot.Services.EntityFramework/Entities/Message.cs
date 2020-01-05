using System;

namespace GCBot.Services.EntityFramework.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime DateSent { get; set; }
        public ulong SenderId { get; set; }
        public ulong ChannelId { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
    }
}
