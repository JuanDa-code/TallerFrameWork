using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult uploadCSV ()
        {
            return View();
        }

        [HttpPost]

        public ActionResult uploadCSV (HttpPostedFileBase fileForm)
        {
            try
            {
                string filePath = string.Empty;

                if(fileForm != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if(!Directory.Exists(path)) 
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(fileForm.FileName);

                    string extension = Path.GetExtension(fileForm.FileName);

                    fileForm.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach(string row in csvData.Split('\n'))
                    {
                        if(!string.IsNullOrEmpty(row))
                        {
                            var newProveedor = new proveedor
                            {
                                nombre = row.Split(';')[0],
                                direccion = row.Split(';')[1],
                                telefono = row.Split(';')[2],
                                nombre_contacto = row.Split(';')[3]
                            };

                            using (var bd = new inventario2021Entities())
                            {
                                bd.proveedor.Add(newProveedor);
                                bd.SaveChanges();
                            }
                        }
                    }
                }

                return RedirectToAction("Index");
            } 
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }
    }
}