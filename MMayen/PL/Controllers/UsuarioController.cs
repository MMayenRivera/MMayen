using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Xml;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        //
        // GET: /Usuario/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll()
        {
            ML.Result result = BL.Usuario.GetAll();

            ML.Usuario usuario = new ML.Usuario();
            usuario.Usuarios = result.Objects;

            return View(usuario);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            if (usuario.Nombre == null)
            {
                usuario.Nombre = "";
            }

            if (usuario.ApellidoPaterno == null)
            {
                usuario.ApellidoPaterno = "";
            }

            if (usuario.ApellidoMaterno == null)
            {
                usuario.ApellidoMaterno = "";
            }
            ML.Result result = BL.Usuario.GetAll();

            result = BL.Usuario.GetAllTest(usuario);
            usuario.Usuarios = result.Objects;

            return View(usuario);
        }


        [HttpGet]
        public ActionResult Form(int? IdUsuario) //Add , Update
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result resultroles = BL.Rol.GetAll();

            usuario.Rol = new ML.Rol();
            usuario.Rol.Roles = resultroles.Objects;

            if (IdUsuario == null)
            {
                return View(usuario);
            }
            else
            {
                ML.Result result = BL.Usuario.GetById(IdUsuario.Value);

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;
                    usuario.Rol.Roles = resultroles.Objects;
                    return View(usuario);
                }
                else
                {
                    ViewBag.Message = result.ErrorMessage;
                    return PartialView("Modal");
                }
            }
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {

            ML.Result result = new ML.Result();

            if (usuario.IdUsuario == 0)//Add
            {
                result = BL.Usuario.Add(usuario);
                ViewBag.Message = "El Usuario se agregó correctamente ";
            }
            else //Update
            {
                result = BL.Usuario.Update(usuario);
                ViewBag.Message = "La Usuario se actualizó correctamente ";
            }

            if (!result.Correct)
            {
                ViewBag.Message = "No se pudo agregar correctamente el Usuario " + result.ErrorMessage;
            }

            return PartialView("Modal");
        }

        public ActionResult UpdateStatus(int IdUsuario)
        {
            ML.Result resultusuario = new ML.Result();
            ML.Usuario usuario = new ML.Usuario();

            resultusuario = BL.Usuario.GetById(IdUsuario);
            usuario = (ML.Usuario)resultusuario.Object;

            if (usuario.Status == true)
            {
                usuario.Status = false;
            }
            else
            {
                usuario.Status = true;
            }

            resultusuario = BL.Usuario.Update(usuario);

            return RedirectToAction("GetAll");
        }

        public ActionResult ListaUP(HttpPostedFile file)
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();
            var Lista = new StreamReader(file.InputStream);

            string line;
            while ((line = Lista.ReadLine()) != null)
            {
                ML.Usuario usuario = new ML.Usuario();
                usuario.Rol = new ML.Rol();

                var temp = line.Split('|');

                usuario.Email = temp[0];
                usuario.Nombre = temp[1];
                usuario.ApellidoPaterno = temp[2];
                usuario.ApellidoMaterno = temp[3];
                usuario.Password = temp[4];
                usuario.Sexo = temp[5];
                usuario.Status = bool.Parse(temp[6]);
                usuario.Rol.IdRol = byte.Parse(temp[7]);
                usuario.Telefono = temp[8];

                result.Objects.Add(usuario);
            }

            return View();
        }

        public FileResult TxtDownload(int IdUsuario)
        {
            ML.Result result = BL.Usuario.GetById(IdUsuario);
            ML.Usuario usuario = (ML.Usuario)result.Object;
            string name = usuario.Nombre + usuario.ApellidoPaterno + ".txt";
            string contenido =
                    "IdUsuario: " + usuario.IdUsuario + "\n" +
                    "Nombre: " + usuario.Nombre + "\n" +
                    "Apellido Paterno: " + usuario.ApellidoPaterno + "\n" +
                    "Apellido Materno: " + usuario.ApellidoMaterno + "\n" +
                    "Email: " + usuario.Email + "\n" +
                    "Fecha de Nacimiento: " + usuario.FechaNacimiento + "\n" +
                    "Sexo: " + usuario.Sexo + "\n" +
                    "Telefono: " + usuario.Telefono + "\n" +
                    "Status: " + usuario.Status + "\n";
              
            var byteArray = Encoding.ASCII.GetBytes(contenido);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", name); 
        }

        public FileResult XMLDownload(int IdUsuario)
        {
            ML.Result result = BL.Usuario.GetById(IdUsuario);
            ML.Usuario usuario = (ML.Usuario)result.Object;
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(usuario.GetType());
            string name = usuario.Nombre + usuario.ApellidoPaterno + ".xml";

            XmlDocument xmlDoc = new XmlDocument();            
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xml.Serialize(xmlStream, usuario);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
            }
            var SerializeData = xmlDoc.InnerXml;
            var byteArray = Encoding.UTF8.GetBytes(SerializeData);
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/xml", name);
        }
    }
}