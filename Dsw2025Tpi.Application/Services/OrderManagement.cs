using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dsw2025Tpi.Application.Dtos.OrderModel;

namespace Dsw2025Tpi.Application.Services
{
    public class OrderManagement : IOrderManagement
    {
        private readonly IRepository _repository;
        public OrderManagement(IRepository repostory)
        {
            _repository = repostory;
        }

        public async Task<OrderModel.OrderResponse> AddOrder(OrderModel.OrderRequest request)
        {
            if (request.CustomerId == Guid.Empty ||
    string.IsNullOrWhiteSpace(request.ShippingAddress) ||
    string.IsNullOrWhiteSpace(request.BillingAddress) ||
    (request.OrderItems == null || !request.OrderItems.Any()))
            {
                throw new ArgumentException("Valores para el pedido no válidos");
            }

            //var exist = await _repository.First<Order>(o => o.Id == request.);
            //if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.InternalCode}");

            var orderItems = request.OrderItems.Select(item => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Name = item.Name,
                Description = item.Description,
                UnitPrice = item.CurrentUnitPrice,
                Quantity = item.Quantity
            }).ToList();

            var order = new Order(
            request.CustomerId,
            request.ShippingAddress,
            request.BillingAddress,
            orderItems,
            DateTime.Now
             );


            await _repository.Add(order);
            return new OrderResponse(order.Id);
        }

    }
}
