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

        public int? menu_MenuID      { get; set; }
    
        
        public string menu_Name      { get; set; }
        public string menu_Note      { get; set; }
        public decimal? menu_Price   { get; set; }
        public int? menu_Order       { get; set; }
        public int? menu_Kitchen1    { get; set; }
        public int? menu_Kitchen2    { get; set; }
        public int? menu_MenuGroupID { get; set; }
    }
}