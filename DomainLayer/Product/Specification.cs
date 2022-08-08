using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DomainLayer
{
    public class Specification
{
        public int id { get; set; }
        [Display(Name = "Storage ")]
        [Required(ErrorMessage = "*This field is Required")]
        public string productStorage { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "*This field is Required")]
        public string productBrand { get; set; }

        [Display(Name = "Sim")]
        [Required(ErrorMessage = "*This field is Required")]
        public string productSim { get; set; }

        [Display(Name = "Color")]
        [Required(ErrorMessage = "*This field is Required")]
        public string productColor { get; set; }

        [Display(Name = "Ram")]
        [Required(ErrorMessage = "*This field is Required")]
        public string productRam { get; set; }

        [Display(Name = "Processor")]
        [Required(ErrorMessage = "*This field is Required")]
        public string productProcessor { get; set; }

        public DateTime modified_on { get; set; }
        public DateTime created_on { get; set; }
        public string modified_by { get; set; }
        public string created_by { get; set; }

    }
}
