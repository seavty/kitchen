using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_Admin_API.Models.DTO.Menu
{
    public class MenuBaseDTO
    {
        public int? menu_MenuID { get; set; }

        public string menu_Name { get; set; }
        public string menu_Note { get; set; }

        public Nullable<decimal> menu_Price { get; set; }
        public Nullable<int> menu_Order { get; set; }
        public Nullable<int> menu_Kitchen1 { get; set; }
        public Nullable<int> menu_Kitchen2 { get; set; }
        public Nullable<int> menu_MenuGroupID { get; set; }
    }
}