using Dsw2025Tpi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderModel
    {
        public record OrderRequest(Guid CustomerId, string ShippingAddress, string BillingAddress, List<OrderItemModel> OrderItems);

        public record OrderResponse(Guid Id);
        
            
        
    }
}
