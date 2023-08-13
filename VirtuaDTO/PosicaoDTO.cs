using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class PosicaoDTO
    {
        public Int32 id_posicao { get; set; }
        public Double vol_usado { get; set; }
        public Double volume_posicao { get; set; }
        public String cod_posicao { get; set; }
        public Int32 bloq_posicao { get; set; }
        public String obs_posicao { get; set; }
        public Int32 ativo { get; set; }
        public Int32 reservado { get; set; }

        public Double volume_disponivel { get; set; }
    }
}
