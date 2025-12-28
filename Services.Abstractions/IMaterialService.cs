using Shared.Dtos;
using Shared.Dtos.Material;
using Microsoft.AspNetCore.Http;
using Shared.Dtos.Subscribe;
namespace Services.Abstractions
{
    public interface IMaterialService
    {
        public Task<GeneralResponseDto> AddMaterial(AddMaterialDto addMaterialDto);
        public Task<GeneralResponseDto> DeleteMaterial(string id);
        public Task<GeneralResponseDto> GetAllMaterial(string groupId);
        public Task<GeneralResponseDto> AddVideoFromYoutube(AddMaterialFromYoutube addMaterialFromYoutube);
        public Task<GeneralResponseDto> ShowHomeworkForStudent(ShowHomeworkDto showHomeworkDto);
        public Task<GeneralResponseDto> AddHomeworkForStudent(AddHomeworkDto addHomeworkDto);
    }
}
