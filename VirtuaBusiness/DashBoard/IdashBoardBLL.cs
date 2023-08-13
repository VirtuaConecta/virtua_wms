using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.DashBoard
{
    public interface IdashBoardBLL
    {
        IEnumerable<ChartDTO> Dash_pedidosLiberados();
        IEnumerable<ChartDTO> Dash_pedidosLiberadosPesos();
        IEnumerable<ChartDTO> Dash_pedidosLiberadosPeriodo();
        IEnumerable<ChartDTO> Dash_Pedido_operador(int dia);
        IEnumerable<ChartDTO> Dash_Volume_Cliente(int dia);
    }
}
