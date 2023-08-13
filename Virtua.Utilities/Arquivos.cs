using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Virtua.Utilities
{
    public class Arquivos
    {
      
        public IConfiguration Config { get; }

        public Arquivos(IConfiguration config)
        {
            
            Config = config;
        }




        /// <summary>
        /// Metodo para ler os arquivos existentes na pasta
        /// </summary>
        /// <param name="CaminhoArquivo"></param>
        /// <returns>Objeto com info dos arquivos na pasta</returns>

        public static List<object> ListarArquivosNaPasta(String CaminhoArquivo)
        {
            //Captura a pasta de origem
            DirectoryInfo dirInfo = new DirectoryInfo(CaminhoArquivo);


            List<object> listaArq = new List<object>();
            try
            {

                foreach (FileInfo file in dirInfo.GetFiles())
                {

                    object item = new object();
                    item = file.Name + "|" + file.Extension;

                    listaArq.Add(item);

                }
            }
            catch (Exception ex)
            {
               throw;
            }

            return listaArq;
        }

        public static String EscreveArquivo(String Arq, String body)
        {
            String retorno = "OK";
            try
            {
                //Declaração do método StreamWriter passando o caminho e nome do arquivo que deve ser salvo


                using (var fs = new FileStream(Arq, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    //   StreamWriter writer = new StreamWriter(Arq);
                    var writer = new StreamWriter(fs);
                    //Escrevendo o Arquivo e pulando uma linha
                    writer.WriteLine(body);

                    writer.Flush();
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                retorno = ex.Message;
            }
            return retorno;
        }

        /// <summary>
        /// Metodo para mover arquivo de local: OK 
        /// </summary>
        /// <param name="Origem"></param>
        /// <param name="Destino"></param>
        /// <param name="Arquivo"></param>
        public static String MoverArquivo(String Origem, String Destino, String Arquivo)
        {
            String retorno = "OK";
            try
            {
                if (!Directory.Exists(Destino))
                {

                    //Criamos um com o nome folder
                    Directory.CreateDirectory(Destino);

                }

                if (File.Exists(Destino + "\\" + Arquivo))
                {


                    File.Delete(Destino + "\\" + Arquivo);
                }

                File.Move(Origem + "\\" + Arquivo, Destino + "\\" + Arquivo);

            }
            catch (Exception ex)
            {

                retorno = ex.Message;
            }

            return retorno;
        }



        public static List<String> LeArquivo(String Arq)
        {

            String line = "";
            var Body = new List<String>();
            try
            {

                using (var fs = new FileStream(Arq, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))

                // cria um leitor e abre o arquivo
                using (var Read = new StreamReader(fs, Encoding.Default))
                {


                    // StreamReader Read = new StreamReader(Arq);
                    while ((line = Read.ReadLine()) != null)
                    {
                        line = line.Replace(Convert.ToChar(0x0).ToString(), " ");
                        Body.Add(line);

                    }
                    Read.Close();
                    Read.Dispose();
                }
            }
            catch (Exception ex)
            {
                //var err = "Erro em LeArquivo (" + Arq + "): " + ex.Message;


            }



            return Body;





        }

        private void TiraEspacoArquivo(String Arquivo)
        {
            var conteudoTxt = File.ReadAllText(Arquivo);

            var conteudoSemEspaco = Regex.Replace(conteudoTxt, @"\s", "");


            Arquivos.EscreveArquivo(Arquivo, conteudoSemEspaco);

        }

        public static String DescompactaZIP(String zipFile, String Caminho_origem)
        {
            String retorno = "OK";
            try
            {
                var localArquivos = Caminho_origem;

                //local para baixar arquivo compatado
                var zipPath = localArquivos + @"\" + zipFile;

                // Caminho para o arquivo descompactado
                var extractPath = localArquivos;

                if (Directory.Exists(extractPath))
                {   //apos atualizado apagar os arquivos
                    //    Arquivos.ApagaDiretorio(extractPath);
                }
                // Descompacta o arquivo com as atualizaçoes na pasta destino
                ZipFile.ExtractToDirectory(zipPath, extractPath);




            }
            catch (Exception ex)
            {

                retorno = ex.ToString();
            }

            return retorno;
        }



        public static void MoveZip(String nomeArq, String Caminho)
        {

            if (!Directory.Exists(Caminho + "\\Zip"))
            {
                Directory.CreateDirectory(Caminho + "\\Zip");
            }
            if (File.Exists(Caminho + "\\" + nomeArq))
            {
                Virtua.Utilities.Arquivos.MoverArquivo(Caminho, Caminho + "\\Zip", nomeArq);
            }





        }
        /// <summary>
        /// Move arquivo para importado ou erro
        /// </summary>
        /// <param name="Status">OK Erro</param>
        /// <param name="nomeArq"></param>
        /// <param name="Caminho"></param>
        /// <returns></returns>
        public static bool MoveArquivo(String Status, String nomeArq, String Caminho)
        {
            bool resp = true;

            String CaminhoDestino = $"{Caminho}\\Importado";
            String CaminhoErro = $"{Caminho}\\Erro";

            String CaminhoFinal = CaminhoDestino;
            try
            {
                if (Status != "OK")
                {
                    CaminhoFinal = CaminhoErro;
                }




                if (File.Exists(Caminho + "\\" + nomeArq))
                {
                    if (File.Exists(CaminhoFinal + "\\" + nomeArq))
                    {

                    }


                    Virtua.Utilities.Arquivos.MoverArquivo(Caminho, CaminhoFinal, nomeArq);
                }


            }
            catch (Exception ex)
            {
                Task.Delay(2000).Wait();
                Virtua.Utilities.Arquivos.MoverArquivo(Caminho, CaminhoErro, nomeArq);

                resp = false;
            }

            return resp;
        }

    }

}
