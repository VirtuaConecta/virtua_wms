using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class ListaConferenciaViewModel
    {
      
        public int Id_cliente { get; set; }
        public String Nome_cli { get; set; }

        public String nfs { get; set; }
        public Decimal Volumes { get; set; }

        public Decimal Pesos { get; set; }
        public String Destino { get; set; }

        public String DataNfe { get; set; }
        public List<ListaCrossDockViewModel> Itens { get; set; }


    }

    public class ListaCrossDockViewModel
    {
        public int Id_item { get; set; }
        public Decimal peso_item { get; set; }
        public String Sku { get; set; }
        public String Descricao { get; set; }
        public Decimal Qtd_item { get; set; }
        public String Cod_aux { get; set; }
        public String Ean { get; set; }
        public String Und { get; set; }
        public String Notas { get; set; }
    }
}
