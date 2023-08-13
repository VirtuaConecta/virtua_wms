using EdiDocument;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaBusiness.Arquivos
{
    public class Edi
    {


        public List<String> DecodificaEdi(EdiDocument.ModEdi Modelo, String CaminhoArquivo)
        {
var retorno = new List<String>();


            if (Modelo.ToString().Contains("NotFis"))
            {

                var Pedidos = LayoutEDINotFis.ObterLayout(Modelo, CaminhoArquivo);

                if (Pedidos != null)
                {
                    var ped = Pedidos.Pedidos != null ? JsonConvert.SerializeObject(Pedidos.Pedidos) : "";
                    retorno.Add(ped);
                    retorno.Add(Pedidos.ArqNotas);
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
