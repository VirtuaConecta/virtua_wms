using VirtuaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaBusiness.Arquivos.LayoutsEDI
{
    public class EdiXls : IbaseLayout
    {
        public EdiXls()
        {
        }

        public List<Nfe> InsereArquivo(string Caminho, String Arquivo)
        {


            List<Nfe> ListaEdiHead = new List<Nfe>();

            //var listaNf = new List<NotaFiscalHeadDTO>();
            //var destinos = new List<DestinatarioDTO>();

            //try
            //{
            //    var Linhas = _ixls.LerPlanilhaXls(Caminho + "\\" + Arquivo);

            //    String Motorista = null;


            //    foreach (var linha in Linhas)
            //    {
            //        EdiHeadDTO EdiHead = new EdiHeadDTO();
            //        var linhaDecod = linha.Split(';');
            //        // Aqui ROTA é usado para identificar o cliente = Cliente JJ
            //        if (linhaDecod[1].ToUpper().IndexOf("N°") > -1)
            //        {
            //            var linhaMot = linhaDecod[1].ToUpper().Split(':');


            //            if (linhaMot[1].Trim().Length >= 3)
            //            {
            //                Motorista = linhaMot[1].Trim();
            //            }
            //        }
            //        if (linhaDecod[1].ToUpper().IndexOf("ROTA") > -1)
            //        {

            //            EdiHead = _ixls.ExecutaClienteJJ(linhaDecod, Motorista);

            //            var dest_item = new DestinatarioDTO();
            //            // para compatibilidade com outros modelos replicamos os dados de nota fiscal e destino no objeto pedidos
            //            dest_item.Destino = EdiHead.EndRecebedor;
            //            dest_item.NotasFiscais = EdiHead.DadosNotaFiscal;
            //            destinos.Add(dest_item);
            //            EdiHead.Pedidos = destinos;
            //            ListaEdiHead.Add(EdiHead);
            //            //zera lista para manter apenas 1 pedido por linha
            //            destinos = new List<DestinatarioDTO>();


            //        }

            //    }

            //}
            //catch (Exception ex)
            //{

            //    _log.WriteEntry("Erro em InsereArquivo XLS " + ex.Message.ToString(), System.Diagnostics.EventLogEntryType.Error);
            //}

            return ListaEdiHead;
        }

    }
}
