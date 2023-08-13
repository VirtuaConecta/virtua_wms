using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using VirtuaBusiness;
using VirtuaBusiness.Arquivos;
using VirtuaBusiness.Arquivos.LayoutsEDI;
using VirtuaBusiness.Cfop;
using VirtuaBusiness.Cliente;
using VirtuaBusiness.DashBoard;
using VirtuaBusiness.Municipio;
using VirtuaBusiness.Nfs_cdoca;
using VirtuaBusiness.Pedidos;
using VirtuaBusiness.Produto;
using VirtuaBusiness.Usuarios;
using VirtuaBusiness.Varredura_pedidos;
//using VirtuaConecta.Wms.UI.BackgroundServices;
using VirtuaRepository;
using Serilog;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using VirtuaBusiness.Perfil;
using VirtuaBusiness.Transportadora;
using VirtuaBusiness.Veiculos;
using VirtuaBusiness.Ordem_carga;
using VirtuaBusiness.Estoque;
using System;
using Microsoft.AspNetCore.Http;
using VirtuaBusiness.Posicao;

namespace VirtuaConecta.Wms.UI
{
    public class Startup
    {
        private readonly IConfiguration _config;


        public Startup(IConfiguration config)
        {
            _config = config;

        }

        //   public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
               // options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews();

            services.AddRazorPages();



            services.AddTransient<IusuarioBLL, UsuarioBLL>();
            services.AddTransient<IcfopBLL, CfopBLL>();
            services.AddTransient<IpedidoBLL, PedidoBll>();
            services.AddTransient<IclienteBLL, ClienteBLL>();
            services.AddTransient<IperfilBLL, PerfilBLL>();
            services.AddTransient<IdashBoardBLL, DashBoardBLL>();
            services.AddTransient<IDataConnection, MysqlConnections>();
            services.AddTransient<ImunicipioBLL, MunicipioBLL>();
            services.AddTransient<IvarreduraBLL, VarreduraBusiness>();
            services.AddTransient<IprodutoBLL, ProdutoBLL>();
            services.AddTransient<InfsCdoca, NfsBLL>();
            services.AddTransient<IRestDAL, RestRepository>();
            services.AddTransient<IExcelBLL, ExcelBLL>();
            services.AddTransient<IArquivoFreteGMB, ArquivoFreteGMB>();
            services.AddTransient<ITransportadoraBLL, TransportadoraBLL>();
            services.AddTransient<IVeiculosBLL, VeiculosBLL>();
            services.AddTransient<IOrdem_cargaBLL, Ordem_cargaBLL>();
            services.AddTransient<IEstoqueBll, EstoqueBLL>();
            services.AddTransient<ArquivoFactory>();
            services.AddTransient<IPosicaoBLL, PosicaoBLL>();
     
            services.AddTransient<EdiTxt>()
              .AddTransient<IbaseLayout, EdiTxt>(s => s.GetService<EdiTxt>());
            services.AddTransient<NfeXML>()
                         .AddTransient<IbaseLayout, NfeXML>(s => s.GetService<NfeXML>());
            //  services.AddHostedService<StartVarredura>();





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
               // app.UseExceptionHandler("/Home/Error");

                app.UseDeveloperExceptionPage();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseNotyf();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
