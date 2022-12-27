using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public int governorateId { get; set; }
        public int areaId { get; set; }
        public long CivilIdNo { get; set; }
        public string address { get; set; }
        public string passportNo { get; set; }
        public string attachments { get; set; }
        public int nationalityId { get; set; }
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
