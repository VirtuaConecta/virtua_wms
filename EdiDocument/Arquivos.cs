using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument
{
    public class Arquivos : IArquivos
    {
        public List<String> LeArquivo(String Arq)
        {

            String Body = "";

            List<string> mensagemLinha = new List<string>();
            try
            {
                // cria um leitor e abre o arquivo
                using (StreamReader Read = new StreamReader(Arq))
                {
                    while ((Body = Read.ReadLine()) != null)
                    {
                        mensagemLinha.Add(Body);
                    }
                }




            }
            catch (Exception ex)
            {
                var t = ex.Message;

            }


            return mensagemLinha;
        }
    }
}
