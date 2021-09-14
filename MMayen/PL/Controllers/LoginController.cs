using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Configuration;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult GetUser(ML.Usuario usuario)
        {
            // ML.Result result = BL.Login.GetByUser(usuario);
            ML.Result result = new ML.Result();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:20104/api/");

                var posttask = client.PostAsJsonAsync<ML.Usuario>("login", usuario);
                posttask.Wait();

                var resulthttp = posttask.Result;

                if (resulthttp.IsSuccessStatusCode)
                {
                    var readtask = resulthttp.Content.ReadAsAsync<ML.Result>();
                    ML.Usuario resultUsuario = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readtask.Result.Object.ToString());
                    result.Correct = true;
                    result.Object = resultUsuario;

                    using (var clientToken = new HttpClient())
                    {
                        string WebToken = ConfigurationSettings.AppSettings["WebDomain"];
                        clientToken.BaseAddress = new Uri(WebToken);

                        var PostTaskToken = client.PostAsJsonAsync<ML.Usuario>("login/Aunthenticate", resultUsuario);
                        posttask.Wait();

                        var ResultTokenApi = PostTaskToken.Result;
                        string Token = PostTaskToken.Result.ToString();

                        if(ResultTokenApi.IsSuccessStatusCode)
                        {
                            Session["TipoUsuario"] = "Logeado";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            //result.Correct = false;
                            //result.ErrorMessage = "El usuario no esta registrado";

                            return RedirectToAction("Login", "Login");
                        }

                    }

                    //if (result != null)
                    //{
                    //    ML.Usuario Aux = (ML.Usuario)result.Object;

                    //    if (usuario.Password == Aux.Password)
                    //    {
                    //        result.Correct = true;
                    //        Session["TipoUsuario"] = "Logeado";
                    //        return RedirectToAction("Index", "Home");
                    //    }

                    //    else
                    //    {
                    //        result.Correct = false;
                    //        return RedirectToAction("Index", "Home");
                    //    }
                    //}
                    
                }

                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
        }
    }
}