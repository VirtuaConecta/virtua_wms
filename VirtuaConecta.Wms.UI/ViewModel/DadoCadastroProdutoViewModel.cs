using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class DadoCadastroProdutoViewModel
    {
        public ProdutosViewModel produto { get; set; }
        public String Erro { get; set; }

        public SelectList emblagens { get; set; }
    }
}
