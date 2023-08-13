using FastMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirtuaBusiness.Nfs_cdoca;
using VirtuaBusiness.Ordem_carga;
using VirtuaBusiness.Transportadora;
using VirtuaBusiness.Veiculos;
using VirtuaConecta.Wms.UI.ViewModel;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class OCargaController : Controller
    {
        private readonly ILogger<OCargaController> _logger;
        private readonly ITransportadoraBLL _transp;
        private readonly IVeiculosBLL _veiculos;
        private readonly IOrdem_cargaBLL _oc;
        private IConfiguration _config;
        private IOrdem_cargaBLL _ordem;
        private readonly InfsCdoca nfe;
        IHostingEnvironment _env;

        public static List<Ordem_CargaViewModel> ListaOcVmodelFiltrada { get; set; }
        public OCargaController(IDataConnection db, IConfiguration config, ILogger<OCargaController> logger,
            ITransportadoraBLL transp, IVeiculosBLL veiculos, IOrdem_cargaBLL oc, IHostingEnvironment env, IOrdem_cargaBLL ordem, InfsCdoca nfe)
        {
            _logger = logger;
            _transp = transp;
            _veiculos = veiculos;
            _oc = oc;
            _config = config;
            _env = env;
            _ordem = ordem;
            this.nfe = nfe;
        }

        /// <summary>
        /// Cria uma nova ordem de carga
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CriarOrdemCarga(String id)
        {
            OCargaViewModel Tela = new OCargaViewModel();
            var listaTranspDto = _transp.Lista_transportadoraBLL().OrderBy(x => x.descricao).ToList();
            var ListaTranspVM = TypeAdapter.Adapt<IEnumerable<ListaGenericaDTO>, IEnumerable<ListaGenericaViewModel>>(listaTranspDto);
            //lista de transportadoras
            Tela.Lista_transportadora = new SelectList(ListaTranspVM, "id", "descricao");

            var listaTipoVeicDto = _veiculos.Lista_tipo_veiculoBLL().OrderBy(x => x.descricao).ToList();
            var listaTipoVeicVM = TypeAdapter.Adapt<IEnumerable<ListaGenericaDTO>, IEnumerable<ListaGenericaViewModel>>(listaTipoVeicDto);
            //lista de tipo de veiculos
            Tela.Lista_tipo_veiculo = new SelectList(listaTipoVeicVM, "id", "descricao");

            //lista tipo carga
            Tela.Lista_tipo_carga = new SelectList(ListaTipoCarga(), "id", "descricao");

            var listaStDto = _oc.Lista_status_OcBLL().OrderBy(x => x.descricao).ToList();
            var listStVM = TypeAdapter.Adapt<IEnumerable<ListaGenericaDTO>, IEnumerable<ListaGenericaViewModel>>(listaStDto);
            //lista de status
            Tela.Lista_status_oc = new SelectList(listStVM, "id", "descricao");
            Tela.Stat_evento = 1;
            Tela.Dt_agenda = DateTime.Now.Date;

            return View(Tela);
        }



        public IEnumerable<ListaGenericaViewModel> ListaTipoCarga()
        {

            return new List<ListaGenericaViewModel>
            {

                new ListaGenericaViewModel{ id=1, descricao="Paletizado"},
                new ListaGenericaViewModel{ id=2, descricao="Estivado"},

            };


        }


        public IActionResult Pesquisa_Ordem_Carga()
        {
            DadoPesquisaViewModel dados = new DadoPesquisaViewModel();
            var statusDTO = _ordem.Lista_status_OcBLL();
            var statusVM = TypeAdapter.Adapt<IEnumerable<ListaGenericaDTO>, List<ListaGenericaViewModel>>(statusDTO);
            listaOrdemCarga.lista_status = statusVM;
            dados.Lista = new SelectList(
                statusVM.Where(x => !String.IsNullOrWhiteSpace(x.descricao)).OrderBy(x => x.id), "id", "descricao");

            return View(dados);
        }
        [HttpPost]
        public IActionResult Pesquisa_Ordem_Carga(DadoPesquisaViewModel dados)
        {
            var verifica = Verifica_Orde_Carga(dados);
            if (verifica == "OK")
            {
                ListaOcVmodelFiltrada = listaOrdemCarga.lista;
                return View("Lista_Ordem_Carga", listaOrdemCarga.lista);
            }
            else
            {
                dados.Lista = new SelectList(
                listaOrdemCarga.lista_status.
                Where(x => !String.IsNullOrWhiteSpace(x.descricao)).OrderBy(x => x.id), "id", "descricao");

                dados.Erro = verifica;
                return View("Pesquisa_Ordem_Carga", dados);
            }


        }

        public string Verifica_Orde_Carga(DadoPesquisaViewModel dados)
        {
            string verifica = "OK";
            var io = "I";
            if (dados.Id == 3)
                io = "O";

            if (String.IsNullOrWhiteSpace(dados.Dt_ini))
            {
                verifica = "Preencha a data inicial";
            }
            else if (String.IsNullOrWhiteSpace(dados.Dt_fim))
            {
                verifica = "Preencha a data final";
            }
            else if (!DateTime.TryParse(dados.Dt_ini, out var ini))
            {
                verifica = "Preencha a data inicial(ano-mês-dia)";

            }
            else if (!DateTime.TryParse(dados.Dt_fim, out var fim))
            {
                verifica = "Preencha a data final(ano-mês-dia)";
            }
            else if (dados.Id == 0)
            {
                verifica = "Selecione o status";
            }
            else if (fim < ini)
            {
                verifica = "A data final não pode ser antes da inicial";
            }
            else if (fim > DateTime.Now)
            {
                verifica = "A data final não pode ser depois de hoje";
            }
            else
            {


                var ordemDTO = _ordem.Lista_Orde_Carga(dados.Dt_ini, dados.Dt_fim, dados.Id, 0,io);
                var ordemVM = TypeAdapter.Adapt<List<Ordem_CargaDTO>, List<Ordem_CargaViewModel>>(ordemDTO);

                if (ordemVM.Count == 0)
                {
                    verifica = "Não existe ordem de carga " + listaOrdemCarga.lista_status[dados.Id - 1].descricao + " nesse período";
                }
                else
                {
                    listaOrdemCarga.lista = ordemVM;
                }
            }

            return verifica;
        }

        public IActionResult Lista_Ordem_Carga()
        {
            return View();
        }

        public IActionResult ExportarExcel_Ordem_Carga()
        {

            String arq = "Relatorio_ordem_carga_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
            string caminho = _env.WebRootPath + @"\Download\" + arq;
            //string caminho = AppDomain.CurrentDomain.BaseDirectory +@"wwwroot\Download\" + arq;


            try
            {

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage excel = new ExcelPackage();

                var planilha = excel.Workbook.Worksheets.Add("Ordem de carga" + DateTime.Now.ToString());

                planilha.Row(1).Style.Font.Bold = true;

                planilha.Cells[1, 1].Value = "Ordem Carga";
                planilha.Cells[1, 2].Value = "Transportadora";
                planilha.Cells[1, 3].Value = "Motorista";
                planilha.Cells[1, 4].Value = "Descrição";
                planilha.Cells[1, 5].Value = "Placa";
                planilha.Cells[1, 6].Value = "Status envio";
                planilha.Cells[1, 7].Value = "Conferente";
                planilha.Cells[1, 8].Value = "Notas";
                planilha.Cells[1, 9].Value = "Clientes";
                planilha.Cells[1, 10].Value = "Destinos";

                planilha.Cells[1, 11].Value = "Data agenda";
                planilha.Cells[1, 12].Value = "Nr coleta";
                planilha.Cells[1, 13].Value = "Volumes";


                int cont = 2;

                foreach (var ordem in listaOrdemCarga.lista)
                {
                    planilha.Cells[cont, 1].Value = ordem.id_ord_carga;
                    planilha.Cells[cont, 2].Value = ordem.nome_transp;
                    planilha.Cells[cont, 3].Value = ordem.motorista;
                    planilha.Cells[cont, 4].Value = ordem.descricao_v;
                    planilha.Cells[cont, 5].Value = ordem.placas;
                    planilha.Cells[cont, 6].Value = ordem.stat_ev;
                    planilha.Cells[cont, 7].Value = ordem.conferente;
                    planilha.Cells[cont, 8].Value = ordem.notas;
                    planilha.Cells[cont, 9].Value = ordem.clientes;
                    planilha.Cells[cont, 10].Value = ordem.destinos;

                    planilha.Cells[cont, 11].Value = ordem.data_agenda;
                    planilha.Cells[cont, 12].Value = ordem.nr_coleta;
                    planilha.Cells[cont, 13].Value = ordem.volumes;

                    cont++;
                }

                for (cont = 1; cont <= 21; cont++)
                    planilha.Column(cont).AutoFit();

                FileStream arquivo = System.IO.File.Create(caminho);
                arquivo.Close();

                System.IO.File.WriteAllBytes(caminho, excel.GetAsByteArray());

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
        public IActionResult ExportarExcel_Ordem_Carga(String link)
        {
            string caminho = _env.WebRootPath + @"\Download\" + link;

            var fs = System.IO.File.OpenRead(caminho);
            return File(fs, "application/octet-stream",
                        link);
        }

  
        public IActionResult EditarOC(Int32 id_os)
        {

            var r = " OK";

         
            if (ListaOcVmodelFiltrada!= null && ListaOcVmodelFiltrada.Count > 0)
            {


                var oc = ListaOcVmodelFiltrada.Where(x => x.id_ord_carga == id_os).FirstOrDefault();

                if (oc != null && oc.id_ord_carga>0)
                {

                    var osSelect = _oc.PesqisaOC_BLL(oc.id_ord_carga);


                    OCargaViewModel Tela = new OCargaViewModel();
                    var listaTranspDto = _transp.Lista_transportadoraBLL().OrderBy(x => x.descricao).ToList();
                    var ListaTranspVM = TypeAdapter.Adapt<IEnumerable<ListaGenericaDTO>, IEnumerable<ListaGenericaViewModel>>(listaTranspDto);
                   
                    var listaStDto = _oc.Lista_status_OcBLL().OrderBy(x => x.descricao).ToList();
                    var listStVM = TypeAdapter.Adapt<IEnumerable<ListaGenericaDTO>, IEnumerable<ListaGenericaViewModel>>(listaStDto);
                    //status selecionado
                    Tela.Stat_evento = osSelect.stat_evento;

                    //lista de transportadoras
                    Tela.Lista_transportadora = new SelectList(ListaTranspVM, "id", "descricao");

                    //transp selecionada
                    Tela.Id_transportadora = osSelect.id_transp;


                    var listaTipoVeicDto = _veiculos.Lista_tipo_veiculoBLL().OrderBy(x => x.descricao).ToList();



                    Tela.Tipo_veiculo = osSelect.tipo_veiculo;

                    var listaTipoVeicVM = TypeAdapter.Adapt<IEnumerable<ListaGenericaDTO>, IEnumerable<ListaGenericaViewModel>>(listaTipoVeicDto);
                    //lista de tipo de veiculos
                    Tela.Lista_tipo_veiculo = new SelectList(listaTipoVeicVM, "id", "descricao");

                    //lista tipo carga
                    Tela.Lista_tipo_carga = new SelectList(ListaTipoCarga(), "id", "descricao");

                    
                    
                    //lista de status
                    Tela.Lista_status_oc = new SelectList(listStVM, "id", "descricao");
                    
                    Tela.Dt_agenda = osSelect.data_agenda;







                }














                //aqui precisa pegar os dados da OS e os itens das notas

                //nfe


            }
            return null;
        }

    }
}
