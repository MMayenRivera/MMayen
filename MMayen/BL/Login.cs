using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Login
    {
        public static ML.Result GetByUser(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.MMayenDataBaseEntities context = new DL.MMayenDataBaseEntities())
                {
                    var query = context.UsuarioGetByUserName(usuario.Email).FirstOrDefault();

                    if(query != null)
                    {
                        ML.Usuario usuarioAux = new ML.Usuario();

                        usuarioAux.IdUsuario = query.IdUsuario;
                        usuarioAux.Nombre = query.UsuarioNombre;
                        usuarioAux.Email = query.Email;
                        usuarioAux.Password = query.Password;
                        usuarioAux.Sexo = query.Sexo;
                        usuarioAux.Status = usuario.Status;
                        usuarioAux.Telefono = query.Telefono;
                        usuarioAux.FechaNacimiento = query.FechaNacimiento;
                        usuarioAux.ApellidoMaterno = query.ApellidoMaterno;
                        usuarioAux.ApellidoPaterno = query.ApellidoPaterno;

                        usuarioAux.Rol = new ML.Rol();
                        usuarioAux.Rol.Nombre = query.RolNombre;
                        usuarioAux.Rol.IdRol = query.IdRol.Value;

                        result.Object = usuarioAux;

                        result.Correct = true;                       
                    }

                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "El Usuario no se pudo recuperar";
                    }

                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
