//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace X_Admin_API.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblMenu
    {
        public int menu_MenuID { get; set; }
        public Nullable<int> menu_CreatedBy { get; set; }
        public Nullable<System.DateTime> menu_CreatedDate { get; set; }
        public Nullable<int> menu_Deleted { get; set; }
        public string menu_Name { get; set; }
        public string menu_Note { get; set; }
        public Nullable<decimal> menu_Price { get; set; }
        public Nullable<int> menu_Order { get; set; }
        public Nullable<int> menu_Kitchen1 { get; set; }
        public Nullable<int> menu_Kitchen2 { get; set; }
        public Nullable<int> menu_MenuGroupID { get; set; }
    }
}
