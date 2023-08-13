using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class TabelaFreteGMBViewModel
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
        public String Peso { get; set; }
        public String Volume { get; set; }
        public String Rota { get; set; }
        public String Cep_Destino { get; set; }
        public String Fornecedor { get; set; }
        public String Valor_frete { get; set; }
        public String Valor_nf { get; set; }
        public String Transportador { get; set; }

        public Decimal Valor_frete_calc { get; set; }
        public Decimal Densidade { get; set; }

        public Decimal Cubagem { get; set; }
        public Decimal Val_cond { get; set; }
        public Decimal Fator { get; set; }
    }
}
