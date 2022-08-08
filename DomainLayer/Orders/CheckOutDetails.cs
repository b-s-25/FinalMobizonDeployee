using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Orders
{
    public class CheckOutDetails
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public RoleType? cancelRequested { get; set; }
        public OrderStatus status { get; set; }
    }
}
