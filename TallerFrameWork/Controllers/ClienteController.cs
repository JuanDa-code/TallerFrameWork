using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerFrameWork.Models;

namespace TallerFrameWork.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.cliente.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(cliente cliente)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var bd = new inventario2021Entities())
                {
                    bd.cliente.Add(cliente);
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
                var findCliente = bd.cliente.Find(id);
                return View(findCliente);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    var findCliente = bd.cliente.Find(id);
                    bd.cliente.Remove(findCliente);
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
                    cliente findCliente = bd.cliente.Where(a => a.id == id).FirstOrDefault();
                    return View(findCliente);
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(cliente editCliente)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    cliente cliente = bd.cliente.Find(editCliente.id);

                    cliente.nombre = editCliente.nombre;
                    cliente.email = editCliente.email;
                    cliente.documento = editCliente.documento;

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