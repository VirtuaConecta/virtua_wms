using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Perfil
{
    public class PerfilBLL: IperfilBLL
    {
        IDataConnection _data;
        public PerfilBLL(IDataConnection db)
        {
            _data = db;
        }
        public IEnumerable<PerfilDTO> Listar_Perfil()
        {
            return _data.Listar_Perfil();
        }

        public Int32 Edita_Inseri_Perfil(PerfilDTO perfil)
        {
            return _data.Edita_Inseri_Perfil(perfil);
        }
    }
}
