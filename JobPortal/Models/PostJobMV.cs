using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Models
{
    public class PostJobMV
    {
        public int PostJobID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int JobCategoryID { get; set; }

        public SelectList JobCategoryList { get; set; }

        [Required(ErrorMessage ="Required")]
        [StringLength(500, ErrorMessage ="Do not enter more than 500 characters")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage ="Required")]
        [StringLength(2000, ErrorMessage = "Do not enter more than 2000 characters")]
        public string JobDescription { get; set; }

        [Required(ErrorMessage = "Required")]
        public int MinSalary { get; set; }

        [Required(ErrorMessage = "Required")]
        public int MaxSalary { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Vacancy { get; set; }
        public int JobNatureID { get; set; }
        public SelectList JobNatureList { get; set; }


        public DateTime PostDate { get; set; } = DateTime.Now;

        public DateTime ApplicationLastDate { get; set; } = DateTime.Now.AddDays(30);

        public DateTime LastDate { get; set; } = DateTime.Now;
        public int JobRequirementID { get; set; }
        public int JobStatusID { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Url)]
        public string WebUrl { get; set; }
    }
}