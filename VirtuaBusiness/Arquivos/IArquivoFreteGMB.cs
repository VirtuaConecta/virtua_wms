using System;
using System.Collections.Generic;
using VirtuaDTO;

namespace VirtuaBusiness.Arquivos
{
    public interface IArquivoFreteGMB
    {
        IEnumerable<TabelaFreteGmbDTO> GeraListaFreteGmb(string Caminho,String valorKg);
        Nfe ParseXml(string nomeArq, string localArquivos);
        String GeraEdiNotFis(String Modelo, List<Nfe> notas, List<TabelaFreteGmbDTO> dadosFrete);
    }
}