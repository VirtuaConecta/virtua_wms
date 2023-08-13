using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Posicao
{
    public class PosicaoBLL : IPosicaoBLL
    {
        private readonly ILogger<PosicaoBLL> logger;
        private readonly IConfiguration config;
        private readonly IDataConnection db;

        public PosicaoBLL(ILogger<PosicaoBLL> logger, IConfiguration config, IDataConnection db)
        {
            this.logger = logger;
            this.config = config;
            this.db = db;
        }

        public IEnumerable<PosicaoDTO> listar_posicaoBll()
        {

            return db.Listar_Posicoes();

        }

    }
}
