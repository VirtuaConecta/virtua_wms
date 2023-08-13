using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Virtua.Utilities;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class UsuarioViewModel
    {
        public Int32 Codigo { get; set; }

        //[StringLength(30, MinimumLength = 3, ErrorMessage = "O nome deve ter de 3 a 30 letras")]
        //[Required(ErrorMessage = "Informe o Campo Nome")]
        //[Display(Name = "Nome")]
        public String NomeUsuario { get; set; }

        //[Required(ErrorMessage = "Informe o Login")]
        //[StringLength(50)]
        public String Login { get; set; }

        //[Required(ErrorMessage = "Informe um Códigode Perfil")]
        //[Display(Name = "Perfil")]
        public Int32 Cod_perfil { get; set; }

        //[Required(ErrorMessage = "Informe a Área")]
        public String Area { get; set; }


        //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        //                  ErrorMessage = "E-Mail Inválido")]
        //[Required(ErrorMessage = "Informe o Campo E-Mail")]

        public String Email { get; set; }
        //[Required(ErrorMessage = "Informe um telefone")]
        //[Display(Name = "Tel")]
        public String Telefone { get; set; }

        //[Required(ErrorMessage = "Informe a Senha")]
        //[StringLength(100, MinimumLength = 5)]
        public String Senha { get; set; }
        //[Display(Name = "Obs")]
        public String Observacao { get; set; }

       // [Required(ErrorMessage = "Informe um Cargo")]
        public String Cargo { get; set; }
        //[Required(ErrorMessage = "Informe o Status")]
        //[Display(Name = "Bloq")]
        public String Bloqueado { get; set; }


        //[Required(ErrorMessage = "Informe um Cpf")]
        //[Cpf(ErrorMessage = "O Cpf é inválido")]
        public String Cpf { get; set; }



        //Nas telas de Cadastrar e Editar temos que
        //Montar uma lista de Pefil(ComboBox, DropDownList)
        //Temos que criar um Campo pra Receber a Lista de Perfil
        //A classe SELECTLIST armazena os dados do DROPDOWN
        public SelectList ListaPerfil { get; set; }

        public String Acao { get; set; }






    }
}
