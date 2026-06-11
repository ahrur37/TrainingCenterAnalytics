using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduRequestSystemAPI.Models
{
    public class StatusHistory
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("OldStatus")]
        public int? OldStatusId { get; set; }
        public Status? OldStatus { get; set; }

        [ForeignKey("NewStatus")]
        public int NewStatusId { get; set; }
        public Status NewStatus { get; set; }

        [ForeignKey("Request")]
        public int RequestId { get; set; }
        public Request Request { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
