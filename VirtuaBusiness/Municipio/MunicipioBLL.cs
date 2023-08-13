using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Municipio
{
    public class MunicipioBLL : ImunicipioBLL
    {
        IDataConnection _db;
        private readonly IRestDAL _rest;

        public MunicipioBLL(IDataConnection db, IRestDAL rest)
        {
            _db = db;
           _rest = rest;
        }

        public IEnumerable<MunicipiosDTO> Listar_Municipios()
        {
            return _db.Listar_Municipios();
        }

        public EnderecoCepDTO CapturaENderecoPeloCep(String cep)
        {
            return _rest.PesquisarCep(cep);

        }
    }
}
