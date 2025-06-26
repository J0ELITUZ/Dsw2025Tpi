using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {

        private IProductsManagementService _productsManagmentService;

        public ProductsController(IProductsManagementService productsManagementService)
        {

            _productsManagmentService = productsManagementService;
        }

        //Agregar un producto
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel.ProductRequest request)
        {
            try
            {
                var product = await _productsManagmentService.AddProduct(request);
                return Created("Products", product

        );
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (DuplicatedEntityException de)
            {
                return Conflict(de.Message);
            }
            catch (Exception)
            {
                return Problem("Se produjo un error al guardar el producto");
            }
        }

        // Obtener todos los productos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productsManagmentService.GetProducts();

            if (!products.Any())
                return NoContent(); // 204

            return Ok(products);  //200
        }

        // Obtener un producto por ID
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productsManagmentService.GetProductById(id);

            if (product is null)
                return NotFound(); // 404

            return Ok(product); // 200
        }

        //deshabilitar un producto
        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> Disable(Guid id)
        {
            var success = await _productsManagmentService.DisableProductAsync(id);

            if (!success)
                return NotFound();

            return NoContent(); // 204
        }

        // Actualizar un producto por ID
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductModel.ProductRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Sku) ||
                string.IsNullOrWhiteSpace(request.InternalCode) ||
                string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest();
            }

            try
            {
                var response = await _productsManagmentService.UpdateAsync(request, id);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
