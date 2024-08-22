using System;

namespace BackendApp.Models
{
    public class TblAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Guid { get; set; }
        public int? CreatedById { get; set; }
        public int? DeletedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public bool? IsDeleted { get; set; }
        public int? AccountId { get; set; }
    }
}
