using System.Collections.Generic;
using VirtuaDTO;

namespace VirtuaBusiness.Posicao
{
    public interface IPosicaoBLL
    {
        IEnumerable<PosicaoDTO> listar_posicaoBll();
    }
}