using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaDTO
{
   public class DocumentoClienteDTO
    {
        public DocumentoClienteHeadDTO HeadDocumento { get; set; }

        public List<DocumentoClienteItemDTO> ItensDocumento { get; set; }
    }
}
