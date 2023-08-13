using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{/// <summary>
/// items do pedido do cliente validar os campos
/// </summary>
    public class DocumentoClienteItemDTO
    {
        public string cProd { get; set; }
        public string cEAN { get; set; }
        public string xProd { get; set; }
        public string NCM { get; set; }
        public string CFOP { get; set; }
        public string uCom { get; set; }
        public double qCom { get; set; }
        public double vUnCom { get; set; }
    }
}
