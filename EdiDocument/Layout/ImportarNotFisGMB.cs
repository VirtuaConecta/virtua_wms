using EdiDocument.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument.Layout
{
    public class ImportarNotFisGMB : NotFisBase
    {
        public override List<Nfe> CapturaNotFis(string Arquivo)
        {
            return null;
        }

        public override string GeraNotFis(string NomeDoc )
        {
            var dados = NomeDoc.Split('|');
            var ListaNfe = JsonConvert.DeserializeObject<List<Nfe>>(dados[1]);

            String Edi = null;
            try
            {
                String DataEdi = DateTime.Now.ToString("ddMMyyy");
                var dadosGerais = ListaNfe.FirstOrDefault();
                Decimal ValTotal = 0;
                Decimal PesoTotal = 0;
                Decimal VolumeTotal = 0;
                Int32 NrdeNotas = ListaNfe.Count();
                //Linha 000
                var NomeCliente = Validacoes.Lpad(dadosGerais.Head.Nome_cliente, 35, ' ');
                var NomeTransportadora = Validacoes.Lpad(dadosGerais.Empresa, 35, ' ');
                var remessa = Validacoes.Lpad(dadosGerais.Num_remessa, 12, ' ');

                Edi = $"000{NomeCliente}{NomeTransportadora}{remessa}\r\n";

                // Linha 500
                var CodDoc = Validacoes.Lpad(dadosGerais.Head.Nr_doc_transporte.ToString(), 14, ' ');
                Edi += $"500{CodDoc}\r\n";

                #region Linha 501 dados cliente embarcador
                var NomeCliente_comp = Validacoes.Lpad(dadosGerais.Head.Nome_cliente, 50, ' ');
                var CnpjCli = Validacoes.Lpad(dadosGerais.Head.Cnpj_cli, 14, ' ');
                var IeCli = Validacoes.Lpad(dadosGerais.Head.IE_cliente, 15, ' ');
                var EndCli = Validacoes.Lpad(dadosGerais.Head.Endereco_cli, 50, ' ');
                var BairroCli = Validacoes.Lpad(dadosGerais.Head.Bairro_cli, 35, ' ');
                var CidadeCli = Validacoes.Lpad(dadosGerais.Head.Cidade_cli, 35, ' ');
                var CepCli = Validacoes.Lpad(dadosGerais.Head.Cep_cli, 9, ' ');
                var UfCli = Validacoes.Lpad(dadosGerais.Head.Estado_cli, 9, ' ');
                Edi += $"501{NomeCliente_comp}{CnpjCli}{IeCli}{new String(' ', 30)}{EndCli}{BairroCli}{CidadeCli}{CepCli}{new String(' ', 9)}{UfCli}\r\n";
                #endregion
                foreach (var nota in ListaNfe)
                {
                    #region 503 destino
                    var NomeDest_comp = Validacoes.Lpad(nota.Head.Nome_destino, 50, ' ');
                    var CnpjDest = Validacoes.Lpad(nota.Head.Cnpj_destino, 14, ' ');
                    var IeDest = Validacoes.Lpad(nota.Head.IE_destino, 15, ' ');
                    var EndDest = Validacoes.Lpad(nota.Head.Endereco_destino, 50, ' ');
                    var BairroDest = Validacoes.Lpad(nota.Head.Bairro_cli, 35, ' ');
                    var CidadeDest = Validacoes.Lpad(nota.Head.Cidade_destino, 35, ' ');
                    var CepDest = Validacoes.Lpad(nota.Head.Cep_destino, 9, ' ');
                    var UfDest = Validacoes.Lpad(nota.Head.Estado_destino, 9, ' ');
                    var codMunDest = Validacoes.Lpad(nota.Head.Id_Cidade_destino.ToString(), 9, ' ');
                    var fj = nota.Head.Fis_jur_destino;
                    var fisJurDest = fj == "J" ? "1" : "2";

                    Edi += $"503{NomeDest_comp}{CnpjDest}{IeDest}{new String(' ', 15)}{EndDest}{BairroDest}{CidadeDest}";
                    Edi += $"{CepDest}{codMunDest}{UfDest}{new string(' ', 43)}{ fisJurDest }\r\n";
                    #endregion
                    #region 505 dados da nota
                    var TipoRemessa = nota.Tipo_remessa == "O" ? "1" : "0";
                    var SerieNf = Validacoes.Lpad(nota.Head.Serie, 3, ' ');
                    var NrNf = Validacoes.Rpad(nota.Head.Nr_original_cliente, 9, '0');
                    var DtEmissao = nota.Head.Data_emissao.ToString("ddMMyyyy");
                    var TipoMercadoria = !String.IsNullOrWhiteSpace(nota.Head.Tipo_mercadoria) ? Validacoes.Lpad(nota.Head.Tipo_mercadoria, 15, ' ') : "DIVERSOS";
                    var Especie = !String.IsNullOrWhiteSpace(nota.Head.Especie) ? Validacoes.Lpad(nota.Head.Especie, 15, ' ') : "VOLUMES";
                    var Rota = !String.IsNullOrWhiteSpace(nota.Head.Cod_rota) ? Validacoes.Lpad(nota.Head.Cod_rota, 7, ' ') : new string(' ', 7);
                    var meioTransp = !String.IsNullOrWhiteSpace(nota.Head.Meio_transporte) ? Validacoes.Lpad(nota.Head.Meio_transporte, 1, ' ') : "1";
                    var TipoTrasCarga = !String.IsNullOrWhiteSpace(nota.Head.Tipo_carga) ? Validacoes.Lpad(nota.Head.Tipo_carga, 1, ' ') : "2";
                    var CifFob = !String.IsNullOrWhiteSpace(nota.Head.Cond_frete) ? Validacoes.Lpad(nota.Head.Cond_frete, 1, ' ') : "C";
                    var dtemb = Convert.ToDateTime("0001-01-01 00:00:00");
                    var DtEmbarque = nota.Head.Data_embarque != dtemb ? Convert.ToDateTime(nota.Head.Data_embarque).ToString("ddMMyyyy") : DtEmissao;
                    var Desdobro = new String(' ', 10);
                    var PlanoCarga = "N";
                    var TipDocFis = "0";
                    var IndBoni = "N";
                    var CfopOp = "0000";
                    var SiglaEFG = new String(' ', 2);
                    var CalcFretDif = "N";
                    var IntervalFret = new String(' ', 112);
                    var TipPeriodoF = "0";
                    var TipCarga = "3";
                    var CompPrevEntrega = new String('0', 16);
                    var CodProtChave = new String('0', 9);
                    var ChaveNf = !String.IsNullOrWhiteSpace(nota.Head.Chave) ? Validacoes.Lpad(nota.Head.Chave, 44, '0') : new String('0', 44);

                    Edi += $"505{SerieNf}{NrNf}{DtEmissao}{TipoMercadoria}{Especie}{Rota}{meioTransp}{TipoTrasCarga}{TipCarga}";
                    Edi += $"{CifFob}{DtEmbarque}{Desdobro}{PlanoCarga}{TipDocFis}{IndBoni}{CfopOp}";
                    Edi += $"{SiglaEFG}{CalcFretDif}{IntervalFret}{TipPeriodoF}{DtEmbarque}";
                    Edi += $"{CompPrevEntrega}{CodProtChave}{ChaveNf}\r\n";
                    #endregion

                    #region 506 – VALORES DA NOTA FISCAL
                    var VolumesNF = Validacoes.Rpad(Convert.ToInt32(Math.Round(nota.Head.Nr_volumes, 2)*100).ToString(), 8, '0');
                    var PBruto = Validacoes.Rpad(Convert.ToInt32(Math.Round(nota.Head.Peso_brt, 3)*1000).ToString(), 9, '0');
                    var PLiquido = Validacoes.Rpad(Convert.ToInt32(Math.Round(nota.Head.Peso_liq, 3)*1000).ToString(), 9, '0');
                    var PesoDesCub = new String('0', 10);
                    var PesoCub = new String('0', 10);
                    var InsidIcms = "N";
                    var Seguro = "N";
                    var valorCobrado = new String('0', 15);
                    var Filler = new String('0', 234);

                    var ValorNota = Validacoes.Rpad(Convert.ToInt32(Math.Round(nota.Head.Valor_Total, 2)*100).ToString(), 15, '0');
                    Edi += $"506{VolumesNF}{PBruto}{PLiquido}{PesoDesCub}{PesoCub}{InsidIcms}{Seguro}";
                    Edi += $"{valorCobrado}{ValorNota}{Filler}\r\n";
                    ValTotal += nota.Head.Valor_Total;
                    VolumeTotal += nota.Head.Nr_volumes;
                    PesoTotal += nota.Head.Peso_brt;
                    #endregion

                    #region 507 – CÁLCULO DO FRETE
                    var ValorFrete = Validacoes.Rpad(Convert.ToInt32(Math.Round(nota.Valor_frete, 2)*100).ToString(), 15, '0');
                    var Filler1 = new String('0', 170);
                    var Filler2 = new String('0', 93);
                    Edi += $"507{VolumesNF}{PBruto}{PesoDesCub}{PesoCub}{ValorFrete}{Filler1}2{Filler2}\r\n";

                    #endregion

                    #region 511 – ITEM DA NOTA FISCAL

                    foreach (var itemNota in nota.ListaItens)
                    {
                        var qtditem = Validacoes.Rpad(Convert.ToInt32(Math.Round(itemNota.Qtd, 2)*100).ToString(),8,'0');
                        var unditem = Validacoes.Lpad(itemNota.Especie, 15, ' ');
                        var skuitem = Validacoes.Lpad(itemNota.Sku, 20, ' ');
                        var descProdItem = Validacoes.Lpad(itemNota.Descricao_prod, 50, ' ');

                        Edi += $"511{qtditem}{unditem}{skuitem}{descProdItem}\r\n";
                    }

                    #endregion

                    
                }

                #region 519 – TOTAIS DO ARQUIVO 

                var Valt = Validacoes.Rpad(Convert.ToInt32(Math.Round(ValTotal, 2) * 100).ToString(), 15, '0');
                var Pesot = Validacoes.Rpad(Convert.ToInt32(Math.Round(PesoTotal, 2) * 100).ToString(), 15, '0');
                var Volt = Validacoes.Rpad(Convert.ToInt32(Math.Round(VolumeTotal, 2) * 100).ToString(), 15, '0');
                var NrNotas = Validacoes.Rpad(NrdeNotas.ToString(), 10, '0');
                var Filler3 = new String('0', 262);

                Edi += $"519{Valt}{Pesot}{Volt}{NrNotas}{Filler3}\r\n";
                #endregion


            }
            catch (Exception ex)
            {

                throw;
            }



            return Edi;
        }
    }
}
