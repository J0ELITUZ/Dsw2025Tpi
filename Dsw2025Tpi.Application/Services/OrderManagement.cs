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
using static Azure.Core.HttpHeader;
using static Dsw2025Tpi.Application.Dtos.OrderModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            // Validar que la lista de OrderItems no esté vacía
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
                throw new ArgumentException("La orden debe tener al menos un producto.");

            // Validar existencia del cliente
            var customer = await _repository.GetById<Customer>(request.CustomerId);
            if (customer == null)
                throw new ArgumentException("Cliente no encontrado.");

            // validar el stock de los productos
            foreach (var item in request.OrderItems)
            {
                var product = await _repository.GetById<Product>(item.ProductId);
                if (product == null)
                    throw new ArgumentException($"Producto con ID {item.ProductId} no encontrado.");
                if (product.StockCuantity < item.Quantity)
                    throw new ArgumentException($"No hay suficiente stock para el producto {product.Name}.");
            }
            //Restar el stock de los productos
            foreach (var item in request.OrderItems)
            {
                var product = await _repository.GetById<Product>(item.ProductId);
                if (product != null)
                {
                    product.StockCuantity -= item.Quantity;
                    await _repository.Update(product);
                }
            }

            // Crear la orden var order = new Order
            {

                var order = new Order
                {
                    CreatedAt = DateTime.UtcNow,
                    ShippingAddress = request.ShippingAddress,
                    BillingAddress = request.BillingAddress,

                    Status = OrderStatus.Pending,
                    CustomerId = customer.Id,
                    Customer = customer,
                    OrderItems = new List<OrderItem>()
                };
                decimal total = 0;


                foreach (var item in request.OrderItems)
                {
                    var product = await _repository.GetById<Product>(item.ProductId);
                    if (product == null) throw new ArgumentException("Producto no encontrado");


                    var subtotal = item.CurrentUnitPrice * item.Quantity;

                    var orderItem = new OrderItem
                    {

                        ProductId = product.Id,
                        Product = product,
                        Quantity = item.Quantity,
                        UnitPrice = item.CurrentUnitPrice,
                        Subtotal = subtotal
                    };
                    order.OrderItems.Add(orderItem);
                    total += subtotal;

                }
                order.TotalAmount = total;
                var added = await _repository.Add(order);

                return new OrderModel.OrderResponse(
                    added.Id,
                    added.CustomerId,
                    added.ShippingAddress,
                    added.BillingAddress,
                    added.CreatedAt,
                    added.TotalAmount,
                    added.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
                oi.ProductId,
                oi.Product?.Name ?? "",
                oi.Product?.Description ?? "",
                oi.UnitPrice,
                oi.Quantity,
                oi.Subtotal
            )).ToList(),
            added.Status.ToString()

                    );




            }
        }
    }
}
