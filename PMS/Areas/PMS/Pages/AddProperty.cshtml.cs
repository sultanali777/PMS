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
            ProStatus.Find(c => c.Value == "2").Selected = true;
        }
        public IActionResult OnPost()
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var userid = _userManager.GetUserAsync(User).Result.Id;
                    var userName = _userManager.GetUserAsync(User).Result.FirstName + " " + _userManager.GetUserAsync(User).Result.LastName;
                                      
                    int _buildingId = int.Parse(Request.Form["buildingId"]);
                    string floor = Request.Form["floor"];
                    int _typeId = int.Parse(Request.Form["typeId"]);
                    int _proStatus = int.Parse(Request.Form["proStatus"]);
                    string _propertyNo = Request.Form["propertyNo"];
                    var propertyCheck = (from b in this.Context.tbl_Property
                                         where b.propertyNo.ToUpper() == _propertyNo.ToUpper()
                                      && b.buildingId == _buildingId && b.floor == floor
                                         select new { b.propertyNo }).FirstOrDefault();

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
                            property.userId = userid;
                    };
                    Context.tbl_Property.Add(property);
                    Context.SaveChanges();

                    //------ Committing Database ------
                    dbContextTransaction.Commit();
                    _toastNotification.AddSuccessToastMessage($"Property has been added successfully.");
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
                             });
                
                data = query.OrderBy(x => x.propertyId).ToList();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return new JsonResult(data);
        }
    }
}
