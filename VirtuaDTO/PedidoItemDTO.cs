using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
      public class PedidoItemDTO
    {
        public Int32? Id { get; set; }
        public Int32? Id_head { get; set; }
        public String Tipo_doc { get; set; }
        public String Serie { get; set; }
        public String Sku { get; set; }
        public Decimal? Qtd { get; set; }
        public Decimal? P_unit { get; set; }
        public String Nr_documento { get; set; }
        public String Descricao { get; set; }
        public String Lote { get; set; }
        public DateTime? Validade { get; set; }
        public String Operador { get; set; }
        public Int32? Id_cliente { get; set; }
        public Int32 Valid_sku { get; set; }
        public Decimal? Peso_brt { get; set; }
        public Decimal? Peso_Liq { get; set; }
        public String Ncm { get; set; }
        public Int32 Cst { get; set; }
        public String id_produto { get; set; }

        public String Und { get; set; }
    }
}
