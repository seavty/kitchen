using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_Admin_API.Models.DTO.Menu
{
    public class MenuEditDTO: MenuBaseDTO
    {
        public int? menu_MenuGroupID { get; set; }
    }
}