using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class viewExpense
    {


            public int expenseId { get; set; }
            public string Govern { get; set; }
            public string area { get; set; }
            public string floor { get; set; }
            public string type { get; set; }
            public string propertyNo { get; set; }
            public string invoiceNo { get; set; }
            public int expenseAmount { get; set; }
            public string expenseDesc { get; set; }
            public string fullName { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }
            public string venType { get; set; }
            public long civilId { get; set; }
            public string companyName { get; set; }
            public string vendAddress { get; set; }
            public string[] expenseAttachments { get; set; }

        


    }
}
