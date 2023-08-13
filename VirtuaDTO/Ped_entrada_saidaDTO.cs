using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class Ped_entrada_saidaDTO
    {
        //  public int indice { get; set; }
        //  public int id_cliente { get; set; }

        public String N_fiscal { get; set; }//
        public String sku { get; set; }//
        public String data_emissao { get; set; }//

        //  public String data_saida_plg { get; set; }

        public String descricao { get; set; }//
        public Decimal qtd { get; set; }//
        public Decimal val_unit { get; set; }//
        public String nf_pelog { get; set; }//

        //  public String processado { get; set; }

        public String remessa { get; set; }//
        public String nf_ok { get; set; }//
        public String cancelado { get; set; }//

        //  public Double peso { get; set; }
        //  public String cnpj { get; set; }

        public String cidade { get; set; }//
        public String estado { get; set; }//
        public String nome_cli { get; set; }//

        //   public String bairro { get; set; }
        //   public Decimal total_nf { get; set; }
        //  public String data_emissao_MySql { get; set; }
        //   public String dt_processado { get; set; }

        public int volume_total { get; set; }//
        public Decimal val_nf { get; set; }//
        public Decimal peso_nf { get; set; }//
        public Decimal valor_nf_retorno { get; set; }//

        public String dt_processado { get; set; }//
        public String ord_carga { get; set; }//
        public String nome_transp { get; set; }//
        public String data_fim { get; set; }//

    }
}
