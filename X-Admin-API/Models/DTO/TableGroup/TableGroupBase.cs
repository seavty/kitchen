using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace X_Admin_API.Models.DTO.TableGroup
{
    public class TableGroupBase
    {
        public int? tblg_TableGroupID { get; set; }

        [Required]
        public string tblg_Name { get; set; }
        public string tblg_Note { get; set; }
    }
}