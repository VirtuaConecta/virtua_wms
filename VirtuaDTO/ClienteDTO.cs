using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
   public class ClienteDTO
    {
        public Int32 IdCliente { get; set; }
        public String DescricaoCliente { get; set; }
        public String NomeCliente { get; set; }
        public String Fantasia { get; set; }
        public String Endereco { get; set; }
        public String Bairro { get; set; }
        public String Cidade { get; set; }

        public String Uf { get; set; }
        public String Complemento { get; set; }
        public String NumeroEndereco { get; set; }
        public String Telefone1 { get; set; }
        public String Telefone2 { get; set; }
        public String Telefone3 { get; set; }
        public String Cep { get; set; }
        public String Cnpj { get; set; }
        public String Cpf { get; set; }
        public String  Letra { get; set; }
        public String InscEst { get; set; }
        public String InscMun { get; set; }
        public String Email { get; set; }
        public String Contato1 { get; set; }
        public Int32? IdHistorico { get; set; }
        public Int32? IdSubHistorico { get; set; }
        public String CodigoMunicipio { get; set; }
        public String ContatoXml1 { get; set; }
        public String EmailXml1 { get; set; }
        public String ContatoXml2 { get; set; }
        public String EmailXml2 { get; set; }
        public String ContatoXml3 { get; set; }
        public String EmailXml3 { get; set; }
        public String Status { get; set; }
        public String EnderecoCob { get; set; }
        public String BairroCob { get; set; }
        public String CidadeCob { get; set; }
        public String ComplementoCob { get; set; }
        public String CepCob { get; set; }
        public String Ddd { get; set; }
        public String Obs { get; set; }
        public Int32? Id_CCusto { get; set; }
        public Int32 Id_remetente { get; set; }
        public DateTime? DataCadastro { get; set; }
        public String Compl_cliente { get; set; }
        public string ListaAutoComplete { get; set; }
    }
}
