using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Authentication
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
