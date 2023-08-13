using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
  public  class ComplementoEstoqueDTO
    {
               
        public Int32? Nr_Ordem { get; set; }
       
        public String Apelido { get; set; }
        public String Nome_cli { get; set; }
        public DateTime? Data_ordem { get; set; }
        public String Data_entrada { get; set; }
        public String Data_saida { get; set; }
        public String Obs { get; set; }
        public String Ped_cli { get; set; }
        public String Operador { get; set; }

        public String Nome_dest { get; set; }
        public String Tipo_ped { get; set; }
        public DateTime? Dt_registro { get; set; }

        public DateTime? Data_entrada2 { get; set; }
     
        public Int32? Id_Head_item { get; set; }
        public String Nr_serie { get; set; }
        public String Remessa { get; set; }
        public String Processado { get; set; }
        public String Cancelado { get; set; }

        public Decimal? qtd_palet { get; set; }
        public Decimal? vol_palet { get; set; }
    }
}
