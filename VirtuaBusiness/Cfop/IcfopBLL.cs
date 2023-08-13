using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Cfop
{
    public interface IcfopBLL
    {
        IEnumerable<CfopDTO> Listar_CfopBLL();
    }
}
