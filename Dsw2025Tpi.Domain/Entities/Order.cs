using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Order : EntityBase
    {

        Guid _customer;
        List<OrderItem> _orderItems;

        public Order(string shippingAddress, string billingAddress, DateTime createdAt)
        {
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Id = Guid.NewGuid();
            CreatedAt = createdAt;


        }

        public Order(Guid customerId, string shippingAddress, string billingAddress,List<OrderItem> orderItems, DateTime createdAt)
        {
            _customer = customerId;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            _orderItems = orderItems;
            Id = Guid.NewGuid();
            CreatedAt = createdAt;
        }

        public Customer Customer { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

    }
}
