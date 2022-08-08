using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DomainLayer.AdminSettings
{
    public class PrivacyAndPolicy
    {
        [Key]
        public int id { get; set; }

        [MaxLength(500)]
        [Required(ErrorMessage = "*This field is required")]
        [Display(Name = "Content")]
        public string content { get; set; }
    }
}
