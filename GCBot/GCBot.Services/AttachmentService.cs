using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GCBot.Models;
using GCBot.Services.Repositories;
using GCBot.Services.Services;

namespace GCBot.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAllowedExtensionRepository _allowedExtensionRepository;

        public AttachmentService(IAllowedExtensionRepository repository)
        {
            _allowedExtensionRepository = repository;
        }

        public IEnumerable<Extension> GetAllAllowedExtensions()
        {
            return _allowedExtensionRepository.GetAll().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>True if all of the message attachments' extensions are allowed</returns>
        async Task<bool> IAttachmentService.VerifyAttachmentExtensions(SocketUserMessage msg)
        {
            bool areAllowed = AreAttachmentsAllowed(msg.Attachments);

            if (!areAllowed)
            {
                await msg.DeleteAsync();
                IDMChannel dmChannelAsync = await msg.Author.GetOrCreateDMChannelAsync();
                await dmChannelAsync.SendMessageAsync(
                    $"You attempted to upload a file that is not permitted in channel `{msg.Channel.Name}`");
            }
            
            return areAllowed;
        }

        /// <summary>
        /// Adds the given extension to the whitelist for uploaded attachments.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns>Returns true if the extension was added, otherwise returns false if the extension exists</returns>
        public bool WhitelistExtension(string extension)
        {
            if(_allowedExtensionRepository.ExtensionExists(extension))
            {
                return false;
            }
            
            _allowedExtensionRepository.Create(new Extension() {Value = extension.Trim('.').ToLowerInvariant()});
            
            return true;
        }
        
        public void BlacklistExtension(string extension)
        {
            _allowedExtensionRepository.Delete(extension);
        }

        private bool AreAttachmentsAllowed(IEnumerable<Attachment> attachments)
        {
            return attachments.All(IsAttachmentAllowed);
        }

        private bool IsAttachmentAllowed(Attachment attachment)
        {
            var allowedExtensions = _extensionRepository.GetAll();

            AllowedExtension allowedExtension = allowedExtensions?.FirstOrDefault(extension =>
                extension.Value.Trim('.').Equals(attachment.GetExtension(), StringComparison.CurrentCultureIgnoreCase));
            
            return  allowedExtension != null;
        }
    }

    public static class AttachmentExtensions
    {
        public static string GetExtension(this Attachment attachment)
        {
            return Path.GetExtension(attachment.Url).Trim('.');
        }
    }
    
}
