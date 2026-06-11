using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduRequestSystemAPI.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        [ForeignKey("Request")]
        public int RequestId { get; set; }
        public Request Request { get; set; }
    }
}
