using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VirtuaBusiness.Municipio;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Nfs_cdoca
{
    public class NfsBLL: InfsCdoca
    {
        private readonly IDataConnection _db;
        private readonly ImunicipioBLL _imunicipio;
        private readonly IConfiguration _config;
        private readonly ILogger<NfsBLL> _logger;
        private readonly ArquivoFactory _arq;

        public NfsBLL(IDataConnection db , ImunicipioBLL imunicipio, IConfiguration config,
            ILogger<NfsBLL> logger, ArquivoFactory arq)
        {
            _db = db;
            _imunicipio = imunicipio;
            _config = config;
            _logger = logger;
            _arq = arq;
        }


        /// <summary>
        /// Inserir nota na tabela nfs do cdoca
        /// </summary>
        /// <param name="nota"></param>
        /// <returns></returns>
        public String InserirNfsBLL(Nfe nota)
        {
            String resp = "Erro";

            // verifica se a nota existe, caso nõa, insere

            var verficaNota = _db.PesquisaNfe(nota.Id_cliente, nota.Nf_Wms);
            if (String.IsNullOrEmpty(verficaNota))
            {
                var notaHeadAlterado = new NfeHead();

                notaHeadAlterado = nota.Head;
                //Verifica se p cod recebido é cod ibge. se for troc pelo id da tabela,
                //caso contrario mantem.

                var cod_mun = nota.Head.Id_Cidade_destino;

                var idMun = _imunicipio.Listar_Municipios().Where(z => z.Cod_municipio == cod_mun).Select(x => x.Id).FirstOrDefault();
                if (idMun != null)
                {
                    notaHeadAlterado.Id_Cidade_destino = Convert.ToInt32(idMun);
                }
                else
                {
                    notaHeadAlterado.Id_Cidade_destino = cod_mun;
                }
                nota.Head = notaHeadAlterado;



                resp = _db.InserirNfe(nota);

            }


            return resp;
        }
        /// <summary>
        /// Lista nfe cdoca
        /// </summary>
        /// <param name="dias">intervalo ultimos n dias</param>
        /// <param name="tipo">Entrad I ou saida O</param>
        /// <param name="oc">Nr da ordem de carga - 0 para sem Oc</param>
        /// <returns>lista de nfs</returns>
        public List<nfeCdoca> ListNfeBll(Int32 dias,String tipo,Int32 oc)
        {

            return _db.ListaNfe(dias, tipo, oc);

        }

        public string PesquisaNfeBLL(Int32 id_cliente, String nf)
        {
            return _db.PesquisaNfe(id_cliente, nf);

        }

        public string InsereArquivosDaPastaCorssDockBll(List<string> listarArquivos, string localArquivos) {
            // fazer a partir da PedidosBLL ->InsereArquivosDaPastaBll
            // InserirNfsBLL dados na => InserirNfeCrossDock(Nfe nota)
            //Lembrar que neste método tem que fazer uma trazação no mesmo estilo de pedidos co o nf_crossdock

            String resp = null;
            try
            {

                var ArquivoOk = "S";

                if (listarArquivos.Count > 0)
                {
                    
                    foreach (var item in listarArquivos)
                    {

                        var arq = item.ToString().Split('.');

                        var ext = arq[1].ToUpper().Replace(".", "");
                        var nomeArq = item;

                        if (ext == ".ZIP")
                        {
                            var arquivoDescopactado = Virtua.Utilities.Arquivos.DescompactaZIP(nomeArq, localArquivos);
                            if (arquivoDescopactado == "OK")
                            {
                                Virtua.Utilities.Arquivos.MoveZip(nomeArq, localArquivos);
                            }
                        }

                        String extensoes = _config.GetSection("Arquivos:Extensoes").Value;
                        String Cnpj_Wms = _config.GetSection("Cnpj_entrada_Wms").Value;
                        // Aqui acrescentamos todas as extensões que queremos inserir
                        if (extensoes.IndexOf(ext) > -1)
                        {
                            var lista_nfes = new List<Nfe>();
                            //formata dados do Arquivo
                            lista_nfes = _arq.CapturaDadosArquivoFactory(ext).InsereArquivo(localArquivos, nomeArq);

                            if (lista_nfes.Count() == 0)
                            {
                                ArquivoOk = "Erro";
                                var retornoMover = Virtua.Utilities.Arquivos.MoveArquivo(ArquivoOk, nomeArq, localArquivos);
                                resp = "Nenhum arquivo encontrado";
                            }

                            //verifica se vem nr de nf repetido na lista

                            var repetidos = lista_nfes
                 .GroupBy(x => x.Nf_Wms )
                 .Where(g => g.Count() > 1)
                 .Select(g => g.Key)
                 .ToList();
                            
                            foreach (var ped in lista_nfes)
                            {

                                try
                                {

                                    //valida nfs existentes
                                    var pedencontrado = PesquisaNfeBLL( ped.Id_cliente, ped.Nf_Wms);
                                    if (String.IsNullOrEmpty(pedencontrado))
                                    {
                                       resp  = _db.InserirNfeCrossDock(ped);

                               


                                    }
                                    else
                                    {
                                        //move o pedido
                                        ArquivoOk = "Erro";
                                        resp += "Pedido ja existe";
                                    }


                                    var retornoMover = Virtua.Utilities.Arquivos.MoveArquivo(ArquivoOk, nomeArq, localArquivos);

                                }
                                catch (Exception ex)
                                {
                                    resp = "Erro";
                                    //  Task.Delay(2000).Wait();
                                   _logger.LogError(ex.Message, "Erro em InsereArquivosDaPastaCorssDockBll");
                                }

                            }




                        }
                    }

                }
            }
            catch (Exception ex)
            {
                resp = "Erro";
                _logger.LogError(ex.Message, "Erro em InsereArquivosCrossDock");

            }


            return resp;





            
        }


        public List<nfeCdoca> ListarNfSaidaCdocaBll(Int32 id_cliente,String dt_in,String dt_f)
        {
            return _db.ListarNfeSaidaCdoca(id_cliente, dt_in, dt_f);


        }

        public List<nfeCdoca> ListarNfSaidaCdocaBll( String nr_nf)
        {
            return _db.ListarNfeSaidaCdocaPorNf(nr_nf);


        }

        public List<NfeCrossDock> BuscaItensListaSeparacao(String filtro)
        {

            return _db.BuscaItensListaSeparacaoDal(filtro);

        }

        public List<NfeCrossDock> ListaNfCrosDockSep(String filtro )
        {

            return _db.ListaNfCrossDockDal(filtro);

        }
    }
}
