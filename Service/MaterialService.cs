using Domain.Entities;
using Domain.Interfaces;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaterialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GeneralResponseDto> AddMaterial(AddMaterialDto addMaterialDto)
        {
            var newMaterial = new Material
            { 
                Title = addMaterialDto.Title,
                Description = addMaterialDto.Description,
                File = addMaterialDto.File,
                Type = addMaterialDto.Type,
                GroupId = addMaterialDto.GroupId,
            };
            await _unitOfWork.GetRepository<Material>().AddAsync(newMaterial);
            var result = await _unitOfWork.SaveChanges();
            if (result == 0)
            {
               return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to add material.",
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Material added successfully.",
            };

        }

        public async Task<GeneralResponseDto> DeleteMaterial(string id)
        {
            var material = await _unitOfWork.GetRepository<Material>().GetByIdAsync(id);
            if (material == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Material not found.",
                };
            }
            _unitOfWork.GetRepository<Material>().Delete(material);
            var result = await _unitOfWork.SaveChanges();
            if (result == 0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to delete material.",
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Material deleted successfully.",
            };

        }
    }
}
