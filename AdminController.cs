using healt_Center.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Drawing;




namespace healt_Center.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        private readonly Patient_DbContext _context;



        public AdminController(Patient_DbContext patient_Db, IWebHostEnvironment environment)
        {
            _context = patient_Db;
            _environment = environment;
        }


        public IActionResult admin()
        {
            if(HttpContext.Session.GetString("admin_session")!= null)
            {
                return View();
            }
            else
            {

            }
            return RedirectToAction("Login_Form");
        }
        public IActionResult Login_Form()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login_Form(string admin_email, string admin_password)
        {
            var admin = _context.tbl_admin.FirstOrDefault(a => a.admin_email == admin_email);

            if (admin == null)
            {
                // If no admin is found with the given email
                ViewBag.ErrorMessagee = "User email not found!";
                return View();
            }

            // Check if the password matches. If using hashed passwords, use hash comparison here.
            if (admin.admin_password == admin_password)
            {
                HttpContext.Session.SetString("admin_session", admin.Id.ToString());
                TempData["Success"] = "Welcome";
                return RedirectToAction("admin");
            }
            else
            {
                ViewBag.ErrorMessage = "Email and Password are incorrect!";
                return View();
            }
        }



        public IActionResult New_Appointments()
        {
            return View(_context.patients.Where(a=>a.status==0).ToList());
        }


        public IActionResult ConfirmedAppointment()
        {
            return View(_context.patients.Where(a => a.status == 1).ToList());
        }



        public IActionResult FixSchedule(int id)
        {
            var app = _context.patients.FirstOrDefault(a=>a.Id==id);
            HttpContext.Session.SetString("appointment_session", app.Id.ToString());
            return View(app);
        }
        [HttpPost]
        public IActionResult FixSchedule(patient patient)
        {
            string apointment_id = HttpContext.Session.GetString("appointment_session");
            var existingAppointment = _context.patients.FirstOrDefault(a => a.Id == int.Parse(apointment_id));
            if(existingAppointment == null)
            {
                return NotFound("Appointment Not Found");
            }
            //existingAppointment.date = patient.date;
            existingAppointment.status = 1;
            //existingAppointment.message = patient.message;
            //existingAppointment.Id = patient.Id;
            existingAppointment.admin_time = patient.admin_time;
            existingAppointment.admin_message = patient.admin_message;
            _context.patients.Update(existingAppointment);
            _context.SaveChanges();
            SendEmail(existingAppointment);
            return RedirectToAction("ConfirmedAppointment");

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["total_user"] = _context.patients.Count();
            ViewData["total_appointments"] = _context.patients.Count();
            ViewData["pending_appoint"]= _context.patients.Count(a=>a.status==0);
            ViewData["Confirmed_appoint"] = _context.patients.Count(a=>a.status==1);
            base.OnActionExecuting(context);
        }

        private void SendEmail(patient patient)
        {
          
            var fromAddress = new MailAddress("avinashapr9616@gmail.com", "Helth Care");
            var toAddress = new MailAddress(patient.email, patient.email);
            const string fromPassword = "clvn tspx xwwc hrzj"; // Use your App Password here if 2FA is enabled, otherwise use your Gmail password
            const string subject = "Appointment Scheduled";
            string body = $"Dear {patient.name},\n\n" +
                          "Your appointment has been scheduled.\n\n" +
                          $"Details:\n" +
                          $"Your Subject is :{patient.Department} \n" +
                          $"Date Time: {patient.date.ToString()}  \n" +
                          $"you can come Today at {patient.admin_time}\n" +
                          $"Message: {patient.admin_message}\n\n" +
                          "Thank you.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                try
                {
                    smtp.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                }
            }
        }
        public IActionResult AdminLogout()
        {
            
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("Login_Form");
        }

        [HttpGet]
        public IActionResult Get_Doctor(int id)
        {
            var subupdate = _context.tbl_About_Doctor.Find(id);
            var sub = _context.tbl_About_Doctor.ToList();
            ViewBag.Id = subupdate.Doctor_id;
            ViewBag.name = subupdate.Doctor_name;
            ViewBag.email = subupdate.Doctor_email;
            ViewBag.number = subupdate.Doctor_number;
            ViewData["image"]=subupdate.Doctor_Pic;
            ViewBag.LinkdIn = subupdate.LinkdIn;
            //ViewBag.gender = subupdate.Doctor_gender;
            //ViewBag.department = subupdate.Doctor_department;
            ViewData["gender"] = subupdate.Doctor_gender;
            ViewData["department"] = subupdate.Doctor_department;
            return View("about_doctor", sub);
        }
        public IActionResult About_Doctor()
        {

            return View(_context.tbl_About_Doctor.ToList());
        }
        [HttpPost]
        public IActionResult About_Doctor(AboutDoctor Doc,IFormFile Doctor_pic)
        {
            
            if(Doctor_pic != null)
            {
                var filepath = Path.Combine(_environment.WebRootPath, "Doctor_pic",Doctor_pic.FileName);
                using (var Stream =  new FileStream(filepath, FileMode.Create))
                {
                    Doctor_pic.CopyTo(Stream);
                    Doc.Doctor_Pic = Doctor_pic.FileName;
                }

            }
            if (Doc.Doctor_id.HasValue)
            {
                var existdoc = _context.tbl_About_Doctor.Find(Doc.Doctor_id.Value);
                _context.SaveChanges();
            }
            else
            {
                _context.tbl_About_Doctor.Add(Doc);
                _context.SaveChanges();
                
            }

            return RedirectToAction("About_Doctor");
        }

        public IActionResult update_Doctor(int id)
        {
            var find = _context.tbl_About_Doctor.Find(id);
            ViewData["gender"] = find.Doctor_gender;
            ViewData["department"] = find.Doctor_department;
            return View(find);
        }
        [HttpPost]
       
        public IActionResult update_Doctor(AboutDoctor Doc, IFormFile Doctor_pic)
        {
            if (Doctor_pic != null)
            {
                var filepath = Path.Combine(_environment.WebRootPath, "Doctor_pic", Doctor_pic.FileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    Doctor_pic.CopyTo(stream);
                    Doc.Doctor_Pic = Doctor_pic.FileName; // Set new image path
                }
            }
            else
            {
                // Doctor_pic is null, retain existing image path
                var existingDoc = _context.tbl_About_Doctor.AsNoTracking().FirstOrDefault(d => d.Doctor_id == Doc.Doctor_id);
                if (existingDoc != null)
                {
                    Doc.Doctor_Pic = existingDoc.Doctor_Pic; // Retain existing image path
                }
            }

            _context.tbl_About_Doctor.Update(Doc);
            _context.SaveChanges();

            return RedirectToAction("About_Doctor");
        }

        public IActionResult Branch()
        { 
            return View(_context.tbl_branch.ToList());
        }
        [HttpPost]
        public IActionResult Branch(branch Branch_obj,IFormFile B_logo)
        {
            if(B_logo != null)
            {
                var filepath = Path.Combine(_environment.WebRootPath, "B_logo", B_logo.FileName);
                using (var stream = new FileStream(filepath, FileMode.Create)) 
                {
                    B_logo.CopyTo(stream);
                    Branch_obj.B_logo = B_logo.FileName;
                }
            }
            _context.tbl_branch.Add(Branch_obj);
            _context.SaveChanges();
            return RedirectToAction("Update_Branch");
        }


        public IActionResult Update_Branch(int id)
        {
            var B_Data = _context.tbl_branch.Find(id);
            return View(B_Data);
        }

        [HttpPost]
        public IActionResult Update_Branch(branch Branch_obj,IFormFile B_logo)
        {
            if (B_logo != null)
            {
                var filepath = Path.Combine(_environment.WebRootPath, "B_logo", B_logo.FileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    B_logo.CopyTo(stream);
                    Branch_obj.B_logo = B_logo.FileName;
                }
            }
            else
            {
                var existupdate = _context.tbl_branch.AsNoTracking().FirstOrDefault(d => d.B_id == Branch_obj.B_id);
                if (existupdate != null)
                {
                    Branch_obj.B_logo= existupdate.B_logo; 
                }
            }
            _context.tbl_branch.Update(Branch_obj);
            _context.SaveChanges();
            return RedirectToAction("branch");
        }
        [HttpGet]
        public IActionResult Branch_get(int id)
            
        {
            var branchupdate = _context.tbl_branch.Find(id);
            var branch= _context.tbl_branch.ToList();
            ViewBag.id = branchupdate.B_id;
            ViewBag.number=branchupdate.B_Number;
            ViewBag.email = branchupdate.B_email;
            ViewBag.mTof = branchupdate.B_opening;
            ViewBag.saturday = branchupdate.B_saturday;
            ViewBag.sunday = branchupdate.B_sunday;
            ViewBag.facebook = branchupdate.fb_link;
            ViewBag.insta = branchupdate.insta_link;
            ViewBag.twt=branchupdate.twt_link;
            ViewBag.b_logo = branchupdate.B_logo;
            ViewData["B-logo"] = branchupdate.B_logo;
            ViewBag.description= branchupdate.B_description;
            return View("Branch", branch);
        }

        public IActionResult Admin_profile()
        {
            var sessionId = HttpContext.Session.GetString("admin_session");

            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Login_Form", "Admin"); // Session null hai toh login pe redirect
            }

            int adminId = Convert.ToInt32(sessionId); // Session me ID hai toh integer me convert karo

            var details = _context.tbl_admin.FirstOrDefault(a => a.Id == adminId); // ID ke basis par data fetch karo

            if (details == null)
            {
                Console.WriteLine("Admin not found in database!");
                return RedirectToAction("Login_Form", "Admin"); // Agar user na mile toh login page pe bhejo
            }

            return View(details); // Agar sab sahi hai toh details bhejo
        }









        [HttpPost]
        public IActionResult Admin_profile(admin profile,IFormFile admin_profile)
        {
            if (admin_profile != null)
            {
                var filepath = Path.Combine(_environment.WebRootPath, "admin_profile", admin_profile.FileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    admin_profile.CopyTo(stream);
                    profile.admin_profile = admin_profile.FileName;
                }

            }
            _context.tbl_admin.Add(profile);
            _context.SaveChanges();
            return RedirectToAction("Admin_profile");
        }

    }
} 
