using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PMS.Data;

namespace PMS.Areas.PMS.Pages
{
    public class printContractModel : PageModel
    {
        private readonly ILogger<printContractModel> _logger;
        private ApplicationDbContext Context { get; }
        public printContractModel(ILogger<printContractModel> logger, ApplicationDbContext _context)
        {
            _logger = logger;
            this.Common = new commonModel();
            this.Context = _context;
        }
        [BindProperty]
        public commonModel Common { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? rentalId { get; set; }
        public class commonModel
        {
            public int rentalId { get; set; }
            public string Govern { get; set; }
            public string area { get; set; }
            public string floor { get; set; }
            public string type { get; set; }
            public string propertyNo { get; set; }
            public string buildName { get; set; }
            public string ownName { get; set; }
            public int rent { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
            public string customerName { get; set; }
            public string rentDesc { get; set; }
            public string nationality { get; set; }
            public string fullName { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }
            public long civilId { get; set; }
            public string passportNo { get; set; }
            public string custAddress { get; set; }
            public string bulAddress { get; set; }
            public string dated { get; set; }

        }
        public async Task<IActionResult> OnGetAsync()
        {

            try
            {
                if (rentalId != null)
                {

                    //------- Loading Common Fields -----
                    var common = (from ren in Context.tbl_RentalsDetails
                                  join cust in Context.tbl_Customer on ren.customerId equals cust.Id
                                  join con in Context.tbl_Country on cust.nationalityId equals con.Id
                                  join pro in Context.tbl_Property on ren.propertyNo equals pro.Id
                                  join bul in Context.tbl_Building on ren.buildingId equals bul.Id
                                  join gov in Context.tbl_Governorates on bul.governorateId equals gov.Id
                                  join ar in Context.tbl_Areas on bul.areaId equals ar.Id
                                  join ty in Context.tbl_PropertyType on pro.propertyTypeId equals ty.Id
                                  where ren.Id == rentalId

                                  select new
                                  {
                                      rentalId = ren.Id,
                                      Govern = gov.Description,
                                      area = ar.EnglishName,
                                      buildName=bul.buildingName,
                                      ownName=bul.ownerName,
                                      floor = pro.floor,
                                      type = ty.Description,
                                      propertyNo = pro.propertyNo,
                                      rent = ren.propertyRent,
                                      startDate = ren.startDate.ToString("dd/MM/yyyy"),
                                      endDate = ren.endDate.ToString("dd/MM/yyyy"),
                                      customerName = cust.fullName,
                                      rentDesc = ren.Description,
                                      dated = ren.date_Created.ToString("dd/MM/yyyy"),
                                      nationality = con.Description,
                                      fullName = cust.fullName,
                                      email = cust.email,
                                      mobile = cust.mobileNo,
                                      civilId = cust.CivilIdNo,
                                      passportNo = cust.passportNo.ToUpper(),
                                      custAddress = cust.address,
                                      bulAddress = bul.address,
                                  }).ToList();

                    if (common != null)
                    {
                        Common.rentalId = common[0].rentalId;
                        Common.Govern = common[0].Govern;
                        Common.area = common[0].area;
                        Common.floor = "FLoor: "+common[0].floor;
                        Common.type = "Type: " + common[0].type;
                        Common.ownName = common[0].ownName;
                        Common.propertyNo = "No: " + common[0].propertyNo;
                        Common.buildName = common[0].buildName;
                        Common.rent = common[0].rent;
                        Common.startDate = common[0].startDate;
                        Common.endDate = common[0].endDate;
                        Common.customerName = common[0].customerName;
                        Common.rentDesc = common[0].rentDesc;
                        Common.dated = common[0].dated;
                        Common.nationality = "Nationality: " + common[0].nationality;
                        Common.fullName = common[0].fullName;
                        Common.email = common[0].email;
                        Common.mobile = common[0].mobile;
                        Common.civilId = common[0].civilId;
                        Common.passportNo = common[0].passportNo;
                        Common.custAddress = common[0].custAddress;
                        Common.bulAddress = common[0].bulAddress;
                    }
                }
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return Page();
        }
    }
}
