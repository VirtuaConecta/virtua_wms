using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class NfeCrossDock
    {
        public int Id_item { get; set; }
        public int Id_cliente { get; set; }
        public String Nome_cli { get; set; }
        public Decimal peso_item { get; set; }
        public String Sku { get; set; }
        public String Descricao { get; set; }
        public Decimal Qtd_item { get; set; }
        public String Cod_aux { get; set; }
        public String Ean { get; set; }
        public String Und { get; set; }
        public String Notas { get; set; }
        public String destino { get; set; }
        public String data_emissao { get; set; }

    }
}
