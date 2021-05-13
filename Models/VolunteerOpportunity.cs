using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team2Geraldton.Models
{
    public class VolunteerOpportunity
    {
        [Key]
        public int OpportunityID { get; set; }
        public string OpportunityName { get; set; }
        public string Description { get; set; }

        public ICollection<VolunteerApplication> VolunteerApplications { get; set; }
    }
    public class VolunteerOpportunityDto
    {
        public int OpportunityID { get; set; }

        [DisplayName("Opportunity Name")]
        public string OpportunityName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }

}