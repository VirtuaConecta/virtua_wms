using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using Virtua.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using VirtuaBusiness.Cliente;
using System.Security.Cryptography.X509Certificates;
using EdiDocument;
using Microsoft.VisualBasic;

namespace VirtuaBusiness.Arquivos.LayoutsEDI
{
    public class EdiTxt : IbaseLayout
    {
        private readonly ILogger<EdiTxt> logger;
        private readonly IclienteBLL clientes;

  
        public EdiTxt(ILogger<EdiTxt> logger, IConfiguration config, IclienteBLL clientes)
        {
            this.logger = logger;
            Config = config;
            this.clientes = clientes;
        }

        public IConfiguration Config { get; }

        public  List<Nfe> InsereArquivo(string Caminho, String Arquivo)
        {
            List<Nfe> listaRetornada = new List<Nfe>();
            var listaCliente = clientes.Listar_Cliente();
            try
            {
               
                //le o arquivo
                var bodyArq = Virtua.Utilities.Arquivos.LeArquivo(Caminho + "\\" + Arquivo);
                Edi _edi = new Edi();
                var nr_linhas = bodyArq.Count();
                foreach (var line in bodyArq)
                {
                    if (!String.IsNullOrEmpty(line))
                    {
                        // detecta o modelo do EDI
                        if (line.Substring(0, 3) == "311")
                        {
                            var vcnpj = line.Substring(3, 14);
                            var cnpjCliente = listaCliente.Where(x => x.Cnpj == vcnpj ).FirstOrDefault();
                            //Cliente encontrado na lista
                            if (cnpjCliente != null && !String.IsNullOrWhiteSpace(cnpjCliente.Cnpj))
                            {

                                var list = _edi.DecodificaEdi(EdiDocument.ModEdi.NotFis1, Caminho + "\\" + Arquivo);

                                var lista= JsonConvert.DeserializeObject<List<Nfe>>(list[0]);
                                foreach (var item in lista)
                                {
                                    item.Id_cliente = cnpjCliente.IdCliente;

                                    var letra = "E";
                                    if (!String.IsNullOrEmpty(cnpjCliente.Letra))
                                    {
                                        letra = cnpjCliente.Letra;
                                    }
                                    var serie = "";
                                    if (Information.IsNumeric(item.Head.Serie))
                                    {
                                        serie = Convert.ToInt32(item.Head.Serie).ToString();
                                    }
                                    var vNrNf1 = "00000000000000" + item.Head.Nr_original_cliente + $"-{letra}" + serie;
                                    var notaPelog = vNrNf1.Substring(vNrNf1.Length - 11);
                                    item.Nf_Wms = notaPelog;



                                    listaRetornada.Add(item);
                                }

                               // listaRetornada = JsonConvert.DeserializeObject<List<Nfe>>(list);

                            }
                            else
                            {
                                logger.LogError("CNPJ não está na lista em InsereArquivo");
                              //  _log.WriteEntry("CNPJ não está na lista", System.Diagnostics.EventLogEntryType.Error);
                            }


                        }

                    }

                }


            }
            catch (Exception ex)
            {


                logger.LogError("Erro no factory .TXT " + ex.Message);
            }

            return listaRetornada;
        }
    }

}
