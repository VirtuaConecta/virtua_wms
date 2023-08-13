using FastMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtuaBusiness.DashBoard;
using VirtuaBusiness.Pedidos;
using VirtuaConecta.Wms.UI.ViewModel;
using VirtuaDTO;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class DashboadController : Controller
    {
        IdashBoardBLL _dash;
        IConfiguration _conf;
        public DashboadController(IdashBoardBLL dash, IConfiguration configuration)
        {
            _conf = configuration;
            _dash = dash;
        }
        public IActionResult DashGeral()
        {

            var char1 = _dash.Dash_pedidosLiberados();
            var char1VM = TypeAdapter.Adapt<IEnumerable<ChartDTO>, IEnumerable<ChartViewModel>>(char1);

            var char2 = _dash.Dash_pedidosLiberadosPeriodo();
            var char2VM = TypeAdapter.Adapt<IEnumerable<ChartDTO>, IEnumerable<ChartViewModel>>(char2);


            var char3 = _dash.Dash_pedidosLiberadosPesos();
            var char3VM = TypeAdapter.Adapt<IEnumerable<ChartDTO>, IEnumerable<ChartViewModel>>(char3);

            var periodo = _conf.GetSection("periodo_dash").Value;
            var char4 = _dash.Dash_Pedido_operador(Convert.ToInt32(periodo));
            var char4VM = TypeAdapter.Adapt<IEnumerable<ChartDTO>, IEnumerable<ChartViewModel>>(char4);

            CharsViewModel chars1 = new CharsViewModel
            {
             charts= char1VM
            };

            CharsViewModel chars2 = new CharsViewModel
            {
             charts = char2VM
            };

            CharsViewModel chars3 = new CharsViewModel
            {
                charts = char3VM
            };

            CharsViewModel chars4 = new CharsViewModel
            {
                charts = char4VM
            };

            List<CharsViewModel> chars_total= new List<CharsViewModel>();
            chars_total.Add(chars1);
            chars_total.Add(chars2);
            chars_total.Add(chars3);
            chars_total.Add(chars4);

            return View(chars_total);
        }
        public IActionResult PedidoAtualDetalhe(String Dia)
        {
            List< ChartViewModel > charts = new List<ChartViewModel>();

            for (int i = 0; i < 5; i++)
            {
                var chart1 = new ChartViewModel
                {
                    Item = "Impresa 1",
                    Valor = 1
                };

                var chart2 = new ChartViewModel
                {
                    Item = "Impresa 2",
                    Valor = 2
                };

                var chart3 = new ChartViewModel
                {
                    Item = "Impresa 3",
                    Valor = 3
                };

                charts.Add(chart1);
                charts.Add(chart2);
                charts.Add(chart3); 
            }

            ViewBag.link = Dia;
            return View("PedidoAtualDetalhe",charts);
        }
        public IActionResult DashDetalhe()
        {
            var periodo = _conf.GetSection("periodo_dash").Value;

            var char1 = _dash.Dash_Volume_Cliente(Convert.ToInt32(periodo));
            var char1VM = TypeAdapter.Adapt<IEnumerable<ChartDTO>, IEnumerable<ChartViewModel>>(char1);

            CharsViewModel chars1 = new CharsViewModel
            {
                charts = char1VM
            };

          

            List<CharsViewModel> chars_total = new List<CharsViewModel>();
            chars_total.Add(chars1);
          

            return View(chars_total);
        }
     }
}
