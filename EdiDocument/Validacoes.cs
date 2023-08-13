using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument
{
    public class Validacoes
    {

        public static String ValidaMeioTransporte(Int32 t)
        {
            String retorno = null;

            switch (t)
            {
                case 1:
                    retorno = "1-RODOVIARIO";
                    break;
                case 2:
                    retorno = "2-AEREO";
                    break;
                case 3:
                    retorno = "3-MARITIMO";
                    break;
                case 4:
                    retorno = "4-FLUVIAL";
                    break;
                case 5:
                    retorno = "5-FERROVIARIO";
                    break;
                default:
                    // code block
                    break;
            }

            return retorno;

        }

        public static String ValidaTipoTransporteCarga(Int32 t)
        {
            String retorno = null;

            switch (t)
            {
                case 1:
                    retorno = "1-CARGA FECHADA";
                    break;
                case 2:
                    retorno = "2-CARGA FRACIONADA";
                    break;

                default:
                    // code block
                    break;
            }

            return retorno;

        }

        public static String ValidaTipoCarga(Int32 t)
        {
            String retorno = null;

            switch (t)
            {
                case 1:
                    retorno = "1-FRIA";
                    break;
                case 2:
                    retorno = "2-SECA";
                    break;
                case 3:
                    retorno = "3-MISTA";
                    break;

                default:
                    // code block
                    break;
            }

            return retorno;

        }

        public static String ValidaCifFob(String t)
        {
            String retorno = null;

            switch (t)
            {
                case "C":
                    retorno = "C-CIF";
                    break;
                case "F":
                    retorno = "F-FOB";
                    break;

                default:
                    // code block
                    break;
            }

            return retorno;

        }

        /// <summary>
        /// Rcebe string numerica e traforma em data
        /// </summary>
        /// <param name="data">data em string numerica</param>
        /// <param name="formato">Formato da atual da string data ex: ddmmyyyy ddmmyy</param>
        /// <returns></returns>
        public static DateTime ValidaData(String data, String formato)
        {


            DateTime retorno = Convert.ToDateTime("01/01/0001 00:00:00");
            if (Information.IsNumeric(data) && data.Length >= 6)
            {

                switch (formato)
                {
                    case "ddmmyyyy":
                        if (data.Length == 8)
                        {
                            var dt = $"{data.Substring(4, 4)}-{data.Substring(2, 2)}-{data.Substring(0, 2)}";
                            retorno = Convert.ToDateTime(dt);
                        }

                        break;
                    case "mmddyyyy":
                        if (data.Length == 8)
                        {
                            var dt = $"{data.Substring(4, 4)}-{data.Substring(0, 2)}-{data.Substring(2, 2)}";
                            retorno = Convert.ToDateTime(dt);
                        }
                        break;
                    case "ddmmyy":
                        if (data.Length == 6)
                        {
                            var dt = $"20{data.Substring(4, 2)}-{data.Substring(2, 2)}-{data.Substring(0, 2)}";
                            retorno = Convert.ToDateTime(dt);
                        }

                        break;
                    case "mmddyy":

                        if (data.Length == 6)
                        {
                            var dt = $"20{data.Substring(4, 2)}-{data.Substring(0, 2)}-{data.Substring(2, 2)}";
                            retorno = Convert.ToDateTime(dt);
                        }

                        break;
                    default:
                        // code block
                        break;
                }
            }

            return retorno;




        }

        /// <summary>
        /// Convert string em decimal
        /// </summary>
        /// <param name="numero">Valor a converter</param>
        /// <param name="Inteiro">nr de casas inteiros</param>
        /// <param name="dec">nr de casas decimais</param>
        /// <returns></returns>
        public static Decimal ValidaStringDecimal(String numero, Int32 Inteiro, Int32 dec)
        {


            Decimal retorno = 0;

            if (Information.IsNumeric(numero) && numero.Length >= Inteiro + dec)
            {
                if (dec == 0)
                {
                    retorno = Convert.ToDecimal(numero);
                }
                else if (dec > 0)
                {


                    var v = Convert.ToInt32(numero) / Math.Pow(10, dec);

                    retorno = Convert.ToDecimal(v);
                }


            }

            return retorno;




        }

        /// <summary>
        /// Alinha a Esquerda com preechimento a direita
        /// </summary
        public static String Lpad(String texto,Int32 tamanho, Char Caractere)
        {
            String retorno = null;
            if (!String.IsNullOrEmpty(texto))
            {

                retorno = (texto.Trim() + new String(Caractere, tamanho)).Substring(0, tamanho);

    }
            return retorno;
        }

        /// <summary>
        /// Alinha a direita com preechimento a esquerda
        /// </summary>
        public static String Rpad(String texto, Int32 tamanho, Char Caractere)
        {
            String retorno = null;
            if (true)
            {

                retorno = Right(( new String(Caractere, tamanho)+ texto.Trim()),tamanho);

            }
            return retorno;
        }

        public static string Right(string original, int numberCharacters)
        {
            return original.Substring(original.Length - numberCharacters);
        }
    }
}
