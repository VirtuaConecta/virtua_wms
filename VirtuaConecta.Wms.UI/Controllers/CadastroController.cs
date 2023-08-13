using FastMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtua.Utilities;
using VirtuaBusiness.Cliente;
using VirtuaBusiness.Municipio;
using VirtuaBusiness.Perfil;
using VirtuaBusiness.Produto;
using VirtuaBusiness.Usuarios;
using VirtuaConecta.Wms.UI.ViewModel;
using VirtuaDTO;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class CadastroController : Controller
    {
        IclienteBLL _clienteBLL;
        ImunicipioBLL _municipio;
        private readonly IprodutoBLL _produto;
        IusuarioBLL _usuario;
        IperfilBLL _perfil;

        public CadastroController(IclienteBLL icliente, ImunicipioBLL imunicipio, IprodutoBLL produto, IusuarioBLL usuario,
            IperfilBLL perfil)
        {
            _clienteBLL = icliente;
            _municipio = imunicipio;
            _produto = produto;
            _usuario = usuario;
            _perfil = perfil;
        }

        
         public String Formata(String formatar)
        {
            if (!String.IsNullOrEmpty(formatar))
            {
                formatar = formatar.Replace(".", "");
                formatar = formatar.Replace("-", "");
                formatar = formatar.Replace("/", "");
                formatar = formatar.Replace("(", "");
                formatar = formatar.Replace(")", "");
                formatar = formatar.Replace("|", "");
                formatar = formatar.Trim();
            }
            

            return formatar;
        }
        public IActionResult Lista_cliente()
        {
            var cliDTO = _clienteBLL.Listar_Cliente().Where(x=>x.NomeCliente != null).ToList();
            var cliVm = TypeAdapter.Adapt<IEnumerable<ClienteDTO>, List<ClienteViewModel>>(cliDTO);
            listacliente.lista = cliVm;

            return View(cliVm);
        }
        [HttpPost]
        public IActionResult Lista_cliente(int Id_cli)
        {
            DadoCadastroCliViewModel dadostela = new DadoCadastroCliViewModel();
            dadostela.listaCli = listacliente.lista;

            if (Id_cli!=0)
            {


               
                    dadostela.cliente = listacliente.lista.Where(x => x.IdCliente == Id_cli).FirstOrDefault();
         
            }
            else
                dadostela.cliente = new ClienteViewModel();

            return View("Cadastro_Cliente", dadostela);
        }
        public IActionResult Cadastro_Cliente(int Id_cli)
        {
            DadoCadastroCliViewModel dadostela = new DadoCadastroCliViewModel();
            dadostela.listaCli = listacliente.lista;

            if (Id_cli!=0)
            {
                
                    dadostela.cliente = listacliente.lista.Where(x => x.IdCliente == Id_cli).FirstOrDefault();                
            }
            else
            {
                dadostela.cliente = new ClienteViewModel();
               
            }
            return View("Cadastro_Cliente", dadostela);
        }
        [HttpPost]
        public IActionResult Cadastro_Cliente(DadoCadastroCliViewModel dadostela)
        {
           
            dadostela.cliente.Cpf = Formata(dadostela.cliente.Cpf);          
           dadostela.cliente.Cnpj = Formata(dadostela.cliente.Cnpj);
            dadostela.cliente.Cep = Formata(dadostela.cliente.Cep);
            dadostela.cliente.Telefone1 = Formata(dadostela.cliente.Telefone1);
            dadostela.cliente.Telefone2 = Formata(dadostela.cliente.Telefone2);
            dadostela.cliente.Telefone3 = Formata(dadostela.cliente.Telefone3);
            dadostela.cliente.Ddd = Formata(dadostela.cliente.Ddd);
            dadostela.cliente.CodigoMunicipio = Formata(dadostela.cliente.CodigoMunicipio);

            var resp = Verifica_Cliente(dadostela);
            if (resp == "OK")
            {
                var cliente = new ClienteDTO();
                cliente = TypeAdapter.Adapt<ClienteViewModel, ClienteDTO>(dadostela.cliente);
                if (dadostela.cliente.IdCliente == 0)
                {
                    if (_clienteBLL.Edita_Inseri_Cliente(cliente) == 1)
                    {
                        
                        
                      

                        return RedirectToAction("Lista_cliente");
                    }
                    else
                    {
                        dadostela.Erro = "Erro no cadastro do cliente, verifique sua conexão";
                        return View("Cadastro_Cliente", dadostela);
                    }

                }
                else
                {
                    if (_clienteBLL.Edita_Inseri_Cliente(cliente) == 1)
                    {

                        return RedirectToAction("Lista_cliente");
                    }
                    else
                    {
                        dadostela.Erro = "Erro na edição do cliente, verifique sua conexão";
                        return View("Cadastro_Cliente", dadostela);
                    }
                }              
             
             
            }
            else
            {
                dadostela.Erro = resp;
                return View("Cadastro_Cliente", dadostela);
            }
        }

      

        [HttpGet]
        public ActionResult AddrByCep(string cep)
        {
            cep = Formata(cep);
            String cep1 = Convert.ToString(cep);
            var dados = _municipio.CapturaENderecoPeloCep(cep1);
            if (dados != null)
            {
                var cidade = _municipio.Listar_Municipios().Where(x => Convert.ToString(x.Cod_municipio) == dados.ibge).FirstOrDefault();
                if (cidade != null && !String.IsNullOrEmpty(cidade.Cod_Id))
                {
                    dados.Cod_municipio_txt = cidade.Cod_Id;
                    dados.cep = dados.cep.Replace("-", "");

                }

                return Json(dados);

            }
            return null;

        }

        public String Verifica_Cliente(DadoCadastroCliViewModel dadostela)
        {
            var resp = "OK";

            if (String.IsNullOrEmpty(dadostela.cliente.NomeCliente))
            {
                resp = "Preencha o nome do cliente";
            }
            else if(String.IsNullOrEmpty(dadostela.cliente.Fantasia))
            {
                resp = "Preencha o nome fantasia do cliente";
            }
            else if (String.IsNullOrEmpty(dadostela.cliente.Endereco))
            {
                resp = "Preencha o endereço do cliente";
            }
            else if (String.IsNullOrEmpty(dadostela.cliente.NumeroEndereco))
            {
                resp = "Preencha o numero de endereço do cliente";
            }
            else if (!Int32.TryParse(dadostela.cliente.NumeroEndereco,out int num ))
            {
                resp = "Preencha o numero de endereço do cliente somente com numeros";
            }
            else if ( !String.IsNullOrEmpty(dadostela.cliente.Cnpj) && !Double.TryParse(dadostela.cliente.Cnpj, out double cnpj))
            {
                resp = "Preencha o cnpj  do cliente somente com numeros";
            }
            else if (!String.IsNullOrEmpty(dadostela.cliente.Cpf) && !Double.TryParse(dadostela.cliente.Cpf, out double cpf))
            {
                resp = "Preencha o cpf  do cliente somente com numeros";
            }
            else if (!String.IsNullOrEmpty(dadostela.cliente.Cnpj) && dadostela.cliente.Cnpj.Length!= 14)
            {
                resp = "Preencha  o cnpj com 14";
            }
            else if (!String.IsNullOrEmpty(dadostela.cliente.Cpf) && dadostela.cliente.Cpf.Length != 11)
            {
                resp = "Preencha o cpf com  11 digitos";
            }
            else if (String.IsNullOrEmpty(dadostela.cliente.Status))
            {
                resp = "Preencha o status do cliente";
            }
            else if (dadostela.cliente.Status.Length!=1 )
            {
                resp = "Preencha o status do cliente com somente uma letra A/F";
            }
            else if (dadostela.cliente.Status != "A"&& dadostela.cliente.Status != "F")
            {
                resp = "O status do cliente de ser preenchido com A/F";
            }else if (String.IsNullOrEmpty(dadostela.cliente.Cep))
            {
                resp = "Preencha o CEP";
            }
            else if (dadostela.cliente.Cep.Length!=8)
            {
                resp = "Preencha o CEP com 8 digitos";
            }


            return resp;
        }
    

        public IActionResult Seleciona_Cliente()
        {
            DadoPesquisaViewModel dadostela = new DadoPesquisaViewModel();
            var cliDTO = _clienteBLL.Listar_Cliente();
            var cliVm = TypeAdapter.Adapt<IEnumerable<ClienteDTO>, List<ClienteViewModel>>(cliDTO);
            listacliente.lista = cliVm;
            dadostela.Lista = new SelectList(cliVm.
               Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(x => x.NomeCliente), "IdCliente", "NomeCliente"); 

            return View(dadostela);
        }
        [HttpPost]
        public IActionResult Seleciona_Cliente(DadoPesquisaViewModel dadostela)
        {

            if (dadostela.Id == 0)
            {
               
                dadostela.Lista = new SelectList(listacliente.lista.
                    Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(x => x.NomeCliente), "IdCliente", "NomeCliente");
                dadostela.Erro = "É necessário selecionar um cliente";
                return View("Seleciona_Cliente", dadostela);
            }
            else { 

            var skuDTO = _produto.PesquisarProdutoBLL(null, dadostela.Id, null).OrderBy(x=>x.Sku).ToList();
             
                if (skuDTO.Count() == 0)
                {
                    dadostela.Lista = new SelectList(listacliente.lista.
                   Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(x => x.NomeCliente), "IdCliente", "NomeCliente");
                    dadostela.Erro = "Não existe SKU desse cliente";
                    return View("Seleciona_Cliente", dadostela);
                }
                else 
                { 
                    var skuVm = TypeAdapter.Adapt<IEnumerable<ProdutosDTO>, List<ProdutosViewModel>>(skuDTO);
                    listaProdutoViewModel.lista = skuVm;
                    var embDTO = _produto.Listar_Embalagem();
                    var embVm = TypeAdapter.Adapt<List<ListaGenericaDTO>, List<ListaGenericaViewModel>>(embDTO);
                    listaProdutoViewModel.embalagens = embVm;
                    return View("Lista_Sku",skuVm);
                }
            
            }
        }

        public IActionResult Lista_Sku()
        {
            
            return View();
        }        
        [HttpPost]
        public IActionResult Lista_Sku(String Sku)
        {
            ProdutosViewModel produto = new ProdutosViewModel();
            DadoCadastroProdutoViewModel dadostela = new DadoCadastroProdutoViewModel();
            if (!String.IsNullOrEmpty(Sku))
            {
                produto = listaProdutoViewModel.lista.Where(x => x.Sku == Sku).FirstOrDefault();
              
            }
            else
               

            dadostela.produto = produto;

            return View("Cadastro_Sku",dadostela);
        }

        public IActionResult Cadastro_Sku(Int32 Id_produto)
        {

            ProdutosViewModel produto = new ProdutosViewModel();
            DadoCadastroProdutoViewModel dadostela = new DadoCadastroProdutoViewModel();
            if (Id_produto >0)
            {
                var lst = listaProdutoViewModel.lista;
                produto = lst.Where(x => x.Id_produto == Id_produto).FirstOrDefault();
        
            }
          
            dadostela.emblagens = new SelectList(listaProdutoViewModel.embalagens.
                  Where(x => !String.IsNullOrWhiteSpace(x.descricao)).OrderBy(x => x.descricao), "id", "descricao");

            dadostela.produto = produto;

            return View(dadostela);
        }

        [HttpPost]
        public IActionResult Cadastro_Sku(DadoCadastroProdutoViewModel dados)
        {
            string verifica = Verifica_Sku(dados);

            if (verifica == "OK")
            {
                ProdutosDTO produto = new ProdutosDTO();
                
                produto = TypeAdapter.Adapt<ProdutosViewModel, ProdutosDTO>(dados.produto);
                if (produto.Id_produto == 0)
                {
                    produto.Data_cad = DateTime.Now.ToString();

                    if (_produto.InserProdutoDal(produto, listaProdutoViewModel.lista[0].Id_cliente) != 0)
                    {
                        DadoPesquisaViewModel dadostela = new DadoPesquisaViewModel();
                        dadostela.Lista = new SelectList(listacliente.lista.
                  Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(x => x.NomeCliente), "IdCliente", "NomeCliente");
                        return RedirectToAction("Seleciona_Cliente");
                    }
                    else
                    {
                        dados.emblagens = new SelectList(listaProdutoViewModel.embalagens.
                 Where(x => !String.IsNullOrWhiteSpace(x.descricao)).OrderBy(x => x.descricao), "id", "descricao");
                        dados.Erro = "Erro na inserção, verifique a conexão wifi";
                        return View("Cadastro_Sku", dados);

                    }
                }
                else
                {

                    if (_produto.EditaProduto(produto, produto.Id_cliente) != 0)
                    {
                        DadoPesquisaViewModel dadostela = new DadoPesquisaViewModel();
                        dadostela.Lista = new SelectList(listacliente.lista.
                  Where(x => !String.IsNullOrWhiteSpace(x.NomeCliente)).OrderBy(x => x.NomeCliente), "IdCliente", "NomeCliente");
                        return RedirectToAction("Seleciona_Cliente");
                    }
                    else
                    {
                        dados.emblagens = new SelectList(listaProdutoViewModel.embalagens.
                 Where(x => !String.IsNullOrWhiteSpace(x.descricao)).OrderBy(x => x.descricao), "id", "descricao");
                        dados.Erro = "Erro na edição, verifique a conexão wifi";
                        return View("Cadastro_Sku", dados);

                    }
                }
                
            }
            else
            {
                dados.Erro = verifica;
                dados.emblagens = new SelectList(listaProdutoViewModel.embalagens.
                 Where(x => !String.IsNullOrWhiteSpace(x.descricao)).OrderBy(x => x.descricao), "id", "descricao");
                return View("Cadastro_Sku", dados);
            }
             
        }

        public String Verifica_Sku(DadoCadastroProdutoViewModel dadostela)
        {
            string verifica = "OK";
            try
            {

                if (String.IsNullOrEmpty(dadostela.produto.Sku))
                {
                    verifica = "Preencha o Sku do produto";
                }
                else if (dadostela.produto.Id_produto == 0 && _produto.PesquisarProdutoBLL(null, null, dadostela.produto.Sku).FirstOrDefault() != null)
                {
                    verifica = "Já existe produto com esse Sku";
                }
                else if (String.IsNullOrEmpty(dadostela.produto.Operador))
                {
                    verifica = "Preencha o operador";
                }
                else if (String.IsNullOrEmpty(dadostela.produto.Descricao))
                {
                    verifica = "Preencha a descrição";
                }
                else if (String.IsNullOrEmpty(dadostela.produto.Unidade))
                {
                    verifica = "Preencha a Unidade";
                }
                else if (dadostela.produto.Ncm == 0)
                {
                    verifica = "Preencha o Ncm com valores numéricos";
                }
                else if (dadostela.produto.qtd_emb == 0)
                {
                    verifica = "Preencha a quantidade com valores numéricos";
                }
                else if (dadostela.produto.Peso_liq == 0)
                {
                    verifica = "Preencha o peso líquido com valores numéricos";
                }
                else if (dadostela.produto.Volume == 0)
                {
                    verifica = "Preencha o volume com valores numéricos";
                }
                else if (String.IsNullOrEmpty(dadostela.produto.Embalagem))
                {
                    verifica = "Preencha a embalagem do produto";
                }
                else if (dadostela.produto.Cst == 0)
                {
                    verifica = "Preencha o Cst com valores numéricos";
                }
                else if (dadostela.produto.P_unit == 0)
                {
                    verifica = "Preencha o peso unitário com valores numéricos";
                }
                else if (dadostela.produto.Laco == 0)
                {
                    verifica = "Preencha o Laco unitário com valores numéricos";
                }
                else if (dadostela.produto.Altura == 0)
                {
                    verifica = "Preencha a altura unitário com valores numéricos";
                }
                else if (String.IsNullOrEmpty(dadostela.produto.Cod_bar1))
                {
                    verifica = "Preencha o código de barra";
                }
                else if (dadostela.produto.Peso_brt == 0)
                {
                    verifica = "Preencha o peso bruto com valores numéricos";
                }

            }
            catch (Exception ex)
            {

                var msg=ex.Message;
                verifica = "Erro";
            }
            return verifica;
        }


        public IActionResult Lista_Usuario()
        {
            var userDTO = _usuario.Listar_Usauario();
            var userVM = TypeAdapter.Adapt<IEnumerable<UsuarioDTO>, List<UsuarioViewModel>>(userDTO);
            listaUsuario.lista = userVM;

            var perfilDTO = _perfil.Listar_Perfil();
            var perfilVM = TypeAdapter.Adapt<IEnumerable<PerfilDTO>, List<PerfilViewModel>>(perfilDTO);
            listaPerfilViewModel.lista = perfilVM;

            return View(userVM);
        }

        [HttpPost]
        public IActionResult Lista_Usuario(String Login)
        {
            UsuarioViewModel user = new UsuarioViewModel();
            DadoCadastroUsuarioViewModel dadostela = new DadoCadastroUsuarioViewModel();
            if (!String.IsNullOrEmpty(Login))
            {
                user = listaUsuario.lista.Where(x => x.Login == Login).FirstOrDefault();

            }
         


                dadostela.usuario = user;
            dadostela.perfils = dadostela.perfils = new SelectList(listaPerfilViewModel.lista.
               Where(x => !String.IsNullOrWhiteSpace(x.NomePerfil)).OrderBy(x => x.NomePerfil), "PerfilId", "NomePerfil"); ;

            return View("Cadastro_Usuario", dadostela);
        }

        public IActionResult Cadastro_Usuario(int Id_user)
        {

            UsuarioViewModel user = new UsuarioViewModel();
            DadoCadastroUsuarioViewModel dadostela = new DadoCadastroUsuarioViewModel();
            if (Id_user != 0)
            {
                user = listaUsuario.lista.Where(x => x.Codigo== Id_user).FirstOrDefault();

            }
           

            
                dadostela.usuario = user;
            dadostela.perfils = new SelectList(listaPerfilViewModel.lista.
               Where(x => !String.IsNullOrWhiteSpace(x.NomePerfil)).OrderBy(x => x.NomePerfil), "PerfilId", "NomePerfil");

            return View("Cadastro_Usuario", dadostela);
        }

        [HttpPost]
        public IActionResult Cadastro_Usuario(DadoCadastroUsuarioViewModel dados)
        {
            dados.usuario.Cpf = Formata(dados.usuario.Cpf);
            dados.usuario.Telefone = Formata(dados.usuario.Telefone);
            string verifica = Verifica_Usuario(dados);
          
            if (verifica == "OK")
            {
                UsuarioDTO usuario = new UsuarioDTO();

                if(!String.IsNullOrEmpty(dados.senha))
                dados.usuario.Senha= MD5Crypto.getMD5Hash(dados.senha);
 
                usuario = TypeAdapter.Adapt<UsuarioViewModel, UsuarioDTO>(dados.usuario);
                if (usuario.Codigo == 0)
                {
                   

                    if (_usuario.Edita_Inseri_Usuario(usuario) != 0)
                    {
                        var userDTO = _usuario.Listar_Usauario();
                        var userVM = TypeAdapter.Adapt<IEnumerable<UsuarioDTO>, List<UsuarioViewModel>>(userDTO);
                        listaUsuario.lista = userVM;
                        return RedirectToAction("Lista_Usuario");
                    }
                    else
                    {
                        dados.Erro = "Erro na inserção, verifique a conexão wifi";
                        dados.perfils = new SelectList(listaPerfilViewModel.lista.
               Where(x => !String.IsNullOrWhiteSpace(x.NomePerfil)).OrderBy(x => x.NomePerfil), "PerfilId", "NomePerfil");
                        return View("Cadastro_Usuario", dados);

                    }
                }
                else
                {

                    if (_usuario.Edita_Inseri_Usuario(usuario) != 0)
                    {
                        
                        return RedirectToAction("Lista_Usuario");
                    }
                    else
                    {
                        dados.Erro = "Erro na edição, verifique a conexão wifi";
                        dados.perfils = new SelectList(listaPerfilViewModel.lista.
               Where(x => !String.IsNullOrWhiteSpace(x.NomePerfil)).OrderBy(x => x.NomePerfil), "PerfilId", "NomePerfil");
                        return View("Cadastro_Usuario", dados);

                    }
                }

            }
            else
            {
                dados.Erro = verifica;
                // carregar as selectlist que existem no metodo que carrega a tela inicialmente (sem [http])
                dados.perfils = new SelectList(listaPerfilViewModel.lista.
               Where(x => !String.IsNullOrWhiteSpace(x.NomePerfil)).OrderBy(x => x.NomePerfil), "PerfilId", "NomePerfil");
                return View("Cadastro_Usuario", dados);
            }

        }

        public String Verifica_Usuario(DadoCadastroUsuarioViewModel dadostela)
        {
            string verifica = "OK";

            if (String.IsNullOrEmpty(dadostela.usuario.NomeUsuario))
            {
                verifica = "Preencha o nome do usuário";
            }
           else if (String.IsNullOrEmpty(dadostela.usuario.Login))
            {
                verifica = "Preencha o login do usuário";
            }
            else if (dadostela.usuario.Codigo == 0 && listaUsuario.
                lista.Where(x => x.Login == dadostela.usuario.Login).FirstOrDefault() != null)
            {
                verifica = "Já existe usuário com esse login";
            }
            else if (dadostela.usuario.Codigo == 0 && String.IsNullOrEmpty(dadostela.senha))
            {
                verifica = "Preencha a senha do usuário";
            }
            else if (String.IsNullOrEmpty(dadostela.usuario.Cargo))
            {
                verifica = "Preencha o cargo do usuário";
            }          
            else if (String.IsNullOrEmpty(dadostela.usuario.Bloqueado))
            {
                verifica = "Preencha o status do usuário";
            }
            else if (String.IsNullOrEmpty(dadostela.usuario.Area))
            {
                verifica = "Preencha a area do usuário";
            }
            else if (String.IsNullOrEmpty(dadostela.usuario.Email))
            {
                verifica = "Preencha o email do usuário";
            }
            else if (String.IsNullOrEmpty(dadostela.usuario.Telefone))
            {
                verifica = "Preencha o telefone do usuário";
            }
            else if (!Double.TryParse(dadostela.usuario.Telefone,out Double aux))
            {
                verifica = "Preencha o telefone do usuário somente com números";
            }
            else if (dadostela.usuario.Telefone.Length != 11 )
            {
                verifica = "O telefone deve conter  11  dígitos";
            }
            else if (String.IsNullOrEmpty(dadostela.usuario.Cpf))
            {
                verifica = "Preencha o Cpf do usuário";
            }
            else if (!Double.TryParse(dadostela.usuario.Cpf, out  aux))
            {
                verifica = "Preencha o Cpf do usuário somente com números";
            }
            else if (dadostela.usuario.Cpf.Length != 11)
            {
                verifica = "O Cpf deve conter 11 dígitos";
            }



            return verifica;
        }

    }
}
