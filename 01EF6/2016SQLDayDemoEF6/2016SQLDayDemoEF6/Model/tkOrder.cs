namespace _2016SQLDayDemoEF6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TK.tkOrders")]
    public partial class tkOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tkOrder()
        {
            tkOrderToArticles = new HashSet<tkOrderToArticle>();
        }

        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DtAdd { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Ts { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        public string Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tkOrderToArticle> tkOrderToArticles { get; set; }
    }
}
