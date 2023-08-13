using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLdoc.ModelSerialization
{
   public class Protocolo
    {

        [XmlElement(ElementName = "infProt")]
        public InfProto InformacoesProto { get; set; }
        public class InfProto
        {
            [XmlElement("chNFe")]
            public String chNFe { get; set; }

            [XmlElement("dhRecbto")]
            public String dhRecbto { get; set; }

            [XmlElement("nProt")]
            public String nProt { get; set; }


        }
        
    }
}
