using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class NfeCdocaViewModel
    {
        public Int32 Id { get; set; }
        public Int32 Id_cliente { get; set; }
        public String Razao { get; set; }
        public String Nf_wms { get; set; }
        public Int32 St_nfe { get; set; }
        public Int32 Ord_carga { get; set; }
        public DateTime Data_entrada { get; set; }
        public Int32 Nr_cte { get; set; }
        public Int32 Id_remetente { get; set; }
        public String Nr_original_cliente { get; set; }
        public DateTime Data_emissao { get; set; }
        public String Serie { get; set; }
        public Decimal Valor_Total { get; set; }
        public String Tipo_pedido { get; set; }
        public Decimal Nr_volumes { get; set; }
        public Decimal Peso_brt { get; set; }
        public String Chave { get; set; }
        public String Nome_destino { get; set; }
        public String Cnpj_destino { get; set; }
        public String Cep_destino { get; set; }
        public String Endereco_destino { get; set; }
        public Int32 Id_Cidade_destino { get; set; }
        public String Fis_jur_destino { get; set; }


    }
}
