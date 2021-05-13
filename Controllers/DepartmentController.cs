using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team2Geraldton.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace team2Geraldton.Controllers
{
    public class DepartmentController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DepartmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://grd-env.eba-szp3mgu2.us-east-2.elasticbeanstalk.com/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Grabs the authentication credentials which are sent to the Controller.
        /// This is NOT considered a proper authentication technique for the WebAPI. It piggybacks the existing authentication set up in the template for Individual User Accounts. Considering the existing scope and complexity of the course, it works for now.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }


        // GET: Department/List

        public ActionResult List()
        {
            string url = "departmentdata/getdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DepartmentDto> SelectedDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                return View(SelectedDepartments);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {

            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            if (response.IsSuccessStatusCode)
            {

                //Put data into Injury data transfer object
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);

            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        // GET: Department/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }


        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Department DepartmentInfo)
        {
            GetApplicationCookie();

            Debug.WriteLine(DepartmentInfo.DepartmentName);
            string url = "departmentdata/adddepartment";
            Debug.WriteLine(jss.Serialize(DepartmentInfo));
            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int DepartmentId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = DepartmentId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Department/Edit/2
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {

            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Put data into Injury data transfer object
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");

            }
        }


        // POST: Department/Edit/2
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Department DepartmentInfo)
        {

            GetApplicationCookie();


            Debug.WriteLine(DepartmentInfo.DepartmentName);
            string url = "departmentdata/updatedepartment/" + id;
            Debug.WriteLine(jss.Serialize(DepartmentInfo));
            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Department/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
          
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");

            }
        }


        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();

            string url = "departmentdata/deletedepartment/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;            
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
