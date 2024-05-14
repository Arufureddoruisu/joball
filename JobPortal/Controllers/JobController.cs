using DatabaseLayer;
using JobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace JobPortal.Controllers
{
    public class JobController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();

        // GET: Job
        public ActionResult PostJob()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 2)
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

            var job = new PostJobMV();

            // Populate JobCategoryList
            job.JobCategoryList = new SelectList(
                db.JobCategoryTables.ToList(),
                "JobCategoryID",   // Make sure this property name matches your database column name
                "JobCategory"
            );

            // Populate JobNatureList
            job.JobNatureList = new SelectList(
                db.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature"
            );

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostJob(PostJobMV postJobMV)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 2)
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

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            postJobMV.UserID = userid;
            postJobMV.CompanyID = companyid;

            if (ModelState.IsValid)
            {
                var post = new PostJobTable();
                post.UserID = postJobMV.UserID;
                post.CompanyID = postJobMV.CompanyID;
                post.JobCategoryID = postJobMV.JobCategoryID;
                post.JobTitle = postJobMV.JobTitle;
                post.JobDescription = postJobMV.JobDescription;
                post.MinSalary = postJobMV.MinSalary;
                post.MaxSalary = postJobMV.MaxSalary;
                post.Location = postJobMV.Location;
                post.Vacancy = postJobMV.Vacancy;
                post.JobNatureID = postJobMV.JobNatureID;
                post.PostDate = postJobMV.PostDate;
                post.ApplicationLastDate = postJobMV.ApplicationLastDate;
                post.LastDate = postJobMV.LastDate;
                post.JobStatusID = 1;
                post.WebUrl = postJobMV.WebUrl;
                db.PostJobTables.Add(post);
                db.SaveChanges();
                return RedirectToAction("CompanyJobList");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            postJobMV.JobCategoryList = new SelectList(
                db.JobCategoryTables.ToList(),
                "JobCategoryID",
                "JobCategory",
                postJobMV.JobCategoryID
            );

            postJobMV.JobNatureList = new SelectList(
                db.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                postJobMV.JobNatureID
            );

            return View(postJobMV);
        }
        public ActionResult CompanyJobList()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 2)
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
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var allpost = db.PostJobTables.ToList();
            if (allpost.Count() > 0)
            {
                allpost = allpost.OrderByDescending(o => o.PostJobID).ToList();
            }
            return View(allpost);
        }
        public ActionResult AllCompanyPendingJob()
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
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var allpost = db.PostJobTables.ToList();
            return View(allpost);
        }
        public ActionResult AddRequirements(int? id)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 2)
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
            var details = db.JobRequirementDetailTables.Where(j => j.PostJobID == id).ToList();
            if (details.Count() > 0)
            {
                details = details.OrderBy(r => r.JobRequirementID).ToList();
            }
            var requirements = new JobRequirementsMV();
            requirements.Details = details;
            requirements.PostJobID = (int)id;
            ViewBag.JobRequirementID = new SelectList(db.JobRequirementTables.ToList(), "JobRequirementID", "JobRequirementTitle", "0");
            return View(requirements);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRequirements(JobRequirementsMV jobRequirementsMV)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 2)
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
            try
            {
                var requirements = new JobRequirementDetailTable();
                requirements.JobRequirementID = jobRequirementsMV.JobRequirementID;
                requirements.JobRequirementDetail = jobRequirementsMV.JobRequirementDetail;
                requirements.PostJobID = jobRequirementsMV.PostJobID;
                db.JobRequirementDetailTables.Add(requirements);
                db.SaveChanges();
                return RedirectToAction("AddRequirements", new { id = requirements.PostJobID });
            }
            catch (Exception ex)
            {

                var details = db.JobRequirementDetailTables.Where(j => j.PostJobID == jobRequirementsMV.PostJobID).ToList();
                if (details.Count() > 0)
                {
                    details = details.OrderBy(r => r.JobRequirementID).ToList();
                }
                jobRequirementsMV.Details = details;
                ModelState.AddModelError("JobRequirementID", "An error occurred while processing your request. Please try again.");
            }


            ViewBag.JobRequirementID = new SelectList(db.JobRequirementTables.ToList(), "JobRequirementID", "JobRequirementTitle", jobRequirementsMV.JobRequirementID);
            return View(jobRequirementsMV);
        }

        public ActionResult DeleteRequirements(int? id)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 2)
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

            var jobpostid = db.JobRequirementDetailTables.Find(id).PostJobID;
            var requirements = db.JobRequirementDetailTables.Find(id);
            db.Entry(requirements).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("AddRequirements", new { id = jobpostid });
        }

        public ActionResult DeleteJobPost(int? id)
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
            var jobpost = db.PostJobTables.Find(id);
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("CompanyJobList");
        }

        public ActionResult ApprovedPost(int? id)
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
            var jobpost = db.PostJobTables.Find(id);
            jobpost.JobStatusID = 2;
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJob");
        }
        public ActionResult CanceledPost(int? id)
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
            var jobpost = db.PostJobTables.Find(id);
            jobpost.JobStatusID = 3;
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJob");
        }



        public ActionResult JobDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var getpostjob = db.PostJobTables.Find(id);
            var postjob = new PostJobDetailMV();
            postjob.PostJobID = getpostjob.PostJobID;
            postjob.Company = getpostjob.CompanyTable.CompanyName;
            postjob.JobCategory = getpostjob.JobCategoryTable.JobCategory;
            postjob.JobTitle = getpostjob.JobTitle;
            postjob.JobDescription = getpostjob.JobDescription;
            postjob.MinSalary = getpostjob.MinSalary;
            postjob.MaxSalary = getpostjob.MaxSalary;
            postjob.Location = getpostjob.Location;
            postjob.Vacancy = getpostjob.Vacancy;
            postjob.JobNature = getpostjob.JobNatureTable.JobNature;
            postjob.PostDate = getpostjob.PostDate;
            postjob.ApplicationLastDate = getpostjob.ApplicationLastDate;
            postjob.WebUrl = getpostjob.WebUrl;

            getpostjob.JobRequirementDetailTables = getpostjob.JobRequirementDetailTables.OrderBy(d => d.JobRequirementID).ToList();
            int jobrequirementid = 0;
            var jobrequirement = new JobRequirementMV();
            foreach (var detail in getpostjob.JobRequirementDetailTables)
            {
                var jobrequirementdetail = new JobRequirementDetailMV();
                if (jobrequirementid == 0)
                {
                    jobrequirement.JobRequirementID = detail.JobRequirementID;
                    jobrequirement.JobRequirementTitle = detail.JobRequirementTable.JobRequirementTitle;
                    jobrequirementdetail.JobRequirementID = detail.JobRequirementID;
                    jobrequirementdetail.JobRequirementDetail = detail.JobRequirementDetail;
                    jobrequirement.Details.Add(jobrequirementdetail);
                    jobrequirementid = detail.JobRequirementID;
                }
                else if (jobrequirementid == detail.JobRequirementID)
                {
                    jobrequirementdetail.JobRequirementID = detail.JobRequirementID;
                    jobrequirementdetail.JobRequirementDetail = detail.JobRequirementDetail;
                    jobrequirement.Details.Add(jobrequirementdetail);
                    jobrequirementid = detail.JobRequirementID;
                }
                else if (jobrequirementid != detail.JobRequirementID)
                {
                    postjob.Requirements.Add(jobrequirement);
                    jobrequirement = new JobRequirementMV();
                    jobrequirement.JobRequirementID = detail.JobRequirementID;
                    jobrequirement.JobRequirementTitle = detail.JobRequirementTable.JobRequirementTitle;
                    jobrequirementdetail.JobRequirementID = detail.JobRequirementID;
                    jobrequirementdetail.JobRequirementDetail = detail.JobRequirementDetail;
                    jobrequirement.Details.Add(jobrequirementdetail);
                    jobrequirementid = detail.JobRequirementID;
                }
            }

            postjob.Requirements.Add(jobrequirement);
            return View(postjob);
        }

        public ActionResult FilterJob()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 3)
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
            var obj = new FilterJobMV();
            var date = DateTime.Now.Date;
            var resultQuery = db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID == 2).ToList();
            obj.Result = resultQuery;

            // Populate ViewBag with SelectList for dropdowns
            ViewBag.JobCategoryID = new SelectList(
                db.JobCategoryTables.ToList(),
                "JobCategoryID",
                "JobCategory",
                0);

            ViewBag.JobNatureID = new SelectList(
                db.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                0);

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterJob(FilterJobMV filterJobMV)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                int userTypeID = Convert.ToInt32(Session["UserTypeID"]);
                if (userTypeID != 3)
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
            var date = DateTime.Now.Date;
            var resultQuery = db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID == 2);

            if (filterJobMV.JobCategoryID != 0)
            {
                resultQuery = resultQuery.Where(r => r.JobCategoryID == filterJobMV.JobCategoryID);
            }

            if (filterJobMV.JobNatureID != 0)
            {
                resultQuery = resultQuery.Where(r => r.JobNatureID == filterJobMV.JobNatureID);
            }

            var result = resultQuery.ToList();

            ViewBag.JobCategoryID = new SelectList(
                db.JobCategoryTables.ToList(),
                "JobCategoryID",
                "JobCategory",
                filterJobMV.JobCategoryID);

            ViewBag.JobNatureID = new SelectList(
                db.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                filterJobMV.JobNatureID);

            filterJobMV.Result = result;
            return View(filterJobMV);
        }
    }
}