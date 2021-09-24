using Rotativa;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
                var producto = bd.producto.Find(id);
                var imagen = bd.producto_imagen.Where(e => e.id_producto == producto.id).FirstOrDefault();
                ViewBag.imagen = imagen.imagen;
                return View(producto);
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
                    producto.id_proveedor = editProducto.id_proveedor;

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

        public ActionResult Reporte()
        {
            try
            {
                var bd = new inventario2021Entities();
                var query = from tabProveedor in bd.proveedor
                            join tabProducto in bd.producto on tabProveedor.id equals tabProducto.id_proveedor
                            select new Reporte
                            {
                                nombreProveedor = tabProveedor.nombre,
                                telefonoProveedor = tabProveedor.telefono,
                                direccionProveedor = tabProveedor.direccion,
                                nombreProducto = tabProducto.nombre,
                                precioProducto = tabProducto.percio_unitario
                            };
                return View(query);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult PdfReporte()
        {
            return new ActionAsPdf("Reporte") { FileName = "Reporte.pdf" };
        }

        public ActionResult PaginadorIndex(int pagina = 1)
        {
            try
            {
                var cantidadRegistros = 5;

                using (var db = new inventario2021Entities())
                {
                    var productos = db.producto.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros).Take(cantidadRegistros).ToList();

                    var totalRegistros = db.producto.Count();
                    var modelo = new ProductoIndex();
                    modelo.Productos = productos;
                    modelo.ActualPage = pagina;
                    modelo.Total = totalRegistros;
                    modelo.RecordsPage = cantidadRegistros;
                    modelo.valueQueryString = new RouteValueDictionary();

                    return View(modelo);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult cargarImagen()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult cargarImagen(int producto, HttpPostedFileBase fileForm)
        {
            string filePath = string.Empty;
            string name = "";

            if (fileForm != null)
            {
                string path = Server.MapPath("~/Uploads/Imagenes/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                name = Path.GetFileName(fileForm.FileName);

                filePath = path + Path.GetFileName(fileForm.FileName);

                string extension = Path.GetExtension(fileForm.FileName);

                fileForm.SaveAs(filePath);
            }


            using (var db = new inventario2021Entities())
            {
                var imagenProducto = new producto_imagen();
                imagenProducto.id_producto = producto;
                imagenProducto.imagen = "/Uploads/Imagenes/" + name;
                db.producto_imagen.Add(imagenProducto);
                db.SaveChanges();
            }

            return View();
        }
    }
}