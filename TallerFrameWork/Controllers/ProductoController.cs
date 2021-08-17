using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerFrameWork.Models;

namespace TallerFrameWork.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.producto.ToList());
            }
        }

        public static String NombreProveedor(int idProveedor)
        {
            using (var bd = new inventario2021Entities())
            {
                return bd.proveedor.Find(idProveedor).nombre;
            }
        }

        public ActionResult ListarProveedores()
        {
            using (var bd = new inventario2021Entities())
            {
                return PartialView(bd.proveedor.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto producto)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var bd = new inventario2021Entities())
                {
                    bd.producto.Add(producto);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Details (int id)
        {
            using ( var bd = new inventario2021Entities())
            {
                return View(bd.producto.Find(id));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    producto findProducto = bd.producto.Where(a => a.id == id).FirstOrDefault();
                    return View(findProducto);
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

        public ActionResult Edit(producto editProducto)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    producto producto = bd.producto.Find(editProducto.id);

                    producto.nombre = editProducto.nombre;
                    producto.cantidad = editProducto.cantidad;
                    producto.descripcion = editProducto.descripcion;
                    producto.percio_unitario = editProducto.percio_unitario;
                    producto.proveedor = editProducto.proveedor;

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
                    producto producto = bd.producto.Find(id);
                    bd.producto.Remove(producto);
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