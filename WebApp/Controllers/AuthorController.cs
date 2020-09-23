
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
    public class AuthorController : Controller
    {
        private string BaseURL = "https://localhost:44399/api/";
        private string token = null;

        public async Task<IActionResult> Index(string? message)
        {
            ViewBag.Message = message;
            var authors = await getAuthors();
            return View(authors);
        }

        public async Task<IActionResult> Books([FromQuery] Guid? id, string? message)
        {
            ViewBag.Message = message;
            if(id != null)
            {
                var books = await GetBooksByAuthorId(id.Value);
                if(books.Count() == 0) return RedirectToAction("Index", "Author", new { message = "autor sem livros" });
                else return View(books);
            } else
            {
                return RedirectToAction("Index", "Author", new { message = "erro ao carregar livros" });
            }
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
                return View("New", model);

            model.Id = Guid.NewGuid();
            var post = await PostAuthor(model);
            if(post) return RedirectToAction("Index", "Author", new { message = "cadastrado com sucesso" });
            else return RedirectToAction("Index", "Author", new { message = "erro ao cadastrar" });
        }

        public async Task<IActionResult> Edit([FromQuery] Guid id)
        {
            var author = await GetAuthorById(id);
            if (author != null) return View(author);
            else return RedirectToAction("Index", "Author", new { message = "erro ao carregar autor" });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(Guid id, Author model)
        {
            if (ModelState.IsValid == false)
                return View("Edit", model);

            var put = await PutAuthor(model);
            if(put) return RedirectToAction("Index", "Author", new { message = "editado com sucesso" });
            else return RedirectToAction("Index", "Author", new { message = "erro ao editar" });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var delete = await DeleteAuthor(id);
            if(delete) return RedirectToAction("Index", "Author", new { message = "excluído com sucesso" });
            else return RedirectToAction("Index", "Author", new { message = "erro ao excluir" });
        }

        private async Task<IEnumerable<Author>> getAuthors()
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.GetAsync("Authors");

                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Author>>(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return Enumerable.Empty<Author>();
                }
            }
        }

        private async Task<Author> GetAuthorById(Guid id)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.GetAsync($"Authors/{id}");

                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Author>(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return null;
                }
            }
        }

        private async Task<IEnumerable<Book>> GetBooksByAuthorId(Guid id)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.GetAsync($"Authors/Books/{id}");

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

        private async Task<bool> PostAuthor(Author model)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var serializedModel = JsonConvert.SerializeObject(model);
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PostAsync("Authors", content);

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

        private async Task<bool> PutAuthor(Author model)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var serializedModel = JsonConvert.SerializeObject(model);
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PutAsync("Authors", content);

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

        private async Task<bool> DeleteAuthor(Guid id)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.DeleteAsync($"Authors/{id}");

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
