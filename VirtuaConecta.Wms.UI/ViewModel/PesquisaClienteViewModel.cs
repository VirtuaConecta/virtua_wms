using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class PesquisaClienteViewModel
    {
        public String Dt_ini { get; set; }
        public String Dt_fim { get; set; }
        public Int32 Id_cliente { get; set; }
        public SelectList ListaCliente { get; set; }

        public String Erro { get; set; }
    }
}
