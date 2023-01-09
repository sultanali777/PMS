using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class rentCollection
    {
        public int Id { get; set; }
        public string userIdReceived { get; set; }
        public int rentalId { get; set; }
        public int propertyRent { get; set; }
        public DateTime monthRent { get; set; }
        public string Description { get; set; }
        public string receivedType { get; set; }
        public Nullable<DateTime> receivedDate { get; set; } 
        public bool receivedRent { get; set; } = false;
    }
}
