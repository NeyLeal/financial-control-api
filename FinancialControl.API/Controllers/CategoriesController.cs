using FinancialControl.Application.DTOs.CategoryDto;
using FinancialControl.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        public readonly CreateCategoryUseCase _create;
        public readonly GetCategoryUseCase _get;
        public readonly UpdateCategoryUseCase _update;
        public readonly DeleteCategoryUseCase _delete;

        public CategoriesController(
            CreateCategoryUseCase create, 
            GetCategoryUseCase get, 
            UpdateCategoryUseCase update, 
            DeleteCategoryUseCase delete)
        {
            _create = create;
            _get = get;
            _update = update;
            _delete = delete;
        }
        #region Get
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId =
               Guid.Parse(
                   User.FindFirst(ClaimTypes.NameIdentifier)!
                   .Value);

            var categories = await _get.ExecutAsync(userId);

            return Ok(categories);
        }
        #endregion
        #region Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var userId =
               Guid.Parse(
                   User.FindFirst(ClaimTypes.NameIdentifier)!
                   .Value);
            var category = await _create.ExecuteAsync(userId, dto.Name);

            return Ok(category);  
        }
        #endregion
        #region Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCategoryDto dto)
        {
            var userId =
               Guid.Parse(
                   User.FindFirst(ClaimTypes.NameIdentifier)!
                   .Value);
            var update = await _update.ExecuteAsync(id, userId, dto.Name);

            if (!update)
                return NotFound();

            return NoContent();
        }
        #endregion
        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId =
               Guid.Parse(
                   User.FindFirst(ClaimTypes.NameIdentifier)!
                   .Value);
            var deleted = await _delete.ExecuteAsync(id, userId);    

            if(!deleted)
                return NotFound();

            return NoContent();
        }
        #endregion
    }
}
