using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X_Admin_API.Models.DTO.MenuGroup;

namespace X_Admin_API.Models.DTO.Menu
{
    public class MenuViewDTO: MenuBaseDTO
    {
        public MenuGroupViewDTO menuGroup { get; set; }
    }
}