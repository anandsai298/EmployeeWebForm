using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using static iTextSharp.text.pdf.AcroFields;
using Document = iTextSharp.text.Document;

namespace EmployeeWebForm.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeManagementEntities1 db = new EmployeeManagementEntities1();

        // GET: Employee
        public JsonResult Index()
        {
            return Json(db.Employees.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Studentinfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee emp = db.Employees.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // GET: Studentinfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public string Create([Bind(Include = "EmployeeID,EmployeeName,EmailID,PhoneNumber,Address,DateOfBirth,DateOfJoining,Salary")] Employee emp)
        {
            
                db.Employees.Add(emp);
                db.SaveChanges();
                return "Employee Created";
           
        }

        // GET: Studentinfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee emp = db.Employees.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Data = serializer.Serialize(emp);
            ViewData["create"] = "hidden";
            ViewData["Save"] = "button";
            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Edit([Bind(Include = "EmployeeID,EmployeeName,EmailID,PhoneNumber,Address,DateOfBirth,DateOfJoining,Salary")] Employee emp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return "Employee Edited";
            }
            return "Edited failed";
        }


        /* GET: Home/Delete/5  
        public ActionResult Delete(int? id)
        {
            if (id == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var employee = db.Employees.Find(id);
            if (employee == null) 
                return HttpNotFound();
            var serializer = new JavaScriptSerializer();
            ViewBag.SelectedEmployee = serializer.Serialize(employee);
            return View();
        }*/
        // POST: Home/Delete/5  
        [ActionName("Delete")]
        public string Delete(int Id)
        {
            Employee emp = db.Employees.Find(Id);
            if (emp != null)
            {
                db.Employees.Remove(emp);
                db.SaveChanges();
                return "Student Deleted";
            }
            return "Delete failed";
        }
        public FileResult Export(string id)
        {
            List<Employee> emp = db.Employees.ToList();

            //Building an HTML string.
            StringBuilder sb = new StringBuilder();

            //Table start.
            sb.Append("<table border='1' cellpadding='5' cellspacing='0'  style='border: 1px solid #ccc; font-family: Arial; font-size: 10pt;'>");

            sb.Append("<h2 style='text-align:center'><strong>EMPLOYEE MANAGEMENT</strong></h2>");
            sb.Append("<tr>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>EmployeeID</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>EmployeeName</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>EmailID</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>PhoneNumber</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Address</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>DateOfBirth</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>DateOfJoining</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Salary</th>");
            sb.Append("</tr>");
            //Building the Data rows.
           

            for (int i = 0; i < emp.Count; i++)
            {
                var item = emp[i];
                sb.Append("<tr>");
                sb.Append("<td style='border: 1px solid #ccc'>");
                sb.Append(item.EmployeeID);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc'>");
                sb.Append(item.EmployeeName);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc'>");
                sb.Append(item.EmailID);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc'>");
                sb.Append(item.PhoneNumber);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc'>");
                sb.Append(item.Address);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc'>");
                sb.Append(item.DateOfBirth);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc'>");
                sb.Append(item.DateOfJoining);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc'>");
                sb.Append(item.Salary);
                sb.Append("</td>");
                sb.Append("</tr>");
            }

            //Table end.
            sb.Append("</table>");

            if (id == "excel")
            {
                return File(Encoding.ASCII.GetBytes(sb.ToString()), "application/vnd.ms-excel", "Grid.xls");

            }

            using (MemoryStream stream = new MemoryStream())
            {
                StringReader sr = new StringReader(sb.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}  
