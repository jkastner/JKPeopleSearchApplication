using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using JKPeopleSearchApplication.Models;
using JKPersonSearcherModels;

namespace JKPeopleSearchApplication.Controllers
{
    public class PersonInformationController : Controller
    {
        private PersonInfoContext _personInfoContext = new PersonInfoContext();

        // GET: PersonInformation
        public ActionResult Search()
        {
            return View();
        }

        // GET: PersonInformation/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInformation personInformation = await _personInfoContext.AllPersonInfo.FindAsync(id);
            if (personInformation == null)
            {
                return HttpNotFound();
            }
            return View(personInformation);
        }

        // GET: PersonInformation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PersonInformationId,FirstName,LastName,Age,Address,Interests")] PersonInformation personInformation)
        {
            if (ModelState.IsValid)
            {
                _personInfoContext.AllPersonInfo.Add(personInformation);
                await _personInfoContext.SaveChangesAsync();
                return RedirectToAction("Search");
            }

            return View(personInformation);
        }

        // GET: PersonInformation/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInformation personInformation = await _personInfoContext.AllPersonInfo.FindAsync(id);
            if (personInformation == null)
            {
                return HttpNotFound();
            }
            return View(personInformation);
        }

        // POST: PersonInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PersonInformationId,FirstName,LastName,Age,Address,Interests")] PersonInformation personInformation)
        {
            if (ModelState.IsValid)
            {
                _personInfoContext.Entry(personInformation).State = EntityState.Modified;
                await _personInfoContext.SaveChangesAsync();
                return RedirectToAction("Search");
            }
            return View(personInformation);
        }

        // GET: PersonInformation/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInformation personInformation = await _personInfoContext.AllPersonInfo.FindAsync(id);
            if (personInformation == null)
            {
                return HttpNotFound();
            }
            return View(personInformation);
        }

        // POST: PersonInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PersonInformation personInformation = await _personInfoContext.AllPersonInfo.FindAsync(id);
            _personInfoContext.AllPersonInfo.Remove(personInformation);
            await _personInfoContext.SaveChangesAsync();
            return RedirectToAction("Search");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _personInfoContext.Dispose();
            }
            base.Dispose(disposing);
        }




        public ActionResult PersonSearchMatch()
        {
            string data = Request.Params["searchQuery"];
            string requestTimeStamp = Request.Params["timeStamp"];
            string serverDelay = Request.Params["serverDelay"];

            //Artificial delay for slowing search
            if (!String.IsNullOrWhiteSpace(serverDelay))
            {
                int delayVal = 0;
                if (int.TryParse(serverDelay, out delayVal))
                {
                    if (delayVal <= 15000 && delayVal >= 0)
                    {
                        Thread.Sleep(delayVal);
                    }
                }
            }
            
            var searchResult = PersonSearcher.SearchForMatch(data, _personInfoContext.AllPersonInfo.ToList());
            searchResult.RequestTimeStamp = requestTimeStamp;
            if (searchResult.SearchResults.Length > 20)
            {
                //The user hasn't entered in enough information to narrow it, don't return anything.
                var failMessage =
                    new SearchResultMessage(
                        $"{searchResult.SearchResults.Length} results for '{data}'. Please refine search.",
                        new PersonInformation[0], SearchResultType.TooManyResults);
                failMessage.RequestTimeStamp = requestTimeStamp;
                return Json(failMessage.ToJsonString(), JsonRequestBehavior.AllowGet);
            }
            return Json(searchResult.ToJsonString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNameSuggestions()
        {
            var curPeople = _personInfoContext.AllPersonInfo.ToList();
            var allNames = curPeople.Select(x => x.FullName).ToArray();
            Newtonsoft.Json.JsonConvert.SerializeObject(allNames);
            return Json(allNames, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> InsertSeedData()
        {
            var allCurrent = _personInfoContext.AllPersonInfo.ToArray();
            foreach (var curData in allCurrent)
            {
                _personInfoContext.AllPersonInfo.Remove(curData);
            }
            await _personInfoContext.SaveChangesAsync();
            var seedData = SeedData.GetSeedInformation();
            foreach (var cur in seedData)
            {
                if (ModelState.IsValid)
                {
                    _personInfoContext.AllPersonInfo.Add(cur);
                }
            }
            await _personInfoContext.SaveChangesAsync();


            return RedirectToAction("Search");
        }

    }
}
