using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dsw2025Tpi.Api.Controllers
{       [ApiController]
        [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private IOrderManagement _orderManagmentService;
        public OrderController(IOrderManagement orderManagement)
        {
            _orderManagmentService = orderManagement;
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody]OrderModel.OrderRequest request)
        {
            try
            {
                var order = await _orderManagmentService.AddOrder(request);
                return Ok(order
            
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


    }
}
