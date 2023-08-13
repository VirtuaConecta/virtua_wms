using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.SS.Formula;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Arquivos
{
    public class ArquivoFreteGMB : IArquivoFreteGMB
    {
        private readonly IExcelBLL _excel;
        private readonly ILogger<ArquivoFreteGMB> _logger;
        private readonly IConfiguration _config;

        ArquivoFactory _arq;
        public ArquivoFreteGMB(IExcelBLL excel, ILogger<ArquivoFreteGMB> logger ,IConfiguration config, ArquivoFactory arq)
        {
            _excel = excel;
            _logger = logger;
            _config = config;
            _arq = arq;
        }

        public IEnumerable<TabelaFreteGmbDTO> GeraListaFreteGmb(String Caminho,String valorKg)
        {
            var listaRetorno = new List<TabelaFreteGmbDTO>();
            try
            {
                var listaSerializada = new List<string>();
                var exten = Caminho.Split('.');
             
                if (exten[1].ToUpper() == "XLSX")
                {
 

                 listaSerializada = _excel.LerPlanilhaFreteGMBXlsx(Caminho);
                }
                else// XLS
                {
                    // tem que ajustar
                     listaSerializada = _excel.LerPlanilhaFreteGMBXls(Caminho);
                }
                if (listaSerializada.Count() > 0)
                {
                    foreach (var item in listaSerializada)
                    {
                        var itemDTO = new TabelaFreteGmbDTO();
                        var itemDeserializado = item.Split(';');

                        itemDTO.Ordem = itemDeserializado[0];
                        itemDTO.Remessa = itemDeserializado[1];
                        itemDTO.Transporte = itemDeserializado[2];
                        itemDTO.Placa = itemDeserializado[3];
                        itemDTO.Cidade_destino = itemDeserializado[4];
                        itemDTO.Nfiscal_doc = itemDeserializado[5];
                        itemDTO.Numero_Nota_Fiscal = itemDeserializado[6];
                        itemDTO.Nf_status = itemDeserializado[7];
                        itemDTO.Data_Doc = itemDeserializado[8];
                        itemDTO.Nome_Destino = itemDeserializado[9];
                        itemDTO.Emissor_ordem = itemDeserializado[10];
                        try { itemDTO.Peso = Convert.ToDecimal(itemDeserializado[11]); }
                        catch (Exception) { itemDTO.Peso = 0; }

                        try { itemDTO.Volume = Convert.ToDecimal(itemDeserializado[12]); }
                        catch (Exception) { itemDTO.Volume = 0; }

                        itemDTO.Rota = itemDeserializado[13];
                        itemDTO.Cep_Destino = itemDeserializado[14];
                        itemDTO.Fornecedor = itemDeserializado[15];
                        try { itemDTO.Valor_frete = Convert.ToDecimal(itemDeserializado[16]); }
                        catch (Exception) { itemDTO.Valor_frete = 0; }
                        try { itemDTO.Valor_nf = Convert.ToDecimal(itemDeserializado[17]); }
                        catch (Exception) { itemDTO.Valor_nf = 0; }

                        itemDTO.Transportador = itemDeserializado[18];

                 

                        listaRetorno.Add(itemDTO);
                    }



                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, "Erro em GeraListaFreteGmb");
            }

            var listaCalculada = CalculaFrete(listaRetorno, valorKg);

            return listaCalculada;
        }


        public Nfe ParseXml(string nomeArq, string localArquivos)
        {
            var nota = new Nfe();
            try
            {
                if (!String.IsNullOrEmpty(nomeArq) && !String.IsNullOrEmpty(localArquivos))
                {
                    var arq = nomeArq.ToString().Split('.');

                    var ext = arq[1].ToUpper().Replace(".", "");
                  

                    String extensoes = _config.GetSection("Arquivos:Extensoes").Value;
                    // Aqui acrescentamos todas as extensões que queremos inserir
                    if (extensoes.IndexOf(ext) > -1)
                    {
                        var lista_nfes = new List<Nfe>();
                        //formata dados do Arquivo
                        lista_nfes = _arq.CapturaDadosArquivoFactory(ext).InsereArquivo(localArquivos, nomeArq);

                        if (lista_nfes.Count() == 0)
                        {
                            
                             var retornoMover = Virtua.Utilities.Arquivos.MoveArquivo("Erro", nomeArq, localArquivos);
                           
                        }
                        else
                        {
                            nota = lista_nfes.FirstOrDefault();
                            var retornoMover = Virtua.Utilities.Arquivos.MoveArquivo("OK", nomeArq, localArquivos);
                        }


                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return nota;
        }

        public String GeraEdiNotFis(String Modelo,List<Nfe> notas, List<TabelaFreteGmbDTO> dadosFrete)
        {
           

           

            String arqString = null;
            Edi _edi = new Edi();
            var notasFormatadas = validaNfeFrete(notas,dadosFrete);
            var nfSerializada = JsonConvert.SerializeObject(notasFormatadas);
            if (Modelo == "GMB")
            {
                var lis= _edi.DecodificaEdi(EdiDocument.ModEdi.NotFisGMB, "NomeArq|" + nfSerializada);
                arqString = lis[1];
            }
           
            return arqString;
        }

        private List<Nfe> validaNfeFrete(List<Nfe> notas, List<TabelaFreteGmbDTO> dadosFrete)
        {
            var listaFinal = new List<Nfe>();
            try
            {
                foreach (var item in notas)
                {
                    var nota = new Nfe();
                    var frete = dadosFrete.Where(x => x.Numero_Nota_Fiscal == item.Head.Nr_original_cliente).FirstOrDefault();

                    nota.Empresa = item.Empresa;
                    nota.IdPed = item.IdPed;
                    nota.Id_cliente = item.Id_cliente;
                    nota.Id_rementente = item.Id_rementente;
                    nota.Nf_Wms = item.Nf_Wms;
                    nota.Num_remessa = frete.Remessa;
                    nota.Tipo_remessa = item.Tipo_remessa;
                    
                    nota.Valor_frete = frete.Valor_frete_calc;

                    var hd = new NfeHead();
                    hd = item.Head;
                    hd.Cod_rota = String.IsNullOrWhiteSpace(frete.Rota)?"0": frete.Rota;
                    hd.Nr_doc_transporte = frete.Transporte;
                    nota.Head = hd;
                    nota.ListaItens = item.ListaItens;
                    listaFinal.Add(nota);
                }

                 


            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, "Erro em validaNfeFrete");
            }


            return listaFinal;
        }


        private List<TabelaFreteGmbDTO> CalculaFrete(List<TabelaFreteGmbDTO> lista, String valorKg)
        {
            Decimal resp =0;
            Decimal volumeTotal = 0;
            Decimal pesoTotal=0;
            Decimal DensidadeTotal = 0;
            Decimal CubagemTotal = 0;
            Decimal valTab =  Convert.ToDecimal(valorKg);
            // primeira parte
            var listaSaida = new List<TabelaFreteGmbDTO>();
            var listaSaidaFinal = new List<TabelaFreteGmbDTO>();
            //rateia Dencidade e cubagem
            foreach (var item in lista)
            {
                var itemSaida = new TabelaFreteGmbDTO();
                itemSaida = item;

   
                if (item.Volume > 0)
                {
                    itemSaida.Densidade = item.Peso / item.Volume;
                }
                itemSaida.Cubagem = itemSaida.Densidade <= 300M ? 300M * item.Volume : item.Peso;
                volumeTotal += item.Volume;
                pesoTotal += item.Peso;
                DensidadeTotal += itemSaida.Densidade;
                CubagemTotal += itemSaida.Cubagem;
                listaSaida.Add(itemSaida);

            }
            var totalPesoInt = Math.Truncate(pesoTotal);//pega a parte inteira do numero
           
            // rateia valor condicao e Fator
            foreach (var item in listaSaida)
            {
                var itemSaidafinal = new TabelaFreteGmbDTO();
                itemSaidafinal = item;

                itemSaidafinal.Val_cond = valTab*totalPesoInt;
                itemSaidafinal.Fator = itemSaidafinal.Cubagem / CubagemTotal;
                itemSaidafinal.Valor_frete_calc = Math.Round(itemSaidafinal.Val_cond * itemSaidafinal.Fator,2);

                listaSaidaFinal.Add(itemSaidafinal);
            }




            return listaSaidaFinal;
        }

    }
}
