using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLdoc.ModelSerialization
{
    public class Destinatario
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string xNome { get; set; }
        public string IE { get; set; }
        [XmlElement("enderDest")]
        public Endereco Endereco { get; set; }
        public string email { get; set; }



    }
}
