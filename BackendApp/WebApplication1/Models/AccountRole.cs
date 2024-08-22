using System;

namespace BackendApp.Models
{
    public class AccountRole
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public Account Account { get; set; }
        //public TblMasterRole TblMasterRole { get; set; }
        public ICollection<TblMasterRole> TblMasterRoles { get; set; }
    }
}
