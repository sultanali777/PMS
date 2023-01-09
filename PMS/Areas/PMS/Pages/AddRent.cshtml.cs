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
using System.Collections;

namespace PMS.Areas.PMS
{

    public class AddRentModel : PageModel
    {
        private readonly ILogger<AddRentModel> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext Context { get; }
        private IWebHostEnvironment _hostingEnvironment;
        public AddRentModel(ILogger<AddRentModel> logger, 
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
        public int? rentalId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? guid { get; set; }

        public List<SelectListItem> Building { get; set; }
        public List<SelectListItem> Customer { get; set; }
        public List<SelectListItem> ProType { get; set; }
        public List<SelectListItem> ProStatus { get; set; }
        public List<string> FileNames { get; set; }
        public class commonModel
        {
            public int rentalId { get; set; }
            public int buildingId { get; set; }
            public string floor { get; set; }
            public int propertyTypeId { get; set; }
            public int propertyNo { get; set; }
            public int customerId { get; set; }
            public int propertyRent { get; set; }
            public string dateRange { get; set; }
            public DateTime endDate { get; set; }
            public string Description { get; set; }

        }
        public void OnGet()
        {
            var data = from c in this.Context.tbl_Building
                       join d in this.Context.tbl_Areas
                       on c.areaId equals d.Id
                       select new
                       {
                           Value = c.Id.ToString(),
                           Text = d.EnglishName + " - " + c.buildingno
                       };
            SelectList list = new SelectList(data, "Value", "Text");
            Building = list.ToList();
            ProType = this.Context.tbl_PropertyType.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.Id.ToString(),
                                     Text = a.Description
                                 }).ToList();
            Customer = this.Context.tbl_Customer.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.Id.ToString(),
                                     Text = a.fullName + " - " + a.passportNo.ToUpper()
                                 }).ToList();
            ProStatus = this.Context.tbl_Status.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Description
                                }).ToList();
            ProStatus.Find(c => c.Value == "2").Selected = true;
            if (rentalId != null)
            {

                //------- Loading Common Fields -----
                var common = (from cust in Context.tbl_RentalsDetails
                              where cust.Id == rentalId 
                              select new
                              {
                                  rentalId = cust.Id,
                                  buildingId = cust.buildingId,
                                  floor = cust.floor,
                                  propertyTypeId = cust.propertyTypeId,
                                  propertyNo = cust.propertyNo,
                                  customerId = cust.customerId,
                                  propertyRent = cust.propertyRent,
                                  startDate = cust.startDate.ToString("MM/dd/yyyy"),
                                  endDate = cust.endDate.ToString("MM/dd/yyyy"),
                                  Description = cust.Description,
                              }).ToList();

                if (common != null)
                {
                    Common.rentalId = common[0].rentalId;
                    Common.buildingId = common[0].buildingId;
                    Common.floor = common[0].floor;
                    Common.propertyTypeId = common[0].propertyTypeId;
                    Common.propertyNo = common[0].propertyNo;
                    Common.customerId = common[0].customerId;
                    Common.propertyRent = common[0].propertyRent;
                    Common.dateRange = common[0].startDate + " - " + common[0].endDate;
                    Common.Description = common[0].Description;
                }
                Building.Find(c => c.Value == Common.buildingId.ToString()).Selected = true;
                ProType.Find(c => c.Value == Common.propertyTypeId.ToString()).Selected = true;
                Customer.Find(c => c.Value == Common.customerId.ToString()).Selected = true;
            }
        }
        public IActionResult OnGetBuildingNo(int typeId, int BuidId, string floor,int updat)
        {
            if (BuidId != 0 && typeId != 0 && floor != "")
            {
                if (updat==0) {
                    IEnumerable<SelectListItem> BuildingNos = Context.tbl_Property.AsNoTracking()
                 .OrderBy(n => n.Id)
                 .Where(n => n.buildingId == BuidId && n.floor == floor && n.propertyTypeId == typeId && n.statusId == 2)
                 .Select(n =>
                     new SelectListItem
                     {
                         Value = n.Id.ToString(),
                         Text = n.propertyNo
                     }).ToList();
                    return new JsonResult(BuildingNos);
                } else {
                    IEnumerable<SelectListItem> BuildingNos = Context.tbl_Property.AsNoTracking()
               .OrderBy(n => n.Id)
               .Where(n => n.buildingId == BuidId && n.floor == floor && n.propertyTypeId == typeId)
               .Select(n =>
                   new SelectListItem
                   {
                       Value = n.Id.ToString(),
                       Text = n.propertyNo
                   }).ToList();
                    return new JsonResult(BuildingNos);
                }     

               
            }
            return null;
        }
        public IActionResult OnPost(IFormFile[] docContract)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    string guid = Guid.NewGuid().ToString("N").Substring(0, 5);
                    string _inputCheck = Request.Form["inputCheck"];
                    var userid = _userManager.GetUserAsync(User).Result.Id;
                    var userName = _userManager.GetUserAsync(User).Result.FirstName + " " + _userManager.GetUserAsync(User).Result.LastName;
                    string files = null;
                    DateTime startDate = new DateTime();
                    DateTime endDate = new DateTime();
                    int _buildingId = int.Parse(Request.Form["buildingId"]);
                    string floor = Request.Form["floor"];
                    int _typeId = int.Parse(Request.Form["typeId"]);
                    int _propertyNo = int.Parse(Request.Form["propertyNo"]);
                    int _customerId = int.Parse(Request.Form["customerId"]);
                    int _rent = int.Parse(Request.Form["rent"]);
                    string _dateRange = Request.Form["datrange"];
                    string _desc = Request.Form["desc"];
                    if (_dateRange != null)
                    {
                        string[] spearator = { " - " };
                        Int32 count = 2;
                        string[] dats = _dateRange.Split(spearator, count, StringSplitOptions.RemoveEmptyEntries);
                         startDate = DateTime.ParseExact(dats[0], "MM/dd/yyyy", null);
                         endDate = DateTime.ParseExact(dats[1], "MM/dd/yyyy", null);
                    }

                        


                    //----load Document if exist -----
                    if (docContract != null && docContract.Length > 0)
                    {
                        FileNames = new List<string>();
                        foreach (IFormFile photo in docContract)
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
                    if (_inputCheck == "0")//Insert
                    {
                        var rental = new RentalDetails();
                        {
                            rental.buildingId = _buildingId;
                            rental.floor = floor;
                            rental.propertyTypeId = _typeId;
                            rental.propertyNo = _propertyNo;
                            rental.customerId = _customerId;
                            rental.propertyRent = _rent;
                            rental.startDate = startDate;
                            rental.endDate = endDate;
                            rental.Description = _desc;
                            rental.attachments = files;
                            rental.guid = guid;
                            rental.userId = userid;
                        };
                        Context.tbl_RentalsDetails.Add(rental);
                        Context.SaveChanges();
                        int rentalId = rental.Id;
                        //generating rent collection plan
                        while (startDate < endDate)
                        {                          
                            var rentPlan = new rentCollection();
                            {
                                rentPlan.rentalId = rentalId;
                                rentPlan.monthRent = startDate;
                                rentPlan.propertyRent = _rent;
                            };
                            Context.tbl_rentCollection.Add(rentPlan);
                            Context.SaveChanges();
                            startDate = startDate.AddMonths(1);
                        }
                        
                        //Updating status
                        foreach (var pro in Context.tbl_Property.Where(x => x.Id == _propertyNo).ToList())
                        {
                            pro.statusId = 1;
                        }
                        Context.SaveChanges();
                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Property has been rented successfully.");
                    }
                    else {
                        foreach (var rental in Context.tbl_RentalsDetails.Where(x => x.Id == int.Parse(_inputCheck)).ToList())
                        { 
                            rental.buildingId = _buildingId;
                            rental.floor = floor;
                            rental.propertyTypeId = _typeId;
                            rental.propertyNo = _propertyNo;
                            rental.customerId = _customerId;
                            rental.propertyRent = _rent;
                            rental.startDate = startDate;
                            rental.endDate = endDate;
                            rental.Description = _desc;
                            rental.attachments = files;
                            rental.userId = userid;
                        }
                        Context.SaveChanges();
                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Rental Property has been updated successfully.");

                    }              
                    
                    
                }
                catch (Exception ex) { _logger.LogError(ex.Message); dbContextTransaction.Rollback(); }
            }
            return new RedirectToPageResult("/AddRent", new { area = "PMS" });
        }
  
        public JsonResult OnGetLoadData()
        {
            object data = "";
            try
            {
                var query = (from ren in Context.tbl_RentalsDetails
                             join bu in this.Context.tbl_Building on ren.buildingId equals bu.Id
                             join ar in Context.tbl_Areas on bu.areaId equals ar.Id
                             join pro in Context.tbl_Property on ren.propertyNo equals pro.Id
                             join st in Context.tbl_Status on pro.statusId equals st.Id
                             join ty in Context.tbl_PropertyType on pro.propertyTypeId equals ty.Id
                             join cust in Context.tbl_Customer on ren.customerId equals cust.Id
                             select new
                             {
                                 rentalId = ren.Id,
                                 building = ar.EnglishName + " - " + bu.buildingno,
                                 floor = pro.floor,
                                 type = ty.Description,
                                 status = st.Description,
                                 propertyNo = pro.propertyNo,
                                 rent=ren.propertyRent,
                                 startDate=ren.startDate,
                                 endDate=ren.endDate,
                                 customer=cust.fullName,
                                 desc=ren.Description,
                                 viewString=ren.Id+"&guid="+ren.guid
                             });
                
                data = query.OrderBy(x => x.rentalId).ToList();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return new JsonResult(data);
        }
    }
}
