using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRS_GM.Models {
    public class Request {
        [Key] public int ID { get; set; }
        [StringLength(80)] public string Description { get; set; }
        [StringLength(80)] public string Justification { get; set; }
        [StringLength(80)] public string? RejectionReason { get; set; }
        [StringLength(20)] public string DeliveryMode { get; set; } = "Pickup";
        [StringLength(10)] public string Status { get; set; } = "NEW";
        [Column(TypeName = "Decimal(11,2)")] public decimal Total { get; set; } = 0;
        public int UserID { get; set; }
        public virtual User? Users { get; set; }
    }
}
