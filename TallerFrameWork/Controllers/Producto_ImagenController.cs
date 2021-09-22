using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerFrameWork.Models;

namespace TallerFrameWork.Controllers
{
    public class Producto_ImagenController : Controller
    {
        // GET: Producto_Imagen
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.producto_imagen.ToList());
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto_imagen producto_Imagen)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var bd = new inventario2021Entities())
                {
                    bd.producto_imagen.Add(producto_Imagen);
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
                return View(bd.producto_imagen.Find(id));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    producto_imagen findProductoImagen = bd.producto_imagen.Where(a => a.id == id).FirstOrDefault();
                    return View(findProductoImagen);
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

        public ActionResult Edit(producto_imagen editProductoImagen)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    producto_imagen producto_imagen = bd.producto_imagen.Find(editProductoImagen.id);

                    producto_imagen.imagen = editProductoImagen.imagen;
                    producto_imagen.id_producto = editProductoImagen.id_producto;

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
                    producto_imagen producto_imagen = bd.producto_imagen.Find(id);
                    bd.producto_imagen.Remove(producto_imagen);
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

        public ActionResult CargarImagen ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CargarImagen (int producto, HttpPostedFileBase imagen)
        {
            try
            {
                string filePath = string.Empty;
                string nameFile = "";

                if (imagen != null)
                {
                    string path = Server.MapPath("~/Uploads/Imagenes/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    nameFile = Path.GetFileName(imagen.FileName);

                    filePath = path + Path.GetFileName(imagen.FileName);

                    string extension = Path.GetExtension(imagen.FileName);

                    imagen.SaveAs(filePath);
                }

                using (var bd = new inventario2021Entities())
                {
                    var imagenProducto = new producto_imagen();
                    imagenProducto.id_producto = producto;
                    imagenProducto.imagen = "/Uploads/Imagenes/" + nameFile;
                    bd.producto_imagen.Add(imagenProducto);
                    bd.SaveChanges();
                }

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }
    }
}