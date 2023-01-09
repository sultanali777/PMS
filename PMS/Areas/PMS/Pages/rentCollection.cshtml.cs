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

    public class rentCollectionModel : PageModel
    {
        private readonly ILogger<rentCollectionModel> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext Context { get; }
        private IWebHostEnvironment _hostingEnvironment;
        public rentCollectionModel(ILogger<rentCollectionModel> logger, 
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
        public int rentalId { get; set; }
        public class commonModel
        {
            public int rentalId { get; set; }

        }
        public void OnGet()
        {
            Common.rentalId =rentalId;
        }
        public IActionResult OnPost()
        {
            int rentId = 0;
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    string _desc = Request.Form["desc"];
                    int _planId = int.Parse(Request.Form["planId"]);
                    int _rentalId = int.Parse(Request.Form["rentalId"]);
                    string _type = Request.Form["type"];
                    var userid = _userManager.GetUserAsync(User).Result.Id;
                    var userName = _userManager.GetUserAsync(User).Result.FirstName + " " + _userManager.GetUserAsync(User).Result.LastName;
                    rentId = _rentalId;

                    foreach (var rental in Context.tbl_rentCollection.Where(x => x.Id == _planId && x.rentalId== _rentalId).ToList())
                    {
                        rental.receivedDate = DateTime.Now;
                        rental.receivedType = _type;
                        rental.userIdReceived = userid;
                        rental.Description = _desc;
                        rental.receivedRent = true;
                    }
                    Context.SaveChanges();
                    //------ Committing Database ------
                    dbContextTransaction.Commit();
                    _toastNotification.AddSuccessToastMessage($"Rent has been received successfully.");
                }
                catch (Exception ex) { _logger.LogError(ex.Message); dbContextTransaction.Rollback(); }
            }
            return new RedirectToPageResult("/rentCollection", new { area = "PMS", rentalId= rentId });
        }
  
        public JsonResult OnGetLoadData(int rentalId)
        {
            object data = "";
            try
            {
                var query = (from ren in Context.tbl_rentCollection
                             where ren.rentalId== rentalId
                             select new
                             {
                                 planId=ren.Id,
                                 checkReceived = ren.receivedRent.ToString() + "&Id=" + ren.Id.ToString(),
                                 monthlyRent = ren.propertyRent,
                                 month = ren.monthRent,
                                 type = ren.receivedType,
                                 receivedDate = ren.receivedDate,
                                 desc=ren.Description
                             });
                
                data = query.OrderBy(x => x.planId).ToList();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return new JsonResult(data);
        }
    }
}
