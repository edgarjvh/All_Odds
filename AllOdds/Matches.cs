//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AllOdds
{
    using System;
    using System.Collections.Generic;
    
    public partial class Matches
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Matches()
        {
            this.Events = new HashSet<Events>();
            this.Odds = new HashSet<Odds>();
            this.TvStations = new HashSet<TvStations>();
            this.events = new List<Events>();
        }

        public List<Events> events { get; set; }
        public long match_id { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }
        public string stadium { get; set; }
        public Nullable<long> static_id { get; set; }
        public Nullable<long> fix_id { get; set; }
        public Nullable<long> category_id { get; set; }
        public Nullable<long> localteam_id { get; set; }
        public Nullable<long> visitorteam_id { get; set; }
        public string ht_score { get; set; }
        public string ft_score { get; set; }
        public Nullable<int> localteam_goals { get; set; }
        public Nullable<int> visitorteam_goals { get; set; }
    
        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Events> Events { get; set; }
        public virtual Teams Teams { get; set; }
        public virtual Teams Teams1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odds> Odds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TvStations> TvStations { get; set; }

        public string datetime { get; set; }
        public string category_name { get; set; }
        public string localteam_name { get; set; }
        public string visitorteam_name { get; set; }

    }
}