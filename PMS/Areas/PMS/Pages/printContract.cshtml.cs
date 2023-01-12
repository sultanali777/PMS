using System;
using System.Collections.Generic;
using System.Globalization;
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
            public string propertyAaliNo { get; set; }
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
            public string companyName { get; set; }
            public string custAddress { get; set; }
            public string custAaliNo { get; set; }
            public string bulAddress { get; set; }
            public string dated { get; set; }
            public string guranfullName { get; set; }
            public string guranMobileNo { get; set; }
            public long guranCivilIdNo { get; set; }
            public string guranAddress { get; set; }
            public string guranAaliNo { get; set; }
            public string custCompName { get; set; }
            public string custBusiness { get; set; }
            public string contractDate { get; set; }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var culture = CultureInfo.CurrentCulture.Name;
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
                                      Govern = culture == "en" ? gov.EnglishName : gov.ArabicName,
                                      area = culture == "en" ? ar.EnglishName : ar.ArabicName,
                                      buildName=bul.buildingName,
                                      ownName=bul.ownerName,
                                      floor = pro.floor,
                                      type = culture == "en" ? ty.EnglishName : ty.ArabicName,
                                      propertyNo = pro.propertyNo,
                                      rent = ren.propertyRent,
                                      startDate = ren.startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                      endDate = ren.endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                      contractDate= ren.date_Created.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                      customerName = cust.fullName,
                                      rentDesc = ren.Description,
                                      dated = ren.date_Created.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                      nationality = culture == "en" ? con.EnglishName : con.ArabicName,
                                      fullName = cust.fullName,
                                      email = cust.email,
                                      mobile = cust.mobileNo,
                                      civilId = cust.CivilIdNo,
                                      companyName = cust.companyName.ToUpper(),
                                      custAddress = cust.address,
                                      bulAddress = bul.address,
                                      propertyAaliNo=pro.aaliNo,
                                      custAaliNo=cust.aaliNo,
                                      custCompName=cust.companyName,
                                      guranfullName =cust.guranfullName,
                                      guranMobileNo=cust.guranMobileNo,
                                      guranCivilIdNo=cust.guranCivilIdNo,
                                      guranAddress=cust.guranAddress,
                                      guranAaliNo=cust.guranAaliNo,
                                      custBusiness=cust.business,
                                  }).ToList();

                    if (common != null)
                    {
                        Common.rentalId = common[0].rentalId;
                        Common.Govern = common[0].Govern;
                        Common.area = common[0].area;
                        Common.floor = culture == "en" ? "FLoor: " + common[0].floor : "أرضية: " + common[0].floor ;  
                        Common.type = culture == "en" ? "Type: " + common[0].type : "يكتب: " + common[0].type ;
                        Common.ownName = common[0].ownName;
                        Common.propertyNo =  common[0].propertyNo;
                        Common.buildName = common[0].buildName;
                        Common.rent = common[0].rent;
                        Common.startDate = common[0].startDate;
                        Common.endDate = common[0].endDate;
                        Common.customerName = common[0].customerName;
                        Common.rentDesc = common[0].rentDesc;
                        Common.dated = common[0].dated;
                        Common.nationality = culture == "en" ? "Nationality: " + common[0].nationality : "جنسية: " + common[0].nationality ; 
                        Common.fullName = common[0].fullName;
                        Common.email = common[0].email;
                        Common.mobile = common[0].mobile;
                        Common.civilId = common[0].civilId;
                        Common.companyName = common[0].companyName;
                        Common.custAddress = common[0].custAddress;
                        Common.bulAddress = common[0].bulAddress;
                        Common.propertyAaliNo = common[0].propertyAaliNo;
                        Common.custAaliNo = common[0].custAaliNo;
                        Common.guranfullName = common[0].guranfullName;
                        Common.guranMobileNo = common[0].guranMobileNo;
                        Common.guranCivilIdNo = common[0].guranCivilIdNo;
                        Common.guranAddress = common[0].guranAddress;
                        Common.guranAaliNo = common[0].guranAaliNo;
                        Common.custCompName = common[0].custCompName;
                        Common.custBusiness = common[0].custBusiness;
                        Common.contractDate = common[0].contractDate;
                    }
                }
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return Page();
        }
    }
}
