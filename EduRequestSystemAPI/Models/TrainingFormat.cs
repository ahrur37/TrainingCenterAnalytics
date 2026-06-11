using System.ComponentModel.DataAnnotations;

namespace EduRequestSystemAPI.Models
{
    public class TrainingFormat
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
