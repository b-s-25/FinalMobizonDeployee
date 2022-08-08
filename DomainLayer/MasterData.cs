using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
 public class MasterData
{
         [Key]
        [Column("id", Order = 1)]
        public int id { get; set; }
        [Required]
        [Column("MasterData", Order = 2, TypeName = "nvarchar")]
        [MaxLength(150)]
        public string masterData { get; set; }
        [Column("PerentId", Order = 3)]
        public Master parentId { get; set; }
    }
}
