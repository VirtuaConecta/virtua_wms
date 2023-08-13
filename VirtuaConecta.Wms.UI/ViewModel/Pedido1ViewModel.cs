using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class Pedido1ViewModel
    {

        [Required(ErrorMessage = "Cliente")]
        [Display(Name = "Cliente:")]
        public Int32? Id_cliente { get; set; }

        public Int32 Indice { get; set; }

        [Required(ErrorMessage = "Cliente")]
        [Display(Name = "Cliente:")]
        public String Id_cliente_txt { get; set; }

        [Required(ErrorMessage = "Cfop")]
        [Display(Name = "Cfop:")]
        public String Cfop { get; set; }

        public Int32? Id_remetente { get; set; }

        [Required(ErrorMessage = "Nr do Pedido")]
        [Display(Name = "Nr Pedido:")]
        public String Nr_doc_origem { get; set; }

        [Display(Name = "Nr Pedido:")]
        public String Nr_documento { get; set; }


        [Required(ErrorMessage = "Série")]
        [Display(Name = "Serie:")]
        public String Serie_doc { get; set; }

        [Required(ErrorMessage = "Destino")]
        [Display(Name = "Destino:")]
        public String Nome_dest { get; set; }
        public String Cpf_cnpj_cliente { get; set; }

        [Required(ErrorMessage = "Cpf")]
        //[RegularExpression(@"^[0-9]{1,14}?$", ErrorMessage = "Somente numeros Max 14")]
        //[MaxLength(15)]
        [Display(Name = "Cnpj Dest:")]
        public String Cnpj_dest { get; set; }


        [Required(ErrorMessage = "IE - caso não tenha Inserir ISENTO")]
        [Display(Name = "IE Dest:")]
     

        public String IE_dest { get; set; }

        [Required(ErrorMessage = "End Dest")]
        [Display(Name = "End Dest:")]
        public String End_dest { get; set; }

        [Required(ErrorMessage = "Nr Dest")]
        [Display(Name = "Nr Dest:")]
        public String Nr_destino { get; set; }

        [Required(ErrorMessage = "Bairro")]
        [Display(Name = "Bairro:")]
        public String Bairro_dest { get; set; }

       [Required(ErrorMessage = "Cep")]
     //   [RegularExpression(@"^[0-9]{1,8}?$", ErrorMessage = "Somente numeros Max 8")]
       // [StringLength(8)]
        [Display(Name = "Cep Dest:")]
        public String Cep_dest { get; set; }

  


        [Required(ErrorMessage = "Dt Emissao")]
        [Display(Name = "Dt Emiss:")]
      //  [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dt_emissao { get; set; }


        [Required(ErrorMessage = "Dt Emissao")]
        [Display(Name = "Dt Emiss:")]
        public String Dt_emissao_txt { get; set; }

        [Required(ErrorMessage = "Dt Entrada")]
        [Display(Name = "Dt Ent:")]
        public DateTime? Dt_entrada { get; set; }

        [Required(ErrorMessage = "Remessa")]
        [Display(Name = "Remessa:")]
        public String Remessa { get; set; }



        [Required(ErrorMessage = "Tipo do Pedido")]
        [Display(Name = "Tipo Pedido:")]
        public String tipo_doc { get; set; }

       // [Required(ErrorMessage = "Peso Brt")]
        [Display(Name = "Peso Brt:")]
        public Decimal? Peso_brt { get; set; }
      //  [Required(ErrorMessage = "Peso Brt")]

        //[RegularExpression(@"^[0-9]{1,10}([,][0-9]{1,5})?$", ErrorMessage = "Nr Invalido")]
        [Display(Name = "Peso Brt:")]
        public String Peso_brt_txt { get; set; }


      //  [Required(ErrorMessage = "Peso Liq")]
        [Display(Name = "Peso Liq:")]
        public Decimal? Peso_liq { get; set; }

   //     [Required(ErrorMessage = "Peso Liq")]
       // [RegularExpression(@"^[0-9]{1,10}([,][0-9]{1,5})?$", ErrorMessage = "Nr Invalido")]
        [Display(Name = "Peso Liq:")]
        public String Peso_liq_txt { get; set; }


  //      [Required(ErrorMessage = "Volumes")]
      //  [RegularExpression(@"^[0-9]{1,10}([,][0-9]{1,5})?$", ErrorMessage = "Nr Invalido")]
        [Display(Name = "Volumes:")]
        public Int32? Nr_volumes { get; set; }


     //   [Required(ErrorMessage = "Volumes")]
        [Display(Name = "Volumes:")]
        public String Nr_volumes_txt { get; set; }

   //     [Required(ErrorMessage = "Total Nf")]
        [Display(Name = "Total Nf:")]
        public Decimal? Total_nf { get; set; }


     //   [Required(ErrorMessage = "Total Nf")]
 //       [RegularExpression(@"^[0-9]{1,10}([,][0-9]{1,5})?$", ErrorMessage = "Nr Invalido")]
        [Display(Name = "Total Nf:")]
        public String Total_nf_txt { get; set; }

        //vamos salvar o cod do municipio que traz a cidade e o estado
        [Required(ErrorMessage = "Municipio")]
        [Display(Name = "Municipio:")]
        public Int32? Cod_municipio { get; set; }

        [Required(ErrorMessage = "Municipio")]
        [Display(Name = "Municipio:")]
        public String Cod_municipio_txt { get; set; }

        public SelectList Lista_cfop { get; set; }
        public SelectList Lista_cidade { get; set; }
        public SelectList Lista_remessa { get; set; }
        public SelectList Lista_tipo_doc { get; set; }
        public SelectList Lista_Clientes { get; set; }
        public SelectList Lista_Remetentes { get; set; }

        [Required(ErrorMessage = "Chave")]
        [Display(Name = "Chave Nf:")]
        public String Chave { get; set; }
        [Required(ErrorMessage = "Fis_Jur")]
        public String Fis_Jur { get; set; }
        public String Dados_adicionais { get; set; }
        public String Operador { get; set; }
        public String Nf_saida { get; set; }
        public String Serie_Nf_saida { get; set; }
        public String Nf_ok { get; set; }
        public DateTime? Dt_emissao_nf_saida { get; set; }
        public DateTime? Dt_reg { get; set; }
        public String Especie { get; set; }
        public DateTime? Dt_processado { get; set; }
        public String Ip { get; set; }
        public String Processado { get; set; }

        //Itens

        public String Sku { get; set; }
        public Decimal? Qtd { get; set; }
        public Decimal? P_unit { get; set; }
        public String NomeCliente { get; set; }
        public String Desc_Sku { get; set; }
        public Int32? Ncm { get; set; }
        public Int32? Cst { get; set; }
        public List<Pedido1ItensViewModel> ListaItensPedido { get; set; }


    }
}
