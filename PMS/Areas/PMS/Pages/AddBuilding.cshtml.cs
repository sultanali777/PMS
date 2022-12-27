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

namespace PMS.Areas.PMS
{
    public class AddBuildingModel : PageModel
    {
        private readonly ILogger<AddBuildingModel> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext Context { get; }

        private IWebHostEnvironment _hostingEnvironment;
        public AddBuildingModel(ILogger<AddBuildingModel> logger, 
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

        public List<SelectListItem> Governorate { get; set; }
        public void OnGet()
        {
            Governorate = this.Context.tbl_Governorates.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = a.Description
                                  }).ToList();
        }
        public IActionResult OnGetAddressAreas(int governorateId)
        {
            if (governorateId != 0)
            {
                IEnumerable<SelectListItem> addressAreas = Context.tbl_Areas.AsNoTracking()
                    .OrderBy(n => n.Id)
                    .Where(n => n.governorateId == governorateId)
                    .Select(n =>
                        new SelectListItem
                        {
                            Value = n.Id.ToString(),
                            Text = n.EnglishName
                        }).ToList();
                return new JsonResult(addressAreas);
            }
            return null;
        }
        public IActionResult OnPost()
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var userid = _userManager.GetUserAsync(User).Result.Id;
                    var userName = _userManager.GetUserAsync(User).Result.FirstName + " " + _userManager.GetUserAsync(User).Result.LastName;
                                      
                    int _governorateId = int.Parse(Request.Form["governorateId"]);
                    int _areaId = int.Parse(Request.Form["areaId"]);
                    string _buildingNo = Request.Form["buildingNo"];
                    string _address = Request.Form["address"];
                    var buildingCheck = (from b in this.Context.tbl_Building
                                         where b.buildingno.ToUpper() == _buildingNo.ToUpper()
                                      select new { b.buildingno }).FirstOrDefault();

                    if (buildingCheck != null)
                    {
                        _toastNotification.AddSuccessToastMessage($"Buidling is already exist.");
                    }
                    else
                    {
                 var building = new Building();
                    {
                        building.governorateId = _governorateId;
                        building.areaId = _areaId;
                        building.buildingno = _buildingNo;
                        building.address = _address;
                        building.userId = userid;
                    };
                    Context.tbl_Building.Add(building);
                    Context.SaveChanges();

                    //------ Committing Database ------
                    dbContextTransaction.Commit();
                    _toastNotification.AddSuccessToastMessage($"Buidling has been added successfully.");
                    }
                }
                catch (Exception ex) { _logger.LogError(ex.Message); dbContextTransaction.Rollback(); }
            }
            return new RedirectToPageResult("/AddBuilding", new { area = "PMS" });
        }
        public JsonResult OnGetLoadData()
        {
            object data = "";
            try
            {
                var query = (from bu in Context.tbl_Building
                             join ar in Context.tbl_Areas on bu.areaId equals ar.Id
                             join gov in Context.tbl_Governorates on bu.governorateId equals gov.Id
                             select new
                             {
                                 buildingId = bu.Id,
                                 governorate = gov.Description,
                                 area = ar.EnglishName,
                                 buildingno = bu.buildingno,
                                 address = bu.address,
                             });
                
                data = query.OrderByDescending(x => x.buildingId).ToList();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return new JsonResult(data);
        }
    }
}
