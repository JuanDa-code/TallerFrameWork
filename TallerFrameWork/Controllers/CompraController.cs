using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerFrameWork.Models;
using Rotativa;

namespace TallerFrameWork.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.compra.ToList());
            }
        }

        public static String NombreUsuario (int idUsuario)
        {
            using (var bd = new inventario2021Entities())
            {
                return bd.usuario.Find(idUsuario).nombre;
            }
        }

        public static String NombreCliente (int idCliente)
        {
            using (var bd = new inventario2021Entities())
            {
                return bd.cliente.Find(idCliente).nombre;
            }
        }

        public ActionResult ListarUsuarios ()
        {
            using (var bd = new inventario2021Entities())
            {
                return PartialView(bd.usuario.ToList());
            }
        }

        public ActionResult ListarClientes ()
        {
            using (var bd = new inventario2021Entities())
            {
                return PartialView(bd.cliente.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(compra compra)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var bd = new inventario2021Entities())
                {
                    bd.compra.Add(compra);
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

        public ActionResult Details (int id)
        {
            using ( var bd = new inventario2021Entities())
            {
                return View(bd.compra.Find(id));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    compra findCompra = bd.compra.Where(a => a.id == id).FirstOrDefault();
                    return View(findCompra);
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

        public ActionResult Edit(compra editCompra)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    compra compra = bd.compra.Find(editCompra.id);

                    compra.fecha = editCompra.fecha;
                    compra.total = editCompra.total;
                    compra.id_usuario = editCompra.id_usuario;
                    compra.id_cliente = editCompra.id_cliente;

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

        public ActionResult Delete (int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    compra compra = bd.compra.Find(id);
                    bd.compra.Remove(compra);
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

        public ActionResult ReporteCompra()
        {
            try
            {
                var bd = new inventario2021Entities();
                var query = from tabCliente in bd.cliente
                            join tabCompra in bd.compra on tabCliente.id equals tabCompra.id_cliente
                            select new ReporteCompra
                            {
                                nombreCliente = tabCliente.nombre,
                                documentoCliente = tabCliente.documento,
                                emailCliente = tabCliente.email,
                                fechaCompra = tabCompra.fecha,
                                totalCompra = tabCompra.total
                            };
                return View(query);
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult PdfReporte()
        {
            return new ActionAsPdf("ReporteCompra") { FileName = "Reporte.pdf" };
        }
    }
}