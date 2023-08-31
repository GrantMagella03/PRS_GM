using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PRS_GM.Models {
    [Index("Username", IsUnique = true)]
    public class User {
        [Key] public int ID { get; set; }
        [StringLength(30)] public string Username { get; set; }
        [StringLength(30)] public string Password { get; set; }
        [StringLength (30)] public string Firstname { get; set; }
        [StringLength(30)] public string Lastname { get; set; }
        [StringLength(12)] public string? Phone { get; set; }
        [StringLength(255)] public string? Email { get; set; }
        public bool isReviewer { get; set; }
        public bool isAdmin { get; set; }
    }
}
