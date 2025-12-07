using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly IImageStorage _imageStorage;
        public MaterialService(IUnitOfWork unitOfWork, IImageStorage imageStorage)
        {
            _unitOfWork = unitOfWork;
            _imageStorage = imageStorage;
        }
        public async Task<GeneralResponseDto> AddMaterial(AddMaterialDto addMaterialDto,IFormFile file)
        {
            if (file == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "file is required"
                };
            }
            var pathFolder = $"UploadedFiles/Materials/{addMaterialDto.GroupId}";
            var filePath = await _imageStorage.SaveFile(file, pathFolder);
            if (filePath == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to add material.",
                };
            }
            var newMaterial = new Material
            { 
                Title = addMaterialDto.Title,
                Description = addMaterialDto.Description,
                File = filePath,
                Type = addMaterialDto.Type,
                GroupId = addMaterialDto.GroupId,
            };
            await _unitOfWork.GetRepository<Material>().AddAsync(newMaterial);
            var result = await _unitOfWork.SaveChanges();
            if (result == 0)
            {
               var isDeleted= _imageStorage.DeleteFile(filePath);
                if (!isDeleted )
                {
                   return new GeneralResponseDto
                    {
                        IsSuccess = false,
                        message = "Failed to add material with error in delete saved file.",
                    };
                }
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
            else
            {
                var isDeleted = _imageStorage.DeleteFile(material.File);
                if (!isDeleted)
                {
                    return new GeneralResponseDto
                    {
                        IsSuccess = false,
                        message = "Failed to delete material.",
                    };
                }
            }
                return new GeneralResponseDto
                {
                    IsSuccess = true,
                    message = "Material deleted successfully.",
                };

        }
    }
}
