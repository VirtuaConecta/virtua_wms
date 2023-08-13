using EdiDocument.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument
{
    public abstract class NotFisBase
    {
        public List<Nfe> Pedidos { get; set; }
        public String ArqNotas { get; set; }

        public abstract List<Nfe> CapturaNotFis(String Arquivo);

        public abstract String GeraNotFis(String NomeDoc);
        
    }
}
