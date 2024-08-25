namespace BackendApp.ViewModels
{
    public class ResponseVM
    {
    }

    public class ResLogin
    {
        public string TOKEN { get; set; }
    }
    public class ResDataAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateTime  { get; set; }
        public string AccountId { get; set; }

    }

}
