using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Arquivos.LayoutsEDI
{
   public interface IbaseLayout
    {
        List<Nfe> InsereArquivo(string Caminho, string Arquivo);

    }
}
