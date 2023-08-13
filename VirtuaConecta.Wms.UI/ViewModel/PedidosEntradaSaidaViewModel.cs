using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class PedidosEntradaSaidaViewModel
    {
        public String LinkDownload { get; set; }
        public IEnumerable<Ped_entrada_saidaViewModel> ListaPedidos { get; set; }
    }
}
