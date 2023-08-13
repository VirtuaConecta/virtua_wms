using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using VirtuaBusiness.Cliente;
using VirtuaBusiness.Pedidos;
using VirtuaConecta.Wms.UI.ViewModel;
using VirtuaDTO;
using VirtuaRepository;
using FastMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using VirtuaBusiness.Cfop;
using VirtuaBusiness.Municipio;
using Virtua.Utilities;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AspNetCoreHero.ToastNotification.Abstractions;
using Newtonsoft.Json;
using VirtuaBusiness.Produto;
using VirtuaBusiness.Estoque;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class PedidosController : Controller
    {

        IclienteBLL _cliente;
        IpedidoBLL _pedido;
        IcfopBLL _cfop;
        ImunicipioBLL _municipio;
        private readonly ILogger<PedidosController> _logger;
        private readonly IDataConnection db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _config;
        private readonly INotyfService _notyf;
        private readonly IprodutoBLL produto;
        private readonly IEstoqueBll _estoque;
        String Msg = null;
        //  List<Pedido1ItensViewModel> listaPedManualFinal = new List<Pedido1ItensViewModel>();

        public static List<Pedido1ItensViewModel> listaPedManualFinal { get; set; }
        public static List<EstoqueViewModel> ListaEstoqueResumo { get; set; }
        
        
        private static PedAgrupadoViewModel listaAgrupada { get; set; }
        public PedidosController(IDataConnection db, IWebHostEnvironment web,
            IpedidoBLL ipedido, IclienteBLL icliente, ImunicipioBLL imunicipio,
            IcfopBLL icfop, IConfiguration config, ILogger<PedidosController> logger, INotyfService notyf, IprodutoBLL produto, IEstoqueBll estoque)
        {
            this.db = db;
            _webHostEnvironment = web;
            _pedido = ipedido;
            _cliente = icliente;
            _cfop = icfop;
            _municipio = imunicipio;
            _config = config;
            _notyf = notyf;
            this.produto = produto;
            _estoque = estoque;
            _logger = logger;
            //vOperador = HttpContext.Session.GetString("UsrLogado") == null ? "" : HttpContext.Session.GetString("UsrLogado").ToString();
        }

        #region Upload de pedidos
        public IActionResult Upload_arquivos()
        {

                _notyf.Information("Arrastar arquivos a serem importados - fecha em 5 sec.", 5);


            // TempData["UrlDestino"] = "/Pedidos/Upload_arquivos";
            return View();
        }
       
        
        
        
        
        [HttpPost]
        public IActionResult UploadArquivosRecebidos(IFormFile file, String baixa)
        {
            var baixar = "N";

            if (baixa == "on")
            {
                baixar = "S";
            }


            var ret = "Erro";
            // local de destino
            string path = _config.GetSection("Arquivos:Upload_pedidos").Value;
            try
            {


                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                List<string> uploadedfiles = new List<string>();
                string filename = Path.GetFileName(file.FileName);
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


                    ret += _pedido.InsereArquivosDaPastaBll(uploadedfiles, path, baixar);


                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro de Upload ");
            }


            TempData["msg_upload"] = ret;


            return RedirectToAction("Upload_arquivos");
        }
        public IActionResult ListarArqEdi()
        {
            TempData["UrlDestino"] = "/Pedidos/ListarArqEdi";
            string webRootPath = _webHostEnvironment.WebRootPath;

            // busca o itens do diretorio e exibe na view
            ListaDirViewModel listaArq = new ListaDirViewModel();/*string.Format("{0}Arquivos\\Edi\\Wms",webRootPath Server.MapPath(@"\"*/
            var originalDirectory = new DirectoryInfo(_config.GetSection("Arquivos:Upload_pedidos").Value);

            if (!originalDirectory.Exists)
                originalDirectory.Create();

            IEnumerable<FileInfo> files = originalDirectory.GetFiles();
            return View(files);
        }

        #endregion

        #region Entrada

        public IActionResult ListaPedidosEntrada(DadoPesquisaViewModel dadosTela)
        {
            ViewBag.Remessa = "Entrada";

            if (dadosTela.Id > 0)
            {
                TempData["DadoPesquisaViewModel"] = JsonConvert.SerializeObject(dadosTela);
            }
            else
            {
                if (TempData["DadoPesquisaViewModelRetorno"] is string s)
                {
                    dadosTela = JsonConvert.DeserializeObject<DadoPesquisaViewModel>(s);
                }
            }





            //Pesquisa o pedido de entrada na lista de pedidos Pendentes a inserir no Estoque
            String TipoPedidoEntrada = "I";

            var listaDTO = _pedido.ListaPedidoGeralBLL(dadosTela.Dt_ini, dadosTela.Dt_fim, TipoPedidoEntrada, dadosTela.Id).Where(x => x.Cancelado == "N" && x.Criado == 0).ToList();

            var PedidoTela = new List<Pedido1ViewModel>();

            foreach (var item in listaDTO)
            {
                var itemCapturado = new Pedido1ViewModel();
                itemCapturado.Indice = item.indice;
                itemCapturado.Dt_emissao = item.Dt_emissao;
                itemCapturado.Nr_documento = item.Nr_documento;
                itemCapturado.NomeCliente = item.NomeCliente;
                itemCapturado.Nome_dest = item.Nome_dest;
                itemCapturado.Processado = item.Processado;
                itemCapturado.Cnpj_dest = item.Cnpj_dest;
                itemCapturado.Chave = item.Chave;
                itemCapturado.Total_nf = item.Total_nf;
                itemCapturado.Remessa = item.Remessa;

                PedidoTela.Add(itemCapturado);
            }

            //Manda os pedidos para tela

            return View(PedidoTela);
        }

        public IActionResult PesquisaPedidoEntrada()
        {
            ViewBag.Title = "Pequisar Pedidos Entrada";

            var dadosTela = new DadoPesquisaViewModel();
            dadosTela.Dt_ini = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            dadosTela.Dt_fim = DateTime.Now.ToString("yyyy-MM-dd");
            var clientes = _cliente.Listar_Cliente().Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(y => y.NomeCliente).ToList();
            dadosTela.Lista = new SelectList(clientes, "IdCliente", "NomeCliente");




            return View(dadosTela);
        }

        [HttpPost]
        public IActionResult PesquisaPedidoEntrada(DadoPesquisaViewModel dadosTela)
        {

            return RedirectToAction("ListaPedidosEntrada", dadosTela);

        }

        public List<Pedido1ViewModel> listaResumoPedidoPendentes(String TipoPedido_Ent_saida)
        {
            //condições da lista de pedido para retornar a tela.
            String PedidoNaoCancelado = "N";
            String PedidoNaoProcessado = "N";
            var listaDTO = _pedido.ListaPedidoPendente(TipoPedido_Ent_saida, PedidoNaoCancelado, PedidoNaoProcessado);
            var PedidoTela = new List<Pedido1ViewModel>();
            var listaVM = new List<Pedido1ItensViewModel>();
            var listaTela = new List<Pedido1ViewModel>();

            foreach (var item in listaDTO)
            {
                var itemCapturado = new Pedido1ViewModel();
                itemCapturado.Indice = item.indice;
                itemCapturado.Dt_emissao = item.Dt_emissao;
                itemCapturado.Nr_documento = item.Nr_documento;
                itemCapturado.NomeCliente = item.NomeCliente;
                itemCapturado.Nome_dest = item.Nome_dest;

                //   itemCapturado.Cnpj_dest = item.Cnpj_dest;
                itemCapturado.Chave = item.Chave;
                itemCapturado.Total_nf = item.Total_nf;
                itemCapturado.Remessa = item.Remessa;

                PedidoTela.Add(itemCapturado);
            }
            return PedidoTela;
        }

        //gera a lista de itens do pedido pendentes como tabela filha na tela de Pedidos
        public IActionResult ListaItensPedidoPend(Int32 ID)
        {
            var listaItemDTO = _pedido.Listar_Itens_Ped(ID);

            var listaItemVM = TypeAdapter.Adapt<IEnumerable<PedidoItemDTO>, IEnumerable<Pedido1ItensViewModel>>(listaItemDTO);

            return View(listaItemVM);
        }

        //Insere o pedido no Estoque - Estagio de atribuir Lote/Validade
        public ActionResult InserePedidoEntradaEstoque(Int32 ID, String ID2)
        {
            int ErroNoPedido = 0;
            int ErroInserirEstoque = 1;
            int ErroAtualizaHead = 2;
            int ErroAtualizaItem = 3;
            int Sucesso = 4;
            int FaltaVariavel = 5;
            Int32 Nr_Pedido_temp = ID;
            String Tipo_Remassa = ID2;


            var dadosTelaPed = "";
            if (TempData["DadoPesquisaViewModel"] is string s)
            {
                dadosTelaPed = s;
            }


            // Captura operador de inserção

            // Captura operador de inserção


            //Fazer a inserção em Ordem_cli e garantir um item por Sku x Valor
            //Transferir para a posição estagio de Tribuir lote/Validade
            var dadosUsr = Global.Dados_user;
            var resp = _pedido.InserePedidoNoEstoqueBll(Nr_Pedido_temp, Tipo_Remassa, dadosUsr.NomeUsuario).Split('|');

            if (resp[1] == "0")
            {
                _notyf.Error($"{resp[0]}", 10);
            }
            else
            {
                _notyf.Success($"{resp[0]}", 10);
            }
            if (dadosTelaPed != "")
            {
                TempData["DadoPesquisaViewModelRetorno"] = dadosTelaPed;
            }

            // Retornar para a lista de pedidos a entrar.
            return RedirectToAction("ListaPedidosEntrada");
        }
        #endregion

        #region Saidas

        public IActionResult ListaPedidosSaida(DadoPesquisaViewModel dadosTela)
        {
            ViewBag.Remessa = "Saida";

            if (dadosTela.Id > 0)
            {
                TempData["DadoPesquisaViewModel"] = JsonConvert.SerializeObject(dadosTela);
            }
            else
            {
                if (TempData["DadoPesquisaViewModelRetorno"] is string s)
                {
                    dadosTela = JsonConvert.DeserializeObject<DadoPesquisaViewModel>(s);
                }
            }

            //Pesquisa o pedido de entrada na lista de pedidos Pendentes a inserir no Estoque
            String TipoPedidoEntrada = "O";

            var listaDTO = _pedido.ListaPedidoGeralBLL(dadosTela.Dt_ini, dadosTela.Dt_fim, TipoPedidoEntrada, dadosTela.Id).Where(x => x.Cancelado == "N" && x.Processado == "N").ToList();

            var PedidoTela = new List<Pedido1ViewModel>();

            foreach (var item in listaDTO)
            {
                var itemCapturado = new Pedido1ViewModel();
                itemCapturado.Indice = item.indice;
                itemCapturado.Dt_emissao = item.Dt_emissao;
                itemCapturado.Nr_documento = item.Nr_documento;
                itemCapturado.NomeCliente = item.NomeCliente;
                itemCapturado.Nome_dest = item.Nome_dest;
                itemCapturado.Processado = item.Processado;
                itemCapturado.Cnpj_dest = item.Cnpj_dest;
                itemCapturado.Chave = item.Chave;
                itemCapturado.Total_nf = item.Total_nf;
                itemCapturado.Remessa = item.Remessa;

                PedidoTela.Add(itemCapturado);
            }

            //Manda os pedidos para tela

            return View(PedidoTela);
        }

        public IActionResult PesquisaPedidoSaida()
        {
            ViewBag.Title = "Pequisar Pedidos Saida";

            var dadosTela = new DadoPesquisaViewModel();
            dadosTela.Dt_ini = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            dadosTela.Dt_fim = DateTime.Now.ToString("yyyy-MM-dd");
            var clientes = _cliente.Listar_Cliente().Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(y => y.NomeCliente).ToList();
            dadosTela.Lista = new SelectList(clientes, "IdCliente", "NomeCliente");
            return View(dadosTela);
        }

        [HttpPost]
        public IActionResult PesquisaPedidoSaida(DadoPesquisaViewModel dadosTela)
        {

            return RedirectToAction("ListaPedidosSaida", dadosTela);

        }

        /// <summary>
        /// Captura os Ids dos pedidos a baixar
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IActionResult GeraListadeBaixa(String ID)
        {
            var lista = _pedido.ListaItensPedidosAgrupados(ID);
            if (lista.Count() == 0)
            {
                return RedirectToAction("ListaPedidosSaida");
            }
            else
            {
                var tela = TypeAdapter.Adapt<IEnumerable<PedidoItemDTO>, IEnumerable<Pedido1ItensViewModel>>(lista);

                TempData["ItensPedBaixaAgrupado"] = JsonConvert.SerializeObject(tela);
                return RedirectToAction("ListadeBaixaAgrupada");


            }

        }

        /// <summary>
        /// Agrupa os itens dos pedidos selecionados para baixa
        /// </summary>
        /// <param name="ID">Nr de pedidos separados por vírgula</param>
        /// <returns></returns>
        public IActionResult ListadeBaixaAgrupada(String ID)
        {
            var lista = _pedido.ListaItensPedidosAgrupados(ID);
            if (lista.Count() == 0)
            {
                return RedirectToAction("ListaPedidosSaida");
            }
            var tela = new List<Pedido1ItensViewModel>();
            foreach (var item in lista)
            {
                var itemTela = new Pedido1ItensViewModel();
                itemTela.id_produto = item.id_produto;
                itemTela.Sku = item.Sku;
                itemTela.Descricao = item.Descricao;
                itemTela.Und = item.Und;
                itemTela.Qtd = item.Qtd;
                itemTela.Acao = "Á Validar";

                tela.Add(itemTela);
            }
            // var tela = TypeAdapter.Adapt<IEnumerable<PedidoItemDTO>, IEnumerable<Pedido1ItensViewModel>>(lista);
            ViewBag.Pedidos = ID;
            return View(tela);
        }


        public ViewResult CriaListaDeConferenciaDeBaixa(String Js)
        {
            listaAgrupada = new PedAgrupadoViewModel();


            listaAgrupada = JsonConvert.DeserializeObject<PedAgrupadoViewModel>(Js); // ver se vai pecisar da dto dest model senão apagar

            // montar a lista na tela
            //  var listaTela = new ListaAgrupadaEstoqueBaixaViewModel(); // validar se precisara desta  model senão apagar era para montar a baixa
            var listaTela = new List<EstoqueDTO>();

            //var t = ListaEstoqueResumo;
            foreach (var pedido in listaAgrupada.items)
            {
                var itemEstoque = _estoque.CapturaRegistroEstoquePorIDBll(Convert.ToInt32(pedido.Id));

                itemEstoque.Qtd_baixar = pedido.Qtd;

                listaTela.Add(itemEstoque);

                

            }

            //Lista a ser mostrada para confimação na tela
            ListaEstoqueResumo = TypeAdapter.Adapt<IEnumerable<EstoqueDTO>, List<EstoqueViewModel>>(listaTela);



            ViewBag.Pedidos = listaAgrupada.pedidos;

            return View("ResumoPedidoBaixaAgrupado", ListaEstoqueResumo);
        }


        public IActionResult BaixarPedidosSaida()
        {

            var pedidosSaidaBaixa = listaAgrupada;

            if (pedidosSaidaBaixa.items.Count()>0)
            {
                var ListaPedidosDTO = TypeAdapter.Adapt<PedAgrupadoViewModel, PedAgrupadoDTO>(pedidosSaidaBaixa);

                var resp = _pedido.BaixarPedidoAgrupado(ListaPedidosDTO);

                if (resp=="OK")
                {
                    _notyf.Information("Pedido(s) baixado(s)!", 5);
                }
                else
                {
                    _notyf.Error("Erro ao baixar pedido(s)", 5);
                }

            }
            else
            {
                _notyf.Error("Erro ao capturar pedidos", 5);
            }



            return RedirectToAction("PesquisaPedidoSaida");
        }

        //public IActionResult ResumoPedidoBaixaAgrupado()
        //{
        //    ViewBag.Pedidos = listaAgrupada.pedidos;



        //    var listaTela = ListaEstoqueResumo;
        //    return View(listaTela);
        //}


        public IActionResult ListaProdutoEstoque(Int32 ID)
        {
            var listaItemDTO = _estoque.ListaEstoqueProdutoBll(ID, null);

            var listaItemVM = TypeAdapter.Adapt<IEnumerable<EstoqueDTO>, IEnumerable<EstoqueViewModel>>(listaItemDTO);

            return View(listaItemVM);
        }

        #endregion

        #region Pedido Manual

        /// <summary>
        ///   Insere pedidos criados na tela
        /// </summary>
        public IActionResult CriarPedManual(String id)
        {
            // Criar Listas de carregamento
            // 1 - lista de clientes
            // lista de cfop
            // lista de Cidades
            Pedido1ViewModel Tela = new Pedido1ViewModel();
            listaPedManualFinal = new List<Pedido1ItensViewModel>();

            //lista de Tipos de Doc metodo 1
            Tela.Lista_tipo_doc = new SelectList(ListaOpcaoTipoDoc(), "Valor", "Nome");

            //Lista de Opções Remessa metodo 2
            if (Tela.Lista_remessa == null)
            {
                Tela.Lista_remessa = new SelectList(new List<SelectListItem>()
                {
                    new SelectListItem() { Text= "Entrada", Value = "I" },
                    new SelectListItem() { Text= "Saida", Value = "O"}
                }, "Value", "Text");
            }
            // carregar a lista de clientes
            var listaCliDto = _cliente.Listar_Cliente().Where(z => String.IsNullOrWhiteSpace(z.DescricaoCliente) == false).OrderBy(x => x.NomeCliente).ToList();
            var ListaCliVM = TypeAdapter.Adapt<IEnumerable<ClienteDTO>, IEnumerable<ClienteViewModel>>(listaCliDto);

            Tela.Lista_Clientes = new SelectList(ListaCliVM, "DescricaoCliente", "DescricaoCliente");
            // Carrega a lista de cfop
            var lisatcfopDto = _cfop.Listar_CfopBLL();
            var ListacfopVM = TypeAdapter.Adapt<IEnumerable<CfopDTO>, IEnumerable<CfopViewModel>>(lisatcfopDto);
            Tela.Lista_cfop = new SelectList(ListacfopVM, "Cfop_txt", "Descricao");

            var ListaMunDTO = _municipio.Listar_Municipios().OrderBy(x => x.Municipio).ToList();
            var ListaMunVM = TypeAdapter.Adapt<IEnumerable<MunicipiosDTO>, IEnumerable<MunicipiosViewModel>>(ListaMunDTO);

            Tela.Lista_cidade = new SelectList(ListaMunVM, "Cod_municipio", "Nome_municipio");

            Tela.ListaItensPedido = listaPedManualFinal;
            if (id == "I")
            {//alterar para buscar do db
                Tela.Remessa = "I";
                Tela.Cnpj_dest = "03986934000132";
                Tela.IE_dest = "206644676115";
                Tela.Nome_dest = "PELOG ARMAZENS GERAIS";
                Tela.Bairro_dest = "TAMBORE";
                Tela.End_dest = "AV MARCOS PENTEADO DE ULHOA RODRIGUES";
                Tela.Cep_dest = "06460040";
                Tela.Cod_municipio = 3505708;
                Tela.Cod_municipio_txt = "3505708";
                Tela.Nr_destino = "491";
                Tela.Fis_Jur = "J";

                ViewBag.remessa = "I";
            }
            else
            {
                Tela.Remessa = "O";
                ViewBag.remessa = "O";
            }

            return View(Tela);
        }
        [HttpPost]
        public ActionResult CriarPedManual(Pedido1ViewModel Tela)
        {
            //Insere os itens manuai na model
            Tela.ListaItensPedido = listaPedManualFinal;
            //monta o nr do pedido
            // Busca a Letra do Cliente
            var dadosCli = Tela.Id_cliente_txt.Split('|');
            var Id_cliente = Convert.ToInt32(dadosCli[1]);

            var idMunicipio = Tela.Cod_municipio_txt;


            //var Cliente = _cliente.Pesquisar_Cliente(Id_cliente);
            String Letra = dadosCli[2];


            if (Letra == null)
            {

                _notyf.Error("Não foi encontrado A letra Complemento do cliente", 5);
                return RedirectToAction("CriarPedManual/" + Tela.Remessa, "Pedidos");
            }
            String vNf = null;
            vNf = Tela.Nr_doc_origem.Trim();

            if (Tela.tipo_doc == "NF")
            {
                vNf = "M" + FuncoesString.Direta("000000000" + vNf, 9) + "-" + Tela.Remessa + Letra + Tela.Serie_doc;
            }
            else
            {
                vNf = "M" + FuncoesString.Direta("000000000" + vNf, 9) + "-" + "P" + Tela.Remessa + Tela.Serie_doc;
            }



            // Verifica Se o Pedido já está sendo criado por outra pessoa ou pelo proprio usuario na base temporaria e no DB
            //  Tela.Operador = Convert.ToString(Session["UsrLogado"]);
            //Captura o ip da maquina
            string nome = Dns.GetHostName();

            IPAddress[] ip = Dns.GetHostAddresses(nome);

            String vIp = ip[0].ToString();

            // Aqui colocar uma regra para validar o total do valor  com o valor e qtd dos itens
            var checkPedTemp = _pedido.ValidaPedManual(vNf, Id_cliente, Tela.Operador, vIp);

            if (checkPedTemp.Retorno == 4)
            {
                _notyf.Error("Esta nota já existe no Banco de dados", 5);
                return RedirectToAction("CriarPedManual/" + Tela.Remessa, "Pedidos");
            }

            Tela.Nr_documento = vNf;


            Tela.Dt_entrada = DateTime.Now;

            Tela.Id_cliente = Id_cliente;
            Tela.Cod_municipio = Convert.ToInt32(idMunicipio);


            //Envia o Ip da maquina para inserir no DB Temporario
            Tela.Ip = vIp;
            Tela.Id_remetente = Convert.ToInt32(String.IsNullOrEmpty(dadosCli[3]) ? 0 : dadosCli[3]);
            Tela.NomeCliente = dadosCli[0];
            // destino.uf = Tela.us
            var TelaConvertida = ConvertModelPedido(Tela);


            var RetPed = _pedido.Inserir_PedidoManualBLL(TelaConvertida).Split('|');



            if (RetPed[0] != "OK")
            {
                _notyf.Error($"Erro ao Salvar Pedido. {RetPed[0]}", 5);
                return RedirectToAction("CriarPedManual/" + Tela.Remessa, "Pedidos");
            }

            // TempData["IdPedido"] = RetPed[1].ToString();
            _notyf.Information($"Pedido Criado: {vNf}", 5);
            // Envia os dados para cria o item
            return RedirectToAction("Inicio", "Main");
        }

        public String InserePedidoItem([FromBody] List<Pedido1ItensViewModel> produtos)
        {

            Int32 cont = 0;
            if (produtos == null)
            {
                produtos = new List<Pedido1ItensViewModel>();
            }


            foreach (var item in produtos)
            {
                var itemSep = new Pedido1ItensViewModel();
                var SkuDesc = item.Sku.Split('|');
                itemSep.Sku = SkuDesc[0];
                itemSep.Descricao = SkuDesc[1];
                itemSep.id_produto = SkuDesc[2];
                itemSep.Qtd = item.Qtd;
                itemSep.P_unit = item.P_unit;

                listaPedManualFinal.Add(itemSep);
                cont++;
            }

            return cont.ToString(); ;

        }

        //gera a lista IprodutoBLL autocompletar SKU para entrada do pedido Manual
        public String AutoCompleteSku(String Id)
        {
            var dadosCli = Id.Split('|');
            var Id_cliente = Convert.ToInt32(dadosCli[1]);


            var resultadoDTO = produto.PesquisarProdutoBLL(null, Id_cliente, null).ToList();

            var listaTela = new List<listaSelecao>();

            List<String> listaString = new List<string>();
            //CriarItemManual uma lista com SKU + Descrição
            foreach (var itens in resultadoDTO)
            {
                //   var itemcapturado = new listaSelecao();

                //  itemcapturado.Valor = itens.Sku;
                //  itemcapturado.Nome =   itens.Sku + "-" + itens.Descricao;
                //      listaTela.Add(itemcapturado);

                listaString.Add(itens.Sku + "|" + itens.Descricao + "|" + itens.Id_produto);


            }

            var listaF = JsonConvert.SerializeObject(listaString);


            return listaF;
        }




        //====================================================================
        #endregion

        #region Geral


        [HttpGet]
        public ActionResult AddrByCep(string cep)
        {

            String cep1 = cep.Replace("-", "");
            var dados = _municipio.CapturaENderecoPeloCep(cep1);
            if (dados != null)
            {
                var cidade = _municipio.Listar_Municipios().Where(x => Convert.ToString(x.Cod_municipio) == dados.ibge).FirstOrDefault();
                if (cidade != null && cidade.Cod_municipio != null)
                {
                    dados.Cod_municipio_txt = cidade.Cod_municipio.ToString();
                    dados.cep = dados.cep.Replace("-", "");

                }

                return Json(dados);

            }
            return null;

        }
        public IEnumerable<listaSelecao> ListaOpcaoTipoDoc()
        {

            return new List<listaSelecao>
            {

                new listaSelecao{ Nome="Nota Fiscal", Valor="NF"},
                new listaSelecao{ Nome="Pedido", Valor="PED"},

            };


        }
        public IActionResult ApagaArquivo(String Filepath)
        {

            // ...or by using FileInfo instance method.
            System.IO.FileInfo fi = new System.IO.FileInfo(@Filepath);
            try
            {
                fi.Delete();
            }
            catch (System.IO.IOException e)
            {
                _logger.LogError(e.Message, "Erro ApagaArquivo ");

            }

            return Redirect(TempData["UrlDestino"].ToString());
        }
        public ActionResult InsereArquivo(String Filepath)
        {

            //captura o camiho do arquivo
            ViewBag.CAMINHO = Filepath;

            //Captura o ip da maquina
            string nome = Dns.GetHostName();
            IPAddress[] ip = Dns.GetHostAddresses(nome);
            String vIp = ip[0].ToString();

            TempData.Add("FALHA", "Arquivo Não Reconhecido!");
            return Redirect(TempData["UrlDestino"].ToString());



        }


        public IActionResult BlankPage()
        {

           // String Msg = null;

            if (TempData["msg_upload"] != null)
            {
                Msg = TempData["msg_upload"].ToString();
            }
            if (String.IsNullOrEmpty(Msg))
            {
                _notyf.Success("Arrastar arquivos a serem importados - fecha em 5 sec.", 5);
            }
            else if (Msg == "OK")
            {

                _notyf.Success("Arquivo Importado - fecha em 5 sec.", 5);
            }

            else
            {
                _notyf.Error($"{Msg} - fecha em 5 sec.", 5);
            }
            return RedirectToAction("Upload_arquivos");
        }

        public String Retorna_Notificacao() 
        {
           

          
      
            if (TempData["msg_upload"].ToString().Contains("OK"))
            {

                _notyf.Success($"Arquivo {TempData["msg_upload"]} Importado ", 5);
            }

            else
            {
                _notyf.Error($"{TempData["msg_upload"]} ", 5);
            }
            return "OK";
        }

        private Nfe ConvertModelPedido(Pedido1ViewModel pedido)
        {
            var nfe = new Nfe();
            // tratar o cpf/cnpj retirar a mascara e validar pf e pj
            try
            {
                var localPedHead = new NfeHead();
                var local = _municipio.Listar_Municipios().Where(x => x.Cod_municipio == pedido.Cod_municipio).FirstOrDefault();
                if (local != null && !String.IsNullOrEmpty(local.Nome_municipio))
                {
                    localPedHead.Cidade_destino = local.Municipio;
                    localPedHead.Estado_destino = local.Uf;
                }
                else
                {
                    nfe.Head.Cidade_destino = "-";
                }



                nfe.Id_cliente = Convert.ToInt32(pedido.Id_cliente);
                nfe.Id_rementente = Convert.ToInt32(pedido.Id_remetente);
                nfe.Nf_Wms = pedido.Nr_documento;


                localPedHead.Nome_cliente = pedido.NomeCliente;
                localPedHead.Nr_original_cliente = pedido.Nr_doc_origem;
                localPedHead.Cnpj_cli = pedido.Cpf_cnpj_cliente;
                if (pedido.Chave == "0")
                {
                    localPedHead.Chave = new String('0', 44);
                }
                else
                { localPedHead.Chave = pedido.Chave; }
                localPedHead.Especie = pedido.Especie;
                localPedHead.Fis_jur_destino = pedido.Fis_Jur;

                localPedHead.Nome_destino = pedido.Nome_dest.ToUpper();
                localPedHead.Bairro_destino = pedido.Bairro_dest.ToUpper();
                localPedHead.Cep_destino = pedido.Cep_dest;
                //localPedHead.Cidade_destino = dest.cidade.ToUpper();
                localPedHead.Cnpj_destino = pedido.Cnpj_dest.Replace("/", "").Replace("-", "").Replace(".", "");
                localPedHead.IE_destino = pedido.IE_dest.ToUpper();
                localPedHead.Endereco_destino = pedido.End_dest.ToUpper();
                //localPedHead.Estado_destino = dest.uf.ToUpper();
                localPedHead.Id_Cidade_destino = Convert.ToInt32(pedido.Cod_municipio);
                localPedHead.Tipo_pedido = pedido.tipo_doc;
                localPedHead.Serie = pedido.Serie_doc;
                localPedHead.Fis_jur_destino = pedido.Fis_Jur;
                nfe.Head = localPedHead;


                var listNfeItens = new List<NfeItens>();

                foreach (var item in pedido.ListaItensPedido)
                {
                    var itemNfe = new NfeItens();

                    itemNfe.Sku = item.Sku;
                    itemNfe.Qtd = Convert.ToDecimal(item.Qtd);
                    itemNfe.Punit = Convert.ToDecimal(item.P_unit);

                    listNfeItens.Add(itemNfe);


                }

                nfe.ListaItens = listNfeItens;





            }
            catch (Exception ex)
            {

            }






            return nfe;
        }
        #endregion
    }



}
