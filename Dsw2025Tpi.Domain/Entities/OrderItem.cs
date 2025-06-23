using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        //Forean Key Product
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        //Forean Key Order
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        //public OrderItem(Guid ProductIdp, int QuantityProduct, string ProductName, string ProductDescription, decimal CurrentPriceProduct  )
        //{
        //    Product.CurrentUnitPrice = CurrentPriceProduct;
        //    Product.Name = ProductName;
        //    Product.Description = ProductDescription;
        //    Quantity = QuantityProduct;
        //    ProductId = ProductIdp;


        //}
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
       // public decimal Subtotal => Quantity * UnitPrice;

        
    }
}
