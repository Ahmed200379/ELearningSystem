using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces
{
    public interface IImageStorage
    {
        Task<string> SaveFile(IFormFile file,string folder);
        bool DeleteFile(string imagePath);
    }
}
