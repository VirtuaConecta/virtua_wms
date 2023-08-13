using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class DocumentoClienteHeadViewModel
    {

        public int Id { get; set; }
        public int CodCliente { get; set; } = 0;
        public String NrDocWms { get; set; }
        public String NrDocOriginal { get; set; } //nf do cliente
        public String SeriedocOriginal { get; set; }
        public String DocEntSai { get; set; }
        public DateTime DataCriacaoWms { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime DataLiberadoEnvio { get; set; }
        public DateTime DataSaidaDoca { get; set; }
        public DateTime DataEntregaDestino { get; set; }
        public Decimal PesoBrt { get; set; }
        public Decimal PesoLiq { get; set; }
        public Decimal QtdTotalItens { get; set; }

        public String ValorTotalDoc { get; set; }
        public String Tp { get; set; }
        public String EspecieProdutos { get; set; }
        public Decimal ValorFrete { get; set; }
        public Int32 NrOrdemCarga { get; set; }
        public String NomeDestino { get; set; }
        public String CnpjDestino { get; set; }
        public String FisJur { get; set; }

        public Int32 NrCte { get; set; }
        public Decimal ValorCte { get; set; }
        public String SerieCte { get; set; }
        public Int32 IdCodMunicipio { get; set; }
        public String NrDocDevolucao { get; set; }
        public int IdRemetente { get; set; } = 0;
        public string ChaveNfe { get; set; }
        public String Obs { get; set; }
        public String OperadorAbertura { get; set; }
        public String OperadorFechaOrdemCarga { get; set; }
        public String LocalPedido { get; set; }
    }
}
