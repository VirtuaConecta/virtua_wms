using System.Collections.Generic;

namespace VirtuaBusiness.Arquivos
{
    public interface IExcelBLL
    {
        List<string> LerPlanilhaFreteGMBXls(string Arquivo);
        List<string> LerPlanilhaFreteGMBXlsx(string Arquivo);
    }
}