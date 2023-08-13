using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Nfs_cdoca
{
   public interface InfsCdoca
    {
        String InserirNfsBLL(Nfe nota);
        List<nfeCdoca> ListNfeBll(Int32 dias, String tipo, Int32 oc);
        string InsereArquivosDaPastaCorssDockBll(List<string> listarArquivos, string localArquivos);
        List<nfeCdoca> ListarNfSaidaCdocaBll(Int32 id_cliente, String dt_in, String dt_f);
        List<NfeCrossDock> BuscaItensListaSeparacao(String filtro);
        List<NfeCrossDock> ListaNfCrosDockSep(String filtro);
        public List<nfeCdoca> ListarNfSaidaCdocaBll(String nr_nf);
    }
}
