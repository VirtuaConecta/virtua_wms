using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Transportadora
{
    public class TransportadoraBLL : ITransportadoraBLL
    {
        private readonly IDataConnection _db;

        public TransportadoraBLL(IDataConnection db)
        {
            _db = db;
        }
        public IEnumerable<ListaGenericaDTO> Lista_transportadoraBLL()
        {

            return  _db.Listar_transportadora();

        }

    }
}
