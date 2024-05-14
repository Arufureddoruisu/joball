using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace JobPortal.Controllers
{
    public class CompanyTablesController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();

        // GET: CompanyTables
        public ActionResult Index()
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
            var companyTables = db.CompanyTables.Include(c => c.UserTable);
            return View(companyTables.ToList());
        }

        // GET: CompanyTables/Details/5
        public ActionResult Details(int? id)
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
            CompanyTable companyTable = db.CompanyTables.Find(id);
            if (companyTable == null)
            {
                return HttpNotFound();
            }
            return View(companyTable);
        }


        // GET: CompanyTables/Edit/5
        public ActionResult Edit(int? id)
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
            CompanyTable companyTable = db.CompanyTables.Find(id);
            if (companyTable == null)
            {
                return HttpNotFound();
            }
            return View(companyTable);
        }

        // POST: CompanyTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyID,UserID,CompanyName,ContactNo,PhoneNo,EmailAddress,Logo,Description")] CompanyTable companyTable)
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
            if (ModelState.IsValid)
            {
                db.Entry(companyTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyTable);
        }

        // GET: CompanyTables/Delete/5
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
            CompanyTable companyTable = db.CompanyTables.Find(id);
            if (companyTable == null)
            {
                return HttpNotFound();
            }
            return View(companyTable);
        }

        // POST: CompanyTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
            CompanyTable companyTable = db.CompanyTables.Find(id);
            db.CompanyTables.Remove(companyTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
