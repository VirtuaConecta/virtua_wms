using System;
using System.Collections.Generic;
using System.Text;
using VirtuaDTO;

namespace VirtuaBusiness.Perfil
{
     public interface IperfilBLL
    {
        IEnumerable<PerfilDTO> Listar_Perfil();
        Int32 Edita_Inseri_Perfil(PerfilDTO perfil);
    }
}
