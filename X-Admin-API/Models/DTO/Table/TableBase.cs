using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace X_Admin_API.Models.DTO.Table
{
    public abstract class TableBase
    {
        
        public int? tabl_TableID { get; set; }

        public string tabl_Name { get; set; }
        public string tabl_Note { get; set; }
    }
}