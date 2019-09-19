using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Mvc;
using System.Web.Util;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Diagnostics;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FirstMVCApp.Controllers
{
    public class RITAPIController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        //public ActionResult UnderGrad()
        //{
        //    return View("UnderGrad");
        //}

        //public ActionResult Grad()
        //{
        //    return View("Grad");
        //}

        public ActionResult Degrees()
        {
            return View("Degrees");
        }

        public ActionResult SelectedDegree()
        {
            if (Session["returnedJSON"] != null)
            {
                return View("SelectDegree");
            }
            return RedirectToAction("Index");
        }

        public ActionResult DisplayAbout()
        {
            if (Session["returnedJSON"] != null)
            {
                return View("AboutInfo");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Minors()
        {
            if (Session["returnedJSON"] != null)
            {
                return View("Minors");
            }
            return RedirectToAction("Index");
        }

        public ActionResult People()
        {
            if (Session["returnedJSON"] != null)
            {
                return View("People");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Map()
        {
            return View("Map");
        }

        public async Task<JsonResult> SelectAbout()
        {
            var returnedJSON = await GetAbout();
            if (returnedJSON == null)
            {
                return ThrowJsonError(new Exception(String.Format("No Data found.")));
            }
            Session["returnedJSON"] = returnedJSON;
            return null;
        }

        public async Task<JsonResult> SelectMinors()
        {
            var returnedJSON = await GetMinors();
            if (returnedJSON == null)
            {
                return ThrowJsonError(new Exception(String.Format("No Data found.")));
            }
            Session["returnedJSON"] = returnedJSON;
            return null;
        }

        public static async Task<Object> GetAbout()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.ist.rit.edu/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/about/", HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    var rtnResults = JsonConvert.DeserializeObject(data);
                    return rtnResults;
                }
                catch (HttpRequestException hre)
                {
                    var msg = hre.Message;
                    return "HttpRequestException";
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return "Exception";
                }
            }
        }

        public static async Task<Object> GetMinors()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.ist.rit.edu/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/minors/", HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    var rtnResults = JsonConvert.DeserializeObject(data);
                    return rtnResults;
                }
                catch (HttpRequestException hre)
                {
                    var msg = hre.Message;
                    return "HttpRequestException";
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return "Exception";
                }
            }
        }

        public async Task<JsonResult> GetFaculty()
        {
            var returnedJSON = await GetPeople("faculty");
            if (returnedJSON == null)
            {
                return ThrowJsonError(new Exception(String.Format("No faculty found.")));
            }
            Session["returnedJSON"] = returnedJSON;
            return null;
        }

        public async Task<JsonResult> GetStaff()
        {
            var returnedJSON = await GetPeople("staff");
            if (returnedJSON == null)
            {
                return ThrowJsonError(new Exception(String.Format("No staff found.")));
            }
            Session["returnedJSON"] = returnedJSON;
            return null;
        }

        public static async Task<Object> GetPeople(string people)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://www.ist.rit.edu/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    if (people == "faculty")
                    {
                        HttpResponseMessage response = await client.GetAsync("api/people/faculty", HttpCompletionOption.ResponseHeadersRead);
                        response.EnsureSuccessStatusCode();
                        var data = await response.Content.ReadAsStringAsync();
                        var rtnResults = JsonConvert.DeserializeObject(data);
                        return rtnResults;
                    }
                    else if (people == "staff")
                    {
                        HttpResponseMessage response = await client.GetAsync("api/people/staff", HttpCompletionOption.ResponseHeadersRead);
                        response.EnsureSuccessStatusCode();
                        var data = await response.Content.ReadAsStringAsync();
                        var rtnResults = JsonConvert.DeserializeObject(data);
                        return rtnResults;
                    }
                    return null;
                }
                catch (HttpRequestException hre)
                {
                    var msg = hre.Message;
                    return "HttpRequestException";
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return "Exception";
                }
            }
        }

        public async Task<JsonResult> SelectLocation()
        {
            var returnedJSON = await GetLocationData();
            if (returnedJSON == null)
            {
                return ThrowJsonError(new Exception(String.Format("No location info found")));
            }
            Session["returnedJSON"] = returnedJSON;
            return null;
        }

        public static async Task<Object> GetLocationData()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.ist.rit.edu/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/location/", HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    var rtnResults = JsonConvert.DeserializeObject(data);
                    return rtnResults;
                }
                catch (HttpRequestException hre)
                {
                    var msg = hre.Message;
                    return "HttpRequestException";
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return "Exception";
                }
            }
        }

        public async Task<JsonResult> GetUnderGrad()
        {
            var returnedJSON = await GetUndergradDegrees("under");
            if (returnedJSON == null)
            {
                return ThrowJsonError(new Exception(String.Format("No Degree Programs found.")));
            }
            Session["returnedJSON"] = returnedJSON;
            return null;
        }

        public async Task<JsonResult> GetGrad()
        {
            var returnedJSON = await GetUndergradDegrees("grad");
            if (returnedJSON == null)
            {
                return ThrowJsonError(new Exception(String.Format("No Degree Programs found.")));
            }
            Session["returnedJSON"] = returnedJSON;
            return null;
        }

        public static async Task<Object> GetUndergradDegrees(string degree)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://www.ist.rit.edu/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                try
                {
                    if (degree == "under")
                    {
                        HttpResponseMessage response = await client.GetAsync("api/degrees/undergraduate/", HttpCompletionOption.ResponseHeadersRead);
                        response.EnsureSuccessStatusCode();
                        var data = await response.Content.ReadAsStringAsync();
                        var rtnResults = JsonConvert.DeserializeObject(data);
                        return rtnResults;
                    }
                    else if (degree == "grad")
                    {
                        HttpResponseMessage response = await client.GetAsync("api/degrees/graduate/", HttpCompletionOption.ResponseHeadersRead);
                        response.EnsureSuccessStatusCode();
                        var data = await response.Content.ReadAsStringAsync();
                        var rtnResults = JsonConvert.DeserializeObject(data);
                        return rtnResults;
                    }
                    return null;
                }
                catch (HttpRequestException hre)
                {
                    var msg = hre.Message;
                    return "HttpRequestException";
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return "Exception";
                }
            }
        }

        public JsonResult ThrowJsonError(Exception e)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            Response.StatusDescription = e.Message;

            return Json(new { Message = e.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}
