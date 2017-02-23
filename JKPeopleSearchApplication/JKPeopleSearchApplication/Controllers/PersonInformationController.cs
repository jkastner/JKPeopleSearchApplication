using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JKPeopleSearchApplication.Models;
using JKPersonSearcherModels;

namespace JKPeopleSearchApplication.Controllers
{
    public class PersonInformationController : Controller
    {
        private PersonInfoContext _personInfoContext = new PersonInfoContext();

        // GET: PersonInformations
        public async Task<ActionResult> Index()
        {
            return View(await _personInfoContext.AllPersonInfo.ToListAsync());
        }

        // GET: PersonInformations/Details/5
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

        // GET: PersonInformations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PersonInformationId,FirstName,LastName")] PersonInformation personInformation)
        {
            if (ModelState.IsValid)
            {
                _personInfoContext.AllPersonInfo.Add(personInformation);
                await _personInfoContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(personInformation);
        }

        // GET: PersonInformations/Edit/5
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

        // POST: PersonInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PersonInformationId,FirstName,LastName")] PersonInformation personInformation)
        {
            if (ModelState.IsValid)
            {
                _personInfoContext.Entry(personInformation).State = EntityState.Modified;
                await _personInfoContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(personInformation);
        }

        // GET: PersonInformations/Delete/5
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

        // POST: PersonInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PersonInformation personInformation = await _personInfoContext.AllPersonInfo.FindAsync(id);
            _personInfoContext.AllPersonInfo.Remove(personInformation);
            await _personInfoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public PartialViewResult GetAllPeople()
        {
            var allPeople = _personInfoContext.AllPersonInfo.ToArray();
            return PartialView(allPeople);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _personInfoContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
