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
    
    public partial class tblTable
    {
        public int tabl_TableID { get; set; }
        public Nullable<int> tabl_CreatedBy { get; set; }
        public Nullable<System.DateTime> tabl_CreatedDate { get; set; }
        public Nullable<int> tabl_UpdatedBy { get; set; }
        public Nullable<System.DateTime> tabl_UpdatedDate { get; set; }
        public string tabl_Name { get; set; }
        public string tabl_Note { get; set; }
        public Nullable<int> tabl_TableGroupID { get; set; }
        public Nullable<int> tabl_Deleted { get; set; }
    }
}
