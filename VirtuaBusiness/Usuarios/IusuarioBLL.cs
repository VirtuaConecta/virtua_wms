using System;
using System.Collections.Generic;
using System.Text;
using VirtuaDTO;

namespace VirtuaBusiness.Usuarios
{
   public interface IusuarioBLL
    {
        IEnumerable<UsuarioDTO> Listar_Usauario();
        Int32 Edita_Inseri_Usuario(UsuarioDTO usuario);

        UsuarioDTO Pesquisar_Login_Usuario(string login);
        Boolean AutenticaUsr(string login, string password);

        Boolean ValidaCpf(string cpf);
        Boolean ValidaUsuario(string usuario);
        Boolean ValidaEmail(string email);
    }
}
