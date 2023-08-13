using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Ordem_carga
{
    public class Ordem_cargaBLL : IOrdem_cargaBLL
    {
        private readonly IDataConnection _db;
        public Ordem_cargaBLL(IDataConnection db)
        {
            _db = db;
        }

        public IEnumerable<ListaGenericaDTO> Lista_status_OcBLL()
        {

            return _db.Listar_status_oc();

        }
        
        public List<Ordem_CargaDTO> Lista_Orde_Carga(string dt_ini, string dt_fim, int status, int tp,String io)
        {
            return _db.Lista_Orde_Carga(dt_ini, dt_fim, status, tp,io);
        }

        public Ordem_CargaDTO PesqisaOC_BLL(int id)
        {

            return _db.Pesquisa_Orde_Carga(id);
        }

      
    }
}
