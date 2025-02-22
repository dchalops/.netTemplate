using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Orders.Domain.Entities
{
    public class Order : BaseEntity
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid ClientID { get; set; }

        [ForeignKey("ClientID")]
        public Client Client { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}

