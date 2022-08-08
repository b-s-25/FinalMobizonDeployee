using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
  public class Address
    {
        public int id { get; set; }
        [Required(ErrorMessage ="*This field is Required")]
        [Display(Name ="Name")]
        public string name { get; set; }
        [Required(ErrorMessage = "*This field is Required")]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required(ErrorMessage = "*This field is Required")]
        [Display(Name = "District")]
        public string district { get; set; }
        [Required(ErrorMessage = "*This field is Required")]
        [Display(Name = "State")]
        public string state { get; set; }
        [Required(ErrorMessage = "*This field is Required")]
        [Display(Name = "Country")]
        public string country { get; set; }
        [Required(ErrorMessage = "*This field is Required")]
        [Display(Name = "Pincode")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})$", ErrorMessage = "Entered pincode format is not valid.")]
        public string pincode { get; set; }
        [Required(ErrorMessage = "*This field is Required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }
        [Required(ErrorMessage = "*This field is Required")]
        [Display(Name = "Land Mark")]
        public string additionalInfo { get; set; }
        [NotMapped]
        public bool IsChecked { get; set; }
        [NotMapped]
        public int userId { get; set; }
    }
}
