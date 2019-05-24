using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X_Admin_API.Models.DTO.TableGroup;

namespace X_Admin_API.Models.DTO.Table
{
    public class TableViewDTO : TableBase
    {
        public TableGroupViewDTO tableGroup { get; set; }
    }
}