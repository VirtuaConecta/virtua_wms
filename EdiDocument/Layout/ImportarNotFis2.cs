using EdiDocument.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument.Layout
{
    public class ImportarNotFis2 : NotFisBase
    {
        public override List<Nfe> CapturaNotFis(String Arquivo)
        {
            var resp = new List<Nfe>();
            //

            return resp;
        }

        public override string GeraNotFis(string NomeDoc)
        {
            throw new NotImplementedException();
        }
    }
}
