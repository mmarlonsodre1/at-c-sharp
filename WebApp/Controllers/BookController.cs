using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index(string? message)
        {
            ViewBag.Message = message;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Login", new { message = "Usuário deslogado" });
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(Book model)
        {
            if (ModelState.IsValid == false)
                return View();

            model.Id = Guid.NewGuid();
            //await postBook(model);

            return RedirectToAction("Index", "Book", new { message = "cadastrado com sucesso" });
        }

        public async Task<IActionResult> Edit([FromQuery] Guid id)
        {
            //var friend = await getFriendForId(id);
            //return View(friend);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(Guid id, Book model)
        {
            if (ModelState.IsValid == false)
                return View();

            //await putBook(model);

            return RedirectToAction("Index", "Book", new { message = "editado com sucesso" });
        }

        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid == false)
                return View();

            //DeleteBook(id);
            return RedirectToAction("Index", "Book", new { message = "excluído com sucesso" });
        }
    }
}
