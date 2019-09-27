using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChildDispalySchoolEdit.Models;

namespace ChildDispalySchoolEdit.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Child chdData = new Child();
            //chdData.DataChdNamCurrSchDOBProp = GetChdNameSchDOB();

            //return View(chdData);
            return View();
        }

        public ViewResult Search(string SearchString)
        {
            Child resultList = new Child();

            resultList.DataChdNamCurrSchDOBProp = GetChdNameSchDOB(SearchString);

            DBEntities context = new DBEntities();

            //var listOfSchoolIds = context.Schools
            //  .Select(m => m.Id);

            //var listOfSchoolNames = context.Schools
            //  .Select(m => m.Name);

            ViewBag.SchoolNameAndId = context.Schools
                .Select(x => new { schName = x.Name, schId = x.Id });


           // ViewBag.SchoolIds = listOfSchoolIds;

           // ViewBag.SchoolNames = listOfSchoolNames;


            return View(resultList);
        }

        public List<DataChdNamCurrSchDOB> GetChdNameSchDOB(string SearchString)
        {
            DBEntities context = new DBEntities();
            List<DataChdNamCurrSchDOB> result = new List<DataChdNamCurrSchDOB>();

            var obj = context.Children
                .Join(context.Contacts, ch => ch.ContactId, co => co.Id, (ch, co) => new { Child = ch, Contact = co })
                .Join(context.Schools, ch => ch.Child.SchoolId, sch => sch.Id, (ch, sch) => new { School = sch, Child = ch })
                .Select(m => new
                {
                    ChdName = m.Child.Child.Name,
                    SchName = m.School.Name,
                    DOB = m.Child.Contact.DateOfBirth,
                    ChdId = m.Child.Child.Id
                });

            

            if (obj != null)
            {
                foreach (var data in obj)
                {
                    DataChdNamCurrSchDOB dataChdNamSchDOB = new DataChdNamCurrSchDOB();

                    if(data.ChdName.Contains(SearchString))
                    {
                        dataChdNamSchDOB.ChdName = data.ChdName;
                        dataChdNamSchDOB.School = data.SchName;

                        dataChdNamSchDOB.DOB = Convert.ToDateTime(data.DOB);
                        dataChdNamSchDOB.ChdId = data.ChdId;

                        result.Add(dataChdNamSchDOB);
                    }
                }
            }
            return result;
        }
        
        [HttpPost]
        public ActionResult UpdateSchool(int SchoolNames, int ChdId)
        //public ActionResult UpdateSchool(int schId, string schName, int Id)
        {
            DBEntities context = new DBEntities();

            //var chdModel = item.ChdId;

           //var query = from x in context.Children where x.Id == myChdId select x;

            //foreach (var child in query)
            //    child.SchoolId = SchoolNames;

            context.SaveChanges();

            return RedirectToAction("Index");
        }
        
    }
}