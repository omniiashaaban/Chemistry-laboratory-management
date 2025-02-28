﻿using laboratory.DAL.DTOs;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using LinkDev.Facial_Recognition.BLL.Helper.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace laboratory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly GenericRepository<Material> _materialRepository;

        public MaterialController(GenericRepository<Material> materialRepository)
        {
            _materialRepository = materialRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialDTO>>> GetMaterials()
        {
            var materials = await _materialRepository.GetAllAsync();
            var materialDTOs = materials.Select(MapToDTO).ToList();
            return Ok(materialDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialDTO>> GetMaterial(int id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null) return NotFound();
            return Ok(MapToDTO(material));
        }

        [HttpPost]
        public async Task<ActionResult> AddMaterial([FromBody] MaterialDTO materialDTO)
        {
            var material = MapToEntity(materialDTO);
            await _materialRepository.AddAsync(material);
            return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, MapToDTO(material));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMaterial(int id, [FromBody] MaterialDTO materialDTO)
        {
            if (id != materialDTO.Id)
                return BadRequest(new ApiResponse(400, "The provided ID does not match the material ID."));

            if (string.IsNullOrWhiteSpace(materialDTO.Name) ||
                string.IsNullOrWhiteSpace(materialDTO.Code) ||
                string.IsNullOrWhiteSpace(materialDTO.Type))
            {
                return BadRequest(new ApiResponse(400, "Name, Code, and Type are required fields."));
            }

            if (materialDTO.Quantity < 0)
                return BadRequest(new ApiResponse(400, "Quantity cannot be negative."));

            if (materialDTO.ProductionDate >= materialDTO.ExpirationDate)
                return BadRequest(new ApiResponse(400, "Production date must be before expiration date."));

            var existingMaterial = await _materialRepository.GetByIdAsync(id);
            if (existingMaterial == null)
                return NotFound(new ApiResponse(404, "Material not found."));

            existingMaterial.Name = materialDTO.Name;
            existingMaterial.Code = materialDTO.Code;
            existingMaterial.Type = materialDTO.Type;
            existingMaterial.Quantity = materialDTO.Quantity;
            existingMaterial.ProductionDate = materialDTO.ProductionDate;
            existingMaterial.ExpirationDate = materialDTO.ExpirationDate;

            await _materialRepository.UpdateAsync(existingMaterial);

            return Ok(new ApiResponse(200, "Material updated successfully."));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMaterial(int id)
        {
            var existingMaterial = await _materialRepository.GetByIdAsync(id);
            if (existingMaterial == null)
                return NotFound(new ApiResponse(404, "Material not found."));

            await _materialRepository.DeleteAsync(id);

            return Ok(new ApiResponse(200, "Material deleted successfully."));
        }


        // Manual mapping 
        private MaterialDTO MapToDTO(Material material)
        {
            return new MaterialDTO
            {
                Id = material.Id,
                Name = material.Name,
                Code = material.Code,
                Type = material.Type,
                Quantity = material.Quantity,
                ProductionDate = material.ProductionDate,
                ExpirationDate = material.ExpirationDate
            };
        }

        private Material MapToEntity(MaterialDTO materialDTO)
        {
            return new Material
            {
                Id = materialDTO.Id,
                Name = materialDTO.Name,
                Code = materialDTO.Code,
                Type = materialDTO.Type,
                Quantity = materialDTO.Quantity,
                ProductionDate = materialDTO.ProductionDate,
                ExpirationDate = materialDTO.ExpirationDate
            };
        }
    }
}
