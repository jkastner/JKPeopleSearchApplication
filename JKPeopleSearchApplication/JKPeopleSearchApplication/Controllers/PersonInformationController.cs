using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using JKPeopleSearchApplication.Models;
using JKPersonSearcherModels;

namespace JKPeopleSearchApplication.Controllers
{
    public class PersonInformationController : Controller
    {
        private PersonInfoContext _personInfoContext = new PersonInfoContext();

        // GET: PersonInformation
        public async Task<ActionResult> Index()
        {
            return View(await _personInfoContext.AllPersonInfo.ToListAsync());
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
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _personInfoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private static string _noResults= SearchResultMessage.JsonFailResultWithMessage("Search yielded no results");
        private static string _emptySearch = SearchResultMessage.JsonFailResultWithMessage("");

        public ActionResult PersonSearchMatch()
        {

            //Note to self: So after doing some researching, it appears that returning many results 
            //could be a security vulnerability.
            //In retrospect, this makes sense -- it's not a lot of data unless someone is attempting 
            //a DOS or there are many simulatenous users.
            //Likewise, we don't want to flood a regular user with thousands of results 
            //(although I like that look as it shows all my pretty data, a real user won't care.
            string data = Request.Params["searchQuery"];
            if (String.IsNullOrWhiteSpace(data))
            {
                return Json(_emptySearch, JsonRequestBehavior.AllowGet);
            }
            var trimmedData = data.Trim().ToLower();
            //If there are any perfect matches, we should just return those.
            var perfectMatches = _personInfoContext.AllPersonInfo.Where(
                x => (x.FirstName + " " + x.LastName).ToLower().Equals(data)).ToArray();
            if (perfectMatches.Any())
            {
                //It's possible that there could be so many perfect matches that the system could flood.
                //But for this sample project we're ignoring that unlikely scenario.
                //The user would have to have some way of narrowing which John Smith they're looking for anyway.
                var response = SearchResultMessage.JsonSuccessResultWithMessage(perfectMatches);
                return Json(response);
            }
            var matchingPeople = _personInfoContext.AllPersonInfo.Where(
                x =>
                    x.FirstName.ToLower().Contains(trimmedData) ||
                    x.LastName.ToLower().Contains(trimmedData)
                ).ToArray();
            //The user hasn't entered in enough information to narrow it, don't return anything.
            if (matchingPeople.Length > 15)
            {
                return
                    Json(
                        SearchResultMessage.JsonFailResultWithMessage(
                            $"{matchingPeople.Length} results. Please refine search."),
                        JsonRequestBehavior.AllowGet);
            }

            if (!matchingPeople.Any())
            {
                return Json(_noResults, JsonRequestBehavior.AllowGet);
            }
            var response2 = SearchResultMessage.JsonSuccessResultWithMessage(matchingPeople);
            return Json(response2, JsonRequestBehavior.AllowGet);
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


            return RedirectToAction("Index");
        }

    }
}
