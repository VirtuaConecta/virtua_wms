using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLdoc.ModelSerialization
{
    public class InfoAdic
    {
        [XmlElement("infCpl")]
        public String infCpl { get; set; }

    }
}
