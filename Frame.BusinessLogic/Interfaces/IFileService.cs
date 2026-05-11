using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Frame.BusinessLogic.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
    }
}
