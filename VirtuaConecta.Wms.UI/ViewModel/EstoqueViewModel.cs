using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class EstoqueViewModel
    {
        public Int32 id { get; set; }
        public String Nr_nf { get; set; }
        public Int32 Id_cliente { get; set; }
        public String Data_entrada { get; set; }
        public String Descricao { get; set; }
        public String Sku { get; set; }
        public Int32 Id_produto { get; set; }

        public Decimal Qtd_ped { get; set; }
        public Decimal Qtd_entrada { get; set; }

        public Decimal Qtd_saida { get; set; }

        public Decimal Saldo { get; set; }

        public String Lote { get; set; }
        public DateTime Validade { get; set; }

        public Decimal P_unit { get; set; }

        public String Posicao { get; set; }

        public Decimal Qtd_baixar { get; set; }
        public Int32? Nr_Ordem { get; set; }

        public String Apelido { get; set; }
        public String Nome_cli { get; set; }
        public DateTime? Data_ordem { get; set; }


        public String Obs { get; set; }
        public String Ped_cli { get; set; }
        public String Operador { get; set; }

        public String Tipo_ped { get; set; }
        public DateTime? Dt_registro { get; set; }

        public DateTime? Data_entrada2 { get; set; }

        public Int32? Id_Head_item { get; set; }
        public String Nr_serie { get; set; }

        public Decimal? qtd_palet { get; set; }
        public Decimal? vol_palet { get; set; }

    }
}
