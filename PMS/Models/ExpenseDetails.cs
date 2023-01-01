using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class ExpenseDetails
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int buildingId { get; set; }
        public string floor { get; set; }
        public int propertyTypeId { get; set; }
        public int propertyNo { get; set; }
        public int expenseAmount { get; set; }
        public string invoiceNo { get; set; }
        public int vendorId { get; set; }
        public string Description { get; set; }
        public string attachments { get; set; }
        public string guid { get; set; }
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
