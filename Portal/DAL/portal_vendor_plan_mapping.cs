//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class portal_vendor_plan_mapping
    {
        public int pk_vendor_plan_mapping_id { get; set; }
        public Nullable<int> fk_vendor_id { get; set; }
        public Nullable<int> fk_vendor_plan_id { get; set; }
    
        public virtual portal_user portal_user { get; set; }
        public virtual portal_vendor_plan portal_vendor_plan { get; set; }
    }
}