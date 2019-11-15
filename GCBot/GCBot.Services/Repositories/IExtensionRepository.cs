using System.Linq;
using System.Threading.Tasks;
using GCBot.Shared;
using GCBot.Shared.Backup;

namespace GCBot.Services.Repositories
{
    public interface IExtensionRepository
    {
        IQueryable<AllowedExtension> GetAll();
        AllowedExtension GetById(int id);
        void Create(AllowedExtension entity);
        void Delete(int id);
        void Delete(string extension);
        bool ExtensionExists(string extension);
        bool ExtensionExists(AllowedExtension extension);
    }
}