using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument.DTO
{
    public class Linha_000
    {
        public String Nome_Emitente { get; set; }
        public String Nome_Recebedor { get; set; }
        public DateTime Data { get; set; }
        public DateTime Hora { get; set; }
    }
}
