using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument
{
    public sealed class LayoutEDINotFis
    {
        public static NotFisBase ObterLayout(ModEdi tipo, String Arquivo)
        {
            NotFisBase LayoutEscolhido = null;

            if (tipo == ModEdi.NotFis1)
            {
                LayoutEscolhido = new Layout.ImportarNotFis1();
            }
            if (tipo == ModEdi.NotFis2)
            {
                LayoutEscolhido = new Layout.ImportarNotFis2();
            }

            if (tipo == ModEdi.NotFis2)
            {
                LayoutEscolhido = new Layout.ImportarNotFis3();
            }
            if (tipo == ModEdi.NotFisGMB)
            {
                LayoutEscolhido = new Layout.ImportarNotFisGMB();
            }
            LayoutEscolhido.Pedidos = LayoutEscolhido.CapturaNotFis(Arquivo);
            LayoutEscolhido.ArqNotas = LayoutEscolhido.GeraNotFis(Arquivo);

            return LayoutEscolhido;
        }


    }
}
