using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace VirtuaConecta.Wms.UI
{
    public class Worker
    {
        private readonly IConfiguration _iconfig;

        public Worker(IConfiguration config)
        {
            _iconfig = config;
        }
        /// <summary>
        /// Executa a leitura de arquivo
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DoWork(CancellationToken cancellationToken)
        {
            //liberacao.LiberaLeitura = true;

            while (!cancellationToken.IsCancellationRequested)
            {
                // fab.testaNurSerie();

                var time = Convert.ToInt32(_iconfig.GetSection("Config_Api:CicloLeituraArquivo").Value);
                //_ileitura.LerArquivosLeitura();
                //_logger.LogInformation($"Ciclo de leitura {time} - {DateTime.Now}");
                await Task.Delay(time);
            }
        }
    }
}
