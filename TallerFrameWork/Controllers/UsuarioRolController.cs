using System;
using System.Linq;
using System.Web.Mvc;
using TallerFrameWork.Models;

namespace TallerFrameWork.Controllers
{
    public class UsuarioRolController : Controller
    {
        // GET: UsuarioRol
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.usuariorol.ToList());
            }
        }

        public static String NombreUsuario(int idUsuario)
        {
            using (var bd = new inventario2021Entities())
            {
                return bd.usuario.Find(idUsuario).nombre;
            }
        }

        public static String DescripcionRol(int idRol)
        {
            using (var bd = new inventario2021Entities())
            {
                return bd.roles.Find(idRol).descripcion;
            }
        }

        public ActionResult ListarUsuarios()
        {
            using (var bd = new inventario2021Entities())
            {
                return PartialView(bd.usuario.ToList());
            }
        }

        public ActionResult ListarRoles()
        {
            using (var bd = new inventario2021Entities())
            {
                return PartialView(bd.roles.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(usuariorol usuariorol)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var bd = new inventario2021Entities())
                {
                    bd.usuariorol.Add(usuariorol);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.usuariorol.Find(id));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    usuariorol findUsuarioRol = bd.usuariorol.Where(a => a.id == id).FirstOrDefault();
                    return View(findUsuarioRol);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(usuariorol editUsuarioRol)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    usuariorol usuariorol = bd.usuariorol.Find(editUsuarioRol.id);

                    usuariorol.idUsuario = editUsuarioRol.idUsuario;
                    usuariorol.idRol = editUsuarioRol.idRol;

                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    usuariorol usuariorol = bd.usuariorol.Find(id);
                    bd.usuariorol.Remove(usuariorol);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }
    }
}