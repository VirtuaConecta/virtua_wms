using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Virtua.Utilities
{
    public class Funcoes
    {

        public static String Filtrar<T>(List<T> lista, String NomeCampo, bool str)
        {
            StringBuilder sb = new StringBuilder();
            //"Pegamos" o tipo de T, para termos acessos as propriedades e valores.

            try
            {
                Type tipoEntidade = typeof(T);


                //Pegamos as propriedades da entidade.

                PropertyInfo[] propriedades = tipoEntidade.GetProperties();


                foreach (var item in lista)
                {
                    var itemObj = GetValObjBy(item, NomeCampo);

                    if (sb.Length != 0)
                    {
                        sb.Append(",");
                    }
                    if (str)
                    {
                        sb.Append($"'{itemObj}'");
                    }
                    else
                    {
                        sb.Append(itemObj);
                    }

                }
            }
            catch (Exception)
            {


            }

            //Passo por todas as propriedades




            //Retorno a String

            return sb.ToString();






            //  return null;
        }
        public static object GetValObjBy(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
        /// <summary>
        /// convert formto de datas
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tipo">ddmmyyyy yyyy-mm-dd</param>
        /// <returns></returns>
        public static String formataData(String dt, String tipo)
        {
            String ret = null;
            var dtArray = dt.Split('/');

            if (dtArray.Length == 3)
            {
                if (tipo == "ddmmyyyy")
                {
                    ret = $"{dtArray[0]}{dtArray[1]}{dtArray[2]}";
                }
                else if (tipo == "yyyy-mm-dd")
                {
                    ret = $"{dtArray[2]}-{dtArray[1]}-{dtArray[0]}";
                }
            }

            return ret;
        }


        public static String RemoveAccents(String text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }






    }
}
