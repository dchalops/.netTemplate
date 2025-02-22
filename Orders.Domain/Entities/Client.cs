using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Entities
{
    public class Client : BaseEntity
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public string name { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
