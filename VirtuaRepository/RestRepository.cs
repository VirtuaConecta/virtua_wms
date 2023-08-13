using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaRepository
{
    public class RestRepository:IRestDAL
    {
        private readonly IConfiguration _config;
        private ILogger<RestRepository> _logger;
        public RestRepository(ILogger<RestRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config= config;
        }

        public EnderecoCepDTO PesquisarCep(string cep)
        {
            //Pesquisa no viacep

            var end = new EnderecoCepDTO();
            try
            {
             var   EnderecoURL = _config.GetSection("UrlsRest:viaCep").Value;


                string NovoEnderecoURL = string.Format(EnderecoURL, cep);
                WebClient wc = new WebClient();
                string conteudo = wc.DownloadString(NovoEnderecoURL);
                end = JsonConvert.DeserializeObject<EnderecoCepDTO>(conteudo);

            }
            catch (Exception ex)
            {


                _logger.LogError(ex.Message,"Erro em PesquisarCep");
            }
            if (end.cep == null) return null;
            return end;
        }


    }
}
