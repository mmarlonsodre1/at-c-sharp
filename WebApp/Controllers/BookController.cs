using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BookController : Controller
    {
        private string BaseURL = "https://localhost:44399/api/";
        private string token = null;

        public async Task<IActionResult> Index(string? message)
        {
            ViewBag.Message = message;
            var books = await GetBooks();
            return View(books);
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
                return View("New", model);

            model.Id = Guid.NewGuid();
            var post = await PostBook(model);
            if (post) return RedirectToAction("Index", "Book", new { message = "cadastrado com sucesso" });
            else return RedirectToAction("Index", "Book", new { message = "erro ao cadastrar" });
        }

        public async Task<IActionResult> Edit([FromQuery] Guid id)
        {
            var book = await GetBookById(id);
            if (book != null) return View(book);
            else return RedirectToAction("Index", "Book", new { message = "erro ao carregar livro" });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(Guid id, Book model)
        {
            model.AuthorId = Guid.NewGuid().ToString();
            if (ModelState.IsValid == false)
                return View("Edit", model);

            var put = await PutBook(model);
            if (put) return RedirectToAction("Index", "Book", new { message = "editado com sucesso" });
            else return RedirectToAction("Index", "Book", new { message = "erro ao editar" });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var delete = await DeleteBook(id);
            if (delete) return RedirectToAction("Index", "Book", new { message = "excluído com sucesso" });
            else return RedirectToAction("Index", "Book", new { message = "erro ao excluir" });
        }

        private async Task<IEnumerable<Book>> GetBooks()
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.GetAsync("Books");

                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Book>>(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return Enumerable.Empty<Book>();
                }
            }
        }

        private async Task<Book> GetBookById(Guid id)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.GetAsync($"Books/{id}");

                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Book>(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return null;
                }
            }
        }

        private async Task<bool> PostBook(Book model)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var serializedModel = JsonConvert.SerializeObject(model);
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PostAsync("Books", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return false;
                }
            }
        }

        private async Task<bool> PutBook(Book model)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var serializedModel = JsonConvert.SerializeObject(model);
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PutAsync("Books", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return false;
                }
            }
        }

        private async Task<bool> DeleteBook(Guid id)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.DeleteAsync($"Books/{id}");

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return false;
                }
            }
        }
    }
}
