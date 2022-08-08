using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.AdminSettings
{
    public class About
    {
        [Key]
        [Column("id", Order = 1)]
        public int id { get; set; }
        [Column("content", TypeName = "nvarchar", Order = 2)]
        [MaxLength(500)]
        [Required(ErrorMessage = "*This Field is Required")]
        public string content { get; set; }
    }
}
