using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument
{
    public interface IArquivos
    {
        List<string> LeArquivo(string Arq);
    }
}
