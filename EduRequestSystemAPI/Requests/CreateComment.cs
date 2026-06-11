using EduRequestSystemAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduRequestSystemAPI.Requests
{
    public class CreateComment
    {
        public string Content { get; set; }
        public int RequestId { get; set; }
        public int AuthorId { get; set; }
    }
}
