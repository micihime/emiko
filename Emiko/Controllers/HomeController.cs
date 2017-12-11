using Emiko.Models;
using ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emiko.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Evolution()
        {
            try
            {
                PopulationOfHaikus p = new PopulationOfHaikus(); //initial population of haiku poems
                p.Replace();
                p.SerializeXml();
                return View(p);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Evolution(PopulationOfHaikus p)
        {
            try
            {
                p.DeserializeXml();
                //interactive evolution of haiku poems will be performed
                if (p.Generation < 7)
                {
                    if (ModelState.IsValid)
                    {
                        //reevaluate- add human evaluation
                        for (int i = 0; i < p.Population.Count; i++)
                        {
                            var choice = p.Population[i].HumanFitness;
                            p.Population[i].write();
                        }
                        p.Replace();
                        p.SerializeXml();
                    }
                    return View(p);
                }
                //interactive evolution of haiku poems will stop in 10th generation and results will be shown to user 
                else
                {
                    return RedirectToAction("ThankYou");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", ex.Message);
            }
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public FileResult GetHaikus() //ActionResult
        {
            File f = new File();
            return File(f.Content, f.contentType, f.Name); //File(f.Content, f.Name);
        }

        public ActionResult Opinions()
        {
            return View();
        }

        //public FileContentResult GetFile(int id)
        //{
        //    SqlDataReader rdr; byte[] fileContent = null;
        //    string mimeType = ""; string fileName = "";
        //    const string connect = @"Server=.\SQLExpress;Database=FileTest;Trusted_Connection=True;";

        //    using (var conn = new SqlConnection(connect))
        //    {
        //        var qry = "SELECT FileContent, MimeType, FileName FROM FileStore WHERE ID = @ID";
        //        var cmd = new SqlCommand(qry, conn);
        //        cmd.Parameters.AddWithValue("@ID", id);
        //        conn.Open();
        //        rdr = cmd.ExecuteReader();
        //        if (rdr.HasRows)
        //        {
        //            rdr.Read();
        //            fileContent = (byte[])rdr["FileContent"];
        //            mimeType = rdr["MimeType"].ToString();
        //            fileName = rdr["FileName"].ToString();
        //        }
        //    }
        //    return File(fileContent, mimeType, fileName);
        //}

        public ActionResult Error(String message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}