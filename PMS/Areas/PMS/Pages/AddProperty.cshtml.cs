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

 

    public class AddPropertyModel : PageModel
    {
        private readonly ILogger<AddPropertyModel> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext Context { get; }

        private IWebHostEnvironment _hostingEnvironment;
        public AddPropertyModel(ILogger<AddPropertyModel> logger, 
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
        public int? propertyId { get; set; }
        public class commonModel
        {
            public int Id { get; set; }
            public int buildingId { get; set; }
            public int propertyTypeId { get; set; }
            public int proStatus { get; set; }
            public string propertyNo { get; set; }
            public string floor { get; set; }
            public string aaliNo { get; set; }
            public string legalCost { get; set; }
        }

        public List<SelectListItem> Building { get; set; }
        public List<SelectListItem> ProType { get; set; }
        public List<SelectListItem> ProStatus { get; set; }
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
            ProStatus = this.Context.tbl_Status.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Description
                                }).ToList();
           
            if (propertyId != null)
            {

                //------- Loading Common Fields -----
                var common = (from cust in Context.tbl_Property
                              where cust.Id == propertyId
                              select new
                              {
                                  Id = cust.Id,
                                  buildingId = cust.buildingId,
                                  floor = cust.floor,
                                  propertyTypeId = cust.propertyTypeId,
                                  propertyNo = cust.propertyNo,
                                  proStatus = cust.statusId,
                                  aaliNo = cust.aaliNo,
                                  legalCost = cust.legalCost,
                                }).ToList();

                if (common != null)
                {
                    Common.Id = common[0].Id;
                    Common.buildingId = common[0].buildingId;
                    Common.floor = common[0].floor;
                    Common.propertyTypeId = common[0].propertyTypeId;
                    Common.propertyNo = common[0].propertyNo;
                    Common.proStatus = common[0].proStatus;
                    Common.aaliNo = common[0].aaliNo;
                    Common.legalCost = common[0].legalCost;
                }
                Building.Find(c => c.Value == Common.buildingId.ToString()).Selected = true;
                ProType.Find(c => c.Value == Common.propertyTypeId.ToString()).Selected = true;
                ProStatus.Find(c => c.Value == Common.proStatus.ToString()).Selected = true;
            }
        }
        public IActionResult OnPost()
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    string _inputCheck = Request.Form["inputCheck"];
                    var userid = _userManager.GetUserAsync(User).Result.Id;
                    var userName = _userManager.GetUserAsync(User).Result.FirstName + " " + _userManager.GetUserAsync(User).Result.LastName;
                                      
                    int _buildingId = int.Parse(Request.Form["buildingId"]);
                    string floor = Request.Form["floor"];
                    int _typeId = int.Parse(Request.Form["typeId"]);
                    int _proStatus = int.Parse(Request.Form["proStatus"]);
                    string _propertyNo = Request.Form["propertyNo"];
                    string _aaliNo = Request.Form["aaliNo"];
                    string _legalCost = Request.Form["legalCost"];
                    var propertyCheck = (from b in this.Context.tbl_Property
                                         where b.propertyNo.ToUpper() == _propertyNo.ToUpper()
                                      && b.buildingId == _buildingId && b.floor == floor
                                         select new { b.propertyNo }).FirstOrDefault();
                    if (_inputCheck == "0")//Insert
                    {
                        if (propertyCheck != null)
                        {
                            _toastNotification.AddSuccessToastMessage($"Property is already exist.");
                        }
                        else
                        {
                            var property = new Property();
                            {
                                property.buildingId = _buildingId;
                                property.floor = floor;
                                property.statusId = _proStatus;
                                property.propertyTypeId = _typeId;
                                property.propertyNo = _propertyNo;
                                property.aaliNo = _aaliNo;
                                property.legalCost = _legalCost;
                                property.userId = userid;
                            };
                            Context.tbl_Property.Add(property);
                            Context.SaveChanges();

                            //------ Committing Database ------
                            dbContextTransaction.Commit();
                            _toastNotification.AddSuccessToastMessage($"Property has been added successfully.");
                        } 
                    }
                    else
                    {
                        foreach (var property in Context.tbl_Property.Where(x => x.Id == int.Parse(_inputCheck)).ToList())
                        {
                            property.buildingId = _buildingId;
                            property.floor = floor;
                            property.statusId = _proStatus;
                            property.propertyTypeId = _typeId;
                            property.propertyNo = _propertyNo;
                            property.aaliNo = _aaliNo;
                            property.legalCost = _legalCost;
                            property.userId = userid;
                        };
                        Context.SaveChanges();

                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Property has been updated successfully.");
                    }
                }                
                catch (Exception ex) { _logger.LogError(ex.Message); dbContextTransaction.Rollback(); }
            }
            return new RedirectToPageResult("/AddProperty", new { area = "PMS" });
        }
  
        public JsonResult OnGetLoadData()
        {
            object data = "";
            try
            {
                var query = (from pro in Context.tbl_Property
                             join bu in this.Context.tbl_Building on pro.buildingId equals bu.Id
                             join ar in Context.tbl_Areas on bu.areaId equals ar.Id
                             join st in Context.tbl_Status on pro.statusId equals st.Id
                             join ty in Context.tbl_PropertyType on pro.propertyTypeId equals ty.Id
                            
                             select new
                             {
                                 propertyId = pro.Id,
                                 building = ar.EnglishName + " - " + bu.buildingno,
                                 floor = pro.floor,
                                 type = ty.Description,
                                 status = st.Description,
                                 propertyNo = pro.propertyNo,
                                 aaliNo = pro.aaliNo,
                                 legalCost = pro.legalCost,
            });
                
                data = query.OrderBy(x => x.propertyId).ToList();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return new JsonResult(data);
        }
    }
}
