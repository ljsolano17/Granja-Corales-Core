using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SolutionFrontEnd.Models;
using data = SolutionFrontEnd.Models;

namespace SolutionFrontEnd.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly GranjaCoralesContext _context;
        string baseurl = "http://localhost:62718/";
        public CategoriasController(GranjaCoralesContext context)
        {
            _context = context;      
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            List<data.Categorias> aux = new List<data.Categorias>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/Categorias");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Categorias>>(auxres);
                }
            }
            return View(aux);
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            /*   if (id == null)
               {
                   return NotFound();
               }

               var categorias = await _context.Categorias
                   .FirstOrDefaultAsync(m => m.IdCategoria == id);
               if (categorias == null)
               {
                   return NotFound();
               }

               return View(categorias);*/

            if (id == null)
            {
                return NotFound();
            }

            var categorias = GetById(id);


            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);

        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoria,Tipo")] Categorias categorias)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            /* if (ModelState.IsValid)
             {
                 _context.Add(categorias);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }*/
            if (ModelState.IsValid)
            {
                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    var content = JsonConvert.SerializeObject(categorias);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var postTask = cl.PostAsync("api/Categorias", byteContent).Result;

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(categorias);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            /* var categorias = await _context.Categorias.FindAsync(id);
             if (categorias == null)
             {
                 return NotFound();
             }
             return View(categorias);*/

            if (id == null)
            {
                return NotFound();
            }

            var categorias = GetById(id);
            if (categorias == null)
            {
                return NotFound();
            }
            return View(categorias);

        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoria,Tipo")] Categorias categorias)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            if (id != categorias.IdCategoria)
            {
                return NotFound();
            }

            /*if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriasExists(categorias.IdCategoria))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));


            }*/
            if (ModelState.IsValid)
            {
                try
                {
                    using (var cl = new HttpClient())
                    {
                        cl.BaseAddress = new Uri(baseurl);
                        var content = JsonConvert.SerializeObject(categorias);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PutAsync("api/Categorias/" + id, byteContent).Result;

                        if (postTask.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception)
                {
                    var aux2 = GetById(id);
                    if (aux2 == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categorias);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            /*     if (id == null)
                 {
                     return NotFound();
                 }

                 var categorias = await _context.Categorias
                     .FirstOrDefaultAsync(m => m.IdCategoria == id);
                 if (categorias == null)
                 {
                     return NotFound();
                 }

                 return View(categorias);*/
            if (id == null)
            {
                return NotFound();
            }

            var categorias = GetById(id);
            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            /* var categorias = await _context.Categorias.FindAsync(id);
             _context.Categorias.Remove(categorias);
             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));*/
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/Categorias/" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriasExists(int id)
        {
            // return _context.Categorias.Any(e => e.IdCategoria == id);
            return (GetById(id) != null);
        }


        private data.Categorias GetById(int? id)
        {
            data.Categorias aux = new data.Categorias();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage res = await cl.GetAsync("api/Pais/5?"+id);
                HttpResponseMessage res = cl.GetAsync("api/Categorias/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.Categorias>(auxres);
                }
            }
            return aux;
        }

    }
}
