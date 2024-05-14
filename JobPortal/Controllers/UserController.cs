using DatabaseLayer;
using JobPortal.Forgot;
using JobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace JobPortal.Controllers
{
    public class UserController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();

        // GET: User
        public ActionResult NewUser()
        {
            int userTypeID;
            if (Session["UserTypeID"] != null && int.TryParse(Session["UserTypeID"].ToString(), out userTypeID))
            {
                // Log out the user
                Session.Clear(); // Clear all session variables
                Session.Abandon(); // Abandon the session
                return RedirectToAction("NewUser", "User");
            }
            return View(new UserMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserMV userMV)
        {
            if (ModelState.IsValid)
            {
                int userTypeID;
                if (Session["UserTypeID"] != null && int.TryParse(Session["UserTypeID"].ToString(), out userTypeID))
                {
                    // Log out the user
                    Session.Clear(); // Clear all session variables
                    Session.Abandon(); // Abandon the session
                    return RedirectToAction("NewUser", "User");
                }
                var checkuser = db.UserTables.FirstOrDefault(u => u.EmailAddress == userMV.EmailAddress);
                if (checkuser != null)
                {
                    ModelState.AddModelError("EmailAddress", "Email is Already Registered!");
                    return View(userMV);
                }
                checkuser = db.UserTables.FirstOrDefault(u => u.UserName == userMV.UserName);
                if (checkuser != null)
                {
                    ModelState.AddModelError("UserName", "UserName is Already Registered!");
                    return View(userMV);
                }
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new UserTable();
                        user.UserName = userMV.UserName;
                        user.Password = userMV.Password;
                        user.ContactNo = userMV.ContactNo;
                        user.EmailAddress = userMV.EmailAddress;
                        user.UserTypeID = userMV.AreYouProvider == true ? 2 : 3;
                        db.UserTables.Add(user);
                        db.SaveChanges();

                        if (userMV.AreYouProvider == true)
                        {
                            var company = new CompanyTable();
                            company.UserID = user.UserID;
                            if (String.IsNullOrEmpty(userMV.Company.EmailAddress))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.EmailAddress", "Required!");
                                return View(new UserMV());
                            }
                            if (String.IsNullOrEmpty(userMV.Company.CompanyName))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.CompanyName", "Required!");
                                return View(new UserMV());
                            }
                            if (String.IsNullOrEmpty(userMV.Company.PhoneNo))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.PhoneNo", "Required!");
                                return View(new UserMV());
                            }
                            if (String.IsNullOrEmpty(userMV.Company.Description))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.Description", "Required!");
                                return View(new UserMV());
                            }

                            company.EmailAddress = userMV.Company.EmailAddress;
                            company.CompanyName = userMV.Company.CompanyName;
                            company.ContactNo = userMV.ContactNo;
                            company.PhoneNo = userMV.Company.PhoneNo;
                            company.Logo = "~/Content/assets/img/logo/logo.png";
                            company.Description = userMV.Company.Description;
                            db.CompanyTables.Add(company);
                            db.SaveChanges();
                        }
                        trans.Commit();
                        return RedirectToAction("Login");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(String.Empty, "Please Provide Correct Details");
                        trans.Rollback();
                    }
                }
            }
            return View(new UserMV());
        }

        public ActionResult Edit(int id)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 1)
                {
                    // Log out the user
                    Session.Clear(); // Clear all session variables
                    Session.Abandon(); // Abandon the session
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            var user = db.UserTables.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userMV = new UserMV
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Password = user.Password,
                ContactNo = user.ContactNo,
                EmailAddress = user.EmailAddress
            };

            return View(userMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserMV userMV)
        {
            
            if (ModelState.IsValid)
            {
                var user = db.UserTables.Find(userMV.UserID);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = userMV.UserName;
                user.Password = userMV.Password;
                user.ContactNo = userMV.ContactNo;
                user.EmailAddress = userMV.EmailAddress;

                db.SaveChanges();
                return RedirectToAction("AllUsers");
            }
            return View(userMV);
        }

        public ActionResult Delete(int? id)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 1)
                {
                    // Log out the user
                    Session.Clear(); // Clear all session variables
                    Session.Abandon(); // Abandon the session
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            var userMV = new UserMV
            {
                UserName = userTable.UserName,
                Password = userTable.Password,
                EmailAddress = userTable.EmailAddress,
                ContactNo = userTable.ContactNo
            };
            return View(userMV);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Check if there are associated companies, if so, prompt the admin to delete them first
                if (db.CompanyTables.Any(c => c.CompanyID == id))
                {
                    TempData["ErrorMessage"] = "Cannot delete provider because there are associated companies. Please delete the associated companies first.";
                    return RedirectToAction("AllUsers"); // Or wherever you want to redirect
                }

                UserTable userTable = db.UserTables.Find(id);
                db.UserTables.Remove(userTable);
                db.SaveChanges();
                return RedirectToAction("AllUsers");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the provider: " + ex.Message;
                return RedirectToAction("AllUsers"); // Or wherever you want to redirect
            }
        }

        public ActionResult Login()
        {
            int userTypeID;
            if (Session["UserTypeID"] != null && int.TryParse(Session["UserTypeID"].ToString(), out userTypeID))
            {
                // Log out the user
                Session.Clear(); // Clear all session variables
                Session.Abandon(); // Abandon the session
                return RedirectToAction("Login", "User");
            }
            return View(new UserLoginMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginMV userLoginMV)
        {
            int userTypeID;
            if (Session["UserTypeID"] != null && int.TryParse(Session["UserTypeID"].ToString(), out userTypeID))
            {
                // Log out the user
                Session.Clear(); // Clear all session variables
                Session.Abandon(); // Abandon the session
                return RedirectToAction("Login", "User");
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(userLoginMV);
            }

            var user = db.UserTables.FirstOrDefault(u => u.UserName == userLoginMV.UserName && u.Password == userLoginMV.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(userLoginMV);
            }

            Session["UserID"] = user.UserID;
            Session["UserName"] = user.UserName;
            Session["UserTypeID"] = user.UserTypeID;

            if (user.UserTypeID == 2)
            {
                Session["CompanyID"] = user.CompanyTables.FirstOrDefault()?.CompanyID;
            }
            else
            {
                Session["CompanyID"] = null;
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult AllUsers()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 1)
                {
                    // Log out the user
                    Session.Clear(); // Clear all session variables
                    Session.Abandon(); // Abandon the session
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            var users = db.UserTables.ToList();
            return View(users);
        }
        public ActionResult Forgot()
        {
            int userTypeID;
            if (Session["UserTypeID"] != null && int.TryParse(Session["UserTypeID"].ToString(), out userTypeID))
            {
                // Log out the user
                Session.Clear(); // Clear all session variables
                Session.Abandon(); // Abandon the session
                return RedirectToAction("Forgot", "User");
            }
            return View(new ForgotPasswordMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forgot(ForgotPasswordMV forgotPasswordMV)
        {
            int userTypeID;
            if (Session["UserTypeID"] != null && int.TryParse(Session["UserTypeID"].ToString(), out userTypeID))
            {
                // Log out the user
                Session.Clear(); // Clear all session variables
                Session.Abandon(); // Abandon the session
                return RedirectToAction("Forgot", "User");
            }
            var user = db.UserTables.Where(u => u.EmailAddress == forgotPasswordMV.Email).FirstOrDefault();
            if (user != null)
            {
                try
                {
                    string userAndPassword = "Username: " + user.UserName + "\n" + "Password: " + user.Password;
                    string body = userAndPassword;

                    Email.MailManager mailManager = new Email.MailManager();
                    string errorMessage = "";
                    bool isSendEmail = mailManager.SendEmail(user.EmailAddress, "Account Details", body, ref errorMessage);

                    if (isSendEmail)
                    {
                        ModelState.AddModelError(string.Empty, "Username and Password have been sent to your email.");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "There was a problem sending the email. Please try again later. Error: " + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Email", "An error occurred while sending the email: " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email is not registered.");
            }
            return View(forgotPasswordMV);
        }
    }
}
