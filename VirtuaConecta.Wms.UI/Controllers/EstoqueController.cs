using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;
using FastMapper;
using VirtuaDTO;
using VirtuaConecta.Wms.UI.ViewModel;
using VirtuaBusiness.Estoque;
using VirtuaBusiness.Pedidos;
using VirtuaBusiness.Posicao;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IEstoqueBll estoque;
        private readonly IpedidoBLL pedidos;
        private readonly IPosicaoBLL _posicao;

        public EstoqueController( INotyfService notyf,IEstoqueBll estoque, IpedidoBLL pedidos, IPosicaoBLL posicao)
        {
            _notyf = notyf;
            this.estoque = estoque;
            this.pedidos = pedidos;
            _posicao = posicao;
        }


       public string validaPosicao(String posicao)
        {
           
            var dadosTela = JsonConvert.DeserializeObject<PosEntradaViewModel>(posicao);
            
            String resp = "falso";
            if (dadosTela.Posicao == null || dadosTela.Posicao == "01000000")
            {
                resp = "posicao inválida!";
            }
            else if (Information.IsDate(dadosTela.Validade)==false)
            {
                resp = "Data Validade inválida!";
            }

            else if (String.IsNullOrEmpty(dadosTela.Lote))
            {
                resp = "Lote inválido!";
            }
            else if (dadosTela.Qtd_ent == 0 )
            {
                resp = "Qtd  entrada inválida!";
            }
            else if (dadosTela.Qtd_ent > dadosTela.Qtd_ped)
            {
                resp = "Qtd Entrada maior que a qtd do pedido!";
            }
            else
            {
                try
                {
                    var posPesquisada = _posicao.listar_posicaoBll().Where(x => x.cod_posicao == dadosTela.Posicao).FirstOrDefault();
                    if (posPesquisada != null && posPesquisada.id_posicao > 0)
                    {


                        var volNecessario = (dadosTela.Vol_pos / dadosTela.Qtd_pos) * dadosTela.Qtd_ent;

                        if (Convert.ToDecimal(posPesquisada.volume_disponivel) >= volNecessario)
                        {
                            resp = "OK";
                        }
                        else
                        {
                            resp = "Posição não suporta a qtd de entrada!";
                        }



                    }
                    else
                    {
                        resp = "Posição não encontrada!";
                    }
                }
                catch (Exception ex)
                {

                    resp = $"Erro na captura dos dados! {ex.Message}";
                }
            }
            return resp;
        }

        public IActionResult ListaLoteValidade()
        {
            List<EstoqueViewModel> ListaVm = new List<EstoqueViewModel>();
            var pedidosPedentes = pedidos.ListaPedidoPendente("I", "N", "N");
            var EstoqeuAentrar = estoque.ListaEstoqueEstagioBll(null, null, "01000000", null, "NAO", "NAO").OrderBy(x=>x.Nr_nf).OrderBy(y=>y.Sku);

            
 
             ListaVm = TypeAdapter.Adapt<IEnumerable<EstoqueDTO> , List<EstoqueViewModel>>(EstoqeuAentrar);

            transferScreen.ListaestagioVM = ListaVm;

            return View(ListaVm);
        }

        public IEnumerable<EstoqueViewModel> ListaEstagio(String Estagio)
        {
            //filtra a lista de Estoque no estagio de posição escolhido
            string vPosicaoStatus = Estagio;
            string vPosOcultas = "NAO";
            string vConsideraSaldo = "NAO";


            var listaDTO = estoque.ListaEstoqueEstagioBll(null,null, vPosicaoStatus,null, vPosOcultas, vConsideraSaldo);
                //ListaEstoque(null, null, vPosicaoStatus, null, vPosOcultas, vConsideraSaldo);

            var ListaVM = TypeAdapter.Adapt<IEnumerable<EstoqueDTO>, IEnumerable<EstoqueViewModel>>(listaDTO);
            return ListaVM;
        }



        public IActionResult InsereLoteValidade(Int32 ID, Decimal ID2)
        {
            EstoqueViewModel listaTela = transferScreen.ListaestagioVM.Where(x => x.id == ID).Single();

            //listaTela.id = ID;
            //listaTela.Qtd_ped = ID2;
            return View(listaTela);
        }

        [HttpPost]
        public IActionResult InsereLoteValidade(EstoqueViewModel Item)
        {

            return RedirectToAction("ListaLoteValidade");
        }
        }
}

public class transferScreen
{
    public static List<EstoqueViewModel> ListaestagioVM { get; set; }
}
