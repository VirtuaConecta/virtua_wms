using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using Virtua.Utilities;
using System.Xml.Linq;
using XMLdoc.ModelSerialization;
using static XMLdoc.ModelSerialization.NFe;
using System.Diagnostics;

namespace XMLdoc
{
    public class XmlBusiness
    {



        public T GetObjectFromFile<T>(string arquivo) where T : class
        {
            //    var doc = XElement.Load(arquivo);
            //  var ms = doc.Elements("nfe");
            //var arqu = Arquivos.LeArquivo(arquivo);

            //String arquivo_lido = null;
            //foreach (var item in arqu)
            //{
            //    arquivo_lido += item;
            //}
            //System.Xml.XmlReaderSettings settings = new XmlReaderSettings();

            //settings.IgnoreWhitespace = true;
            //  StringWriter resultWriter = new StringWriter();
            var serialize = new XmlSerializer(typeof(T));
            var xmlArquivo = XmlReader.Create(arquivo);


            //  XmlReader xmlArquivo= null;
            try
            {
                //using (TextReader sr = new StringReader(arquivo_lido))
                //  {


                //  var test = (T)serialize.Deserialize(xmlArquivo);

                var resp = (T)serialize.Deserialize(xmlArquivo);

                xmlArquivo.Close();
                return resp;
                // }


            }
            catch (Exception ex)
            {
                var e = ex.Message;
                xmlArquivo.Close();
                return null;
            }


        }

