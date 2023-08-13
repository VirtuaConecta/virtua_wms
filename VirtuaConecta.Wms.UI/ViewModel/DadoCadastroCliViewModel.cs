using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class DadoCadastroCliViewModel
    {
       public List<ClienteViewModel> listaCli { get; set; }
       public ClienteViewModel cliente { get; set; }
        public String Erro { get; set; }
    }
}
