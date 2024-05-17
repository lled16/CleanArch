using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchMvc.WebUi.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryServices _categoryServices;
        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryServices.GetCategories();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                await _categoryServices.Add(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return NotFound();

            var categoryDTO = await _categoryServices.GetById(id);

            if (categoryDTO is null)
                return NotFound();

            return View(categoryDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryServices.Update(categoryDTO);
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(categoryDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var categoryDTO = await _categoryServices.GetById(id);

            if (categoryDTO is null)
                return NotFound();

            return View(categoryDTO);
        }

        [HttpPost(), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {

            await _categoryServices.Remove(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet()]
        public async Task<IActionResult> Details(int? id)
        {
            if(id is null)
                return NotFound();

            var categoryDTO = await _categoryServices.GetById(id);

            if (categoryDTO is null)
                return NotFound();

            return View(categoryDTO);
        }
    }
}



