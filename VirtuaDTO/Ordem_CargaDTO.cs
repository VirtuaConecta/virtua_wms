using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class Ordem_CargaDTO
    {
        public int id_ord_carga { get; set; }
        public String nome_transp { get; set; }
        public String motorista { get; set; }
        public String descricao_v { get; set; }
        public String placas { get; set; }
        public DateTime data_agenda { get; set; }
        public String nr_coleta { get; set; }
        public String stat_ev { get; set; }
        public String conferente { get; set; }
        public String volumes { get; set; }
        public String clientes { get; set; }
        public String destinos { get; set; }
        public String notas { get; set; }
        public int id_transp { get; set; }
        public int stat_evento { get; set; }
        public DateTime hora_fim { get; set; }
        public int NrDePallets { get; set; }
        public int tipo_veiculo { get; set; }



    }
}
