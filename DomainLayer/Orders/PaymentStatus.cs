using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static DomainLayer.Orders.PaymentStatus;

namespace DomainLayer.Orders
{
    public enum PaymentStatus : int
    {
        [Display(Name = "Cash on Delivery")]
        CashOnDelivery = 1
    }
    public class PaymentStatuses
    {
        public int id { get; set; }

        [Display(Name = "Cash on Delivery")]
        public PaymentStatus mode { get; set; }
        public bool isChecked { get; set; }
    }
}


