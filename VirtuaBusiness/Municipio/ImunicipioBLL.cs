using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Municipio
{
    public interface ImunicipioBLL
    {
        IEnumerable<MunicipiosDTO> Listar_Municipios();
        EnderecoCepDTO CapturaENderecoPeloCep(String cep);
    }
}
