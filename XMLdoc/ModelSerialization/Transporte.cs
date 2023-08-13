using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLdoc.ModelSerialization
{
    public class Transporte
    {
        public int modFrete { get; set; }

        [XmlElement("transporta")]
        public Tranportadora transportadora { get; set; }

        [XmlElement("vol")]
        public Volumes Volumes { get; set; }
    }
}
