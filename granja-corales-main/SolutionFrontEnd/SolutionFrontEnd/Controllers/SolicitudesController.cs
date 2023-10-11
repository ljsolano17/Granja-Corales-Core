using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using SolutionFrontEnd.Models;
using data = SolutionFrontEnd.Models;

namespace SolutionFrontEnd.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly GranjaCoralesContext _context;
        string baseurl = "http://localhost:62718/";
        public SolicitudesController(GranjaCoralesContext context)
        {
            _context = context;
        }

        // GET: Solicitudes
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
             // var solicitudes = await _context.Solicitudes.ToListAsync();

              /* foreach (var solicitud in solicitudes)
               {
                   var soli = await _context.Users.FirstOrDefaultAsync(m => m.Id == solicitud.IdUsuario);
                   solicitud.IdUsuario = soli.UserName;

               }*/
            List<data.Solicitudes> aux = new List<data.Solicitudes>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/Solicitudes");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Solicitudes>>(auxres);
                    foreach (var solicitud in aux)
                    {
                        var soli = await _context.Users.FirstOrDefaultAsync(m => m.Id == solicitud.IdUsuario);
                        solicitud.IdUsuario = soli.UserName;

                    }
                }
            }
          

            return View(aux);
        }

        // GET: Solicitudes/Details/5
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

               var solicitudes = await _context.Solicitudes
                   .FirstOrDefaultAsync(m => m.IdSolicitud == id);
               if (solicitudes == null)
               {
                   return NotFound();
               }

               return View(solicitudes);*/

            if (id == null)
            {
                return NotFound();
            }

            var solicitudes = GetById(id);


            if (solicitudes == null)
            {
                return NotFound();
            }

            return View(solicitudes);

        }

        // GET: Solicitudes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Solicitudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FecCreacion,EstadoSolicitud,IdUsuario")] Solicitudes solicitudes)
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
                  _context.Add(solicitudes);
                  await _context.SaveChangesAsync();
                  return RedirectToAction(nameof(Index));
              }*/
            if (ModelState.IsValid)
            {
                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    var content = JsonConvert.SerializeObject(solicitudes);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var postTask = cl.PostAsync("api/Solicitudes", byteContent).Result;

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }



            return View(solicitudes);
        }

        // GET: Solicitudes/Edit/5
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
          /*  if (id == null)
            {
                return NotFound();
            }

              var solicitudes = await _context.Solicitudes.FindAsync(id);
              if (solicitudes == null)
              {
                  return NotFound();
              }
              return View(solicitudes);*/

            if (id == null)
            {
                return NotFound();
            }

            var solicitudes = GetById(id);
            if (solicitudes == null)
            {
                return NotFound();
            }
            return View(solicitudes);

        }

        // POST: Solicitudes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSolicitud,FecCreacion,EstadoSolicitud,EstadoAprobacion,IdUsuario")] Solicitudes solicitudes)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }
            if (id != solicitudes.IdSolicitud)
            {
                return NotFound();
            }

            /* if (ModelState.IsValid)
             {
                 try
                 {
                     solicitudes.EstadoSolicitud = "1";
                     _context.Update(solicitudes);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!SolicitudesExists(solicitudes.IdSolicitud))
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
                        var content = JsonConvert.SerializeObject(solicitudes);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PutAsync("api/Solicitud/" + id, byteContent).Result;

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

            return View(solicitudes);
        }

        // GET: Solicitudes/Delete/5
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
            /*   if (id == null)
               {
                   return NotFound();
               }

               var solicitudes = await _context.Solicitudes
                   .FirstOrDefaultAsync(m => m.IdSolicitud == id);
               if (solicitudes == null)
               {
                   return NotFound();
               }
               */
            if (id == null)
            {
                return NotFound();
            }

            var solicitudes = GetById(id);
            if (solicitudes == null)
            {
                return NotFound();
            }
            return View(solicitudes);
        }

        // POST: Solicitudes/Delete/5
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
            /*  var solicitudes = await _context.Solicitudes.FindAsync(id);
              _context.Solicitudes.Remove(solicitudes);
              await _context.SaveChangesAsync();*/
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/Solicitudes/" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitudesExists(int id)
        {
            //  return _context.Solicitudes.Any(e => e.IdSolicitud == id);
            return (GetById(id) != null);
        }

        private data.Solicitudes GetById(int? id)
        {
            data.Solicitudes aux = new data.Solicitudes();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage res = await cl.GetAsync("api/Pais/5?"+id);
                HttpResponseMessage res = cl.GetAsync("api/Solicitudes/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.Solicitudes>(auxres);
                }
            }
            return aux;
        }

        public async Task<IActionResult> ReporteSolicitud(int id)
        {
            //if (User == null || !User.Identity.IsAuthenticated)
            //{
            //    return Redirect("Home/Index");
            //}
            //else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            //{
            //    return LocalRedirect("/Home/Index");
            //}

            var solicitudes = await _context.Solicitudes.Include(m => m.ArticulosSolicitud)
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);

            if (solicitudes == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == solicitudes.IdUsuario);
            solicitudes.IdUsuario = user.UserName;

            foreach (var item in solicitudes.ArticulosSolicitud)
            {
                var articulo = _context.Articulos.FirstOrDefault(m => m.IdArticulo == item.IdArticulo);
                item.IdArticuloNavigation = articulo;
            }

            return new ViewAsPdf("ReporteSolicitud", solicitudes) { };
        }

    }
}
