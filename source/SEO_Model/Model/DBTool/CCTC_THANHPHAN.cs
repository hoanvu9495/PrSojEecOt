//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.DBTool
{
    using System;
    using System.Collections.Generic;
    
    public partial class CCTC_THANHPHAN
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public Nullable<System.DateTime> NGAYTAO { get; set; }
        public Nullable<int> NGUOITAO { get; set; }
        public Nullable<System.DateTime> NGAYSUA { get; set; }
        public Nullable<int> NGUOISUA { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public Nullable<int> PARENT_ID { get; set; }
        public int TYPE { get; set; }
        public Nullable<int> ITEM_LEVEL { get; set; }
        public string CODE { get; set; }
        public string EMAIL { get; set; }
        public string DIENTHOAI { get; set; }
    }
}
