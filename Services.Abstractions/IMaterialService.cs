using Shared.Dtos;
using Shared.Dtos.Material;
using Microsoft.AspNetCore.Http;
namespace Services.Abstractions
{
    public interface IMaterialService
    {
        public Task<GeneralResponseDto> AddMaterial(AddMaterialDto addMaterialDto, IFormFile file);
        public Task<GeneralResponseDto> DeleteMaterial(string id);
        public Task<GeneralResponseDto> GetAllMaterial(string groupId);
    }
}
