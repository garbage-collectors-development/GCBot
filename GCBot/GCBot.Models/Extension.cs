using System;

namespace GCBot.Models
{
    public class Extension
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public ulong AllowedByUserId { get; set; }
        public DateTime DateAllowed { get; set; }
    }
}