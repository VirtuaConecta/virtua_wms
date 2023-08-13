using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class PedAgrupadoDTO
    {

        public string pedidos { get; set; }
        public List<Item> items { get; set; }

    }
    public class Item
    {
        public string Id { get; set; }
        public Int32 Id_produto { get; set; }
        public Decimal Qtd { get; set; }

        public Decimal Qtd_baixada { get; set; }
    }
}
