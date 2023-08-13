using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class ListaAgrupadaEstoqueBaixaViewModel
    {
        public String Id_pedido { get; set; }
        public List<EstoqueViewModel> ListaEstoque { get; set; }
    }
}
 