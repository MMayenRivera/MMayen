using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.MMayenDataBaseEntities context = new DL.MMayenDataBaseEntities())
                {
                    var usuarios = context.UsuarioGetAll().ToList();
                    result.Objects = new List<object>();

                    if(usuarios != null)
                    {
                        foreach(var obj in usuarios)
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.IdUsuario = obj.IdUsuario;
                            usuario.Email = obj.Email;
                            usuario.Password = obj.Password;
                            usuario.Nombre = obj.NombreUsuario;
                            usuario.ApellidoMaterno = obj.ApellidoMaterno;
                            usuario.ApellidoPaterno = obj.ApellidoPaterno;
                            usuario.FechaNacimiento = obj.FechaNacimiento;
                            usuario.Sexo = obj.Sexo;
                            usuario.Status = obj.Status.Value;

                            usuario.Rol = new ML.Rol();
                            usuario.Rol.Nombre = obj.NombreRol;
                            
                            usuario.Telefono = obj.Telefono;

                            result.Objects.Add(usuario);

                            result.Correct = true;
                        }                      
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo recuperar los usuarios";
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

        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            
            try
            {
                using(DL.MMayenDataBaseEntities context = new DL.MMayenDataBaseEntities())
                {
                    var query = Convert.ToString(context.UsuarioAdd(usuario.Email, usuario.Password, usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno, 
                        usuario.FechaNacimiento.ToString("dd/MM/yyyy"), usuario.Sexo, usuario.Status, usuario.Rol.IdRol, usuario.Telefono));

                    if(query == "Usuario Insertado")
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        if (query.Length >= 49 && query.Substring(0, 50) == "Violation of UNIQUE KEY constraint 'CONS_Email_Unique'.")
                        {
                            result.Correct = false;
                            result.ErrorMessage = "El email ya se encuentra registrado";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = query;
                        }
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

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.MMayenDataBaseEntities context = new DL.MMayenDataBaseEntities())
                {
                    var query = Convert.ToString(context.UsuarioUpdate(usuario.IdUsuario, usuario.Email, usuario.Password, usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno,
                         usuario.FechaNacimiento.ToString("dd/MM/yyyy"), usuario.Sexo, usuario.Status, usuario.Rol.IdRol, usuario.Telefono));

                    if (query == "Usuario Insertado")
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        if (query.Length >= 49 && query.Substring(0, 50) == "Violation of UNIQUE KEY constraint 'CONS_Email_Unique'.")
                        {
                            result.Correct = false;
                            result.ErrorMessage = "El email ya se encuentra registrado";
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = query;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result GetById(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.MMayenDataBaseEntities context = new DL.MMayenDataBaseEntities())
                {
                    var UsuarioDL = context.UsuarioGetById(IdUsuario).FirstOrDefault();

                    if(UsuarioDL != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.IdUsuario = UsuarioDL.IdUsuario;
                        usuario.Email = UsuarioDL.Email;
                        usuario.Password = UsuarioDL.Password;
                        usuario.Nombre = UsuarioDL.NombreUsuario;
                        usuario.ApellidoMaterno = UsuarioDL.ApellidoMaterno;
                        usuario.ApellidoPaterno = UsuarioDL.ApellidoPaterno;
                        usuario.FechaNacimiento = UsuarioDL.FechaNacimiento;
                        usuario.FechaNacimiento.ToString("dd/MM/yyyy");
                        usuario.Sexo = UsuarioDL.Sexo;
                        usuario.Status = UsuarioDL.Status.Value;
                        
                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = UsuarioDL.IdRol;
                        usuario.Rol.Nombre = UsuarioDL.NombreRol;

                        usuario.Telefono = UsuarioDL.Telefono;

                        result.Object = usuario;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo recuperar el usuario";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result GetAllTest(ML.Usuario datos)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MMayenDataBaseEntities context = new DL.MMayenDataBaseEntities())
                {
                    var usuarios = context.UsuarioGetAllTest(datos.Nombre,datos.ApellidoPaterno,datos.ApellidoMaterno).ToList();
                    result.Objects = new List<object>();

                    if (usuarios != null)
                    {
                        foreach (var obj in usuarios)
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.IdUsuario = obj.IdUsuario;
                            usuario.Email = obj.Email;
                            usuario.Password = obj.Password;
                            usuario.Nombre = obj.NombreUsuario;
                            usuario.ApellidoMaterno = obj.ApellidoMaterno;
                            usuario.ApellidoPaterno = obj.ApellidoPaterno;
                            usuario.FechaNacimiento = obj.FechaNacimiento;
                            usuario.Sexo = obj.Sexo;
                            usuario.Status = obj.Status.Value;

                            usuario.Rol = new ML.Rol();
                            usuario.Rol.Nombre = obj.NombreRol;

                            usuario.Telefono = obj.Telefono;

                            result.Objects.Add(usuario);

                            result.Correct = true;
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo recuperar los usuarios";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
