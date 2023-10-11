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
    public class ArticulosSolicitudsController : Controller
    {
        private readonly GranjaCoralesContext _context;
        string baseurl = "http://localhost:62718/";
        public ArticulosSolicitudsController(GranjaCoralesContext context)
        {
            _context = context;                
        }

        // GET: ArticulosSolicituds
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
            /*  var granjaCoralesContext = _context.ArticulosSolicitud.Include(a => a.IdArticuloNavigation).Include(a => a.IdSolicitudNavigation);
               return View(await granjaCoralesContext.ToListAsync());*/
            List<data.ArticulosSolicitud> aux = new List<data.ArticulosSolicitud>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/ArticulosSolicitud");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.ArticulosSolicitud>>(auxres);
                }
            }
            return View(aux);
        }

        // GET: ArticulosSolicituds/Details/5
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

               var articulosSolicitud = await _context.ArticulosSolicitud
                   .Include(a => a.IdArticuloNavigation)
                   .Include(a => a.IdSolicitudNavigation)
                   .FirstOrDefaultAsync(m => m.IdArticuloSolicitud == id);
               if (articulosSolicitud == null)
               {
                   return NotFound();
               }

               return View(articulosSolicitud);*/
            if (id == null)
            {
                return NotFound();
            }

            var articulosSolicitud = GetById(id);


            if (articulosSolicitud == null)
            {
                return NotFound();
            }

            return View(articulosSolicitud);
        }

        // GET: ArticulosSolicituds/Create
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
            ViewData["IdArticulo"] = new SelectList(getAllArticulos(), "IdArticulo", "IdArticulo");
            ViewData["IdSolicitud"] = new SelectList(getAllSolicitudes(), "IdSolicitud", "EstadoSolicitud");
            return View();
        }

        // POST: ArticulosSolicituds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSolicitud,IdArticulo")] ArticulosSolicitud articulosSolicitud)
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
                  _context.Add(articulosSolicitud);
                  await _context.SaveChangesAsync();
                  return RedirectToAction(nameof(Index));
              }*/
            if (ModelState.IsValid)
            {
                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    var content = JsonConvert.SerializeObject(articulosSolicitud);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var postTask = cl.PostAsync("api/ArticulosSolicitud", byteContent).Result;

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            ViewData["IdArticulo"] = new SelectList(getAllArticulos(), "IdArticulo", "IdArticulo", articulosSolicitud.IdArticulo);
            ViewData["IdSolicitud"] = new SelectList(getAllSolicitudes(), "IdSolicitud", "EstadoSolicitud", articulosSolicitud.IdSolicitud);
            return View(articulosSolicitud);
        }

        // GET: ArticulosSolicituds/Edit/5
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

            /* var articulosSolicitud = await _context.ArticulosSolicitud.FindAsync(id);
             if (articulosSolicitud == null)
             {
                 return NotFound();
             }*/
            var articulosSolicitud = GetById(id);
            if (articulosSolicitud == null)
            {
                return NotFound();
            }
            ViewData["IdArticulo"] = new SelectList(getAllArticulos(), "IdArticulo", "IdArticulo", articulosSolicitud.IdArticulo);
            ViewData["IdSolicitud"] = new SelectList(getAllSolicitudes(), "IdSolicitud", "EstadoSolicitud", articulosSolicitud.IdSolicitud);
            return View(articulosSolicitud);
        }

        // POST: ArticulosSolicituds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticuloSolicitud,IdSolicitud,IdArticulo")] ArticulosSolicitud articulosSolicitud)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            if (id != articulosSolicitud.IdArticuloSolicitud)
            {
                return NotFound();
            }

            /* if (ModelState.IsValid)
             {
                 try
                 {
                     _context.Update(articulosSolicitud);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!ArticulosSolicitudExists(articulosSolicitud.IdArticuloSolicitud))
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
                        var content = JsonConvert.SerializeObject(articulosSolicitud);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PutAsync("api/ArticulosSolicitud/" + id, byteContent).Result;

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
            ViewData["IdArticulo"] = new SelectList(getAllArticulos(), "IdArticulo", "IdArticulo", articulosSolicitud.IdArticulo);
            ViewData["IdSolicitud"] = new SelectList(getAllSolicitudes(), "IdSolicitud", "EstadoSolicitud", articulosSolicitud.IdSolicitud);
            return View(articulosSolicitud);
        }

        // GET: ArticulosSolicituds/Delete/5
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
            /*  if (id == null)
              {
                  return NotFound();
              }

              var articulosSolicitud = await _context.ArticulosSolicitud
                  .Include(a => a.IdArticuloNavigation)
                  .Include(a => a.IdSolicitudNavigation)
                  .FirstOrDefaultAsync(m => m.IdArticuloSolicitud == id);
              if (articulosSolicitud == null)
              {
                  return NotFound();
              }

              return View(articulosSolicitud);*/
            if (id == null)
            {
                return NotFound();
            }

            var articulosSolicitud = GetById(id);
            if (articulosSolicitud == null)
            {
                return NotFound();
            }

            return View(articulosSolicitud);
        }

        // POST: ArticulosSolicituds/Delete/5
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
            /*var articulosSolicitud = await _context.ArticulosSolicitud.FindAsync(id);
            _context.ArticulosSolicitud.Remove(articulosSolicitud);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/

            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/ArticulosSolicitud/" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));

        }

        private bool ArticulosSolicitudExists(int id)
        {
            // return _context.ArticulosSolicitud.Any(e => e.IdArticuloSolicitud == id);
            return (GetById(id) != null);
        }
        private data.ArticulosSolicitud GetById(int? id)
        {
            data.ArticulosSolicitud aux = new data.ArticulosSolicitud();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage res = await cl.GetAsync("api/Pais/5?"+id);
                HttpResponseMessage res = cl.GetAsync("api/ArticulosSolicitud/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.ArticulosSolicitud>(auxres);
                }
            }
            return aux;
        }


        private List<data.Articulos> getAllArticulos()
        {

            List<data.Articulos> aux = new List<data.Articulos>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Articulos").Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Articulos>>(auxres);
                }
            }
            return aux;
        }
        private List<data.Solicitudes> getAllSolicitudes()
        {

            List<data.Solicitudes> aux = new List<data.Solicitudes>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Solicitudes").Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Solicitudes>>(auxres);
                }
            }
            return aux;
        }
    }
}
