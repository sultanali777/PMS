using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int buildingId { get; set; }
        public string floor { get; set; }
        public int propertyTypeId { get; set; }
        public string propertyNo { get; set; }
        public int statusId { get; set; }
        public DateTime date_Created
        {
            get
            {
                return this.dateCreated.HasValue
                   ? this.dateCreated.Value
                   : DateTime.Now;
            }

            set { this.dateCreated = value; }
        }
        private DateTime? dateCreated = null;
    }
}
