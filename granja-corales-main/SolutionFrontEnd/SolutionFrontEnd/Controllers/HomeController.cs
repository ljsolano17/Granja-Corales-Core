using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SolutionFrontEnd.Models;
using data = SolutionFrontEnd.Models;

namespace SolutionFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        string baseurl = "http://localhost:62718/";

        private readonly GranjaCoralesContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, GranjaCoralesContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetPeces()
        {

            List<data.Articulos> aux = new List<data.Articulos>();
            List<ArticulosLista> listaArticulos = new List<ArticulosLista>();
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

        public async Task<IActionResult> GetCorales()
        {

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

        public async Task<IActionResult> Details(int? id)
        {


            ArticulosLista listaArticulos = new ArticulosLista();
            

            if (id == null)
            {
                return NotFound();
            }

            var articulo = GetById(id);
            

            if (articulo == null)
            {
                return NotFound();
            }
            if (TempData["MessageSuccess"] != null)
            {
                ViewBag.Exito = TempData["MessageSuccess"].ToString();
            }
            else if (TempData["MessageDanger"] != null)
            {
                ViewBag.Error = TempData["MessageDanger"].ToString();
            }
            listaArticulos.articulo = articulo;
            return View(listaArticulos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddArticuloSolicitud([Bind("articulo,Cantidad")] ArticulosLista articulos)
        {
            if (ModelState.IsValid)
             {
                 var applicationUser = await _userManager.GetUserAsync(User);
                 String userid = applicationUser.Id;

                 var solicitudExistente = _context.Solicitudes.Where(m => m.EstadoSolicitud == "0" && m.IdUsuario == userid).FirstOrDefault();

                 if (solicitudExistente == null)
                 {
                     try
                     {
                         Solicitudes newsolicitud = new Solicitudes();
                         newsolicitud.FecCreacion = DateTime.Now;
                         newsolicitud.EstadoSolicitud = "0";
                         newsolicitud.IdUsuario = userid;               

                         _context.Add(newsolicitud);
                         await _context.SaveChangesAsync();

                         var solicitud = _context.Solicitudes.Where(m => m.EstadoSolicitud == "0" && m.IdUsuario == userid).FirstOrDefault();

                         ArticulosSolicitud articulo = new ArticulosSolicitud();
                         articulo.IdArticulo = articulos.articulo.IdArticulo;
                         articulo.IdSolicitud = solicitud.IdSolicitud;
                         articulo.Cantidad = articulos.Cantidad;

                         _context.Add(articulo);
                         await _context.SaveChangesAsync();
                     }
                     catch (Exception)
                     {
                         TempData["MessageDanger"] = "Ha ocurrido un error, no se ha podido agregar su artículo a la lista de solicitudes";

                         return RedirectToAction("Details", new { id = articulos.articulo.IdArticulo });
                     }
                 }
                 else
                 {
                     ArticulosSolicitud articulo = new ArticulosSolicitud();
                     articulo.IdArticulo = articulos.articulo.IdArticulo;
                     articulo.IdSolicitud = solicitudExistente.IdSolicitud;
                     articulo.Cantidad = articulos.Cantidad;

                     _context.Add(articulo);
                     await _context.SaveChangesAsync();
                 }


                TempData["MessageSuccess"] = "¡Articulo agregado con éxito!";

        }


            return RedirectToAction("Details", new { id = articulos.articulo.IdArticulo });
        }

        public async Task<IActionResult> ListaDeseos()
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            String userid = applicationUser.Id;
            List<ArticulosLista> articulos = new List<ArticulosLista>();

            var solicitud = _context.Solicitudes.Where(m => m.IdUsuario == userid && m.EstadoSolicitud == "0").FirstOrDefault();

            if(solicitud != null)
            {
                var articuloSolicitud = _context.ArticulosSolicitud.Where(m => m.IdSolicitud == solicitud.IdSolicitud).ToList();
                
                if(articuloSolicitud != null)
                {                   
                    foreach (var articulo in articuloSolicitud)
                    {
                        var art = _context.Articulos.Where(m => m.IdArticulo == articulo.IdArticulo).FirstOrDefault();
                        ArticulosLista artList = new ArticulosLista();
                        artList.articulo = art;
                        artList.Cantidad = articulo.Cantidad;
                        articulos.Add(artList);
                    }
                }               
            }

            if (TempData["MessageSuccess"] != null)
            {
                ViewBag.Exito = TempData["MessageSuccess"].ToString();
            }

            return View(articulos);
        }

        public async Task<IActionResult> EliminarArticulo(int id)//id = 3
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            String userid = applicationUser.Id;


            var solicitud = _context.ArticulosSolicitud.Where(m => m.IdArticulo == id).FirstOrDefault();

            int idArticulo = solicitud.IdArticuloSolicitud;
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/ArticulosSolicitud/" + idArticulo);

                if (res.IsSuccessStatusCode)
                {
                    TempData["MessageSuccess"] = "¡Articulo eliminado con éxito!";
                }
                else
                {
                    TempData["MessageSuccess"] = "Error, el articulo no se pudo eliminar";
                }
            }

            return RedirectToAction(nameof(ListaDeseos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarArticulo([Bind("articulo,Cantidad")] ArticulosLista articulos)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            String userid = applicationUser.Id;

            var solicitud = _context.Solicitudes.Where(m => m.IdUsuario == userid && m.EstadoSolicitud == "0").FirstOrDefault();

            if (solicitud != null)
            {
                if(articulos.Cantidad > 0)
                {
                    var articulo = _context.ArticulosSolicitud.Where(m => m.IdSolicitud == solicitud.IdSolicitud && m.IdArticulo == articulos.articulo.IdArticulo).FirstOrDefault();
                    articulo.Cantidad = articulos.Cantidad;
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }              
            }

             return RedirectToAction(nameof(ListaDeseos));
        }

        public async Task<IActionResult> RealizarSolicitud()
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            String userid = applicationUser.Id;

            var solicitud = _context.Solicitudes.Where(m => m.IdUsuario == userid && m.EstadoSolicitud == "0").FirstOrDefault();
            solicitud.EstadoSolicitud = "1";
            solicitud.FecCreacion = DateTime.Now;
            solicitud.EstadoAprobacion = "Enviada";
            await _context.SaveChangesAsync();

            TempData["MessageSuccess"] = "¡Solicitud realizada con éxito!";

            return RedirectToAction(nameof(ListaDeseos));
        }

        public async Task<IActionResult> MisSolicitudes()
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            String userid = applicationUser.Id;

            var solicitudes = _context.Solicitudes.Where(m => m.IdUsuario == userid && m.EstadoSolicitud == "1").ToList();
     
            return View(solicitudes);
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
    }
}
