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

            // Validar existencia del cliente
            var customerExists = await _repository.Exists<Customer>(c => c.Id == request.CustomerId);
            if (!customerExists)
            {
                throw new ArgumentException("El cliente especificado no existe.");
            }

            // Crear la orden primero
            var order = new Order(
                request.CustomerId,
                request.ShippingAddress,
                request.BillingAddress,
                new List<OrderItem>(),
                DateTime.Now
            );

            // Crear los OrderItems y asignar el OrderId
            var orderItems = request.OrderItems.Select(item => new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Name = item.Name,
                Description = item.Description,
                UnitPrice = item.CurrentUnitPrice,
                Quantity = item.Quantity
            }).ToList();

            // Calcular el total de la orden
            order.TotalAmount = orderItems.Sum(item => item.Subtotal);

            // Guardar la orden
            await _repository.Add(order);

            // Guardar los OrderItems
            foreach (var item in orderItems)
            {
                await _repository.Add(item);
            }

            return new OrderResponse(order.Id);
        }
    }
}
