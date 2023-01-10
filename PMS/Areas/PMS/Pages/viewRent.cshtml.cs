using PMS.Data;
using PMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace PMS.Areas.PMS
{
    public class viewRentModel : PageModel
    {
        private readonly ILogger<viewRentModel> _logger;
        private ApplicationDbContext Context { get; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public viewRentModel(ILogger<viewRentModel> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext _context)
        {
            _logger = logger;
            this.Common = new commonModel();
            _userManager = userManager;
            _signInManager = signInManager;
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
            public int rent { get; set; }
            public string dateRange { get; set; }
            public string customerName { get; set; }
            public string rentDesc { get; set; }
            public string rentAttach { get; set; }
            public string custAttach { get; set; }
            public string nationality { get; set; }
            public string fullName { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }
            public long civilId { get; set; }
            public string companyName { get; set; }
            public string custAddress { get; set; }
            public string bulAddress { get; set; }
            public string[] custAttachments { get; set; }

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
                                      floor = pro.floor,
                                      type = ty.Description,
                                      propertyNo = pro.propertyNo,
                                      rent = ren.propertyRent,
                                      startDate = ren.startDate.ToString("dd/MM/yyyy"),
                                      endDate = ren.endDate.ToString("dd/MM/yyyy"),
                                      customerName = cust.fullName,
                                      rentDesc = ren.Description,
                                      rentAttach = ren.attachments,
                                      custAttach = cust.attachments,
                                      nationality = con.Description,
                                      fullName = cust.fullName,
                                      email = cust.email,
                                      mobile = cust.mobileNo,
                                      civilId = cust.CivilIdNo,
                                      companyName = cust.companyName.ToUpper(),
                                      custAddress = cust.address,
                                      bulAddress = bul.address,
                                  }).ToList();

                    if (common != null)
                    {
                        Common.rentalId = common[0].rentalId;
                        Common.Govern = common[0].Govern;
                        Common.area = common[0].area;
                        Common.floor = common[0].floor;
                        Common.type = common[0].type;
                        Common.propertyNo = common[0].propertyNo;
                        Common.rent = common[0].rent;
                        Common.dateRange = common[0].startDate + " - " + common[0].endDate;
                        Common.customerName = common[0].customerName;
                        Common.rentDesc = common[0].rentDesc;
                        Common.rentAttach = common[0].rentAttach;
                        Common.custAttach = common[0].custAttach;
                        Common.nationality = common[0].nationality;
                        Common.fullName = common[0].fullName;
                        Common.email = common[0].email;
                        Common.mobile = common[0].mobile;
                        Common.civilId = common[0].civilId;
                        Common.companyName = common[0].companyName;
                        Common.custAddress = common[0].custAddress;
                        Common.bulAddress = common[0].bulAddress;
                        char[] spearator = { ',' };
                        if (!(string.IsNullOrEmpty(Common.custAttach)))
                            Common.custAttachments = Common.custAttach.Split(spearator);
                        else
                            Common.custAttachments = null;
                    }
                }
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return Page();
        }

    }
}
