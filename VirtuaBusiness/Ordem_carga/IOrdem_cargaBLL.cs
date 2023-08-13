using System;
using System.Collections.Generic;
using VirtuaDTO;

namespace VirtuaBusiness.Ordem_carga
{
    public interface IOrdem_cargaBLL
    {
        IEnumerable<ListaGenericaDTO> Lista_status_OcBLL();

        List<Ordem_CargaDTO> Lista_Orde_Carga(string dt_ini, string dt_fim, int status, int tp,String io);
        Ordem_CargaDTO PesqisaOC_BLL(int id);


    }
}