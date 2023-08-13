using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Cfop
{

    public class CfopBLL:IcfopBLL
    {
        IDataConnection _db;
        public CfopBLL(IDataConnection db)
        {
            _db = db;
        }
        public IEnumerable<CfopDTO> Listar_CfopBLL()
        {
            return _db.Listar_Cfop();
        }
    }
}
