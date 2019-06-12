using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace X_Admin_API.Models.DTO.MenuGroup
{
    public class MenuGroupBaseDTO
    {
        public int? mnug_MenuGroupID { get; set; }

        [Required]
        public string mnug_Name { get; set; }
        public string mnug_Note { get; set; }
    }
}