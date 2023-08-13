using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument.DTO
{
    public class Nfe
    {
        
        public String Empresa { get; set; }//Empresa tranportadora
        public int Id_cliente { get; set; }
        public int Id_rementente { get; set; }
        public String Nf_Wms { get; set; }
        public Decimal Valor_frete { get; set; }
        public String Num_remessa { get; set; } // nr fornecido na lista de frete
        public String Tipo_remessa { get; set; } // Entrada=I Saida=O
       
        public NfeHead Head { get; set; }
        public List<NfeItens> ListaItens { get; set; }

    }
}