        public NFe GetObjectFromFileAlfa(string arquivo)
        {

            try
            {
                var dt = new object();

                var arq = Arquivos.LeArquivo(arquivo);

                String chave = null;




                var nfe = new NFe();
                var Identificacao = new Identificacao();
                var Emitente = new Emitente();
                var Destinatario = new Destinatario();
                var EndEmite = new Endereco();
                var EndDest = new Endereco();
                var listaDetalhe = new List<Detalhe>();
                var Icms = new Icms();
                var infoAdic1 = new InfoAdic();
                var transporta = new Tranportadora();
                String no = null;
                var item_detalhe = new Detalhe();
                var prod = new Produto();
                foreach (var item in arq)
                {
                    try
                    {
                        var item1 = item.Trim();

                        if (item1.Contains("<ide>"))
                            no = "<ide>";
                        if (item1.Contains("<emit>"))
                            no = "<emit>";
                        if (item1.Contains("<dest>"))
                            no = "<dest>";
                        if (item1.Contains("<det"))
                            no = "<det";
                        if (item1.Contains("<ICMSTot>"))
                            no = "<ICMSTot>";
                        if (item1.Contains("<infAdic>"))
                            no = "<infAdic>";
                        if (item1.Contains("<transporta>"))
                            no = "<transporta>";


                        if (no == "<ide>")
                        {
                            var posSerie = item1.IndexOf("<serie>");
                            var posNrNf = item1.IndexOf("<nNF>");
                            var posDtEmi = item1.IndexOf("<dhEmi>");
                            var fim = item1.IndexOf("</");

                            if (posSerie > -1)
                                Identificacao.serie = Convert.ToInt32(item1.Substring((posSerie + 7), (fim - (posSerie + 7))));
                            if (posNrNf > -1)
                                Identificacao.nNF = item1.Substring((posNrNf + 5), (fim - (posNrNf + 5)));
                            if (posDtEmi > -1)
                                Identificacao.dhEmi = Convert.ToDateTime(item1.Substring((posDtEmi + 7), (fim - (posDtEmi + 7))));
                        }


                        if (no == "<emit>")
                        {
                            var posCnpj = item1.IndexOf("<CNPJ>");
                            var posIe = item1.IndexOf("<IE>");
                            var posNome = item1.IndexOf("<xNome>");
                            var posEnd = item1.IndexOf("<xLgr>");
                            var posNro = item1.IndexOf("<nro>");
                            var posxCpl = item1.IndexOf("<xCpl>");
                            var posBairro = item1.IndexOf("<xBairro>");

                            var posCodMun = item1.IndexOf("<cMun>");
                            var posCidade = item1.IndexOf("<xMun>");
                            var posEstado = item1.IndexOf("<UF>");
                            var posCep = item1.IndexOf("<CEP>");
                            var posTel = item1.IndexOf("<fone>");

                            var fim = item1.IndexOf("</");
                            if (posCnpj > -1)
                                Emitente.CNPJ = item1.Substring((posCnpj + 6), (fim - (posCnpj + 6)));
                            if (posIe > -1)
                                Emitente.IE = item1.Substring((posIe + 4), (fim - (posIe + 4)));
                            if (posNome > -1)
                                Emitente.xNome = item1.Substring((posNome + 7), (fim - (posNome + 7)));
                            if (posEnd > -1)
                                EndEmite.xLgr = item1.Substring((posEnd + 6), (fim - (posEnd + 6)));
                            if (posNro > -1)
                                EndEmite.nro = item1.Substring((posNro + 5), (fim - (posNro + 5)));
                            if (posxCpl > -1)
                                EndEmite.xCpl = item1.Substring((posxCpl + 6), (fim - (posxCpl + 6)));
                            if (posBairro > -1)
                                EndEmite.xBairro = item1.Substring((posBairro + 9), (fim - (posBairro + 9)));
                            if (posCodMun > -1)
                                EndEmite.cMun = item1.Substring((posCodMun + 6), (fim - (posCodMun + 6)));
                            if (posCidade > -1)
                                EndEmite.xMun = item1.Substring((posCidade + 6), (fim - (posCidade + 6)));
                            if (posEstado > -1)
                                EndEmite.UF = item1.Substring((posEstado + 4), (fim - (posEstado + 4)));
                            if (posCep > -1)
                                EndEmite.CEP = item1.Substring((posCep + 5), (fim - (posCep + 5)));
                            if (posTel > -1)
                                EndEmite.fone = item1.Substring((posTel + 6), (fim - (posTel + 6)));

                        }


                        if (no == "<dest>")
                        {
                            var posCnpj = item1.IndexOf("<CNPJ>");
                            var posCpf = item1.IndexOf("<CPF>");
                            var posIe = item1.IndexOf("<IE>");
                            var posNome = item1.IndexOf("<xNome>");
                            var posEnd = item1.IndexOf("<xLgr>");
                            var posNro = item1.IndexOf("<nro>");
                            var posxCpl = item1.IndexOf("<xCpl>");
                            var posBairro = item1.IndexOf("<xBairro>");

                            var posCodMun = item1.IndexOf("<cMun>");
                            var posCidade = item1.IndexOf("<xMun>");
                            var posEstado = item1.IndexOf("<UF>");
                            var posCep = item1.IndexOf("<CEP>");
                            var posTel = item1.IndexOf("<fone>");

                            var fim = item1.IndexOf("</");
                            if (posCnpj > -1)
                                Destinatario.CNPJ = item1.Substring((posCnpj + 6), (fim - (posCnpj + 6)));
                            if (posCpf > -1)
                                Destinatario.CPF = item1.Substring((posCpf + 5), (fim - (posCpf + 5)));
                            if (posIe > -1)
                                Destinatario.IE = item1.Substring((posIe + 4), (fim - (posIe + 4)));
                            if (posNome > -1)
                                Destinatario.xNome = item1.Substring((posNome + 7), (fim - (posNome + 7)));
                            if (posEnd > -1)
                                EndDest.xLgr = item1.Substring((posEnd + 6), (fim - (posEnd + 6)));
                            if (posNro > -1)
                                EndDest.nro = item1.Substring((posNro + 5), (fim - (posNro + 5)));
                            if (posxCpl > -1)
                                EndDest.xCpl = item1.Substring((posxCpl + 6), (fim - (posxCpl + 6)));
                            if (posBairro > -1)
                                EndDest.xBairro = item1.Substring((posBairro + 9), (fim - (posBairro + 9)));
                            if (posCodMun > -1)
                                EndDest.cMun = item1.Substring((posCodMun + 6), (fim - (posCodMun + 6)));
                            if (posCidade > -1)
                                EndDest.xMun = item1.Substring((posCidade + 6), (fim - (posCidade + 6)));
                            if (posEstado > -1)
                                EndDest.UF = item1.Substring((posEstado + 4), (fim - (posEstado + 4)));
                            if (posCep > -1)
                                EndDest.CEP = item1.Substring((posCep + 5), (fim - (posCep + 5)));
                            if (posTel > -1)
                                EndDest.fone = item1.Substring((posTel + 6), (fim - (posTel + 6)));
                        }

                        if (no == "<det")
                        {

                            var fim = item1.IndexOf("</");


                            if (item1.IndexOf("<det nItem=") > -1)
                            {
                                var tt = item1.Replace("\"", "");
                                var fim1 = tt.IndexOf(">");
                                item_detalhe.nItem = Convert.ToInt32(tt.Substring(11, (fim1 - 11)));
                            }
                            if (item1.IndexOf("<cProd>") > -1)
                                prod.cProd = item1.Substring(7, fim - 7);

                            if (item1.IndexOf("<cEAN>") > -1)
                                prod.cEAN = item1.Substring(6, fim - 6);

                            if (item1.IndexOf("<xProd>") > -1)
                                prod.xProd = item1.Substring(7, fim - 7);

                            if (item1.IndexOf("<NCM>") > -1)
                                prod.NCM = item1.Substring(5, fim - 5);

                            if (item1.IndexOf("<CFOP>") > -1)
                                prod.CFOP = item1.Substring(6, fim - 6);

                            if (item1.IndexOf("<uCom>") > -1)
                                prod.uCom = item1.Substring(6, fim - 6);

                            if (item1.IndexOf("<qCom>") > -1)
                                prod.qCom = Convert.ToDouble(item1.Substring(6, fim - 6).Replace(".", ","));


                            if (item1.IndexOf("<vUnCom>") > -1)
                                prod.vUnCom = Convert.ToDouble(item1.Substring(8, fim - 8).Replace(".", ","));

                            if (item1.IndexOf("</prod>") > -1)
                            {
                                item_detalhe.Produto = prod;
                                listaDetalhe.Add(item_detalhe);
                                item_detalhe = new Detalhe();
                                prod = new Produto();
                            }
                        }
                        
                            if (no == "<transporta>")
                            {
                            var fim = item1.IndexOf("</");
                            if (item1.IndexOf("<CNPJ>") > -1)
                                transporta.CNPJ = item1.Substring(6, fim - 6);
                            if (item1.IndexOf("<xNome>") > -1)
                                transporta.xNome = item1.Substring(7, fim - 7);
                            if (item1.IndexOf("<IE>") > -1)
                                transporta.IE = item1.Substring(4, fim - 4);
                            if (item1.IndexOf("<xEnder>") > -1)
                                transporta.xEnder = item1.Substring(8, fim - 8);
                            if (item1.IndexOf("<xMun>") > -1)
                                transporta.xMun = item1.Substring(6, fim - 6);
                            if (item1.IndexOf("<UF>") > -1)
                                transporta.UF = item1.Substring(4, fim - 4);
                        }

                        if (no == "<ICMSTot>")
                        {
                            var fim = item1.IndexOf("</");
                            if (item1.IndexOf("<vBC>") > -1)
                                Icms.vBC = Convert.ToDecimal(item1.Substring(5, fim - 5).Replace(".", ","));
                            if (item1.IndexOf("<vICMS>") > -1)
                                Icms.vICMS = Convert.ToDecimal(item1.Substring(7, fim - 7).Replace(".", ","));
                            if (item1.IndexOf("<vProd>") > -1)
                                Icms.vProd = Convert.ToDecimal(item1.Substring(7, fim - 7).Replace(".", ","));
                            if (item1.IndexOf("<vIPI>") > -1)
                                Icms.vIPI = Convert.ToDecimal(item1.Substring(6, fim - 6).Replace(".", ","));
                            if (item1.IndexOf("<vPIS>") > -1)
                                Icms.vPIS = Convert.ToDecimal(item1.Substring(6, fim - 6).Replace(".", ","));
                            if (item1.IndexOf("<vCOFINS>") > -1)
                                Icms.vCOFINS = Convert.ToDecimal(item1.Substring(9, fim - 9).Replace(".", ","));
                            if (item1.IndexOf("<vNF>") > -1)
                            {
                                Icms.vNF = Convert.ToDecimal(item1.Substring(5, fim - 5).Replace(".", ","));
                            }
                            if (item1.IndexOf("<vTotTrib>") > -1)
                                Icms.vTotTrib = Convert.ToDecimal(item1.Substring(10, fim - 10).Replace(".", ","));
                        }

                        if (no == "<infAdic>")
                        {
                            var fim = item1.IndexOf("</");
                            if (item1.IndexOf("<infCpl>") > -1)
                                infoAdic1.infCpl = item1.Substring(8, fim - 8);
                        }



                        if (item1.IndexOf("</NFe>") > -1)
                        {
                            var infNfe = new InfNFe();
                            var val = new ValorTotal();
                            var trans = new Transporte();
                            var vol = new Volumes();
                            trans.transportadora = transporta;
                            trans.Volumes = vol;
                            val.ICMSTot = Icms;
                            infNfe.Identificacao = Identificacao;
                            infNfe.Emitente = Emitente;
                            infNfe.Emitente.Endereco = EndEmite;
                            infNfe.Destinatario = Destinatario;
                            infNfe.Destinatario.Endereco = EndDest;
                            infNfe.Detalhe = listaDetalhe;
                            infNfe.Info = infoAdic1;
                            infNfe.Valor = val;
                            infNfe.DadosTransporte = trans;
                            nfe.InformacoesNFe = infNfe;

                        }




                    }
                    catch (Exception ex)
                    {
                        var e = ex.Message;

                    }

                }


                return nfe;
                // }


            }
            catch (Exception ex)
            {
                var e = ex.Message;

                return null;
            }


        }
    }
}
