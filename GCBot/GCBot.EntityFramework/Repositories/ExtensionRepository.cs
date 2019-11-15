using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using GCBot.EntityFramework.Entities;
using GCBot.Services.Repositories;
using GCBot.Shared;

namespace GCBot.EntityFramework.Repositories
{
    public class ExtensionRepository : IExtensionRepository
    {
        private readonly ExtensionContext _context;

        public ExtensionRepository(ExtensionContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public IQueryable<AllowedExtension> GetAll()
        {
            return _context.AllowedExtensions.Select(at => new AllowedExtension(){Id = at.Id, Value = at.Value});
        }

        public AllowedExtension GetById(int id)
        {
            Extension extension = _context.AllowedExtensions.FirstOrDefault(at => at.Id == id);
            return extension == null ? null : new AllowedExtension(){Id = extension.Id, Value = extension.Value};
        }
        
        public void Delete(string ext)
        {
            Extension extension = _context.AllowedExtensions.FirstOrDefault(at => string.Equals(at.Value, ext, StringComparison.InvariantCultureIgnoreCase));
            if (extension != null) this.Delete(extension.Id);
        }

        public bool ExtensionExists(string extension)
        {
            Extension ext = _context.AllowedExtensions.FirstOrDefault(at => string.Equals(at.Value, extension, StringComparison.InvariantCultureIgnoreCase));
            return ext != null;
        }
        
        public bool ExtensionExists(AllowedExtension extension)
        {
            Extension ext = _context.AllowedExtensions.FirstOrDefault(s => s.Id == extension.Id && s.Value.Equals(extension.Value));
            return ext != null;
        }

        public void Create(AllowedExtension entity)
        {
            _context.AllowedExtensions.Add(new Extension(){Value = entity.Value});
            Console.WriteLine("Added entity");
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Extension extension = _context.AllowedExtensions.Find(id);
            _context.AllowedExtensions.Remove(extension);
            _context.SaveChanges();
        }
    }
}