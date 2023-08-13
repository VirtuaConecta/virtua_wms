using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Veiculos
{
    public class VeiculosBLL : IVeiculosBLL
    {
        private readonly IDataConnection _db;
        public VeiculosBLL(IDataConnection db)
        {
            _db = db;
        }

        public IEnumerable<ListaGenericaDTO> Lista_tipo_veiculoBLL()
        {

            return  _db.Listar_tipo_veiculo();

        }
    }
}
