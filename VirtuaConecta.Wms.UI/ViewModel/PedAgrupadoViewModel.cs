using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class PedAgrupadoViewModel
    {
       
            public string pedidos { get; set; }
            public List<Item> items { get; set; }
      
    }
    public class Item
    {
        public string Id { get; set; }
        public Int32 Id_produto { get; set; }
        public Decimal Qtd { get; set; }
    }
}
