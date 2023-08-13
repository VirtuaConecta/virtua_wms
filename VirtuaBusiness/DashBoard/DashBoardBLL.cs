using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaBusiness.Pedidos;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.DashBoard
{
    public class DashBoardBLL: IdashBoardBLL
    {
        IpedidoBLL _pedido;
        IDataConnection _db;
        ILogger<DashBoardBLL> _log;
        public DashBoardBLL(IpedidoBLL pedido, IDataConnection db, ILogger<DashBoardBLL> log)
        {
            _log = log;
            _pedido = pedido;
            _db = db;
        }

        public IEnumerable<ChartDTO>  Dash_pedidosLiberados()
        {
            try
            {
                var list = _pedido.Listar_Doc_Por_StatusBLL("4");
                var max = list.Max(x => x.DataLiberadoEnvio);
                var graf = list.Where(x => x.DataLiberadoEnvio >= max.AddDays(-30)).ToList().OrderBy(x => x.DataLiberadoEnvio);


                var query = graf.GroupBy(x => x.DataLiberadoEnvio.ToString("dd/MM/yyyy"))
                  .Select(gp => new ChartDTO
                  {
                      Item = gp.Key,
                      Valor = gp.Select(z => z.Id).Count()
                  }).ToList();

                var menor = query.OrderBy(x => x.Valor).FirstOrDefault();
                var maior = query.OrderByDescending(x => x.Valor).FirstOrDefault();

                query.Add(menor);
                query.Add(maior);

                return query;
            }
            catch (Exception ex)
            {

                _log.LogError($"Erro em Dash_pedidosLiberados {ex.Message}");
            }
            return null;
        }

        public IEnumerable<ChartDTO> Dash_pedidosLiberadosPesos()
        {
            try
            {
                var list = _pedido.Listar_Doc_Por_StatusBLL("4");
                var max = list.Max(x => x.DataLiberadoEnvio);
                var graf = list.Where(x => x.DataLiberadoEnvio >= max.AddDays(-30)).ToList().OrderBy(x => x.DataLiberadoEnvio);


                var query = graf.GroupBy(x => x.DataLiberadoEnvio.ToString("dd/MM/yyyy"))
                  .Select(gp => new ChartDTO
                  {
                      Item = gp.Key,
                      Valor = gp.Sum(x => x.PesoBrt)
                  }).ToList();

                var menor = query.OrderBy(x => x.Valor).FirstOrDefault();
                var maior = query.OrderByDescending(x => x.Valor).FirstOrDefault();

                query.Add(menor);
                query.Add(maior);

                return query;
            }
            catch (Exception ex)
            {

                _log.LogError($"Erro em Dash_pedidosLiberadosPesos {ex.Message}");
            }
            return null;
        }

        public IEnumerable<ChartDTO> Dash_pedidosLiberadosPeriodo()
        {
            try
            {
                var list = _pedido.Listar_Doc_Por_StatusBLL("4");
                var max = list.Max(x => x.DataLiberadoEnvio);
                var graf = list.Where(x => x.DataLiberadoEnvio >= max.AddDays(-30)).ToList().OrderBy(x => x.DataLiberadoEnvio);


                var query = graf.GroupBy(x => x.DataLiberadoEnvio.ToString("dd/MM/yyyy"))
                  .Select(gp => new ChartDTO
                  {
                      Item = gp.Key,
                      Valor = Math.Round(gp.Average(x => Convert.ToDecimal(x.DataLiberadoEnvio.Subtract(x.DataEntrada).TotalDays)), 2)
                  }).ToList();

                var menor = query.OrderBy(x => x.Valor).FirstOrDefault();
                var maior = query.OrderByDescending(x => x.Valor).FirstOrDefault();

                query.Add(menor);
                query.Add(maior);

                return query;
            }
            catch (Exception ex)
            {

                _log.LogError($"Erro em Dash_pedidosLiberadosPeriodo {ex.Message}");
            }
            return null;
        }

        public IEnumerable<ChartDTO> Dash_Pedido_operador(int dia)
        {
            return _db.Dash_Pedido_operador(dia);
        }
        public IEnumerable<ChartDTO> Dash_Volume_Cliente(int dia)
        {
            return _db.Dash_Volume_Cliente(dia);
        }
      

    }
}
