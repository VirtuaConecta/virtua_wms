using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class DadoCadastroUsuarioViewModel
    {
        public UsuarioViewModel usuario { get; set; }
        public SelectList perfils { get; set; }
        public String Erro { get; set; }
    
        public string senha { get; set; }
    }
}
