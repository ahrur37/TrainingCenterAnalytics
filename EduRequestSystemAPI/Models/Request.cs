using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduRequestSystemAPI.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ContactInfo { get; set; }

        [ForeignKey("Direction")]
        public int DirectionId { get; set; }
        public Direction Direction { get; set; }

        [ForeignKey("TrainingFormat")]
        public int TrainingFormatId { get; set; }
        public TrainingFormat TrainingFormat { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User Author { get; set; }
        
        [ForeignKey("Assignee")]
        public int? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
    }
}
