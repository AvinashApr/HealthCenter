using healt_Center.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace healt_Center.Controllers
{
    public class DaseboardController : Controller
    {
        private readonly Patient_DbContext _context;
        public DaseboardController(Patient_DbContext context)
        {

            _context = context;

        }
        public IActionResult Index()
        {
            return View(_context.tbl_About_Doctor.ToList());
        }
        [HttpPost]
        public IActionResult Index(patient adddata)
        {
            _context.patients.Add(adddata);
            _context.SaveChanges();
            return View("index");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var alldata= _context.tbl_branch.Where(b=> b.B_id==1).FirstOrDefault();
            if (alldata != null)
            {
                ViewData["b_number"]=alldata.B_Number;
                ViewData["b_email"]=alldata.B_email;
                ViewData["b_logo"]=alldata.B_logo;
                ViewData["b_opening"] =alldata.B_opening;
                ViewData["b_saturday"] =alldata.B_saturday;
                ViewData["b_sunday"] =alldata.B_sunday;
                ViewData["fb_link"] =alldata.fb_link;
                ViewData["insta_link"] =alldata.insta_link;
                ViewData["twt_link"] =alldata.twt_link;
                ViewData["b_description"] =alldata.B_description;
                ViewData["B-logo"] =Url.Content( alldata.B_logo);
            }
            base.OnActionExecuting(context);
        }

    }
}
