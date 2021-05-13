using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using team2Geraldton.Models;
using System.Diagnostics;

namespace team2Geraldton.Controllers
{
    public class VolunteerOpportunityDataController : ApiController
    {
        private team2GeraldtonDbContext db = new team2GeraldtonDbContext();


        /// <summary>
        /// Gets a list or VolunteerOpportunity in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of VolunteerOpportunity including their name and description.</returns>
        /// <example>
        /// GET: api/VolunteerOpportunityData/GetVolunteerOpportunities
        /// </example>
        [ResponseType(typeof(IEnumerable<VolunteerOpportunityDto>))]
        public IHttpActionResult GetVolunteerOpportunities()
        {
            List<VolunteerOpportunity> VolunteerOpportunities = db.VolunteerOpportunities.ToList();
            List<VolunteerOpportunityDto> VolunteerOpportunityDtos = new List<VolunteerOpportunityDto> { };

            foreach (var VolunteerOpportunity in VolunteerOpportunities)
            {
                VolunteerOpportunityDto NewVolunteerOpportunity = new VolunteerOpportunityDto
                {
                    OpportunityID = VolunteerOpportunity.OpportunityID,
                    OpportunityName = VolunteerOpportunity.OpportunityName,
                    Description = VolunteerOpportunity.Description
                };
                VolunteerOpportunityDtos.Add(NewVolunteerOpportunity);
            }
            return Ok(VolunteerOpportunityDtos);
        }


        /// <summary>
        /// Finds a particular VolunteerOpportunity in the database with a 200 status code. If the visiting is not found, return 404.
        /// </summary>
        /// <param name="id">The opportunity id</param>
        /// <returns>Information about the VolunteerOpportunity including their name and description.</returns>
        // <example>
        // GET: api/VolunteerOpportunityData/FindVolunteerOpportunity/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(VolunteerOpportunityDto))]
        public IHttpActionResult FindVolunteerOpportunity(int id)
        {
            //Find the data
            VolunteerOpportunity VolunteerOpportunity = db.VolunteerOpportunities.Find(id);
            //if not found, return 404 status code.
            if (VolunteerOpportunity == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            VolunteerOpportunityDto VolunteerOpportunityDto = new VolunteerOpportunityDto
            {
                OpportunityID = VolunteerOpportunity.OpportunityID,
                OpportunityName = VolunteerOpportunity.OpportunityName,
                Description = VolunteerOpportunity.Description
            };


            //pass along data as 200 status code OK response
            return Ok(VolunteerOpportunityDto);
        }


        /// <summary>
        /// Finds a particular VolunteerOpportunity in the database with a 200 status code. If the visiting is not found, return 404.
        /// </summary>
        /// <param name="id">The opportunity id</param>
        /// <returns>Information about the VolunteerOpportunity including their name and description.</returns>
        // <example>
        /// POST: api/VolunteerOpportunityData/UpdateVolunteerOpportunity/5
        /// FORM DATA: VolunteerOpportunity JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateVolunteerOpportunity(int id, [FromBody] VolunteerOpportunity VolunteerOpportunity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != VolunteerOpportunity.OpportunityID)
            {
                return BadRequest();
            }

            db.Entry(VolunteerOpportunity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerOpportunityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Finds a particular VolunteerOpportunity in the database with a 200 status code. If the visiting is not found, return 404.
        /// </summary>
        /// <param name="id">The opportunity id</param>
        /// <returns>Information about the VolunteerOpportunity including their name and description.</returns>
        // <example>
        /// POST: api/VolunteerOpportunityData/AddVolunteerOpportunity
        /// FORM DATA: VolunteerOpportunity JSON Object
        /// </example>
        [ResponseType(typeof(VolunteerOpportunity))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddVolunteerOpportunity([FromBody] VolunteerOpportunity VolunteerOpportunity)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VolunteerOpportunities.Add(VolunteerOpportunity);
            db.SaveChanges();

            return Ok(VolunteerOpportunity.OpportunityID);
        }


        /// <summary>
        /// Deletes a VolunteerOpportunity from the database
        /// </summary>
        /// <param name="id">The id of the VolunteerOpportunity to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/VolunteerOpportunityData/DeleteVolunteerOpportunity/5
        /// </example>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteVolunteerOpportunity(int id)
        {
            VolunteerOpportunity VolunteerOpportunity = db.VolunteerOpportunities.Find(id);
            if (VolunteerOpportunity == null)
            {
                return NotFound();
            }

            db.VolunteerOpportunities.Remove(VolunteerOpportunity);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VolunteerOpportunityExists(int id)
        {
            return db.VolunteerOpportunities.Count(e => e.OpportunityID == id) > 0;
        }
    }
}