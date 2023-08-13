using FastMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VirtuaBusiness.Perfil;
using VirtuaBusiness.Usuarios;
using VirtuaConecta.Wms.UI.ViewModel;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IusuarioBLL _usuario;
        private IperfilBLL _perfil;
        private UsuarioViewModel _usuariovw;
        public HomeController(ILogger<HomeController> logger, IDataConnection db, IusuarioBLL usuario)
        {
            _logger = logger;
            _usuario = usuario;
            _perfil = new PerfilBLL(db);
            _usuariovw = new UsuarioViewModel();
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(UsuarioViewModel dadosTela)
        {


            //Validamos o Usuario
            var validaUser = _usuario.AutenticaUsr(dadosTela.Login, dadosTela.Senha);



            if (validaUser == true)
            {

                //Se tudo ok Vai para a tela de abertura se não retorna
                //     Session["UsrLogado"] = dadosTela.Login;

                HttpContext.Session.SetString("UsrLogado", dadosTela.Login);
                //  return View();
                //FormsAuthentication.SetAuthCookie(dadosTela.Login, false);
                Global.Dados_user = TypeAdapter.Adapt<UsuarioViewModel, UsuarioDTO>(dadosTela);
                return RedirectToAction("Inicio", "Main");
            }
            else
            {
                ModelState.AddModelError("Hacker", "Usuario e Senha Não encontrados!");
                return View();
            }

        }

        public ActionResult CadastrarUsr()
        {

            //Ao carregar a tela incluimos no viewmodel da tela a lista de perfil capturada na Controller(<-Business<-Repositorio<-DTO<-DB)
            var perfilTabela = _perfil.Listar_Perfil();

            // Jogar da DTO para a ViewModel do perfil de usuario. 

            // var perfilTransferidos = TypeAdapter.Adapt<IEnumerable<PerfilUsrDTO>, IEnumerable<PerfilViewModel>>(perfilTabela);
            //Agora que saiu da DTO do PErfil Para a VM do Perfil Vamos acrescentar ao modelo de usuario

            //estanciamos a view model do Usuario
            var usuarioTela = new UsuarioViewModel();

            //Crei uma Flag para direcionar oque será feito na view para bloquear o login
            usuarioTela.Acao = "cadastrar";


            //atribuimos a lista transferida a um modelo de lista previamente criado
            usuarioTela.ListaPerfil = new SelectList(perfilTabela, "PerfilId", "NomePerfil");

            // Devolvemos para a tela listar o dropdown (Model Binding)
            return View(usuarioTela);
        }

        [HttpPost]
        public ActionResult CadastrarUsr(UsuarioViewModel dadosTelaCad)
        {
            //Ao carregar a tela incluimos no viewmodel da tela a lista de perfil capturada na Controller(<-Business<-Repositorio<-DTO<-DB)
            var perfilTabela = _perfil.Listar_Perfil();

            // Jogar da DTO para a ViewModel do perfil de usuario. 

            // var perfilTransferidos = TypeAdapter.Adapt<IEnumerable<PerfilUsrDTO>, IEnumerable<PerfilViewModel>>(perfilTabela);


            //atribuimos a lista transferida a um modelo de lista previamente criado
            dadosTelaCad.ListaPerfil = new SelectList(perfilTabela, "PerfilId", "NomePerfil");

            //Crei uma Flag para direcionar oque será feito na view para bloquear o login
            dadosTelaCad.Acao = "cadastrar";



            if (!ModelState.IsValid)
            {

                //Caso esteja desabilitado mandamos a msg para a tela
                ModelState.AddModelError("Hacker", "Um Erro ocorreu. Por favor os dados preenchidos");
                return View(dadosTelaCad);

            }


            //Validamos o Cpf
            var validaCpf = _usuario.ValidaCpf(dadosTelaCad.Cpf);

            if (validaCpf == true)
            {
                ModelState.AddModelError("Hacker", "Um Erro ocorreu. CPF JA EXISTE NO DB");
                return View(dadosTelaCad);
            }

            //Validamos Usuario
            var validaUsrExistente = _usuario.ValidaUsuario(dadosTelaCad.Login);
            if (validaUsrExistente == true)
            {
                ModelState.AddModelError("Hacker", "Um Erro ocorreu. USUARIO JÁ EXISTE");
                return View(dadosTelaCad);
            }

            // Validamos o E-mail

            var ValidaEmailUsrExistente = _usuario.ValidaEmail(dadosTelaCad.Email);

            if (ValidaEmailUsrExistente == true)
            {
                ModelState.AddModelError("Hacker", "Um Erro ocorreu.E-MAIL JÁ EXISTE");
                return View(dadosTelaCad);
            }

            //Se as validações estão Ok Seguimos.



            //transfere os dado VM para a DTO

            var usuarioTabela = TypeAdapter.Adapt<UsuarioViewModel, UsuarioDTO>(dadosTelaCad);


            //Transefere para a Bussiness comandar a inserção na repositorio
            _usuario.Edita_Inseri_Usuario(usuarioTabela);

            //Mesg de Sucesso
            TempData.Add("SUCESSO", "Usuário Cadastrado com Sucesso!");
            // Retorna para lista
            return RedirectToAction("ListarUsr");

        }

        public ActionResult EditarUsr(Int32 id)
        {
            //Ao carregar a tela incluimos no viewmodel da tela a lista de perfil capturada na Controller(<-Business<-Repositorio<-DTO<-DB)

            var perfilTabela = _perfil.Listar_Perfil();

            // Jogar da DTO para a ViewModel do perfil de usuario. 

            //var perfilTransferidos = TypeAdapter.Adapt<IEnumerable<PerfilUsrDTO>, IEnumerable<PerfilViewModel>>(perfilTabela);

            //Agora que saiu da DTO do PErfil Para a VM do Perfil Vamos acrescentar ao modelo de usuario

            //estanciamos a view model do Usuario escolhido pelo ID
            var usuarioEditar = _usuario.Listar_Usauario().Single(x => x.Codigo == id);

            //Configuramos o AUTOMAPPER (DTO para VIEWMODEL) transferindo par VM os dados filtrados pela business


            var usuarioTela = TypeAdapter.Adapt<UsuarioDTO, UsuarioViewModel>(usuarioEditar);

            //atribuimos a lista transferida a um modelo de lista previamente criado
            usuarioTela.ListaPerfil = new SelectList(perfilTabela, "PerfilId", "NomePerfil");
            usuarioTela.Senha = "00000";

            //Devolve a ação para bloquear campos quando em edição
            usuarioTela.Acao = "editar";



            // Devolvemos para a tela listar o dropdown (Model Binding)
            return View(usuarioTela);
        }

        public ActionResult EditarUsr(UsuarioViewModel dadosTela, Int32 id)
        {

            //Cenário hacker
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Hacker",
                            "Erro ao validar Form!");
                return View();
            }
            ///configuramos o AutoMapper


            //Pegamos o ID da URL e setamos no registro
            dadosTela.Codigo = id;

            //Ligamos a trnsformação
            var usuarioTabela = TypeAdapter.Adapt<UsuarioViewModel, UsuarioDTO>(dadosTela);
            //Transfere a DTO para a business comandar a Edição
            _usuario.Edita_Inseri_Usuario(usuarioTabela);

            //Apenas para alerta na tela para altaração de senha
            string msg = "";
            if (usuarioTabela.Senha == "00000")
            {
                msg = "Usuario Atualizado com Sucesso!! -  NÃO Houve Alteração de senha";
            }
            else
            {
                msg = "Usuario Atualizado com Sucesso!! - Houve Alteração de senha";
            }

            TempData.Add("SUCESSO", msg);

            //Se escrever RETURN VIEW() -> VIEWBAG
            //Se escrever RETURN REDIRECTTOACTION -> TEMPDATA
            return RedirectToAction("ListarUsr");
        }

        public ActionResult ListarUsr()
        {
            //Pega a coleção que veio do Business e salva na var
            var usuarioTabela = _usuario.Listar_Usauario();
            // para levar para tela temos que transferir da DTO para ViewModel
            //Via Fastmapper fizemos a conversão de DTO para VIEWMODEL
            //Configuramos a CLASSE DE ORIGEM e a CLASSE DESTINO
            // Transferimos a coleção de usuarioTabela para UsuariosTransferidos

            var UsuariosTransferidos = TypeAdapter.Adapt<IEnumerable<UsuarioDTO>, IEnumerable<UsuarioViewModel>>(usuarioTabela);


            //Mandamos a coleção UsuariosTransferidos para a tela
            return View(UsuariosTransferidos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
