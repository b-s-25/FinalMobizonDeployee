using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Orders
{
    public enum OrderStatus
    {
        [Display(Name ="Order Placed")]
        orderPlaced = 1,
        [Display(Name = "Dispatched")]
        dispatched,
        [Display(Name = "Delivered")]
        delivered,
        [Display(Name = "Cancelled")]
        cancelled
    }
}
