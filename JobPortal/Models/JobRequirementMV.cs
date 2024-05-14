using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Models
{
    public class JobRequirementMV
    {
        public JobRequirementMV()
        {
            Details = new List<JobRequirementDetailMV>();
        }
        [Required(ErrorMessage = "Required")]
        public int JobRequirementID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string JobRequirementTitle { get; set; }
        public int PostJobID { get; set; }

        public List<JobRequirementDetailMV> Details { get; set; }
    }
}