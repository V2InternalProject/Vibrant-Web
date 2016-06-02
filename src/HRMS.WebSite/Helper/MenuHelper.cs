using System.Collections.Generic;

namespace HRMS.Helper
{
    public class Menu
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public List<Menu> SubMenus { get; set; }
    }

    public class MenuStructure
    {
        public List<Menu> ProcessStructure<T>(string idColumn, string[] menuHierarchyOrder, string[] UrlHierarchyOrder, List<T> source, List<Menu> menus) where T : new()
        {
            return null;
        }
    }
}