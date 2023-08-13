using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtuaConecta.Wms.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            //Implementa o Serilog e direciona para appsettings.json
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
             //  CreateHostBuilder(args).Build().Run();

            try
            {
                Log.Information("O Wms Web foi iniciado");

                CreateHostBuilder(args)
                      .UseWindowsService()
                    .Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Task.Delay(20000).Wait();

                Log.Fatal(ex, "A Aplicação não conseguiu iniciar");
                return;
            }
            finally
            {//descarrega as msg pendentes antes de fechar a aplicação
                Log.CloseAndFlush();
            }


        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });

    }
}
