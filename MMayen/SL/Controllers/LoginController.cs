using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;

namespace SL.Controllers
{
    public class LoginController : ApiController
    {
        // GET api/login
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/login/5
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult GetByUser(ML.Usuario usuario)
        {
            ML.Result result = BL.Login.GetByUser(usuario);


            //Usuario.Email = (ML.Usuario)
            //Usuario.Password

            if (result.Correct)
            {

                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Route("api/login/Authentication")]
        public IHttpActionResult Authentication(ML.Usuario usuario)
        {
            ML.Result result = BL.Login.GetByUser(usuario);
            ML.Usuario Usuario = (ML.Usuario)result.Object;


            //Usuario.Email = (ML.Usuario)
            //Usuario.Password

            if (result.Correct)
            {
                if (Usuario.Email == usuario.Email)
                {
                    if (Usuario.Password == usuario.Password)
                    {
                        var token = TokenGenerator.GenerateTokenJwt(usuario);
                        return Ok(result);
                    }
                    else
                    {
                        result.ErrorMessage = "Contraseña Incorrecta";
                        return Content(HttpStatusCode.NonAuthoritativeInformation, result.ErrorMessage);
                    }
                }
                else
                {
                    result.ErrorMessage = "Usuario Incorrecto";
                    return Content(HttpStatusCode.NonAuthoritativeInformation, result.ErrorMessage);
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/login
        public void Post([FromBody]string value)
        {
        }

        // PUT api/login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/login/5
        public void Delete(int id)
        {
        }


    }

    internal static class TokenGenerator
    {
        public static string GenerateTokenJwt(ML.Usuario usuario)
        {
            // appsetting for Token JWT
            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario.Email) });

            // create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}
