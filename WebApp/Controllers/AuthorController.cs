
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AuthorController : Controller
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
        public async Task<IActionResult> Save(Author model)
        {
            if (ModelState.IsValid == false)
                return View();

            model.Id = Guid.NewGuid();
            //await postAuthor(model);

            return RedirectToAction("Index", "Author", new { message = "cadastrado com sucesso" });
        }

        public async Task<IActionResult> Edit([FromQuery] Guid id)
        {
            //var friend = await getFriendForId(id);
            //return View(friend);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(Guid id, Author model)
        {
            if (ModelState.IsValid == false)
                return View();

            //await putAuthor(model);

            return RedirectToAction("Index", "Author", new { message = "editado com sucesso" });
        }

        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid == false)
                return View();

            //DeleteAuthor(id);
            return RedirectToAction("Index", "Author", new { message = "excluído com sucesso" });
        }
    }
}
