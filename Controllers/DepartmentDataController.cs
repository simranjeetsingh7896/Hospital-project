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
    public class DepartmentDataController : ApiController
    {
        private team2GeraldtonDbContext db = new team2GeraldtonDbContext();

        /// <summary>
        /// Gets a list or department in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of Departments including their ID,name ,starttime, endtime and description.</returns>
        /// <example>
        /// GET: api/DepartmentData/GetDepartments
        /// </example>
        [ResponseType(typeof(IEnumerable<DepartmentDto>))]
        public IHttpActionResult GetDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto> { };

            foreach (var Department in Departments)
            {
                DepartmentDto NewDepartment = new DepartmentDto
                {
                    DepartmentID = Department.DepartmentID,
                    DepartmentName = Department.DepartmentName,
                    Timings = Department.Timings,
                    Description = Department.Description
                };
                DepartmentDtos.Add(NewDepartment);
            }
            return Ok(DepartmentDtos);
        }


        /// <summary>
        /// Finds a particular department in the database with a 200 status code. If the visiting is not found, return 404.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <returns>Information about the department, including department id, ,starttime, endtime and description.</returns>
        // <example>
        // GET: api/DepartmentData/FindDepartment/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult FindDepartment(int id)
        {
            //Find the data
            Department Department = db.Departments.Find(id);
            //if not found, return 404 status code.
            if (Department == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            DepartmentDto DepartmentDto = new DepartmentDto
            {
                DepartmentID = Department.DepartmentID,
                DepartmentName = Department.DepartmentName,
                Timings = Department.Timings,
                Description = Department.Description
            };


            //pass along data as 200 status code OK response
            return Ok(DepartmentDto);
        }

        /// <summary>
        /// Finds a particular department in the database with a 200 status code. If the visiting is not found, return 404.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <returns>Information about the department, including department id, ,starttime, endtime and description.</returns>
        /// <example>
        /// POST: api/DepartmentData/UpdateDepartment/5
        /// FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateDepartment(int id, [FromBody] Department Department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Department.DepartmentID)
            {
                return BadRequest();
            }

            db.Entry(Department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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
        /// Finds a particular department in the database with a 200 status code. If the visiting is not found, return 404.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <returns>Information about the department, including department id, ,starttime, endtime and description.</returns>
        /// <example>
        /// POST: api/DepartmentData/AddDepartment
        ///  FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(Department))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddDepartment([FromBody] Department Department)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(Department);
            db.SaveChanges();

            return Ok(Department.DepartmentID);
        }

        /// <summary>
        /// Deletes a Department from the database
        /// </summary>
        /// <param name="id">The id of the Department to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/DepartmentData/DeleteDepartment/5
        /// </example>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department Department = db.Departments.Find(id);
            if (Department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(Department);
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

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentID == id) > 0;
        }
    }
}