using System;
using System.Collections.Generic;
using System.Text;

namespace VirtuaDTO
{
     public class UsuarioDTO
    {
        public Int32 Codigo { get; set; }
        public String NomeUsuario { get; set; }
        public String Login { get; set; }
        public Int32 Cod_perfil { get; set; }
        public String Area { get; set; }
        public String Email { get; set; }
        public String Telefone { get; set; }
        public String Senha { get; set; }
        public String Observacao { get; set; }
        public String Cargo { get; set; }
        public String Bloqueado { get; set; }
        public String Cpf { get; set; }
        public String Perfil { get; set; }
    }
}
