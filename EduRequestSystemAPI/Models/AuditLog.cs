using EduRequestSystemAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduRequestSystemAPI.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public AuditAction Action { get; set; }
        public AuditEntity EntityName { get; set; }
        public int? EntityId { get; set; } 
        public string? Details { get; set; } 
        public DateTime Timestamp { get; set; } 

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
