using VirtuaDTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Virtua.Utilities;
using VirtuaBusiness.Arquivos;
using VirtuaBusiness.Pedidos;
using VirtuaBusiness.Nfs_cdoca;

namespace VirtuaBusiness.Varredura_pedidos
{
    public class VarreduraBusiness : IvarreduraBLL
    {
        private readonly IConfiguration config;

        private ILogger<VarreduraBusiness> Logger;

        private ArquivoFactory _arq;
        private readonly IpedidoBLL _pedido;

        private readonly InfsCdoca _nota;
      
        System.Timers.Timer myTimer = new System.Timers.Timer();
        public VarreduraBusiness(ILogger<VarreduraBusiness> logger, IConfiguration config,
            ArquivoFactory arq, IpedidoBLL pedido, InfsCdoca nota)
        {
            Logger = logger;
            this.config = config;
            _arq = arq;
            _pedido = pedido;
            _nota = nota;
           var  perido = String.IsNullOrEmpty(config.GetSection("IntervCiclo").Value) ? 10 * 60 * 1000 : Convert.ToInt32(config.GetSection("IntervCiclo").Value);
            myTimer = new System.Timers.Timer(perido);
        }



   
       
        /// <summary>
        /// Inicio da varredura. Aqui vai as etapas a serem executadas pelo Robo
        /// </summary>
        public void Inicio()
        {
            Logger.LogInformation("Inicio do Loop");

            //incia o tokrn

            //   _token.GetToken();



         //   ExecutaInicio();


          //  myTimer.Start();
         //   myTimer.Elapsed += new ElapsedEventHandler(OnTick);
        
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            ExecutaInicio();
        }

        private void ExecutaInicio()
        {
            // Captura pedidos na pasta
            myTimer.Stop();
            var diretorio = config.GetSection("Arquivos:Upload_pedidos").Value;

            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

          var ret =  ImportarArquivosDoDiretorio(diretorio);
            if (ret=="OK")
            {
                
                myTimer.Start();
            }
        
        }


        public String ImportarArquivosDoDiretorio(String Caminho)
        {
            try
            {

                var ArquivoOk = "S";

                var localArquivos = Caminho;
                var listarArquivos = Virtua.Utilities.Arquivos.ListarArquivosNaPasta(localArquivos);

                if (listarArquivos.Count > 0)
                {
                    //valida se o Db esta conectando
                    // if (_ikeep.DbIsOn())
                    //  {

                    foreach (var item in listarArquivos)
                    {

                        var arq = item.ToString().Split('|');

                        var ext = arq[1].ToUpper().Replace(".", "");
                        var nomeArq = arq[0];

                        if (ext == ".ZIP")
                        {
                            var arquivoDescopactado = Virtua.Utilities.Arquivos.DescompactaZIP(nomeArq, localArquivos);
                            if (arquivoDescopactado == "OK")
                            {
                                this.MoveZip(nomeArq, localArquivos);
                            }
                        }

                        String extensoes = config.GetSection("Arquivos:Extensoes").Value;
                        // Aqui acrescentamos todas as extensões que queremos inserir
                        if (extensoes.IndexOf(ext) > -1)
                        {
                            var lista_nfes = new List<Nfe>();
                            //formata dados do Arquivo
                            lista_nfes = _arq.CapturaDadosArquivoFactory(ext).InsereArquivo(localArquivos, nomeArq);

                            if (lista_nfes.Count()==0)
                            {
                                ArquivoOk = "Erro";
                                var retornoMover = this.MoveArquivo(ArquivoOk, nomeArq, localArquivos);
                            }

                            foreach (var ped in lista_nfes)
                            {

                                try
                                {

                                    //valida pedidos existentes
                                    var pedencontrado = _pedido.Pesquisa_Pedido(ped.Nf_Wms,ped.Id_cliente,null);
                                    if (pedencontrado == null || String.IsNullOrEmpty(pedencontrado.Nr_documento))
                                    {
                                        

                                        //Insere o pedido
                                        ArquivoOk = _pedido.InserePedidoDoArquivo(ped);

                                        //Inserir nf no cdoca nfs
                                        if (ArquivoOk == "OK")
                                        { var notaOK = _nota.InserirNfsBLL(ped); }

                                    }
                                    else
                                    {
                                        //move o pedido
                                        ArquivoOk = "Erro";
                                    }
                                    var retornoMover = this.MoveArquivo(ArquivoOk, nomeArq, localArquivos);

                                }
                                catch (Exception ex)
                                {
                                    //  Task.Delay(2000).Wait();
                                    Logger.LogError("Erro em ImportarArquivosDoDiretorio : " + ex.Message.ToString());
                                }
                                finally
                                {
                                    Task.Delay(5000).Wait();

                                }
                            }




                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Logger.LogError("Erro em VarreduraBusiness : " + ex.Message.ToString());
            }
     

            return "OK";
        }


        private void MoveZip(String nomeArq, String Caminho)
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

        private bool MoveArquivo(String Status, String nomeArq, String Caminho)
        {
            bool resp = true;

            String CaminhoDestino = $"{Caminho}\\Importado";
            String CaminhoErro = $"{Caminho}\\Erro";

            String CaminhoFinal = CaminhoDestino;
            try
            {
                if (Status != "S")
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
