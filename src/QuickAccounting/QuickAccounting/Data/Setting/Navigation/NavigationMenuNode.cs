namespace QuickAccounting.Data.Setting.Navigation
{
    public class NavigationMenuNode
    {
        public class MenuGroupNode
        {
            public int MenuGroupId { get; set; }

            public string MenuGroupName { get; set; }

            public string? IconName { get; set; }

            public List<MainMenuNode> MainMenus { get; set; } = new();

            public class MainMenuNode
            {
                public int MainMenuId { get; set; }

                public string MainMenuName { get; set; }

                public string Url { get; set; }

                public string? IconName { get; set; }

                public List<SubMenuNode> SubMenus { get; set; } = new();

                public class SubMenuNode
                {
                    public int SubMenuId { get; set; }

                    public string SubMenuName { get; set; }

                    public string Url { get; set; }

                    public string? IconName { get; set; }
                }
            }
        }
    }
}
