﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class MunicipiosViewModel
    {
        public Int32? Id { get; set; }
        public Int32? Cod_municipio { get; set; }
        public String Municipio { get; set; }
        public String Nome_municipio { get; set; }
        public String Uf { get; set; }
        public Int32? Cod_uf { get; set; }
        public Decimal? Valor { get; set; }
        public String Obs { get; set; }
        public Int32? Rota { get; set; }
        public Int32? Tp { get; set; }
        public Decimal? Valor_seco { get; set; }
        public Int32? Cod_municipio1 { get; set; }
        //Cod composto de Indice e CodMunicipio
        public String Cod_Id { get; set; }
    }
}
