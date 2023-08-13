using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtuaBusiness.Varredura_pedidos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VirtuaConecta.Wms.UI.BackgroundServices
{
    public class StartVarredura:BackgroundService
    {
        private readonly IvarreduraBLL _varre;
        private ILogger<StartVarredura> _logger;



        public StartVarredura(ILogger<StartVarredura> logger,  IvarreduraBLL varre)
        {
            _varre = varre;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Inicia rotina de monitoramento");
            stoppingToken.Register(() => _logger.LogDebug("Serviço de monitoramento está parando"));
         
                _logger.LogDebug("Varrendo a pasta em background");
            Iniciar();


        }


        public  void Iniciar()
        {
          
            new Thread(() =>
            {
                
                startSwap();


            }).Start();

        }

        public  void startSwap()
        {
            _varre.Inicio();
        }
    }
}
