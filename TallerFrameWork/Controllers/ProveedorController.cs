using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerFrameWork.Models;

namespace TallerFrameWork.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.proveedor.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var bd = new inventario2021Entities())
                {
                    bd.proveedor.Add(proveedor);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (var bd = new inventario2021Entities())
            {
                var findProveedor = bd.proveedor.Find(id);
                return View(findProveedor);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    var findProveedor = bd.proveedor.Find(id);
                    bd.proveedor.Remove(findProveedor);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    proveedor findProveedor = bd.proveedor.Where(a => a.id == id).FirstOrDefault();
                    return View(findProveedor);
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(proveedor editProveedor)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    proveedor proveedor = bd.proveedor.Find(editProveedor.id);

                    proveedor.nombre = editProveedor.nombre;
                    proveedor.nombre_contacto = editProveedor.nombre_contacto;
                    proveedor.telefono = editProveedor.telefono;

                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }
    }
}