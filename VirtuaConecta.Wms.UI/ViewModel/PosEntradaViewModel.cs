using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class PosEntradaViewModel
    {
        public String Posicao { get; set; }

        public Decimal Qtd_ped { get; set; }

        public Decimal Qtd_ent { get; set; }

        public Decimal Qtd_pos { get; set; }

        public Decimal Vol_pos { get; set; }

        public DateTime? Validade { get; set; }
        public String Lote { get; set; }
    }
}
