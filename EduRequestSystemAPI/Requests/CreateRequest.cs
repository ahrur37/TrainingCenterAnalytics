namespace EduRequestSystemAPI.Requests
{
    public class CreateRequest
    {
        public string Topic { get; set; }
        public string Description { get; set; }
        public string? ContactInfo { get; set; }
        public int DirectionId { get; set; }
        public int TrainingFormatId { get; set; }
        public int AuthorId { get; set; }
    }
}
