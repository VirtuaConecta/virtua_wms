using System.Collections.Generic;
using VirtuaDTO;

namespace VirtuaBusiness.Veiculos
{
    public interface IVeiculosBLL
    {
        IEnumerable<ListaGenericaDTO> Lista_tipo_veiculoBLL();
    }
}