//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLNHATHAU.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PCHN_HopDong
    {
        public int ID { get; set; }
        public Nullable<int> PhongBanID { get; set; }
        public Nullable<int> HopDongID { get; set; }
    
        public virtual HopDong HopDong { get; set; }
        public virtual PhongBan PhongBan { get; set; }
    }
}
