using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Models
{
    public class PostJobDetailMV
    {
        public PostJobDetailMV()
        {
            Requirements = new List<JobRequirementMV>();
        }
        public int PostJobID { get; set; }
        public string Company { get; set; }
        public string JobCategory { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public string Location { get; set; }
        public int Vacancy { get; set; }
        public string JobNature { get; set; }
        public DateTime PostDate { get; set; } = DateTime.Now;
        public DateTime ApplicationLastDate { get; set; } = DateTime.Now.AddDays(30);
        public string WebUrl { get; set; }


        public List<JobRequirementMV> Requirements { get; set; }
    }
}