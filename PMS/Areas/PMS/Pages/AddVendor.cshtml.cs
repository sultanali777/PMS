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
    public class AddVendorModel : PageModel
    {
        private readonly ILogger<AddVendorModel> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext Context { get; }

        private IWebHostEnvironment _hostingEnvironment;
        public AddVendorModel(ILogger<AddVendorModel> logger, 
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
        public int? vendorId { get; set; }
        public List<SelectListItem> Governorate { get; set; }
        public List<SelectListItem> VendorType { get; set; }
        public List<string> FileNames { get; set; }
        public class commonModel
        {
            public int Id { get; set; }
            public string fullName { get; set; }
            public string companyName { get; set; }
            public string mobileNo { get; set; }
            public string email { get; set; }
            public int governorateId { get; set; }
            public int areaId { get; set; }
            public long CivilIdNo { get; set; }
            public string address { get; set; }
            public int vendorTypeId { get; set; }

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
            VendorType = this.Context.tbl_VendorType.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = culture == "en" ? a.EnglishName : a.ArabicName
                                  }).ToList();
            if (vendorId != null)
            {

                //------- Loading Common Fields -----
                var common = (from cust in Context.tbl_Vendor
                              where cust.Id == vendorId
                              select new
                              {
                                  Id=cust.Id,
                                  governorateId = cust.governorateId,
                                  areaId = cust.areaId,
                                  fullName = cust.fullName,
                                  mobileNo = cust.mobileNo,
                                  email = cust.email,
                                  CivilIdNo = cust.CivilIdNo,
                                  address = cust.address,
                                  vendorTypeId = cust.vendorTypeId,
                                  companyName = cust.companyName,
                                 }).ToList();

                if (common != null)
                {
                    Common.Id = common[0].Id;
                    Common.fullName = common[0].fullName;
                    Common.companyName = common[0].companyName;
                    Common.mobileNo = common[0].mobileNo;
                    Common.email = common[0].email;
                    Common.governorateId = common[0].governorateId;
                    Common.areaId = common[0].areaId;
                    Common.CivilIdNo = common[0].CivilIdNo;
                    Common.address = common[0].address;
                    Common.vendorTypeId = common[0].vendorTypeId;
                }
                VendorType.Find(c => c.Value == Common.vendorTypeId.ToString()).Selected = true;
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
                    int _vendorTypeId = int.Parse(Request.Form["vendorTypeId"]);
                    string _companyName = Request.Form["companyName"];
                    long _CivilIdNo = long.Parse(Request.Form["CivilIdNo"]);
                    string _email = Request.Form["email"];
                    string _fullName = Request.Form["fullName"];
                    string _mobileNo = Request.Form["mobileNo"];
                    string _address = Request.Form["address"];

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

                    var vendorCheck = (from b in this.Context.tbl_Vendor
                                         where b.CivilIdNo == _CivilIdNo
                                       select new { b.CivilIdNo }).FirstOrDefault();
                    if (_inputCheck == "0")//Insert
                    {
                        if (vendorCheck != null)
                        {
                            _toastNotification.AddSuccessToastMessage($"Vendor is already exist.");
                        }
                        else
                        {

                            var vendor = new Vendor();
                            {
                                vendor.governorateId = _governorateId;
                                vendor.areaId = _areaId;
                                vendor.vendorTypeId = _vendorTypeId;
                                vendor.CivilIdNo = _CivilIdNo;
                                vendor.email = _email;
                                vendor.companyName = _companyName;
                                vendor.fullName = _fullName;
                                vendor.mobileNo = _mobileNo;
                                vendor.address = _address;
                                vendor.userId = userid;
                                vendor.attachments = files;
                            };
                            Context.tbl_Vendor.Add(vendor);
                            Context.SaveChanges();
                        }
                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Vendor has been added successfully.");
                    }
                    else//Update
                    {
                        // Query the database for the row to be updated.                     
                       
                            foreach (var vendor in Context.tbl_Vendor.Where(x => x.Id== int.Parse(_inputCheck)).ToList())
                            {
                            vendor.governorateId = _governorateId;
                            vendor.areaId = _areaId;
                            vendor.vendorTypeId = _vendorTypeId;
                            vendor.CivilIdNo = _CivilIdNo;
                            vendor.email = _email;
                            vendor.companyName = _companyName;
                            vendor.fullName = _fullName;
                            vendor.mobileNo = _mobileNo;
                            vendor.address = _address;
                            vendor.userId = userid;
                            vendor.attachments = files;
                        }
                        Context.SaveChanges();
                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Vendor has been updated successfully.");
                    }                  
                    
                }
                catch (Exception ex) { _logger.LogError(ex.Message); dbContextTransaction.Rollback(); }
            }
            return new RedirectToPageResult("/AddVendor", new { area = "PMS" });
        }
        public JsonResult OnGetLoadData()
        {
            object data = "";
            var culture = CultureInfo.CurrentCulture.Name;
            try
            {
                var query = (from bu in Context.tbl_Vendor
                             join ar in Context.tbl_Areas on bu.areaId equals ar.Id
                             join gov in Context.tbl_Governorates on bu.governorateId equals gov.Id
                             join typ in Context.tbl_VendorType on bu.vendorTypeId equals typ.Id
                             select new
                             {
                                 vendorId = bu.Id,
                                 governorate = culture == "en" ? gov.EnglishName : gov.ArabicName,
                                 area = ar.EnglishName,
                                 fullName = bu.fullName,
                                 email = bu.email,
                                 mobile = bu.mobileNo,
                                 civilId = bu.CivilIdNo,
                                 companyName = bu.companyName,
                                 type= culture == "en" ? typ.EnglishName : typ.ArabicName,
                                 address = bu.address,
                             });
                
                data = query.OrderByDescending(x => x.vendorId).ToList();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return new JsonResult(data);
        }
    }
}
