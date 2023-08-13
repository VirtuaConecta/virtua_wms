using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
   public class nfeCdoca:NfeHead
    {
        public Int32 Id { get; set; }
        public Int32 Id_cliente { get; set; }
        public String Razao { get; set; }
        public String Nf_wms { get; set; }
        public Int32 St_nfe { get; set; }
        public Int32 Ord_carga { get; set; }

        public DateTime Data_entrada { get; set; }
        public Int32 Nr_cte { get; set; }
        public Int32 Id_remetente { get; set; }
    }
}
