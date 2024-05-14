using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Models
{
    public class FilterJobMV
    {
        public FilterJobMV()
        {
            Result = new List<PostJobTable>();
        }
        [Required(ErrorMessage ="Required")]
        public int JobCategoryID { get; set; }

        [Required(ErrorMessage = "Required")]
        public int JobNatureID { get; set; }
        public int NoDays { get; set; }

        public List<PostJobTable> Result { get; set; }
    }
}