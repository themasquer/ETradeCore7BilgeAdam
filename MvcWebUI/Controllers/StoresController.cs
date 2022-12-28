﻿#nullable disable
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")] // Controller üzerinde tanımlandığından sadece sisteme giriş yapmış yani authentication cookie'si olanlar
                                 // ve Admin rolündekiler bu controller'ın tüm action'larını çağırabilir
    public class StoresController : Controller
    {
        // Add service injections here
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // GET: Stores
        public IActionResult Index()
        {
            List<StoreModel> storeList = _storeService.Query().ToList(); // Add get list service logic here
            return View(storeList);
        }

        // GET: Stores/Details/5
        public IActionResult Details(int id)
        {
            StoreModel store = _storeService.Query().SingleOrDefault(s => s.Id == id); // Add get item service logic here
            if (store == null)
            {
                return View("_Error", "Store not found!");
            }
            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StoreModel store)
        {
            if (ModelState.IsValid)
            {
                // Add insert service logic here
                var result = _storeService.Add(store);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(store);
        }

        // GET: Stores/Edit/5
        public IActionResult Edit(int id)
        {
            StoreModel store = _storeService.Query().SingleOrDefault(s => s.Id == id); // Add get item service logic here
            if (store == null)
            {
                return View("_Error", "Store not found!");
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(store);
        }

        // POST: Stores/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StoreModel store)
        {
            if (ModelState.IsValid)
            {
                // Add update service logic here
                var result = _storeService.Update(store);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(store);
        }

        // GET: Stores/Delete/5
        public IActionResult Delete(int id)
        {
            StoreModel store = _storeService.Query().SingleOrDefault(s => s.Id == id); // Add get item service logic here
            if (store == null)
            {
                return View("_Error", "Store not found!");
            }
            return View(store);
        }

        // POST: Stores/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Add delete service logic here
            var result = _storeService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
	}
}
