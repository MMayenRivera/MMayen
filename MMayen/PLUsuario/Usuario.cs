using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PLUsuario
{
    public class Usuario
    {
        public static void Add()
        {
            using (var reader = new StreamReader(@"ListaUsuarios.txt"))
            {
                ML.Result result = new ML.Result();
                result.Objects = new List<object>();
                //ML.Usuario usuario = new ML.Usuario();
                
                string line;
                while ((line = reader.ReadLine()) != null)
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
            }  
        }
    }
}
