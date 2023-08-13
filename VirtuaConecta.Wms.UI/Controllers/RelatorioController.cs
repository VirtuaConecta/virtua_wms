using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FastMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using OfficeOpenXml;
using VirtuaBusiness.Cliente;
using VirtuaBusiness.Pedidos;
using VirtuaConecta.Wms.UI.ViewModel;
using VirtuaDTO;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class RelatorioController : Controller
    {
        IpedidoBLL _pedido;
        IclienteBLL _clienteBLL;
        private readonly ILogger _logger;
        IHostingEnvironment _env;
        public RelatorioController(IpedidoBLL ipedido, IclienteBLL icliente, ILogger<RelatorioController> logger, IHostingEnvironment env )
        {
            _pedido = ipedido;
            _clienteBLL = icliente;
            _logger = logger;
            _env = env;
        }

        public IActionResult Mov_Peso_Valor()
        {
            return View();
        }

        public IActionResult Lista_NF_Entrada_Saida_Pesquisa()
        {
            var cliDTO = _clienteBLL.Listar_Cliente();
            var cliVm = TypeAdapter.Adapt<IEnumerable<ClienteDTO>, List<ClienteViewModel>>(cliDTO);
            listacliente.lista = cliVm;
            var dadosTela = new DadoPesquisaViewModel();
            dadosTela.Lista = new SelectList(cliVm.Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(x => x.NomeCliente), "IdCliente", "NomeCliente");
            dadosTela.Dt_ini = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            dadosTela.Dt_fim = DateTime.Now.ToString("yyyy-MM-dd");



            return View(dadosTela);
        }
        [HttpPost]
        public IActionResult Lista_NF_Entrada_Saida_Pesquisa(DadoPesquisaViewModel dadoTela)
        {


            var resp = Verifica(dadoTela);
            if (resp == "OK")
            {
                return RedirectToAction("Lista_NF_Entrada_Saida");
            }
            else
            {
                dadoTela.Lista =
               new SelectList(listacliente.lista.Where(
                   x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(x => x.NomeCliente), "IdCliente", "NomeCliente");

                dadoTela.Erro = resp;
                return View("Lista_NF_Entrada_Saida_Pesquisa", dadoTela);
            }


        }
       
        public IActionResult Lista_NF_Entrada_Saida()
        {
            var ped = new PedidosEntradaSaidaViewModel();
       
                ped.LinkDownload = null;
          

            ped.ListaPedidos = listaPedidoViewModel.lista;


            return View(ped);
        }

      
        public IActionResult Lista_NF_Entrada_SaidaDownload(String ret)
        {
            var ped = new PedidosEntradaSaidaViewModel();

            String link = _env.WebRootPath + @"\Download\"+ret;
       
            if (link == "")
            {
                ped.LinkDownload = null;
            }
            else
            {
                ped.LinkDownload = link;
            }
            ped.ListaPedidos = listaPedidoViewModel.lista;

            return View("Lista_NF_Entrada_Saida", ped);
        }



        private String Verifica(DadoPesquisaViewModel dadoTela)
        {
            string resp = "OK";

            if (!DateTime.TryParse(dadoTela.Dt_ini, out DateTime inicio))
            {
                resp = "O campo data inicial precisa ser preenchido com uma data, iniciando pelo ano em seguida o mês e depois o dia ";
            }
            else if (inicio > DateTime.Now)
            {
                resp = "A data inicial não pode ser depois de hoje ";
            }
            else if (!DateTime.TryParse(dadoTela.Dt_fim, out DateTime fim))
            {
                resp = "O campo data final precisa ser preenchido com uma data, iniciando pelo ano em seguida o mês e depois o dia ";
            }
            else if (fim > DateTime.Now)
            {
                resp = "A data final não pode ser depois de hoje ";
            }
            else
            {
                var pedDTO = _pedido.Listar_Ped_entrada_saida(dadoTela.Id, fim, inicio);
                var pedVm = TypeAdapter.Adapt<IEnumerable<Ped_entrada_saidaDTO>, List<Ped_entrada_saidaViewModel>>(pedDTO);

                listaPedidoViewModel.lista = pedVm;

                if (dadoTela.Id != 0 && listaPedidoViewModel.lista.FirstOrDefault() == null)
                {
                    resp = "Não existe pedido, nesse periodo, desse cliente ";
                }
                else if (listaPedidoViewModel.lista.FirstOrDefault() == null)
                {
                    resp = "Não existe pedido nesse perido de tempo ";
                }

            }
            return resp;
        }

        public IActionResult detalheNf(String ID)
        {

            var pedVm = listaPedidoViewModel.lista;
            var ped = new Ped_entrada_saidaViewModel();

            if (!String.IsNullOrEmpty(ID ))
            {
                ped = pedVm.Where(x => x.N_fiscal == ID).FirstOrDefault();
            }

            return View(ped);
        }

        public IActionResult ExportarExcel()
        {

            String arq= "Relatorio_Entrada_saida_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
             string caminho = _env.WebRootPath + @"\Download\" + arq;
            //string caminho = AppDomain.CurrentDomain.BaseDirectory +@"wwwroot\Download\" + arq;


            try
            {
                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
               // ExcelPackage excel = new ExcelPackage();
                using (var excel = new ExcelPackage(stream))
                {

                    var planilha = excel.Workbook.Worksheets.Add("Fechamento" + DateTime.Now.ToString());

                    planilha.Row(1).Style.Font.Bold = true;


                    planilha.Cells[1, 1].Value = "Nf Pelog";
                    planilha.Cells[1, 2].Value = "Destinatario";
                    planilha.Cells[1, 3].Value = "Sku";
                    planilha.Cells[1, 4].Value = "Descrição";
                    planilha.Cells[1, 5].Value = "Qtd";
                    planilha.Cells[1, 6].Value = "Dt processado";
                    planilha.Cells[1, 7].Value = "Dt emissão";
                    planilha.Cells[1, 8].Value = "Dt saida";
                    planilha.Cells[1, 9].Value = "Nf Cliente";

                    planilha.Cells[1, 10].Value = "I/O";
                    planilha.Cells[1, 11].Value = "Remessa";
                    planilha.Cells[1, 12].Value = "Cidade";
                    planilha.Cells[1, 13].Value = "Estado";
                    planilha.Cells[1, 14].Value = "Volume Nf";
                    planilha.Cells[1, 15].Value = "Valor nf";
                    planilha.Cells[1, 16].Value = "Peso Nf";
                    planilha.Cells[1, 17].Value = "Valor Nf retorno";
                    planilha.Cells[1, 18].Value = "Nf gerado";

                    planilha.Cells[1, 19].Value = "Ordem carga";
                    planilha.Cells[1, 20].Value = "Transportadora";
                    planilha.Cells[1, 21].Value = "Valor unitario";

                    int cont = 2;

                    foreach (var pedido in listaPedidoViewModel.lista)
                    {

                        planilha.Cells[cont, 1].Value = pedido.nf_pelog;
                        planilha.Cells[cont, 2].Value = pedido.nome_cli;
                        planilha.Cells[cont, 3].Value = pedido.sku;
                        planilha.Cells[cont, 4].Value = pedido.descricao;
                        planilha.Cells[cont, 5].Value = pedido.qtd;
                        planilha.Cells[cont, 6].Value = pedido.dt_processado;
                        planilha.Cells[cont, 7].Value = pedido.data_emissao;
                        planilha.Cells[cont, 8].Value = pedido.data_fim;
                        planilha.Cells[cont, 9].Value = pedido.N_fiscal;

                        planilha.Cells[cont, 10].Value = pedido.cancelado;
                        planilha.Cells[cont, 11].Value = pedido.remessa;
                        planilha.Cells[cont, 12].Value = pedido.cidade;
                        planilha.Cells[cont, 13].Value = pedido.estado;
                        planilha.Cells[cont, 14].Value = pedido.volume_total;
                        planilha.Cells[cont, 15].Value = pedido.val_nf;
                        planilha.Cells[cont, 16].Value = pedido.peso_nf;
                        planilha.Cells[cont, 17].Value = pedido.valor_nf_retorno;
                        planilha.Cells[cont, 18].Value = pedido.nf_ok;

                        planilha.Cells[cont, 19].Value = pedido.ord_carga;
                        planilha.Cells[cont, 20].Value = pedido.nome_transp;
                        planilha.Cells[cont, 21].Value = pedido.val_unit;
                        cont++;
                    }

               
                for (cont = 1; cont <= 21; cont++)
                    planilha.Column(cont).AutoFit();
                
                FileStream arquivo = System.IO.File.Create(caminho);
                arquivo.Close();

                System.IO.File.WriteAllBytes(caminho, excel.GetAsByteArray());
                }

                stream.Position = 0;
                return File(stream,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", arq);
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
        public IActionResult ExportarExcel(String link)
        {
                      string caminho = _env.WebRootPath + @"\Download\" + link;

            var fs = System.IO.File.OpenRead(caminho);
            return File(fs, "application/octet-stream",
                        link);
        }

    }
}
