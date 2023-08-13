using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
    public class DestinoDTO
    {
        public String nome { get; set; }
        public String codigo { get; set; }
        public String tipo_pessoa { get; set; }
        public String cpf_cnpj { get; set; }
        public String ie { get; set; }
        public String rg { get; set; }
        public String endereco { get; set; }
        public String numero { get; set; }
        public String complemento { get; set; }
        public String bairro { get; set; }
        public String cidade { get; set; }
        public String uf { get; set; }
        public String fone { get; set; }
        public String email { get; set; }
        public String cep { get; set; }
        public Int32 Id_destino { get; set; }
        public Int32 Cod_municipio { get; set; }
        public Int32 id_cidade { get; set; }


    }
}
