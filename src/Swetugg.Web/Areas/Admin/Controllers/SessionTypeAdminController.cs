using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swetugg.Web.Models;

namespace Swetugg.Web.Areas.Admin.Controllers
{
    public class SessionTypeAdminController : ConferenceAdminControllerBase
    {
        public SessionTypeAdminController(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        [Route("{conferenceSlug}/session-types")]
        public async Task<ActionResult> Index()
        {
            var conferenceId = ConferenceId;
            var sessionTypes = from s in dbContext.SessionTypes
                where s.ConferenceId == conferenceId
                orderby s.Priority
                select s;
            var sessionTypeList = await sessionTypes.ToListAsync();

            return View(sessionTypeList);
        }

        [Route("{conferenceSlug}/session-types/edit/{id:int}", Order = 1)]
        public async Task<ActionResult> Edit(int id)
        {
            var sessionType = await dbContext.SessionTypes.SingleAsync(s => s.Id == id);
            return View(sessionType);
        }

        [HttpPost]
        [Route("{conferenceSlug}/session-types/edit/{id:int}", Order = 1)]
        public async Task<ActionResult> Edit(int id, SessionType sessionType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sessionType.ConferenceId = ConferenceId;
                    dbContext.Entry(sessionType).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Updating", ex);
                }
            }
            return View(sessionType);
        }

        [Route("{conferenceSlug}/session-types/new", Order = 2)]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("{conferenceSlug}/session-types/new", Order = 2)]
        public async Task<ActionResult> Edit(SessionType sessionType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sessionType.ConferenceId = ConferenceId;
                    dbContext.SessionTypes.Add(sessionType);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Updating", ex);
                }
            }
            return View(sessionType);
        }

    }
}