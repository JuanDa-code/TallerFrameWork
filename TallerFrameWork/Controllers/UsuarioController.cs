using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using TallerFrameWork.Models;

namespace TallerFrameWork.Controllers
{
    public class UsuarioController : Controller
    {
        [Authorize]
        // GET: Usuario
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())
            {
                return View(bd.usuario.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(usuario usuario)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var bd = new inventario2021Entities())
                {
                    usuario.password = UsuarioController.HashSHA1(usuario.password);
                    bd.usuario.Add(usuario);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex);
                return View();
            }
        }

        public static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public ActionResult Details(int id)
        {
            using (var bd = new inventario2021Entities())
            {
                var findUser = bd.usuario.Find(id);
                return View(findUser);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    var findUser = bd.usuario.Find(id);
                    bd.usuario.Remove(findUser);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    usuario findUser = bd.usuario.Where(a => a.id == id).FirstOrDefault();
                    return View(findUser);
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

        public ActionResult Edit(usuario editUser)
        {
            try
            {
                using (var bd = new inventario2021Entities())
                {
                    usuario user = bd.usuario.Find(editUser.id);

                    user.nombre = editUser.nombre;
                    user.apellido = editUser.apellido;
                    user.email = editUser.email;
                    user.fecha_nacimiento = editUser.fecha_nacimiento;
                    user.password = editUser.password;

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
        public ActionResult Login(string mensaje = "")
        {
            ViewBag.Message = mensaje;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string user, string password)
        {
            try
            {
                string passEncrip = UsuarioController.HashSHA1(password);
                using (var db = new inventario2021Entities())
                {
                    var userLogin = db.usuario.FirstOrDefault(e => e.email == user && e.password == passEncrip);
                    if (userLogin != null)
                    {
                        FormsAuthentication.SetAuthCookie(userLogin.email, true);
                        Session["User"] = userLogin;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Login("Verifique sus datos");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        [Authorize]
        public ActionResult CloseSession()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult uploadCSV()
        {
            return View();
        }

        [HttpPost]

        public ActionResult uploadCSV(HttpPostedFileBase fileForm)
        {
            try
            {
                string filePath = string.Empty;

                if (fileForm != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(fileForm.FileName);

                    string extension = Path.GetExtension(fileForm.FileName);

                    fileForm.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            var newUsuario = new usuario
                            {
                                nombre = row.Split(';')[0],
                                apellido = row.Split(';')[1],
                                email = row.Split(';')[2],
                                fecha_nacimiento = Convert.ToDateTime(row.Split(';')[3]),
                                password = row.Split(';')[4]
                            };

                            using (var bd = new inventario2021Entities())
                            {
                                bd.usuario.Add(newUsuario);
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

        public ActionResult PaginadorIndex(int pagina = 1)
        {
            try
            {
                var cantidadRegistros = 5;

                using (var db = new inventario2021Entities())
                {
                    var usuarios = db.usuario.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros).Take(cantidadRegistros).ToList();

                    var totalRegistros = db.usuario.Count();
                    var modelo = new UsuarioIndex();
                    modelo.Usuarios = usuarios;
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
    }
}