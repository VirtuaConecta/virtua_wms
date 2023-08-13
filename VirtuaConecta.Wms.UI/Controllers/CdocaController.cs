using AspNetCoreHero.ToastNotification.Abstractions;
using FastMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Virtua.Utilities;
using VirtuaBusiness.Arquivos;
using VirtuaBusiness.Arquivos.LayoutsEDI;
using VirtuaBusiness.Cliente;
using VirtuaBusiness.Nfs_cdoca;
using VirtuaBusiness.Ordem_carga;
using VirtuaConecta.Wms.UI.ViewModel;
using VirtuaDTO;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class CdocaController : Controller
    {

        private readonly IArquivoFreteGMB _arqGmb;
        private readonly IConfiguration _config;
        private readonly ILogger<CdocaController> _logger;
        private readonly INotyfService _notyf;
        private readonly IbaseLayout _arq;

        IHostingEnvironment _env;
        private readonly InfsCdoca cdoca;
        private readonly IclienteBLL _icliente;

        public static List<NfeCrossDock> listaNfeCD { get; set; }
        public static List<ListaConferenciaViewModel> lista1 { get; set; }
        public CdocaController(IArquivoFreteGMB arqGmb, IConfiguration config, ILogger<CdocaController> logger, INotyfService notyf, IbaseLayout arq,
            IHostingEnvironment env, InfsCdoca cdoca, IclienteBLL icliente)
        {

            _logger = logger;
            _arqGmb = arqGmb;
            _config = config;
            _notyf = notyf;
            _arq = arq;
            _env = env;
            this.cdoca = cdoca;
            _icliente = icliente;
        }

        public IActionResult GeraEdiGMB()
        { // carrega vslor/kg da configuração
            ViewBag.ValKilo = _config.GetSection("Frete:Val_kilo").Value;

            return View();
        }


        //recebe os arquivos de GeraEdiGMB
        [HttpPost]
        public IActionResult UploadArquivosRecebidos(IFormFile file, String valorKg)
        {
            listaTabelaFreteGMBViewModel.btn_ver = false;
            String msg = "";

            if (listaTabelaFreteGMBViewModel.lista_nf == null)
                listaTabelaFreteGMBViewModel.lista_nf = new List<Nfe>();

            if (listaTabelaFreteGMBViewModel.lista == null)
                listaTabelaFreteGMBViewModel.lista = new List<TabelaFreteGMBViewModel>();

            // local de destino
            var resp = new List<TabelaFreteGMBViewModel>();
            string path = _config.GetSection("Arquivos:Upload_pedidos").Value;

            String caminho = $"{path}\\" + file.FileName;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            string filename = Path.GetFileName(file.FileName);
            using (FileStream strem = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                file.CopyTo(strem);
            }


            try
            {
                //verifica se é excel
                if (file.FileName.Substring(file.FileName.Length - 4).ToUpper() == "XLSX" || file.FileName.Substring(file.FileName.Length - 3).ToUpper() == "XLS")
                {


                    var freteDTO = _arqGmb.GeraListaFreteGmb(caminho, valorKg);
                    var freteVM = TypeAdapter.Adapt<IEnumerable<TabelaFreteGmbDTO>, List<TabelaFreteGMBViewModel>>(freteDTO);
                    resp = freteVM;


                    if (resp.Count != 0)
                    {


                        listaTabelaFreteGMBViewModel.lista = resp;
                        //move para importado
                        Arquivos.MoveArquivo("OK", filename, path);
                        msg = "Arquivo Lista de fretre Importado";
                        listaTabelaFreteGMBViewModel.lista_nf = new List<Nfe>();





                    }
                    else
                    {
                        msg = "Erro ao importar arquivo excel. Verifique se o arquivo está no formato correto e tente novamente";
                    }

                }
                else if (file.FileName.Substring(file.FileName.Length - 3).ToUpper() == "XML")
                {
                    // usar o decodificado do importar pedidos                                 
                    // Count dos itens na tabela de frete para saber o total
                    // decodificar o xml
                    // pegar o nr da nota 
                    // fazer select na lista de frete
                    // Se existri insere na model
                    // se não existir devolve erro para tela
                    //verificar se  a qtd inserida na modela das notas é igual a qtd na lista de frete se for termina e envia msg para tela
                    // devolve pop up com o nr de notas inseridas

                    var nf = _arqGmb.ParseXml(file.FileName, path);

                    if (nf != null && nf.ListaItens.Count > 0)
                    {

                        if (listaTabelaFreteGMBViewModel.lista.Where(x => x.Numero_Nota_Fiscal == nf.Head.Nr_original_cliente).FirstOrDefault() == null)
                        {
                            msg += "Nota numero " + nf.Nf_Wms + " não foi importada pelo excel \n";
                        }
                        else if (listaTabelaFreteGMBViewModel.lista_nf != null &&
                        listaTabelaFreteGMBViewModel.lista_nf.Where(x => x.Head.Nr_original_cliente == nf.Head.Nr_original_cliente)

                            .FirstOrDefault() != null)
                        {
                            msg += "Nota numero " + nf.Nf_Wms + " já foi importada anteriormente \n";
                        }
                        else
                        {
                            listaTabelaFreteGMBViewModel.lista_nf.Add(nf);
                        }

                        var qtd_frete = 0;
                        var qtd_nf = 0;

                        qtd_frete = listaTabelaFreteGMBViewModel.lista.Count;
                        qtd_nf = listaTabelaFreteGMBViewModel.lista_nf.Count;
                        if (listaTabelaFreteGMBViewModel.lista.Count == 0)
                        {
                            msg = "Não há tabela de frete importada";
                        }
                        else if (qtd_frete == qtd_nf)
                        {
                            msg = "Foram importadas todas as " + qtd_nf.ToString() + " notas\n";
                            msg += "Click no Btn para baixar Edi no fim da lista";
                            listaTabelaFreteGMBViewModel.btn_ver = true;
                        }
                        else
                        {
                            msg += "Foram importadas " + qtd_nf.ToString() + "/" + qtd_frete.ToString();
                        }
                    }
                    else
                    {
                        msg += $"Arquivo{file.FileName} não foi importado ";
                    }
                }
                else
                {
                    msg = "O arquivo precisa ser um excel(xlsx/xls) ou uma nota fiscal(xml)";
                }

            }
            catch (Exception ex)
            {
                msg = "Erro no upload";
                _logger.LogError(ex.Message, "Erro de Upload ");
            }

            TempData["msgPopUp"] = msg;

            return RedirectToAction("GeraEdiGMB");
        }

        public List<TabelaFreteGMBViewModel> RetornaTabela()
        {
            List<TabelaFreteGMBViewModel> frete = new List<TabelaFreteGMBViewModel>();
            try
            {
                if (TempData["msgPopUp"] != null && TempData["msgPopUp"].ToString() == "Arquivo Lista de fretre Importado")
                {

                    _notyf.Success($"{TempData["msgPopUp"]}", 10);
                    frete = listaTabelaFreteGMBViewModel.lista;
                }
                else if (listaTabelaFreteGMBViewModel.btn_ver)
                {
                    var resposta = new TabelaFreteGMBViewModel();
                    resposta.Emissor_ordem = "OK";
                    frete.Add(resposta);

                    _notyf.Success($"{TempData["msgPopUp"]}", 10);
                }
                else if (TempData["msgPopUp"] == null)
                {
                    _notyf.Warning("Foram importadas " + listaTabelaFreteGMBViewModel.lista_nf.Count.ToString() + "/" +
                        listaTabelaFreteGMBViewModel.lista.Count.ToString(), 5);
                }
                else if (TempData["msgPopUp"].ToString().Contains("Foram"))
                {
                    _notyf.Warning($"{TempData["msgPopUp"]}", 5);
                }
                else
                {
                    _notyf.Error($"{TempData["msgPopUp"]}", 5);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, "Erro de Upload ");
            }

            return frete;
        }


        public IActionResult ExportarEDI()
        {
            var listaFrete = listaTabelaFreteGMBViewModel.lista;
            var listaNf = listaTabelaFreteGMBViewModel.lista_nf;

            var nomeArquivo = listaFrete.Select(x => x.Transporte).FirstOrDefault();

            var tabFrete = TypeAdapter.Adapt<List<TabelaFreteGMBViewModel>, List<TabelaFreteGmbDTO>>(listaFrete);

            String conteudo = _arqGmb.GeraEdiNotFis("GMB", listaNf, tabFrete);

            String arq = $"Arquivo_EDI_{nomeArquivo}_{ DateTime.Now.ToString("yyyy_MM_dd")}.txt";

            string path = _config.GetSection("Arquivos:Upload_pedidos").Value;
            //  string caminho = _env.WebRootPath + @"\Download\" + nome_arquivo;
            string caminho = $"{path}\\{arq}";


            //string caminho = _env.WebRootPath + @"\Download\" + arq;
            //string caminho = AppDomain.CurrentDomain.BaseDirectory +@"wwwroot\Download\" + arq;


            try
            {
                //formatar arquivo edi de acordo com os dados


                FileStream arquivo = System.IO.File.Create(caminho);
                arquivo.Close();

                System.IO.File.WriteAllText(caminho, conteudo);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, "Erro em ExportarExcel");
            }


            ViewBag.link = arq;
            //  return RedirectToAction("Lista_NF_Entrada_SaidaDownload", new {ret=Arq });

            return View();


        }

        [HttpPost]
        public String ApagaEdi(String nomeArquivo)
        {
            String ret = "Erro";

            try
            {
                string path = _config.GetSection("Arquivos:Upload_pedidos").Value;
                //  string caminho = _env.WebRootPath + @"\Download\" + nome_arquivo;
                string caminho = $"{path}\\{nomeArquivo}";
                System.IO.File.Delete(caminho);
                ret = "OK";
            }
            catch (Exception ex)
            {
                var err = ex.Message;

            }

            return ret;
        }

        [HttpPost]
        public IActionResult ExportarEDI(String nome_arquivo)
        {

            string path = _config.GetSection("Arquivos:Upload_pedidos").Value;

            //  string caminho = _env.WebRootPath + @"\Download\" + nome_arquivo;

            string caminho = $"{path}\\{nome_arquivo}";
            var fs = System.IO.File.OpenRead(caminho);
            //FileStream myByte  ;
            //using (var fs = System.IO.File.OpenRead(caminho))
            //{ myByte = fs; }


            try
            {

                System.IO.Directory.Delete(path + "\\Importado", true);
            }
            catch (Exception)
            {


            }

            listaTabelaFreteGMBViewModel.lista = new List<TabelaFreteGMBViewModel>();
            listaTabelaFreteGMBViewModel.lista_nf = new List<Nfe>();
            listaTabelaFreteGMBViewModel.btn_ver = false;
            return File(fs, "application/octet-stream",
                    nome_arquivo);

        }


        public IActionResult Upload_arquivos_crossDoking()
        {
            String Msg = null;

            if (TempData["msg_upload"] != null)
            {
                Msg = TempData["msg_upload"].ToString();

                //var e = Msg.Contains("Arquivo");

            }

            if (String.IsNullOrEmpty(Msg))
            {
                _notyf.Success("Arrastar arquivos a serem importados - fecha em 5 sec.", 5);
            }
            else if (Msg == "OK")
            {

                _notyf.Success("Arquivo Importado - fecha em 5 sec.", 5);
            }

            else if (Msg.Contains("Arquivo"))
            {

                _notyf.Success(Msg, 5);
            }
            else
            {
                _notyf.Error($"{Msg} - fecha em 5 sec.", 5);
            }

            // TempData["UrlDestino"] = "/Pedidos/Upload_arquivos";
            return View();
        }

        [HttpPost]
        public IActionResult UploadXmlRecebidos(IFormFile file, String baixa)
        {

            string filename = Path.GetFileName(file.FileName);

            String msg = ProcessaArquivo(file);
            TempData["msg_upload"] = msg;
            return RedirectToAction("Upload_arquivos_crossDoking");
        }

        private String ProcessaArquivo(IFormFile file)
        {
            var ret = "Erro";
            // local de destino
            string path = _config.GetSection("Arquivos:Upload_pedidos").Value;
            string filename = "";
            try
            {


                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                List<string> uploadedfiles = new List<string>();
                filename = Path.GetFileName(file.FileName);
                using (FileStream strem = new FileStream(Path.Combine(path, filename), FileMode.Create))
                {
                    file.CopyTo(strem);
                    uploadedfiles.Add(filename);

                }

                if (uploadedfiles.Count() > 0)
                {
                    if (file.FileName != null)
                    {
                        ret = file.FileName + " - ";
                    }


                    ret = cdoca.InsereArquivosDaPastaCorssDockBll(uploadedfiles, path);


                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro de Upload ");
            }

            String msg = ret == "OK" ? $"Arquivo {filename} inserido" : "Ocorreu Algum erro no processo";

            return msg;

        }


        public IActionResult PesquisaNfCdoca()
        {
            ViewBag.Title = "Pequisa  Notas Cdoca";

            var dadosTela = new DadoPesquisaViewModel();
            dadosTela.Dt_ini = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            dadosTela.Dt_fim = DateTime.Now.ToString("yyyy-MM-dd");
            var clientes = _icliente.Listar_Cliente().Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(y => y.NomeCliente).ToList();
            dadosTela.Lista = new SelectList(clientes, "IdCliente", "NomeCliente");




            return View(dadosTela);
        }

        [HttpPost]
        public IActionResult PesquisaNfCdoca(DadoPesquisaViewModel dadosTela)
        {

            return RedirectToAction("ListaNfe", dadosTela);

        }

        public IActionResult ListaNfe(DadoPesquisaViewModel dadosTela)
        {
            List<nfeCdoca> listaDTO;

            if (dadosTela.Id > 0)
            {
                TempData["DadoPesquisaViewModel"] = JsonConvert.SerializeObject(dadosTela);

                listaDTO = cdoca.ListarNfSaidaCdocaBll(dadosTela.Id, dadosTela.Dt_ini, dadosTela.Dt_fim).ToList();

            }
            else if (dadosTela.Id == 0 && !String.IsNullOrWhiteSpace(dadosTela.Id_str) )
            {
                listaDTO = cdoca.ListarNfSaidaCdocaBll(dadosTela.Id_str).ToList();
            }
            else
            {
                if (TempData["DadoPesquisaViewModelRetorno"] is string s)
                {
                    dadosTela = JsonConvert.DeserializeObject<DadoPesquisaViewModel>(s);
                }
                listaDTO = cdoca.ListarNfSaidaCdocaBll(dadosTela.Id, dadosTela.Dt_ini, dadosTela.Dt_fim).ToList();
            }

            var freteVM = TypeAdapter.Adapt<List<nfeCdoca>, List<NfeCdocaViewModel>>(listaDTO);

            //Manda os pedidos para tela

            return View(freteVM);
        }

        public IActionResult RedirecionaCrossDock(String ID, String ID1)
        {
            var itensSelecionados = ID.Split(',');
            // filtrar caso comece com ,
            String filtro = "";
            int cont = 0;
            foreach (var item in itensSelecionados)
            {

                if (!String.IsNullOrEmpty(item))
                {
                    if (cont > 0)
                        filtro += ",";

                    filtro += item;
                    cont++;
                }

            }
            // filtro += ")";
            if (cont > 0)
            {

                if (ID1 =="1")
                {
                    listaNfeCD = cdoca.BuscaItensListaSeparacao(filtro);
                    return RedirectToAction("lista_separacao_bulk");
                }
                else
                {

                    listaNfeCD = cdoca.ListaNfCrosDockSep(filtro);
                    return RedirectToAction("lista_conferencia_separada");
                }
            }
            return null;
        }

        public IActionResult lista_conferencia_separada()
        {
            var GroupByNF = listaNfeCD.GroupBy(s => s.Notas);
            var listaHead = new List<ListaConferenciaViewModel>();
            foreach (var item in GroupByNF)
            {
                var listaItem = new ListaConferenciaViewModel();

                var nota = listaNfeCD.Where(x => x.Notas == item.Key).OrderBy(x=>x.Sku).ToList();
                var head = nota.FirstOrDefault();
                var volumes = nota.Sum(x => x.Qtd_item);
                var pesos = nota.Sum(x => x.peso_item);
                // numera os itens
                for (int i = 0; i < nota.Count; i++)
                {
                    nota[i].Id_item = i + 1;
                }
                if (nota.Count >= 45)// desdobra a nota
                {

 

                    // int qtdPag = nota.Count % 60 !=0 ? (nota.Count / 60)+1: (nota.Count / 60);
                    int qtdItens = nota.Count;
                    int qtdini = 1;
                    int qtdfim = 45;

                    while (qtdItens > 0)
                    {
                        var listParcial = nota.Where(x => x.Id_item >= qtdini && x.Id_item <= qtdfim).ToList();

                        var tela = new ListaConferenciaViewModel();

                        tela.nfs = head.Notas;
                        tela.DataNfe = head.data_emissao;
                        tela.Id_cliente = head.Id_cliente;
                        tela.Nome_cli = $"{head.Nome_cli} ** Continua";
                        tela.Destino = head.destino;
                       
                        tela.Pesos = pesos;
                        var lista_tela2 = TypeAdapter.Adapt<List<NfeCrossDock>, List<ListaCrossDockViewModel>>(listParcial);
                        tela.Itens = lista_tela2.OrderBy(x=>x.Sku).ToList();

                      

                        qtdini = qtdfim + 1;
                        qtdfim = qtdfim + 45;

                        qtdItens = qtdItens - listParcial.Count;

                        if (qtdItens==0)
                        {
                            tela.Volumes = volumes;

                        }
                        listaHead.Add(tela);
                    }



                }
                else

                {
                    var tela = new ListaConferenciaViewModel();
                    tela.nfs = head.Notas;
                tela.DataNfe = head.data_emissao;
                tela.Id_cliente = head.Id_cliente;
                tela.Nome_cli = head.Nome_cli;
                tela.Destino = head.destino;
                tela.Volumes = volumes;
                tela.Pesos = pesos;
                var lista_tela = TypeAdapter.Adapt<List<NfeCrossDock>, List<ListaCrossDockViewModel>>(nota);
                tela.Itens = lista_tela;




                listaHead.Add(tela);
            }
            }


            return View(listaHead); 

        }

        //public IActionResult ListadeSeparacaoAgrupada(String ID,String ID1)
        //{
        //    var itensSelecionados = ID.Split(',');
        //    // filtrar caso comece com ,
        //    String filtro = "";
        //    int cont = 0;
        //    foreach (var item in itensSelecionados)
        //    {

        //        if (!String.IsNullOrEmpty(item))
        //        {
        //            if (cont > 0)
        //                filtro += ",";

        //            filtro += item;
        //            cont++;
        //        }

        //    }
        //   // filtro += ")";
        //    if (cont > 0)
        //    {

        //        lista = cdoca.BuscaItensListaSeparacao(filtro);
               
        //    }
        //    return RedirectToAction("lista_separacao_bulk");
        //}


        public IActionResult lista_separacao_bulk()
        {
            var head = listaNfeCD.FirstOrDefault();
            var volumes = listaNfeCD.Sum(x => x.Qtd_item);
            var pesos = listaNfeCD.Sum(x => x.peso_item);


            var tela = new ListaConferenciaViewModel();
            if (head != null)
            {
                tela.Id_cliente = head.Id_cliente;
                tela.Nome_cli = head.Nome_cli;
                tela.Volumes = volumes;
                tela.Pesos = pesos;
                var lista_tela = TypeAdapter.Adapt<List<NfeCrossDock>, List<ListaCrossDockViewModel>>(listaNfeCD);
                tela.Itens = lista_tela; 
            }


            return View(tela);
        }
    }
}
