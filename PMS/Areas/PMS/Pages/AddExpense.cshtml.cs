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

    public class AddExpenseModel : PageModel
    {
        private readonly ILogger<AddExpenseModel> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext Context { get; }
        private IWebHostEnvironment _hostingEnvironment;
        public AddExpenseModel(ILogger<AddExpenseModel> logger, 
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
        public int? expenseId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? guid { get; set; }

        public List<SelectListItem> Building { get; set; }
        public List<SelectListItem> Vendor { get; set; }
        public List<SelectListItem> ProType { get; set; }
        public List<string> FileNames { get; set; }
        public class commonModel
        {
            public int Id { get; set; }
            public int buildingId { get; set; }
            public string floor { get; set; }
            public int propertyTypeId { get; set; }
            public int propertyNo { get; set; }
            public int expenseAmount { get; set; }
            public string invoiceNo { get; set; }
            public int vendorId { get; set; }
            public string Description { get; set; }
            public string attachments { get; set; }
            public string guid { get; set; }

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
            Vendor = this.Context.tbl_Vendor.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.Id.ToString(),
                                     Text = a.fullName 
                                 }).ToList();
            if (expenseId != null)
            {

                //------- Loading Common Fields -----
                var common = (from cust in Context.tbl_ExpenseDetails
                              where cust.Id == expenseId 
                              select new
                              {
                                  Id = cust.Id,
                                  buildingId = cust.buildingId,
                                  floor = cust.floor,
                                  propertyTypeId = cust.propertyTypeId,
                                  propertyNo = cust.propertyNo,
                                  vendorId = cust.vendorId,
                                  expenseAmount = cust.expenseAmount,
                                  invoiceNo = cust.invoiceNo,
                                  Description = cust.Description,
                                  guid = cust.guid,


    }).ToList();

                if (common != null)
                {
                    Common.Id = common[0].Id;
                    Common.buildingId = common[0].buildingId;
                    Common.floor = common[0].floor;
                    Common.propertyTypeId = common[0].propertyTypeId;
                    Common.propertyNo = common[0].propertyNo;
                    Common.vendorId = common[0].vendorId;
                    Common.expenseAmount = common[0].expenseAmount;
                    Common.invoiceNo = common[0].invoiceNo;
                    Common.Description = common[0].Description;
                    Common.guid = common[0].guid;
                }
                Building.Find(c => c.Value == Common.buildingId.ToString()).Selected = true;
                ProType.Find(c => c.Value == Common.propertyTypeId.ToString()).Selected = true;
                Vendor.Find(c => c.Value == Common.vendorId.ToString()).Selected = true;
            }
        }
        public IActionResult OnGetBuildingNo(int typeId, int BuidId, string floor)
        {
            if (BuidId != 0 && typeId != 0 && floor != "")
            {
                
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
            return null;
        }
        public IActionResult OnPost(IFormFile[] imgInvoice)
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
                    int _buildingId = int.Parse(Request.Form["buildingId"]);
                    string floor = Request.Form["floor"];
                    int _typeId = int.Parse(Request.Form["typeId"]);
                    int _propertyNo = int.Parse(Request.Form["propertyNo"]);
                    int _vendorId = int.Parse(Request.Form["vendorId"]);
                    int _expenseAmount = int.Parse(Request.Form["expenseAmount"]);
                    string _invoiceNo = Request.Form["invoiceNo"];
                    string _desc = Request.Form["desc"];
                    
                    //----load Document if exist -----
                    if (imgInvoice != null && imgInvoice.Length > 0)
                    {
                        FileNames = new List<string>();
                        foreach (IFormFile photo in imgInvoice)
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
                        var expense = new ExpenseDetails();
                        {
                            expense.buildingId = _buildingId;
                            expense.floor = floor;
                            expense.propertyTypeId = _typeId;
                            expense.propertyNo = _propertyNo;
                            expense.vendorId = _vendorId;
                            expense.expenseAmount = _expenseAmount;
                            expense.invoiceNo = _invoiceNo;
                            expense.Description = _desc;
                            expense.attachments = files;
                            expense.guid = guid;
                            expense.userId = userid;
                        };
                        Context.tbl_ExpenseDetails.Add(expense);
                        Context.SaveChanges();
                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Expense has been added successfully.");
                    }
                    else {
                        foreach (var expense in Context.tbl_ExpenseDetails.Where(x => x.Id == int.Parse(_inputCheck)).ToList())
                        {
                            expense.buildingId = _buildingId;
                            expense.floor = floor;
                            expense.propertyTypeId = _typeId;
                            expense.propertyNo = _propertyNo;
                            expense.vendorId = _vendorId;
                            expense.expenseAmount = _expenseAmount;
                            expense.invoiceNo = _invoiceNo;
                            expense.Description = _desc;
                            expense.attachments = files;
                            expense.guid = guid;
                            expense.userId = userid;
                        }
                        Context.SaveChanges();
                        //------ Committing Database ------
                        dbContextTransaction.Commit();
                        _toastNotification.AddSuccessToastMessage($"Expense has been updated successfully.");

                    }              
                    
                    
                }
                catch (Exception ex) { _logger.LogError(ex.Message); dbContextTransaction.Rollback(); }
            }
            return new RedirectToPageResult("/AddExpense", new { area = "PMS" });
        }
  
        public JsonResult OnGetLoadData()
        {
            object data = "";
            try
            {
                var query = (from ren in Context.tbl_ExpenseDetails
                             join bu in this.Context.tbl_Building on ren.buildingId equals bu.Id
                             join ar in Context.tbl_Areas on bu.areaId equals ar.Id
                             join pro in Context.tbl_Property on ren.propertyNo equals pro.Id
                             join st in Context.tbl_Status on pro.statusId equals st.Id
                             join ven in Context.tbl_Vendor on ren.vendorId equals ven.Id
                             join vty in Context.tbl_VendorType on ven.vendorTypeId equals vty.Id
                             select new
                             {
                                 expenseId = ren.Id,
                                 building = ar.EnglishName + " - " + bu.buildingno,
                                 floor = pro.floor,
                                 type = vty.Description,
                                 status = st.Description,
                                 propertyNo = pro.propertyNo,
                                 expenseAmount=ren.expenseAmount,
                                 invoiceNo=ren.invoiceNo,
                                 vendor=ven.fullName,
                                 desc=ren.Description,
                                 viewString=ren.Id+"&guid="+ren.guid
                             });
                
                data = query.OrderBy(x => x.expenseId).ToList();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
            return new JsonResult(data);
        }
    }
}
