using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Product
{
    [Table("ProductsModel")]
    public class ProductsModel
    {
        [Key]
        public int id { get; set; }

        public int specificationId { get; set; }
        public Specification specification { get; set; }

        [Display(Name = "Name ")]
        [Required(ErrorMessage = "*This field is Required")]
        public string productName { get; set; }
        [Display(Name = "Model")]
        [Required(ErrorMessage = "*This field is Required")]
        public string productModel { get; set; }
        [Display(Name = "Price")]
        [Required(ErrorMessage = "*This field is Required")]
        public int productPrice { get; set; }
        public string image { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "*This field is Required")]
        public string description { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "*This field is Required")]
        public int quantity { get; set; }
        public Status productStatus { get; set; }
        [Display(Name = "IsActive")]
        public bool isActive { get; set; }
        [Display(Name = "Modified By")]
        public string modifiedBy { get; set; }
        [Display(Name = "Modified On")]
        public DateTime modifiedOn { get; set; }
        [Display(Name = "Created On")]
        public DateTime createdOn { get; set; }
        [Display(Name = "Created By")]
        public string createdBy { get; set; }
        [Display(Name = "Deleted By")]
        public string deletedBy { get; set; }
    }
}
