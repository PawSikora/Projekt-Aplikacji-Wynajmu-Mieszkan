//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projekt_PBD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Oferta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Oferta()
        {
            this.Zainteresowanis = new HashSet<Zainteresowani>();
        }
    
        public int idO { get; set; }
        public Nullable<System.DateTime> dataWystawienia { get; set; }
        public Nullable<int> idM { get; set; }
        public string opis { get; set; }
        public Nullable<decimal> cenaZaMiesiac { get; set; }
        public string wyposazenie { get; set; }
        public Nullable<double> metraz { get; set; }
        public Nullable<bool> aktualne { get; set; }
    
        public virtual DaneMieszkania DaneMieszkania { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zainteresowani> Zainteresowanis { get; set; }

        public override string ToString()
        {
            return $"{DaneMieszkania.Miasto} {DaneMieszkania.Ulica} - {cenaZaMiesiac:C}";
        }
    }
}
