namespace BackendApp.ViewModels
{
    public class RequestVM
    {
    }
    public class ReqLogin
    {
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
    }

    public class ReqAddUser
    {
        public int? Userid { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public DateTime? RegistDate { get; set; }

    }
    public class ReqIdUser
    {
        public string id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class ReqFile
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string CreatedById { get; set; }
        public string CreatedDateTime { get; set; }
        public int AccountId { get; set; }

    }
}
