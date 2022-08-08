using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DomainLayer
{
    public class ForgotPassword
    {
        public int userId { get; set; }
        [Required(ErrorMessage = "*Email Address is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z\s]+\.[a-zA-Z\s.]+$", ErrorMessage = "*Email address should be in the format adc@domain.com")]
        [Display(Name = "*Registered Email Address")]
        public string email { get; set; }
        public bool emailSent { get; set; }
    }
}
