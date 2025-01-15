namespace QuickAccounting.Data.Setting.SystemUser
{
    public class UserPrivilegeView
    {
        public int UserPrivilegeId { get; set; }

        public int UserRoleId { get; set; }

        public int NavigationMenuId { get; set; }

        public string MenuGroupName { get; set; }

        public string MainMenuName { get; set; }

        public string SubMenuName { get; set; }

        public bool? CanView { get; set; }

        public bool? CanAdd { get; set; }

        public bool? CanEdit { get; set; }

        public bool? CanDelete { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? Active { get; set; }
    }
}
