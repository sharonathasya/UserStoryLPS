namespace FrontendApp.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateTime { get; set; }
        public string AccountId { get; set; }
        public IFormFile UploadFile { get; set; }


    }

}
