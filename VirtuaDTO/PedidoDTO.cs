using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class PedidoDTO
    {
        public String tipo_doc { get; set; }
        public Int32? Id_cliente { get; set; }
        public String NomeCliente { get; set; }
        public Int32 indice { get; set; }
        public Int32? Id_remetente { get; set; }
        public String Nr_documento { get; set; }
        public String Nr_doc_origem { get; set; }
        public String Serie_doc { get; set; }
        public String Nome_dest { get; set; }
        public String Cancelado { get; set; }
        public String Cnpj_dest { get; set; }
        public String Cpf_cnpj_cliente { get; set; }
        public String Cfop { get; set; }
        public DateTime? Dt_emissao { get; set; }
        public DateTime? Dt_entrada { get; set; }
        public String Remessa { get; set; }
        public String Processado { get; set; }
        public String tipo { get; set; }
        public Decimal? Peso_brt { get; set; }

        public Decimal? Peso_liq { get; set; }

        public Int32? Nr_volumes { get; set; }

        public Decimal? Total_nf { get; set; }

        public Int32? Cod_municipio { get; set; }
        public String cod_estado { get; set; }
        public String Chave { get; set; }
        public String Fis_Jur { get; set; }
        public String Dados_adicionais { get; set; }
        public String Operador { get; set; }
        public String Nf_saida { get; set; }
        public String Serie_Nf_saida { get; set; }
        public String Nf_ok { get; set; }

        public DateTime? Dt_emissao_nf_saida { get; set; }
        public DateTime? Dt_reg { get; set; }

        public String Especie { get; set; }
        public DateTime? Dt_processado { get; set; }
        public String Ip { get; set; }
        public int Criado { get; set; }
        //Itens

        //public Int32 Id_produto { get; set; }
        //public String Sku { get; set; }
        //public Decimal? Qtd { get; set; }
        //public Decimal? P_unit { get; set; }

        //public String Desc_Sku { get; set; }
        //public Int32? Ncm { get; set; }
        //public Int32? Cst { get; set; }
    }
}
