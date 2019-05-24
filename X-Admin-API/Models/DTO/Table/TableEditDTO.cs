using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_Admin_API.Models.DTO.Table
{
    public class TableEditDTO : TableBase
    {
        public Nullable<int> tabl_TableGroupID { get; set; }
    }
}