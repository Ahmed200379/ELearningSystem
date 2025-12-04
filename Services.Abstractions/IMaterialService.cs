using Shared.Dtos;
using Shared.Dtos.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IMaterialService
    {
        public Task<GeneralResponseDto> AddMaterial(AddMaterialDto addMaterialDto);
        public Task<GeneralResponseDto> DeleteMaterial(string id);

    }
}
