using System;

namespace GCBot.Services.EntityFramework.Entities
{
    public class AllowedExtension
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public ulong AllowedByUserId { get; set; }
        public DateTime DateAllowed { get; set; }
    }
}