using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaBusiness.Arquivos.LayoutsEDI;

namespace VirtuaBusiness
{
    public class ArquivoFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ArquivoFactory(IServiceProvider ServiceProvider)
        {
            serviceProvider = ServiceProvider;
        }

        public IbaseLayout CapturaDadosArquivoFactory(String Extensao)
        {

            try
            {

                // Aqui direcionamos para os metodos extendidos para cada layout de arquivo
                switch (Extensao)
                {
                    case "TXT":
                        return (IbaseLayout)serviceProvider.GetService(typeof(EdiTxt));

                    case "XLS":
                        //      Captura = new EdiXls();
                        break;
                    case "XLSX":
                        //      Captura = new EdiXlsx();
                        break;

                    case "XML":
                        return (IbaseLayout)serviceProvider.GetService(typeof(NfeXML));


                    default:

                        break;
                }




            }
            catch (Exception ex)
            {
                var t = ex.Message;
                //throw;
            }

            return null;

        }


    }
}
