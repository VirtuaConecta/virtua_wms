using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class DadoPesquisaViewModel
    {
        public Int32 Id { get; set; }
        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "Inserir Dt Inicial")]
        public String Dt_ini { get; set; }
        [Required(ErrorMessage = "Inserir Dt Final")]
        [Display(Name = "Data Final")]
        public String Dt_fim { get; set; }
        public SelectList Lista { get; set; }

        public string Erro { get; set; }

        public String Id_str { get; set; }
    }
}
