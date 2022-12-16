#nullable disable
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    // MvcWebUI projesinde Controllers klasörü seçilerek fareye sağ tıklanıp Add -> Controller -> MVC Controller with views, using Entity Framework
    // seçildikten sonra Model class olarak Category (mutlaka entity seçilmeli), Data context class olarak da ETicaretContext seçildikten sonra
    // action view'larının da oluşturulması için Generate views işaretlenir, ilk aşamada client side validation yapmayacağımız için Reference script libraries
    // seçilmeden Use a layout page işaretlenip projenin tanımlanmış layout view'ının kullanılması için boş bırakılarak, son olarak da Controller name
    // istenilirse değiştirilip scaffolding ile controller, action'ları ve view'larının oluşturulması sağlanabilir.
    // Daha sonra da controller'da belirtilen yönlendirmeler üzerinden kodlar yazılır.

    public class CategoriesController : Controller
    {
        // Add service injections here
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public IActionResult Index()
        {
            List<CategoryModel> categoryList = _categoryService.Query().ToList(); // Add get list service logic here
            return View(categoryList);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(c => c.Id == id); // Add get item service logic here
            if (category == null)
            {
                return View("_Error", "Category not found!");
            }
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items

            // burada kategori dışında kullanacağımız herhangi bir model verimiz olmadığı ve bunları view'a taşımamız gerekmediği için ViewBag veya ViewData kullanmadık.

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                // Add insert service logic here
                var result = _categoryService.Add(category);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(c => c.Id == id); // Add get item service logic here
            if (category == null)
            {
                return View("_Error", "Category not found!");
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(category);
        }

        // POST: Categories/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                // Add update service logic here
                var result = _categoryService.Update(category);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Details), new { id = category.Id }); // Index action'ına dönmek yerine route value olarak id tanımlayarak Details action'ına dönüyoruz
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(c => c.Id == id); // Add get item service logic here
            if (category == null)
            {
                return View("_Error", "Category not found!");
            }
            return View(category);
        }

        // POST: Categories/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Add delete service logic here
            var result = _categoryService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
	}
}
