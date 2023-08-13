using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class TabelaFreteGmbDTO
    {

        public String Ordem { get; set; }
        public String Remessa { get; set; }
        public String Transporte { get; set; }
        public String Placa { get; set; }
        public String Cidade_destino { get; set; }
        public String Nfiscal_doc { get; set; }

        public String Nf_status { get; set; }
        public String Numero_Nota_Fiscal { get; set; }
       
        public String Data_Doc { get; set; }
        public String Nome_Destino { get; set; }
        public String Emissor_ordem { get; set; }
        public Decimal Peso { get; set; }
        public Decimal Volume { get; set; }
        public String Rota { get; set; }
        public String Cep_Destino { get; set; }
        public String Fornecedor { get; set; }
        public Decimal Valor_frete { get; set; }
        
        public Decimal Valor_nf { get; set; }
        public String Transportador { get; set; }

        //Dados de calculo
        public Decimal Valor_frete_calc { get; set; }
        public Decimal Densidade { get; set; }

        public Decimal Cubagem { get; set; }
        public Decimal  Val_cond { get; set; }
        public Decimal Fator { get; set; }
    }
}
