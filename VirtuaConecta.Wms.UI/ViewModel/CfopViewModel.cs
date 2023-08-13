using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class CfopViewModel
    {
        public Int32? Id { get; set; }
        public String Cfop_txt { get; set; }
        public String Descricao { get; set; }
        public String Tipo { get; set; }
        public Decimal? Alicota { get; set; }
        public Int32? Cfop_int { get; set; }
    }
}
