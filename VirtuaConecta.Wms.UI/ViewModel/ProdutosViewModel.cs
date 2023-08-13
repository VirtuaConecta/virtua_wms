using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class ProdutosViewModel
    {
        public String Sku { get; set; }
        public Int32 Id_cliente { get; set; }
        public String Unidade { get; set; }
        public Int32 Id_produto { get; set; }
        public String Descricao { get; set; }
        public String Data_cad { get; set; }
        public String Operador { get; set; }
        public Decimal Peso_brt { get; set; }
        public Decimal Peso_liq { get; set; }
        public Decimal Laco { get; set; }
        public Decimal Altura { get; set; }
        public String Cod_bar1 { get; set; }
        public String Obs { get; set; }
        public String Embalagem { get; set; }
        public Decimal P_unit { get; set; }
        public Decimal Volume { get; set; }
        public Decimal qtd_emb { get; set; }
        public double Ncm { get; set; }
        public double Cst { get; set; }
    }
}
