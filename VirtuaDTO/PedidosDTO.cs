using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class PedidosDTO
    {

        public PedidoDTO PedidoHead { get; set; }
        public DestinoDTO Destino { get; set; }

        public List<PedidoItemDTO> PedidoItens { get; set; }

        public String DestinoJson { get; set; }
    }
}
