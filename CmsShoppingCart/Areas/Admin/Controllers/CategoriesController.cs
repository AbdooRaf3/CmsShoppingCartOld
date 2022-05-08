using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {

        private readonly CmsShoppingCartContext context;

        public CategoriesController(CmsShoppingCartContext context)
        {
            this.context = context;
        }

        // GET /admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }

        //GET  /admin/categories/Create new category
        public IActionResult Create()
        {
            return View();
        }


        // POST /admin/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exists.");
                    return View(category);
                }

                context.Add(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The category has been added!";



                return RedirectToAction("Index");


            }

            return View(category);
        }



        //GET  /admin/categories/edit/id(some-Id lik 5)
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        // POST /admin/categories/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");

                var slug = await context.Pages.Where(x => x.Id != category.Id).FirstOrDefaultAsync(x => x.Slug == category.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists.");
                    return View(category);
                }

                context.Update(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been Edited!";

                return RedirectToAction("Edit", new { id = category.Id });

            }

            return View(category);
        }




        //GET  /admin/categories/delete/id(some-Id lik 5)
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category dose not exit!";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The category has been deleted!";

            }
            return RedirectToAction("Index");
        }







        // POST /admin/pages/categories
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var categoryId in id)
            {
                Category category = await context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }

            return Ok();
        }
    }
}