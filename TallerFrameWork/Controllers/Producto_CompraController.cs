using System;
using System.Linq;
using System.Web.Mvc;
using TallerFrameWork.Models;

namespace TallerFrameWork.Controllers
{
    public class Producto_CompraController : Controller
    {
        // GET: Producto_Compra
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.producto_compra.ToList());
            }
        }

        public static String NombreProducto(int idProducto)
        {
            using (var bd = new inventario2021Entities())
            {
                return bd.producto.Find(idProducto).nombre;
            }
        }

        public ActionResult ListarProductos()
        {
            using (var bd = new inventario2021Entities())
            {
                return PartialView(bd.producto.ToList());
            }
        }

        public static DateTime FechaCompra(int idCompra)
        {
            using (var bd = new inventario2021Entities())
            {
                return bd.compra.Find(idCompra).fecha;
            }
        }

        public ActionResult ListarCompras()
        {
            using (var bd = new inventario2021Entities())
            {
                return PartialView(bd.compra.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto_compra producto_compra)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var bd = new inventario2021Entities())
                {
                    bd.producto_compra.Add(producto_compra);
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
                return View(bd.producto_compra.Find(id));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    producto_compra findProductoCompra = bd.producto_compra.Where(a => a.id == id).FirstOrDefault();
                    return View(findProductoCompra);
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

        public ActionResult Edit(producto_compra editProductoCompra)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    producto_compra producto_compra = bd.producto_compra.Find(editProductoCompra.id);

                    producto_compra.id_compra = editProductoCompra.id_compra;
                    producto_compra.id_producto = editProductoCompra.id_producto;
                    producto_compra.cantidad = editProductoCompra.cantidad;

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
                    producto_compra producto_compra = bd.producto_compra.Find(id);
                    bd.producto_compra.Remove(producto_compra);
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