using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument.DTO
{
    public class NfeItens
    {
        public String Sku { get; set; }
        public String Especie { get; set; }
        public String Ean { get; set; }
        public String Ncm { get; set; }
        public Decimal Qtd { get; set; }
        public String Descricao_prod { get; set; }
    }
}
