using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument.DTO
{
    public class NfeHead
    {

        public DateTime Data_emissao { get; set; }
        public String Nr_doc_transporte { get; set; } // nr do documento de transporte do EDI
        public DateTime? Data_embarque { get; set; }
        public String Nr_original_cliente { get; set; }
        public String Serie { get; set; }
        public Decimal Valor_Total { get; set; }
        public Int32 Nr_romaneio { get; set; }
        public String Cod_rota { get; set; }
        public String Acao_doc { get; set; }
        public String Meio_transporte { get; set; }
        public String Tipo_transporte { get; set; }
        public String Tipo_carga { get; set; }
        public String Tipo_mercadoria { get; set; } // tipo generico Diversos, caixas etc
        public String Cond_frete { get; set; }
        public String Placas { get; set; }
        public String Tipo_pedido { get; set; }
        public String Especie { get; set; }
        public Decimal Nr_volumes { get; set; }
        public Decimal Peso_brt { get; set; }
        public Decimal Peso_liq { get; set; }
        public String Chave { get; set; }

        public String InfoAdicional { get; set; }


        public String Nome_cliente { get; set; }
        public String Cnpj_cli { get; set; }
        public String IE_cliente { get; set; }
        public String Endereco_cli { get; set; }
        public String Cidade_cli { get; set; }
        public String Cep_cli { get; set; }
        public String Estado_cli { get; set; }
        public String Bairro_cli { get; set; }


        public String Nome_destino { get; set; }
        public String Cnpj_destino { get; set; }
        public String IE_destino { get; set; }
        public String Cep_destino { get; set; }
        public String Endereco_destino { get; set; }
        public String Bairro_destino { get; set; }
        public String Cidade_destino { get; set; }
        public String Estado_destino { get; set; }
        public Int32 Id_Cidade_destino { get; set; }
        public String Telefone_destino { get; set; }

        public String Fis_jur_destino { get; set; }



    }
}
