using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument
{
    public class Edi
    {
        public List<String> DecodificaEdi(ModEdi Modelo, String CaminhoArquivo)
        {
            List<String> retorno = null;


            if (Modelo.ToString().Contains("NotFis"))
            {

                var Pedidos = LayoutEDINotFis.ObterLayout(Modelo, CaminhoArquivo);

                if (Pedidos != null)
                {
                    var listp = new List<String>();
                    listp.Add(JsonConvert.SerializeObject(Pedidos.Pedidos));
                    listp.Add(Pedidos.ArqNotas);
                    retorno = listp;
                }


            }
            else if (Modelo.ToString().Contains("Conemb"))
            {


            }
            else if (Modelo.ToString().Contains("DocCob"))
            {

            }
            return retorno;

        }

    }

}
