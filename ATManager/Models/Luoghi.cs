//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ATManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Luoghi
    {
        public string ID { get; set; }
        public string IDTipoLuogo { get; set; }
        public string IDNazione { get; set; }
        public string DescrITA { get; set; }
        public System.DateTime InsertDate { get; set; }
        public byte[] RowVersion { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Trilettera { get; set; }
        public string Provincia { get; set; }
        public string DescrITAestesa { get; set; }
        public Nullable<double> Latitudine { get; set; }
        public Nullable<double> Longitudine { get; set; }
        public Nullable<int> ID_Localita { get; set; }
    }
}