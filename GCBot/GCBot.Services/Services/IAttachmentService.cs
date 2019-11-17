using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCBot.Models;

namespace GCBot.Services.Services
{
    public interface IAttachmentService
    {
        IEnumerable<Extension> GetAllAllowedExtensions();
        bool WhitelistExtension(string extension);
        void BlacklistExtension(string extension);
    }
}