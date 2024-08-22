using System;

namespace BackendApp.Models
{
    public class TblMasterRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public AccountRole AccountRole { get; set; }
    }
}
