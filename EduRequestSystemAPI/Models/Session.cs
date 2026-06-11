using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduRequestSystemAPI.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
