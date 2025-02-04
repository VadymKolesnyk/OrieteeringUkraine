﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public class EventController : ControllerBase
    {
        public EventController(IDataManager dataManager, ICacheManager cacheManager) : base(dataManager, cacheManager) { }
        private void SetSelectLists()
        {
            var regions = cacheManager.GetRegions();
            if (regions == null)
            {
                regions = dataManager.GetAllRegions();
                cacheManager.SetRegions(regions);
            }
            ViewBag.Regions = new SelectList(regions, "Id", "Name");
        }
        public IActionResult Applications(int id)
        {
            var model = dataManager.GetApplicationsById(id);
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ShowAdminModule = (User.IsInRole("admin") || User.IsInRole("moderator") || User.Identity.Name == model.OrganizerLogin);
            return View(model);
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        [HttpGet]
        public IActionResult New()
        {
            SetSelectLists();
            return View();
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        [HttpPost]
        public IActionResult New(EventData data)
        {
            SetSelectLists();
            if (ModelState.IsValid)
            {
                data.OrganizerLogin = User.Identity.Name;
                int id = dataManager.AddNewEvent(data);
                return RedirectToAction("Applications", new { Id = id });
            }
            return View(data);
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            SetSelectLists();
            var @event = dataManager.GetEventById(id);
            if (User.IsInRole("organizer") && User.Identity.Name != @event?.OrganizerLogin)
            {
                return RedirectToAction("Applications", new { Id = id });
            }
            if (@event == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(@event);
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        [HttpPost]
        public IActionResult Edit(int id, EventData data)
        {
            SetSelectLists();
            if (ModelState.IsValid)
            {
                string groups = dataManager.UpdateEvent(id, data);
                if (groups != "" && groups != null)
                {
                    ModelState.AddModelError("", $"Не удалось удалить группы \"{groups}\", по скольку в эти группы заявленны участники");
                }
                return View(data);
            }
            ModelState.AddModelError("", "Некоректнные изменения");
            return View(data);
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        public IActionResult Export(int id)
        {
            return RedirectToAction("Applications", new { Id = id });
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        public IActionResult Delete(int id)
        {
            var @event = dataManager.GetEventById(id);
            if (User.IsInRole("organizer") && User.Identity.Name != @event?.OrganizerLogin)
            {
                return RedirectToAction("Index", "Home");
            }
            dataManager.DeleteEvent(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
