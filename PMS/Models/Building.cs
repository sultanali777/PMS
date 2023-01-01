using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class Building
    {

        public int Id { get; set; }
        public string userId { get; set; }
        public string buildingno { get; set; }
        public int governorateId { get; set; }
        public int areaId { get; set; }
        public string address { get; set; }
        public string buildingName { get; set; }
        public string ownerName { get; set; }
        public string ownerMobile { get; set; }
        public string ownerAddress { get; set; }
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
