using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace PRS_GM.Models {
        [Index("PartNbr", IsUnique = true)]
    public class Product {
        public int ID { get; set; }
        [StringLength(30)]public string PartNbr { get; set; }
        [StringLength(30)]public string Name { get; set; }
        [Column(TypeName = "Decimal(11,2)")] public decimal Price { get; set; }
        [StringLength(30)]public string Unit { get; set; }
        [StringLength(255)]public string? PhotoPath { get; set; }
        public int VendorID { get; set; }
        public virtual Vendor? Vendor { get; set; }
    }
}
