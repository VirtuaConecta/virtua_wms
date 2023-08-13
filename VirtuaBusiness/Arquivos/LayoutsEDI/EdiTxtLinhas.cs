using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Arquivos.LayoutsEDI
{
    public class EdiTxtLinhas
    {

        //public static NotaFiscalHeadDTO Linha313_Leroy(String line)
        //{
        //  //  LogEvento _log = new LogEvento();
        //    var DadosNota = new NotaFiscalHeadDTO();

        //    try
        //    {
        //        DadosNota.SerieNotaFiscal = line.Substring(29, 3).Trim();

        //        var dadosEntrega = line.Substring(18, 7);

        //        if (dadosEntrega.Contains(".") && dadosEntrega.Contains("-"))
        //        {
        //            DadosNota.EntregaLeroy = dadosEntrega;
        //        }

        //        if (!String.IsNullOrWhiteSpace(line.Substring(32, 8).Trim()))
        //        {
        //            DadosNota.NrNotaFiscal = line.Substring(32, 8).Trim();
        //        }
        //        else
        //        {
        //            DadosNota.NrNotaFiscal = line.Substring(261, 9).Trim();
        //        }

        //        //gera o nr da nota fiscal
        //        var serie = "0";
        //        if (Information.IsNumeric(DadosNota.SerieNotaFiscal))
        //        {
        //            serie = Convert.ToInt32(DadosNota.SerieNotaFiscal).ToString();
        //        }

        //        var vNrNf1 = "00000000000000" + DadosNota.NrNotaFiscal + "-E" + serie;
        //        DadosNota.NfWms = vNrNf1.Substring(vNrNf1.Length - 11);

        //        DadosNota.DataEmissao = Convert.ToDateTime(line.Substring(44, 4) + "-" + line.Substring(42, 2) + "-" + line.Substring(40, 2));
        //        DadosNota.EspecieProdutos = line.Substring(63, 15).Trim();
        //        var qtd = line.Substring(78, 7).Trim();

        //        DadosNota.QtdItens = line.Substring(78, 7);

        //        var qtd_num = Convert.ToInt32(DadosNota.QtdItens);
        //        if (qtd_num < 1)
        //        {
        //            qtd_num = 1;
        //        }
        //        var pesoTotal = Convert.ToDecimal(line.Substring(100, 7)) / 100;
        //        DadosNota.PesoTotal = Conversoes.ConvertDescimalStr(line.Substring(100, 7), 2, "00.00");
        //        var pesouni = Math.Round(pesoTotal / qtd_num, 4);
        //        DadosNota.Valor = Conversoes.ConvertDescimalStr(line.Substring(85, 15), 2, "00.00");


        //        DadosNota.Peso = pesouni.ToString().Replace(',', '.');

        //        Decimal valorF = 0;

        //        if (Information.IsNumeric(line.Substring(198, 15)))
        //        {
        //            valorF = Convert.ToDecimal(line.Substring(198, 15)) / 100;
        //        }
        //        DadosNota.ValorFrete = valorF.ToString().Replace(",", ".");

        //        var tamanhoDalinha = line.Length;

        //        var sr = "0";
        //        if (!String.IsNullOrEmpty(DadosNota.SerieNotaFiscal))
        //        {
        //            sr = DadosNota.SerieNotaFiscal;
        //        }


        //        DadosNota.PedidoCliente = String.Format("{0}-{1}-{2}", line.Substring(241, 20).Trim(), DadosNota.NrNotaFiscal, sr);

        //        if (tamanhoDalinha >= 314)
        //        {
        //            DadosNota.ChaveNfe = line.Substring(270, 44).Trim();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _log.WriteEntry("Erro linha 313 Leroy" + ex.Message, System.Diagnostics.EventLogEntryType.Error);

        //    }

        //    return DadosNota;



        //}

        //public static NotaFiscalHeadDTO Linha313_Renner(String line)
        //{
        //    LogEvento _log = new LogEvento();
        //    var DadosNota = new NotaFiscalHeadDTO();
        //    try
        //    {

        //        DadosNota.SerieNotaFiscal = line.Substring(29, 3).Trim();

        //        if (!String.IsNullOrWhiteSpace(line.Substring(32, 8).Trim()))
        //        {
        //            DadosNota.NrNotaFiscal = line.Substring(32, 8).Trim();
        //        }
        //        else
        //        {
        //            DadosNota.NrNotaFiscal = line.Substring(261, 9).Trim();
        //        }

        //        //gera o nr da nota fiscal
        //        var serie = "0";
        //        if (Information.IsNumeric(DadosNota.SerieNotaFiscal))
        //        {
        //            serie = Convert.ToInt32(DadosNota.SerieNotaFiscal).ToString();
        //        }

        //        var vNrNf1 = "00000000000000" + DadosNota.NrNotaFiscal + "-E" + serie;
        //        DadosNota.NfWms = vNrNf1.Substring(vNrNf1.Length - 11);
        //        var TES = line.Substring(44, 4) + "-" + line.Substring(42, 2) + "-" + line.Substring(40, 2);



        //        if (TES == "0000-00-00")
        //        {
        //            DadosNota.DataEmissao = DateTime.Now.Date;
        //        }
        //        else
        //        {
        //            DadosNota.DataEmissao = Convert.ToDateTime(line.Substring(44, 4) + "-" + line.Substring(42, 2) + "-" + line.Substring(40, 2));
        //        }
        //        DadosNota.EspecieProdutos = line.Substring(63, 15).Trim();

        //        //quatidade total
        //        var qtd = line.Substring(78, 7).Trim();
        //        if (String.IsNullOrEmpty(qtd))
        //        {
        //            qtd = "1";
        //        }
        //        Decimal qtd_num = Convert.ToInt32(qtd) / 100;
        //        if (qtd_num < 1)
        //        {
        //            qtd_num = 1;
        //        }

        //        DadosNota.QtdItens = Math.Round(qtd_num, 0, MidpointRounding.AwayFromZero).ToString();

        //        var pesoTotal = Convert.ToDecimal(line.Substring(100, 7)) / 100;
        //        DadosNota.PesoTotal = pesoTotal.ToString("F2").Replace(',', '.');
        //        var pesouni = Math.Round(pesoTotal / qtd_num, 4);
        //        if (pesouni > 0.001M)
        //        {
        //            pesouni = 0.01M;
        //        }
        //        DadosNota.Peso = pesouni.ToString("F2").Replace(',', '.');
        //        var valNota = Convert.ToDecimal(line.Substring(85, 15)) / 100;

        //        DadosNota.Valor = valNota.ToString("F2").Replace(',', '.');


        //        Decimal valorF = 0;

        //        if (Information.IsNumeric(line.Substring(198, 15)))
        //        {
        //            valorF = Convert.ToDecimal(line.Substring(198, 15)) / 100;
        //        }
        //        DadosNota.ValorFrete = valorF.ToString("F2").Replace(",", ".");

        //        var tamanhoDalinha = line.Length;

        //        var ped = line.Substring(3, 15).Trim();
        //        if (String.IsNullOrEmpty(ped))
        //        {
        //            ped = "0";
        //        }
        //        DadosNota.PedidoCliente = String.Format("{0}-{1}", ped, DadosNota.NrNotaFiscal);

        //        if (tamanhoDalinha >= 283)
        //        {
        //            DadosNota.ChaveNfe = line.Substring(238, 44).Trim();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _log.WriteEntry("Erro linha 313 Renner" + ex.Message, System.Diagnostics.EventLogEntryType.Error);
        //    }

        //    return DadosNota;



        //}


        //public static NotaFiscalHeadDTO Linha313_Mobi(String line, String cliente)
        //{
        //    LogEvento _log = new LogEvento();

        //    var dadosDestino = JsonConvert.DeserializeObject<RazaoDTO>(cliente);



        //    var DadosNota = new NotaFiscalHeadDTO();
        //    try
        //    {

        //        DadosNota.SerieNotaFiscal = line.Substring(29, 3).Trim();

        //        if (!String.IsNullOrWhiteSpace(line.Substring(32, 8).Trim()))
        //        {
        //            DadosNota.NrNotaFiscal = line.Substring(32, 8).Trim();
        //        }
        //        else
        //        {
        //            DadosNota.NrNotaFiscal = line.Substring(261, 9).Trim();
        //        }

        //        //gera o nr da nota fiscal
        //        var serie = "0";
        //        if (Information.IsNumeric(DadosNota.SerieNotaFiscal))
        //        {
        //            serie = Convert.ToInt32(DadosNota.SerieNotaFiscal).ToString();
        //        }

        //        var vNrNf1 = "00000000000000" + DadosNota.NrNotaFiscal + "-E" + serie;
        //        DadosNota.NfWms = vNrNf1.Substring(vNrNf1.Length - 11);
        //        var TES = line.Substring(44, 4) + "-" + line.Substring(42, 2) + "-" + line.Substring(40, 2);



        //        if (TES == "0000-00-00")
        //        {
        //            DadosNota.DataEmissao = DateTime.Now.Date;
        //        }
        //        else
        //        {
        //            DadosNota.DataEmissao = Convert.ToDateTime(line.Substring(44, 4) + "-" + line.Substring(42, 2) + "-" + line.Substring(40, 2));
        //        }
        //        DadosNota.EspecieProdutos = line.Substring(63, 15).Trim();

        //        //quatidade total
        //        var qtd = line.Substring(78, 7).Trim();
        //        if (String.IsNullOrEmpty(qtd))
        //        {
        //            qtd = "1";
        //        }
        //        Decimal qtd_num = Convert.ToInt32(qtd) / 100;
        //        if (qtd_num < 1)
        //        {
        //            qtd_num = 1;
        //        }

        //        DadosNota.QtdItens = Math.Round(qtd_num, 0, MidpointRounding.AwayFromZero).ToString();

        //        var pesoTotal = Convert.ToDecimal(line.Substring(100, 7)) / 100;
        //        DadosNota.PesoTotal = pesoTotal.ToString("F2").Replace(',', '.');
        //        var pesouni = Math.Round(pesoTotal / qtd_num, 4);
        //        if (pesouni > 0.001M)
        //        {
        //            pesouni = 0.01M;
        //        }
        //        DadosNota.Peso = pesouni.ToString("F2").Replace(',', '.');
        //        var valNota = Convert.ToDecimal(line.Substring(85, 15)) / 100;

        //        DadosNota.Valor = valNota.ToString("F2").Replace(',', '.');


        //        Decimal valorF = 0;

        //        if (Information.IsNumeric(line.Substring(198, 15)))
        //        {
        //            valorF = Convert.ToDecimal(line.Substring(198, 15)) / 100;
        //        }
        //        DadosNota.ValorFrete = valorF.ToString("F2").Replace(",", ".");

        //        var tamanhoDalinha = line.Length;

        //        var ped = line.Substring(3, 15).Trim();
        //        if (String.IsNullOrEmpty(ped))
        //        {
        //            ped = "0";
        //        }
        //        //  DadosNota.PedidoCliente = String.Format("{0}-{1}", ped, DadosNota.NrNotaFiscal);
        //        DadosNota.PedidoCliente = String.Format("{0}-{1}-{2}-{3}", ped, DadosNota.NrNotaFiscal, serie, dadosDestino.Cnpj);
        //        if (tamanhoDalinha >= 283)
        //        {
        //            DadosNota.ChaveNfe = line.Substring(238, 44).Trim();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _log.WriteEntry("Erro linha 313 Renner" + ex.Message, System.Diagnostics.EventLogEntryType.Error);
        //    }

        //    return DadosNota;



        //}

    }
}
