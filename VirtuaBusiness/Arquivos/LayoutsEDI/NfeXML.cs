using EdiDocument;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtua.Utilities;
using VirtuaBusiness.Cliente;

using VirtuaDTO;
using VirtuaRepository;
using XMLdoc;
using XMLdoc.ModelSerialization;
using System.IO;

namespace VirtuaBusiness.Arquivos.LayoutsEDI
{
    public class NfeXML : IbaseLayout
    {
        private readonly ILogger<EdiTxt> logger;
        private readonly IclienteBLL cliente;
     
        private readonly IDataConnection db;
        private IConfiguration _config;
        public NfeXML(ILogger<EdiTxt> logger, IConfiguration config, IclienteBLL cliente)
        {
            this.logger = logger;
            _config = config;
            this.cliente = cliente;
        }




        public List<Nfe> InsereArquivo(string Caminho, string Arquivo)
        {


            XmlBusiness xmlBLL = new XmlBusiness();
            var ListEdiHead = new List<Nfe>();

            var CaminhoErro = $"{Caminho}\\Erro";

            var nfe = new NFeProc();
            var conteudoTxt = File.ReadAllText(Caminho+"\\"+Arquivo);


            if (conteudoTxt.IndexOf("<nfeProc") > -1)
            {
                // Nfe XML
                nfe = xmlBLL.GetObjectFromFile<NFeProc>(Caminho + "\\" + Arquivo);
            }
            else // modelo pedido
            {

                var nfedet = xmlBLL.GetObjectFromFileAlfa(Caminho + "\\" + Arquivo);
                nfe.NotaFiscalEletronica = nfedet;
                //ListEdiHead.Add(nfe.NotaFiscalEletronica);
            }
            if (nfe == null)
                {

                    logger.LogError("Falha ao ler o arquivo xml. Verifique se o arquivo é de uma NF-e/NFC-e autorizada!");
                }
                else
                {
                    try
                    {
                        var Emitente = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente;

                        //verifca se o cliente está cadastrado

                        var cli = new ClienteDTO();

                        cli = cliente.Pesquisar_ClienteCnpj(Emitente.CNPJ);
                        if (cli != null && nfe != null)
                        {

                            // Model do objeto resultado
                            var nfeItem = new Nfe();
                            var head = new NfeHead();
                            var itens = new List<NfeItens>();
                            var ChaveXml = "0".PadLeft(44, '0');

                        if (!String.IsNullOrWhiteSpace(nfe.NotaFiscalEletronica.InformacoesNFe.ChaveNfe))
                        {
                            ChaveXml = nfe.NotaFiscalEletronica.InformacoesNFe.ChaveNfe.Replace("NFe", "");
                        }
                           

                            var DestinoXml = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario;

                            var InfoXml = nfe.NotaFiscalEletronica.InformacoesNFe.Info;

                            var IdXml = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao;

                            var ValorXml = nfe.NotaFiscalEletronica.InformacoesNFe.Valor;

                            var TransportXML = nfe.NotaFiscalEletronica.InformacoesNFe.DadosTransporte;

                            var DetalheXML = nfe.NotaFiscalEletronica.InformacoesNFe.Detalhe;

                            nfeItem.Id_rementente = cli.Id_remetente;
                            nfeItem.Id_cliente = cli.IdCliente;

                            if (TransportXML.transportadora != null)
                            {
                                nfeItem.Empresa = TransportXML.transportadora.xNome;
                            }

                            var letra = "E";
                            if (!String.IsNullOrEmpty(cli.Letra))
                            {
                                letra = cli.Letra;
                            }
                            var vNrNf1 = "00000000000000" + IdXml.nNF + $"-{letra}" + IdXml.serie;
                            var notaPelog = vNrNf1.Substring(vNrNf1.Length - 11);
                            // Valida se cliente esta no db

                            head.Nr_original_cliente = IdXml.nNF;
                            head.Serie = IdXml.serie.ToString();
                            head.Chave = ChaveXml;
                            head.Data_emissao = IdXml.dhEmi;
                            head.Data_embarque = IdXml.dhSaiEnt;


                            if (TransportXML.Volumes != null)
                            {
                                head.Peso_brt = TransportXML.Volumes.pesoB;
                                head.Peso_liq = TransportXML.Volumes.pesoL;
                                if (Information.IsNumeric(TransportXML.Volumes.qVol))
                                    head.Nr_volumes = Convert.ToDecimal(TransportXML.Volumes.qVol);
                                head.Especie = TransportXML.Volumes.esp;
                                head.Tipo_mercadoria = TransportXML.Volumes.marca;
                            }


                            head.Valor_Total = ValorXml.ICMSTot.vNF;
                            head.Acao_doc = "I-INCLUIR";

                            nfeItem.Nf_Wms = notaPelog;

                            //dados cliente
                            head.Nome_cliente = Emitente.xNome;
                            head.Endereco_cli = $"{Emitente.Endereco.xLgr},{Emitente.Endereco.nro}";
                            head.Cidade_cli = Emitente.Endereco.xMun;
                            head.Estado_cli = Emitente.Endereco.UF;
                            head.Cep_cli = Emitente.Endereco.CEP;
                            head.Cnpj_cli = Emitente.CNPJ;
                            head.IE_cliente = Emitente.IE;
                            head.Bairro_cli = Emitente.Endereco.xBairro;

                            //Destino

                            head.Nome_destino = DestinoXml.xNome;
                            head.Cnpj_destino = string.IsNullOrEmpty(DestinoXml.CNPJ) ? DestinoXml.CPF : DestinoXml.CNPJ;

                            //remessa

                            head.Tipo_pedido = "XML";


                            head.Fis_jur_destino = string.IsNullOrEmpty(DestinoXml.CNPJ) ? "F" : "J";

                            head.Endereco_destino = $"{DestinoXml.Endereco.xLgr},{DestinoXml.Endereco.nro}";

                            head.Cidade_destino = DestinoXml.Endereco.xMun;
                            head.Estado_destino = DestinoXml.Endereco.UF;
                            head.Cep_destino = DestinoXml.Endereco.CEP;
                            head.Bairro_destino = DestinoXml.Endereco.xBairro;

                            if (Information.IsNumeric(DestinoXml.Endereco.cMun))
                            {
                                head.Id_Cidade_destino = Convert.ToInt32(DestinoXml.Endereco.cMun);

                            }
                            head.IE_destino = DestinoXml.IE;
                            head.Telefone_destino = DestinoXml.Endereco.fone;

                            // produtos

                            foreach (var item in DetalheXML)
                            {
                                var prod = new NfeItens();
                                prod.Ean = item.Produto.cEAN;
                                prod.Ncm = item.Produto.NCM;
                                prod.Qtd = Convert.ToDecimal(item.Produto.qCom);
                                prod.Sku = item.Produto.cProd;
                                prod.Especie = item.Produto.uCom;
                                prod.Descricao_prod = item.Produto.xProd;

                                prod.Punit = Convert.ToDecimal(item.Produto.vUnCom);
                                itens.Add(prod);
                            }

                            nfeItem.Head = head;
                            nfeItem.ListaItens = itens;


                            ListEdiHead.Add(nfeItem);
                        }
                        else
                        {
                            Virtua.Utilities.Arquivos.MoverArquivo(Caminho, CaminhoErro, Arquivo);
                        }
                    }
                    catch (Exception ex)
                    {
                        Virtua.Utilities.Arquivos.MoverArquivo(Caminho, CaminhoErro, Arquivo);

                    }
                }
            
          
      

    

            return ListEdiHead;
        }


    }
}
