﻿using System;
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
    public class JobNatureTablesController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();

        // GET: JobNatureTables
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
            return View(db.JobNatureTables.ToList());
        }


        // GET: JobNatureTables/Create
        public ActionResult Create()
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
            return View(new JobNatureTable());
        }

        // POST: JobNatureTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobNatureID,JobNature")] JobNatureTable jobNatureTable)
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
                db.JobNatureTables.Add(jobNatureTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobNatureTable);
        }

        // GET: JobNatureTables/Edit/5
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
            JobNatureTable jobNatureTable = db.JobNatureTables.Find(id);
            if (jobNatureTable == null)
            {
                return HttpNotFound();
            }
            return View(jobNatureTable);
        }

        // POST: JobNatureTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobNatureID,JobNature")] JobNatureTable jobNatureTable)
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
                db.Entry(jobNatureTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobNatureTable);
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
            JobNatureTable jobNature = db.JobNatureTables.Find(id);
            if (jobNature == null)
            {
                return HttpNotFound();
            }
            return View(jobNature);
        }

        // POST: JobCategoryTables/Delete/5
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
            JobNatureTable jobNature = db.JobNatureTables.Find(id);
            if (jobNature == null)
            {
                return HttpNotFound();
            }
            db.JobNatureTables.Remove(jobNature);
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
