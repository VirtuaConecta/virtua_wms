using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaRepository
{
   public interface IRestDAL
    {
        EnderecoCepDTO PesquisarCep(string cep);
    }
}
