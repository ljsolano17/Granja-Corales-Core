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
    public class ArticulosController : Controller
    {
        private readonly GranjaCoralesContext _context;
        string baseurl = "http://localhost:62718/";
        public ArticulosController(GranjaCoralesContext context)
        {
            _context = context;
        }

        // GET: Articulos
        public async Task<IActionResult> Index()
        {
            /*  if (User == null || !User.Identity.IsAuthenticated)
              {
                  return Redirect("Home/Index");
              }
              else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
              {
                  return LocalRedirect("/Home/Index");
              }

              var granjaCoralesContext = _context.Articulos.Include(a => a.IdCategoriaNavigation);
              return View(await granjaCoralesContext.ToListAsync());
              */
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }

            List<data.Articulos> aux = new List<data.Articulos>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/Articulos");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Articulos>>(auxres);
                }
            }
            return View(aux);
        }

        // GET: Articulos/Details/5
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

             var articulos = await _context.Articulos
                 .Include(a => a.IdCategoriaNavigation)
                 .FirstOrDefaultAsync(m => m.IdArticulo == id);
             if (articulos == null)
             {
                 return NotFound();
             }

             return View(articulos);*/

            if (id == null)
            {
                return NotFound();
            }

            var articulos = GetById(id);


            if (articulos == null)
            {
                return NotFound();
            }

            return View(articulos);
        }

        // GET: Articulos/Create
        public IActionResult Create()
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }

            ViewData["IdCategoria"] = new SelectList(GetAllCategories(), "IdCategoria", "Tipo");
            return View();
        }

        // POST: Articulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCientifico,Familia,Tipo,NombreComun,Dificultad,Temperamento,Color,Dieta,TamanoMax,Origen,TamanoMinPecera,ModificadoPor,FecModificacion,ImagePath,IdCategoria")] Articulos articulos)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }

            /*  if (ModelState.IsValid)
              {
                  articulos.ImagePath = "../../public/images/articulos/" + articulos.ImagePath;
                  _context.Add(articulos);
                  await _context.SaveChangesAsync();
                  return RedirectToAction(nameof(Index));
              }*/
            if (ModelState.IsValid)
            {
                articulos.ImagePath = "../../public/images/articulos/" + articulos.ImagePath;
                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    var content = JsonConvert.SerializeObject(articulos);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var postTask = cl.PostAsync("api/Articulos", byteContent).Result;

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            ViewData["IdCategoria"] = new SelectList(GetAllCategories(), "IdCategoria", "Tipo", articulos.IdCategoria);
            return View(articulos);
        }

        // GET: Articulos/Edit/5
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

            /*   var articulos = await _context.Articulos.FindAsync(id);
               if (articulos == null)
               {
                   return NotFound();
               }*/
            var articulos = GetById(id);
            if (articulos == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(GetAllCategories(), "IdCategoria", "Tipo", articulos.IdCategoria);
            return View(articulos);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticulo,NombreCientifico,Familia,Tipo,NombreComun,Dificultad,Temperamento,Color,Dieta,TamanoMax,Origen,TamanoMinPecera,ModificadoPor,FecModificacion,IdCategoria")] Articulos articulos)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }

            if (id != articulos.IdArticulo)
            {
                return NotFound();
            }

            /* if (ModelState.IsValid)
             {
                 try
                 {
                     _context.Update(articulos);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!ArticulosExists(articulos.IdArticulo))
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
                        var content = JsonConvert.SerializeObject(articulos);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PutAsync("api/Articulos/" + id, byteContent).Result;

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
            ViewData["IdCategoria"] = new SelectList(GetAllCategories(), "IdCategoria", "Tipo", articulos.IdCategoria);
            return View(articulos);
        }

        // GET: Articulos/Delete/5
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
            /* if (id == null)
             {
                 return NotFound();
             }

             var articulos = await _context.Articulos
                 .Include(a => a.IdCategoriaNavigation)
                 .FirstOrDefaultAsync(m => m.IdArticulo == id);
             if (articulos == null)
             {
                 return NotFound();
             }*/
            if (id == null)
            {
                return NotFound();
            }

            var articulos = GetById(id);
            if (articulos == null)
            {
                return NotFound();
            }

            

            return View(articulos);
        }

        // POST: Articulos/Delete/5
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

            /* var articulos = await _context.Articulos.FindAsync(id);
             _context.Articulos.Remove(articulos);
             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));*/
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/Articulos/" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ArticulosExists(int id)
        {
            // return _context.Articulos.Any(e => e.IdArticulo == id);
            return (GetById(id) != null);
        }
        private data.Articulos GetById(int? id)
        {
            data.Articulos aux = new data.Articulos();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Articulos/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.Articulos>(auxres);
                }
            }
            return aux;
        }
        private List<data.Categorias> GetAllCategories()
        {

            List<data.Categorias> aux = new List<data.Categorias>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Categorias").Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Categorias>>(auxres);
                }
            }
            return aux;
        }
        
    }
}
