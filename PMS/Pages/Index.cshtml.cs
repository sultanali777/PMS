using PMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PMS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private ApplicationDbContext Context { get; }

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext _context)
        {
            _logger = logger;
            this.Context = _context;
        }



    }
}

