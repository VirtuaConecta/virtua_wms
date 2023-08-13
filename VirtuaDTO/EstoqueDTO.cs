using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class EstoqueDTO: ComplementoEstoqueDTO
    {
        public Int32 id { get; set; }
        public String Nr_nf { get; set; }
        public Int32 Id_cliente { get; set; }

        public String Descricao { get; set; }
        public String Sku { get; set; }
        public Int32 Id_produto { get; set; }
        public String Und { get; set; }

        public Decimal Qtd_ped { get; set; }
        public Decimal Qtd_entrada { get; set; }

        public Decimal Qtd_saida { get; set; }

        public Decimal Saldo { get; set; }

        public String Lote { get; set; }
        public DateTime Validade { get; set; }

        public Decimal P_unit { get; set; }

        public String Posicao { get; set; }

        public Decimal Qtd_baixar { get; set; }

        public String Bloqueio { get; set; }
       
       
    }
}
