using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team2Geraldton.Models
{
    public class VolunteerApplication
    {
        [Key]
        public int VolunteerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public double ContactNumber { get; set; }
        public string Language { get; set; }
        public string WhyInterested { get; set; }
        public string PastVolunteer { get; set; }

        [ForeignKey("VolunteerOpportunity")]
        public int OpportunityID { get; set; }
        public virtual VolunteerOpportunity VolunteerOpportunity { get; set; }

    }

    public class VolunteerApplicationDto
    {
        public int VolunteerID { get; set; }

        [DisplayName("Full Name")]
        public string Name { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("Contact Number")]
        public double ContactNumber { get; set; }

        [DisplayName("Primary Language")]
        public string Language { get; set; }

        [DisplayName("Why are you interested in our hospital ?")]
        public string WhyInterested { get; set; }

        [DisplayName("Any Past Volunteer Experience")]
        public string PastVolunteer { get; set; }

        [DisplayName("What type of opportunity you are interested in ?")]
        public int OpportunityID { get; set; } 

    }
}