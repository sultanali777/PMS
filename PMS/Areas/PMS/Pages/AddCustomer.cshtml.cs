using PMS.Data;
using PMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using NToastNotify;
using System.Numerics;
using System.Globalization;

namespace PMS.Areas.PMS
{
    public class AddCustomerModel : PageModel
    {
        private readonly ILogger<AddCustomerModel> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext Context { get; }

        private IWebHostEnvironment _hostingEnvironment;
        public AddCustomerModel(ILogger<AddCustomerModel> logger, 
            UserManager<ApplicationUser> userManager, ApplicationDbContext _context,
            IWebHostEnvironment hostingEnvironment,
             IToastNotification toastNotification)
        {
            _logger = logger;
            _userManager = userManager;
            this.Context = _context;
            _hostingEnvironment = hostingEnvironment;
            _toastNotification = toastNotification;
            this.Common = new commonModel();
        }
        [BindProperty]
        public commonModel Common { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? customerId { get; set; }
        public List<SelectListItem> Governorate { get; set; }
        public List<SelectListItem> Country { get; set; }
        public List<string> FileNames { get; set; }
        public class commonModel
        {
            public int govId { get; set; }
            public int areaId { get; set; }
            public string fname { get; set; }
            public string mobile { get; set; }
            public string email { get; set; }
            public long CivilId { get; set; }
            public string address { get; set; }
            public string companyName { get; set; }
            public string aaliNo { get; set; }
            public string business { get; set; }
            public int nationalityId { get; set; }
            public string guranfullName { get; set; }
            public string guranMobileNo { get; set; }
            public long guranCivilIdNo { get; set; }
            public string guranAddress { get; set; }
            public string guranAaliNo { get; set; }
        }
        public void OnGet()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            Governorate = this.Context.tbl_Governorates.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.Id.ToString(),
                                     Text = culture == "en" ? a.EnglishName : a.ArabicName
                                 }).ToList();
            Country = this.Context.tbl_Country.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = culture == "en" ? a.EnglishName : a.ArabicName
                                  }).ToList();
            if (customerId != null)
            {

                //------- Loading Common Fields -----
                var common = (from cust in Context.tbl_Customer
                              where cust.Id == customerId
                              select new
                              {
                                  govId = cust.governorateId,
                                  areaId = cust.areaId,
                                  fname = cust.fullName,
                                  mobile = cust.mobileNo,
                                  email = cust.email,
                                  CivilId = cust.CivilIdNo,
                                  address = cust.address,
                                  companyName = cust.companyName,
                                  business = cust.business,
                                  aaliNo = cust.aaliNo,
                                  nationalityId = cust.nationalityId,
                                  guranfullName = cust.guranfullName,
                                  guranMobileNo = cust.guranMobileNo,
                                  guranCivilIdNo = cust.guranCivilIdNo,
                                  guranAddress = cust.guranAddress,
                                  guranAaliNo = cust.guranAaliNo,
                                }).ToList();

                if (common != null)
                {
                    Common.govId = common[0].govId;
                    Common.areaId = common[0].areaId;
                    Common.fname = common[0].fname;
                    Common.mobile = common[0].mobile;
                    Common.email = common[0].email;
                    Common.CivilId = common[0].CivilId;
                    Common.address = common[0].address;
                    Common.companyName = common[0].companyName;
                    Common.aaliNo = common[0].aaliNo;
                    Common.business = common[0].business;
                    Common.nationalityId = common[0].nationalityId;

                    Common.guranfullName = common[0].guranfullName;
                    Common.guranMobileNo = common[0].guranMobileNo;
                    Common.guranCivilIdNo = common[0].guranCivilIdNo;
                    Common.guranAddress = common[0].guranAddress;
                    Common.guranAaliNo = common[0].guranAaliNo;
                }
               // Governorate.Find(c => c.Value == Common.govId.ToString()).Selected = true;
                Country.Find(c => c.Value == Common.nationalityId.ToString()).Selected = true;
            }


        }
        public IActionResult OnGetAddressAreas(int governorateId)
        {
            var culture = CultureInfo.CurrentCulture.Name;
            if (governorateId != 0)
            {
                IEnumerable<SelectListItem> addressAreas = Context.tbl_Areas.AsNoTracking()
                    .OrderBy(n => n.Id)
                    .Where(n => n.governorateId == governorateId)
                    .Select(n =>
                        new SelectListItem
                        {
                            Value = n.Id.ToString(),
                            Text = culture == "en" ? n.EnglishName : n.ArabicName
                        }).ToList();
                return new JsonResult(addressAreas);
            }
            return null;
        }
        public IActionResult OnPost(IFormFile[] pasPhoto)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    string _inputCheck = Request.Form["inputCheck"];

                    var userid = _userManager.GetUserAsync(User).Result.Id;
                    var userName = _userManager.GetUserAsync(User).Result.FirstName + " " + _userManager.GetUserAsync(User).Result.LastName;
                    string files = null;
                    int _governorateId = int.Parse(Request.Form["governorateId"]);
                    int _areaId = int.Parse(Request.Form["areaId"]);
                    int _countryId = int.Parse(Request.Form["countryId"]);
                    long _civilId = long.Parse(Request.Form["civilId"]);
                    string _email = Request.Form["email"];
                    string _fname = Request.Form["fname"];
                    string _mobile = Request.Form["mobile"];
                    string _companyName = Request.Form["companyName"];
                    string _address = Request.Form["address"];
                    string _business = Request.Form["business"];
                    string _aaliNo = Request.Form["aaliNo"];

                    long _guranId = long.Parse(Request.Form["guranId"]);
                    string _guranName = Request.Form["guranName"];
                    string _guranMobile = Request.Form["guranMobile"];
                    string _guranAddress = Request.Form["guranAddress"];
                    string _guranAaliNo = Request.Form["guranAaliNo"];


                    //----load photos if exist -----
                    if (pasPhoto != null && pasPhoto.Length > 0)
                    {
                        FileNames = new List<string>();
                        foreach (IFormFile photo in pasPhoto)
                        {
                            var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploaded_documents", photo.FileName);
                            var stream = new FileStream(path, FileMode.Create);
                            photo.CopyToAsync(stream);
                            FileNames.Add(photo.FileName);
                        }
                    }
                    if (FileNames != null)
                    {
                        files = string.Join(",", FileNames.ToArray());
                    }


                    var customerCheck = (from b in this.Context.tbl_Customer
                                         where b.CivilIdNo == _civilId
                                         select new { b.CivilIdNo }).FirstOrDefault();
                    if (_inputCheck == "0")//Insert
                    {
                        if (customerCheck != null)
                        {
                            _toastNotification.AddSuccessToastMessage($"Customer is already exist.");
                        }
                        else
                        {

                            var customer = new Customer();
                            {
                                customer.governorateId = _governorateId;
                                customer.areaId = _areaId;
                                customer.nationalityId = _countryId;
                                customer.CivilIdNo = _civilId;
                                customer.email = _email;
                                customer.fullName = _fname;
                                customer.mobileNo = _mobile;
                                customer.companyName = _companyName;
                                customer.business = _business;
                                customer.address = _address;
                                customer.aaliNo = _aaliNo;
                                customer.attachments = files;
                                customer.userId = userid;
                                customer.guranfullName = _guranName;
                                customer.guranMobileNo = _guranMobile;
                                customer.guranCivilIdNo = _guranId;
                                customer.guranAddress = _guranAddress;
                                customer.guranAaliNo = _guranAaliNo;

                            };
                            Context.tbl_Customer.Add(customer);
                            Context.SaveChanges();
                        }
                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Customer has been added successfully.");
                    }
                    else//Update
                    {
                        // Query the database for the row to be updated.                     
                       
                            foreach (var customer in Context.tbl_Customer.Where(x => x.CivilIdNo== long.Parse(_inputCheck)).ToList())
                            {
                            customer.governorateId = _governorateId;
                            customer.areaId = _areaId;
                            customer.nationalityId = _countryId;
                            customer.CivilIdNo = _civilId;
                            customer.email = _email;
                            customer.fullName = _fname;
                            customer.mobileNo = _mobile;
                            customer.companyName = _companyName;
                            customer.business = _business;
                            customer.aaliNo = _aaliNo;
                            customer.address = _address;
                            customer.attachments = files;
                            customer.userId = userid;
                            customer.guranfullName = _guranName;
                            customer.guranMobileNo = _guranMobile;
                            customer.guranCivilIdNo = _guranId;
                            customer.guranAddress = _guranAddress;
                            customer.guranAaliNo = _guranAaliNo;
                        }
                        Context.SaveChanges();
                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Customer has been updated successfully.");
                    }                  
                    
                }
                catch (Exception ex) { _logger.LogError(ex.Message); dbContextTransaction.Rollback(); }
            }
            return new RedirectToPageResult("/AddCustomer", new { area = "PMS" });
        }
        public JsonResult OnGetLoadData()
        {
            object data = "";
            var culture = CultureInfo.CurrentCulture.Name;
            try
            {
                var query = (from bu in Context.tbl_Customer
                             join ar in Context.tbl_Areas on bu.areaId equals ar.Id
                             join gov in Context.tbl_Governorates on bu.governorateId equals gov.Id
                             join con in Context.tbl_Country on bu.nationalityId equals con.Id
                             select new
                             {
                                 customerId = bu.Id,
                                 governorate = culture == "en" ? gov.EnglishName : gov.ArabicName,
                                 area = culture == "en" ? ar.EnglishName : ar.ArabicName,
                                 nationality = culture == "en" ? con.EnglishName : con.ArabicName,
                                 fullName = bu.fullName,
                                 email = bu.email,
                                 mobile = bu.mobileNo,
                                 civilId = bu.CivilIdNo,
                                 companyName = bu.companyName,
                                 address = bu.address,
                             });
                
                data = query.OrderByDescending(x => x.customerId).ToList();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return new JsonResult(data);
        }
    }
}
