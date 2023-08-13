using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Virtua.Utilities;
using VirtuaDTO;

namespace VirtuaRepository
{
    public class MysqlConnections : IDataConnection
    {
        private readonly IConfiguration _config;
        private const string db_Mysql = "MyDb";
        private string connMysql;
        private ILogger<MysqlConnections> _logger;


        public MysqlConnections(ILogger<MysqlConnections> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;//"root""123456"
       connMysql = String.Format(_config.GetConnectionString(db_Mysql), "root", "123456");
           
    //        connMysql = String.Format(_config.GetConnectionString(db_Mysql), "userdb", "user@virtua2021");

        }

        #region Clientes

        public List<ClienteDTO> Listar_Cliente(int? id)
        {
            var cliente = new List<ClienteDTO>();
            var p = new DynamicParameters();

            if (id == null)
                id = 0;
            //if (id>0)
            //{

                p.Add("vId_cliente", id);
                try
                {
                    using (var conection = new MySqlConnection(connMysql))

                    {
                        var retorno = conection.Query<ClienteDTO>("listar_cliente", p, commandType: CommandType.StoredProcedure).ToList();

                        cliente = retorno;


                    }

                }
                catch (Exception ex)
                {

                    _logger.LogError(ex.Message, "Erro ao pesquisar cliente");
                }

            //}
            return cliente;
        }

        //public ClienteDTO PesquisaClientesDal(String cnpj)
        //{
        //    var _retorno = new ClienteDTO();

        //    try
        //    {


        //        String str = "SELECT tabcliente.`CODIN_CLIENTE` as Id_cliente ,";
        //        str += " tabcliente.`RAZAO` as Nome_cliente,";
        //        str += " tabcliente.`CGC` as Cnpj,";
        //        str += " tabcliente.`ZONA` as Letra,";
        //        str += " tab_remetente.CODIN_REMETE as Id_remetente ";
        //        str += " FROM financeiro.tabcliente";
        //        str += " JOIN cdoca.tab_remetente";
        //        str += " ON (tabcliente.CODIN_CLIENTE=tab_remetente.CODIN_CLIENTE)";
        //        str += $" Where  tabcliente.`CGC` ='{cnpj}'";

        //        using (IDbConnection connection = new MySqlConnection(connMysql))
        //        {
        //            _retorno = connection.Query<ClienteDTO>(str).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Erro em PesquisaClientesDal: {ex.Message}");
        //    }

        //    return _retorno;
        //}

        public int Edita_Inseri_Cliente(ClienteDTO cliente)
        {
            int resp = 0;
            var p = new DynamicParameters();
            p.Add("vid", cliente.IdCliente);
            p.Add("vnome", cliente.NomeCliente);
            p.Add("vfantasia", cliente.Fantasia);
            p.Add("vEndereco", cliente.Endereco);

            p.Add("vnum", cliente.NumeroEndereco);
            p.Add("vcomplemento", cliente.Complemento);
            p.Add("vfone", cliente.Telefone1);
            p.Add("vstatus", cliente.Status);

            p.Add("vfone2", cliente.Telefone2);
            p.Add("vfone3", cliente.Telefone3);
            p.Add("vcnpj", cliente.Cnpj);

            p.Add("vcpf", cliente.Cpf);
            p.Add("vemail", cliente.Email);
            p.Add("vcontato", cliente.Contato1);

            p.Add("vcontatoxml", cliente.ContatoXml1);
            p.Add("vemailxml", cliente.EmailXml1);
            p.Add("vcontatoxml2", cliente.ContatoXml2);

            p.Add("vcontatoxml3", cliente.ContatoXml3);
            p.Add("vemailxml2", cliente.EmailXml2);
            p.Add("vemailxml3", cliente.EmailXml3);

            p.Add("vddd", cliente.Ddd);
            p.Add("vuf", cliente.Uf);

            p.Add("vcep", cliente.Cep);
            p.Add("vcodin_muni", cliente.CodigoMunicipio);


            p.Add("vobs", cliente.Obs);
            p.Add("vcidade", cliente.Cidade);
            p.Add("vdata_cad", cliente.DataCadastro);
            p.Add("vbairro", cliente.Bairro);
            p.Add("vinsc_est", cliente.InscEst);
            p.Add("vresp", dbType: DbType.Int32, direction: ParameterDirection.Output);


            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("editar_inserir_cliente",
                      p, commandType: CommandType.StoredProcedure);

                    resp = p.Get<int>("vresp");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Editar/inserir cliente");

            }

            return resp;


        }
        #endregion

        #region CFOP
        public IEnumerable<CfopDTO> Listar_Cfop()
        {
            var cs = new List<CfopDTO>();

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<CfopDTO>("listar_cfop", commandType: CommandType.StoredProcedure).ToList();

                    cs = retorno;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar CFOP: " + ex.Message);
            }
            return cs;
        }
        #endregion

        #region Pedido 

        public String InserePedidoDal(Nfe Pedido)
        {
            String resp = "Erro";

            try
            {
                var head = Pedido.Head;
                String CnpjEntrada = _config.GetSection("Cnpj_entrada_Wms").Value;
                var st = _config.GetSection("SitucaoPedido:RetornaEm").Value;
                if (Pedido.ListaItens.Count() > 0 && !String.IsNullOrEmpty(connMysql))
                {
                    // var id_pedido_cliente = Convert.ToInt32(Pedido.PedidoHead.id);

                    //var listaProd = ListaProdutosDal(Pedido.PedidoHead.Id_cliente);

                    String Str = "  INSERT INTO solae.`nf_cli_head`";
                    Str += "(";
                    Str += "`tipo`,";
                    Str += "`cnpj`,";
                    Str += "`nome_cli`,";
                    Str += "`endereco`,";
                    Str += "`Bairro`,";
                    Str += "`cidade`,";
                    Str += "`estado`,";
                    Str += "`cep`,";
                    Str += "`ie`,";
                    Str += "`tipo_doc`,";
                    Str += "`serie`,";
                    Str += "`nr_documento`,";
                    Str += "`data_emissao`,";
                    Str += "`total_nf`,";
                    Str += "`id_cliente`,";
                    Str += "`data_entrada`,";
                    //  Str += "`dados_adic`,";
                    Str += "`especie`,";
                    Str += "`chNfe`,";
                    Str += "`CODIN_REMETE`,";
                    Str += "`data_Emissao2`,";
                    Str += "`data_entrada2`,";
                    Str += "`CodIn_municipio`,";
                    Str += "`remessa`,";
                    Str += "`atualizadopor`)";
                    Str += " VALUES (";
                    Str += "'03',";
                    Str += "@cnpj,";
                    Str += "@nomeCliente,";
                    Str += "@endereco,";
                    Str += "@bairro,";
                    Str += "@cidade,";
                    Str += "@uf,";

                    Str += "@cep,";
                    Str += "@ie,";
                    Str += "@Tipo,";
                    Str += "@serie,";
                    Str += "@nr_documento,";
                    Str += "@dataEmissao,";
                    Str += "@valorPed,";
                    Str += "@id_cliente,";
                    Str += "@dtEntrada,";
                    //   Str += "@infoAdic,";
                    Str += "'UN',";
                    Str += "@chave,";
                    Str += "@remetente,";
                    Str += "@dtEmissao2,";
                    Str += "@dtEntrada2,";
                    Str += "@codMunicipio,";
                    Str += "@remessa,";
                    Str += "'ROBO_WMS')";

                    var agora = DateTime.Now;
                    var p = new DynamicParameters();
                    p.Add("@Tipo", head.Tipo_pedido);
                    p.Add("@nr_documento", Pedido.Nf_Wms);
                    p.Add("@cnpj", head.Cnpj_destino);
                    p.Add("@nomeCliente", (head.Nome_destino.Trim().ToUpper() + new String(' ', 40)).Substring(0, 40).Trim());
                    var end = (head.Endereco_destino.Trim().ToUpper() + new String(' ', 44)).Substring(0, 44).Trim();
                    p.Add("@endereco", $"{end}");
                    p.Add("@bairro", (head.Bairro_destino.Trim().ToUpper() + new String(' ', 25)).Substring(0, 25).Trim());
                    p.Add("@cidade", head.Cidade_destino.Trim().ToUpper());
                    p.Add("@uf", head.Estado_destino.Trim().ToUpper());
                    p.Add("@cep", head.Cep_destino);
                    if (head.Fis_jur_destino == "J")
                    {

                        if (!String.IsNullOrEmpty(head.IE_destino))
                        {
                            p.Add("@ie", head.IE_destino.Replace(".", "").Replace("-", "").Trim());
                        }
                        else
                        {
                            p.Add("@ie", null);
                        }

                    }

                    else { p.Add("@ie", "ISENTO"); }



                    p.Add("@serie", head.Serie);
                    var dt = Funcoes.formataData(head.Data_emissao.ToString(), "ddmmyyyy").Substring(0, 8);
                    p.Add("@dataEmissao", dt);
                    p.Add("@valorPed", head.Valor_Total.ToString().Replace(',', '.'));
                    p.Add("@id_cliente", Pedido.Id_cliente);
                    p.Add("@dtEntrada", agora.ToString("ddMMyyyy"));
                    // p.Add("@infoAdic", $"Pedido_Tiny:{Pedido.PedidoHead.id}");
                    var ch = head.Chave;
                    if (String.IsNullOrEmpty(ch))
                    {
                        ch = new String('0', 44);

                    }
                    p.Add("@chave", ch);
                    p.Add("@remetente", Pedido.Id_rementente);
                    var dtEmi2 = head.Data_emissao.ToString("yyyy-MM-dd");
                    p.Add("@dtEmissao2", dtEmi2);
                    p.Add("@dtEntrada2", agora.ToString("yyyy-MM-dd HH:mm:ss"));
                    p.Add("@codMunicipio", head.Id_Cidade_destino);

                    if (CnpjEntrada == head.Cnpj_destino)
                    {
                        p.Add("@remessa", "I");
                    }
                    else
                    {
                        p.Add("@remessa", "O");
                    }

                    //itens ===================================================================================================

                    String Str1 = "INSERT INTO solae.nf_cli_item ";
                    Str1 += " ( tipo, ";
                    Str1 += " tipo_doc, ";
                    Str1 += " nr_documento, ";
                    Str1 += " val_unit, ";
                    Str1 += " produto,";
                    Str1 += " qtd, ";
                    Str1 += " unidade, ";
                    Str1 += " id_cliente, ";
                    Str1 += " Id_Head, ";
                    Str1 += " Id_produto) ";

                    Str1 += "VALUES ";

                    var cont = 0;

                    foreach (var iteml in Pedido.ListaItens)
                    {
                        if (cont > 0) { Str1 += ","; }

                        Str1 += $"('04',";
                        Str1 += $"'PED',";
                        Str1 += $"'{Pedido.Nf_Wms}',";
                        Str1 += $"{iteml.Punit.ToString().Replace(',', '.')},";
                        Str1 += $"'{iteml.Sku.Trim().ToUpper()}',";
                        Str1 += $"{iteml.Qtd.ToString().Replace(",", ".")},";
                        if (String.IsNullOrEmpty(iteml.Especie)) { Str1 += "'UN',"; }
                        else
                        {
                            var esp = iteml.Especie;
                            if (esp.Length > 4)
                                esp = iteml.Especie.Substring(0, 4);
                            Str1 += $"'{esp}',";
                        }
                        Str1 += $"{Pedido.Id_cliente},";
                        Str1 += "LAST_INSERT_ID(),";
                        //   var idprod = listaProd.Where(x => x.Sku.ToUpper() == iteml.codigo.Trim().ToUpper()).FirstOrDefault();



                        //if (idprod != null && idprod.Id_produto > 0)
                        //{ Str1 += $" {idprod.Id_produto})"; }
                        //else { 

                        //    Str1 += "0)"; 
                        //}
                        Str1 += $"{iteml.Id_Produto})";


                        cont++;
                    }

                    if (Pedido.ListaItens.Count != cont)
                    {
                        return resp;
                    }

                    String Str2 = "SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER); ";

                    String Str3 = $"{Str};{Str1};{Str2}";

                    // var t = $"{dest.endereco.Trim().ToUpper()} , {dest.numero.Trim().ToUpper()}";

                    using (IDbConnection connection = new MySqlConnection(connMysql))
                    {
                        //sql head

                        connection.Open();
                        using (var trans = connection.BeginTransaction())
                        {


                            try
                            {
                                connection.Execute(Str3, p, transaction: trans);
                                trans.Commit();

                                resp = "OK";
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Erro em Inserir pedido DAl Rollback: {ex.Message}");
                                trans.Rollback();
                            }


                        }




                    }

                }
            }
            catch (Exception ex)
            {
                resp = "Erro";
                _logger.LogError(ex.Message, "Erro em Inserir pedido DAl");
            }



            return resp;

        }
        public IEnumerable<DocumentoClienteHeadDTO> Listar_Doc_Por_Status(String st)
        {
            var itens = new List<DocumentoClienteHeadDTO>();

            var p = new DynamicParameters();
            p.Add("vIdStatus", st);
            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<DocumentoClienteHeadDTO>("Pedidos_status_doca", p, commandType: CommandType.StoredProcedure).ToList();

                    itens = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Listar pedido item");

            }

            return itens;
        }

        public PedidoDTO Pesquisa_Pedido(String NrPed, Int32? id_cliente, Int32? id_pedido)
        {
            var pedido = new PedidoDTO();
            var p = new DynamicParameters();
            p.Add("vNrPed", NrPed);
            p.Add("vIdCliente", id_cliente);
            p.Add("vIdCod", id_pedido);

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<PedidoDTO>("pesquisar_pedido", p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    pedido = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao pesquisar pedido");

            }

            return pedido;
        }

        /// <summary>
        /// Lista os itens de pedidos pelo id do head
        /// </summary>
        /// <param name="idHead"></param>
        /// <returns></returns>
        public IEnumerable<PedidoItemDTO> Listar_Itens_Ped(Int32 idHead)
        {
            var itens = new List<PedidoItemDTO>();
            var p = new DynamicParameters();
            p.Add("vId", idHead);
            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<PedidoItemDTO>("listar_pedido_item", p, commandType: CommandType.StoredProcedure).ToList();

                    itens = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Listar pedido item");

            }

            return itens;
        }


        public IEnumerable<PedidoItemDTO> Listar_Ped_itensAgrupados(String id_pedidos)
        {
            var itens = new List<PedidoItemDTO>();

            var p = new DynamicParameters();
            p.Add("jsonStr", id_pedidos);

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<PedidoItemDTO>("cria_lista_ped_agrupada_itens", p, commandType: CommandType.StoredProcedure).ToList();

                    itens = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Listar pedido item");

            }

            return itens;
        }


        public IEnumerable<Ped_entrada_saidaDTO> Listar_Ped_entrada_saida(int Id_cliente, DateTime Dt_fim, DateTime Dt_ini)
        {
            var itens = new List<Ped_entrada_saidaDTO>();
            var p = new DynamicParameters();
            p.Add("vId_cliente", Id_cliente);
            p.Add("vDdataIni", Dt_ini);
            p.Add("vDataFim", Dt_fim);
            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<Ped_entrada_saidaDTO>("ListaNfEntradaSaida", p, commandType: CommandType.StoredProcedure).ToList();

                    itens = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Listar pedido item");

            }

            return itens;
        }


        public IEnumerable<PedidoDTO> ListaPedidoPendente(String vRemessa, String vCancelado, String vProcessado)
        {
            IEnumerable<PedidoDTO> pedidos;
            var p = new DynamicParameters();
            p.Add("vRemessa", vRemessa);
            p.Add("vCancelado", vCancelado);
            p.Add("vProcessado", vProcessado);
            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<PedidoDTO>("listar_pedido_pendente", p, commandType: CommandType.StoredProcedure).ToList();

                    pedidos = retorno;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar pedido item: " + ex.Message);
            }


            return pedidos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DtIni"></param>
        /// <param name="DtFim"></param>
        /// <param name="Remessa"></param>
        /// <param name="idCliente"> Id o cliente ou 0 para todos</param>
        /// <returns></returns>
        public IEnumerable<PedidoDTO> ListaPedidoGeral(String DtIni, String DtFim, String Remessa, Int32 idCliente)
        {
            var pedidos = new List<PedidoDTO>();
            var p = new DynamicParameters();
            p.Add("vRemessa", Remessa);
            p.Add("vDtIni", DtIni);
            p.Add("vDtFim", DtFim);
            p.Add("vIdCliente", idCliente);

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<PedidoDTO>("listar_pedidos_geral", p, commandType: CommandType.StoredProcedure).ToList();

                    pedidos = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Listar ListaPedidoGeral");

            }


            return pedidos;
        }

        /// <summary>
        /// Criado para pedido manual
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public Int32 Edita_Inserir_Pedido(PedidoDTO pedido, DestinoDTO dest)
        {
            var resp = 0;
            var p = new DynamicParameters();
            p.Add("vid", pedido.indice);
            p.Add("vtipo", pedido.tipo);
            p.Add("vcnpj", dest.cpf_cnpj);
            p.Add("vie", dest.ie);
            p.Add("vNomeCliente", dest.nome);
            p.Add("vBairro_dest", dest.bairro);
            p.Add("vCep_dest", dest.cep);
            p.Add("vtipo_doc", pedido.tipo_doc);
            p.Add("vSerie_doc", pedido.Serie_doc);
            p.Add("vNr_documento", pedido.Nr_documento);
            p.Add("vCfop", pedido.Cfop);
            p.Add("vDt_emissao", pedido.Dt_emissao);
            p.Add("vDt_entrada", pedido.Dt_entrada);
            p.Add("vTotal_nf", pedido.Total_nf);
            p.Add("vRemessa", pedido.Remessa);
            p.Add("vId_cliente", pedido.Id_cliente);
            p.Add("vOperador", pedido.Operador);
            p.Add("vId_remetente", pedido.Id_remetente);
            p.Add("vCod_municipio", pedido.Cod_municipio);
            //  p.Add("vNr_doc_origem", pedido.Nr_doc_origem);
            p.Add("vDt_entrada", pedido.Dt_entrada);
            p.Add("vPeso_brt", pedido.Peso_brt);
            p.Add("vPeso_liq", pedido.Peso_liq);
            p.Add("vQtd", pedido.Nr_volumes);
            var end = (dest.endereco.Trim().ToUpper() + new String(' ', 44)).Substring(0, 44).Trim();
            var nr = (dest.numero.Trim().ToUpper() + new String(' ', 4)).Substring(0, 4).Trim();
            p.Add("vEnd_dest", $"{end},{nr}");
            //  p.Add("vIp", pedido.Ip);
            p.Add("vChave", pedido.Chave);



            p.Add("vresp", dbType: DbType.Int32, direction: ParameterDirection.Output);


            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("editar_inserir_pedido",
                      p, commandType: CommandType.StoredProcedure);

                    resp = p.Get<int>("vresp");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Ediatar Usuario");

            }

            return resp;
        }

        public Int32 InserePedidoNoEstoque(Int32 ID, Int32 NrOrdem, String vOperador)
        {

            Int32 retornoInserirEstoque = 0;
            //Retornos
            // 1- erro inserir Ordem_cli

            // 2- erro Atualiza head
            // 3- Erro atualizar Item
            // 4 - Sucesso
            //// 5- Faltou o paremetro ID
            //try
            //{
            //    AbrirConexao();
            //    Cmd = new MySqlCommand();
            //    Cmd.Connection = Con;

            //    Cmd = new MySqlCommand("InsertPedidoOrdem_cli", Con);
            //    Cmd.CommandType = CommandType.StoredProcedure;
            //    Cmd.Parameters.AddWithValue("vID", ID);
            //    Cmd.Parameters.AddWithValue("vId_ordem", NrOrdem);
            //    Cmd.Parameters.AddWithValue("vOperador", vOperador);


            //    //Captura a msg de erros

            //    retornoInserirEstoque = Convert.ToInt32(Cmd.ExecuteScalar());

            //}
            //catch (Exception ex)
            //{

            //    throw new Exception("Erro ao Inserir no Estoque " + ex.Message);
            //}
            //finally
            //{

            //    FecharConexao();
            //}

            return retornoInserirEstoque;


        }


        #endregion

        #region Usuario
        public IEnumerable<UsuarioDTO> Listar_Usauario()
        {
            var lista_usuario = new List<UsuarioDTO>();



            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<UsuarioDTO>("listar_usuarios", commandType: CommandType.StoredProcedure).ToList();

                    lista_usuario = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Listar Usuario");

            }
            return lista_usuario;
        }
        public Int32 Edita_Inseri_Usuario(UsuarioDTO usuario)
        {
            var resp = 0;
            var p = new DynamicParameters();
            p.Add("vitem", usuario.Codigo);
            p.Add("vNome", usuario.NomeUsuario);
            p.Add("vLogin", usuario.Login);
            p.Add("vCargo", usuario.Cargo);
            p.Add("vBloqueado", usuario.Bloqueado);
            p.Add("vSenha", usuario.Senha);
            p.Add("vArea", usuario.Area);
            p.Add("vEmail", usuario.Email);
            p.Add("vTelefone", usuario.Telefone);

            p.Add("vid_perfil", usuario.Cod_perfil);
            p.Add("vObs", usuario.Observacao);
            p.Add("vCpf", usuario.Cpf);


            p.Add("vresp", dbType: DbType.Int32, direction: ParameterDirection.Output);


            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("editar_inserir_usuario",
                      p, commandType: CommandType.StoredProcedure);

                    resp = p.Get<int>("vresp");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Editar Usuario");

            }

            return resp;
        }

        public UsuarioDTO Pesquisar_Login_Usuario(string login)
        {
            var usuario = new UsuarioDTO();
            var p = new DynamicParameters();
            p.Add("vLogin", login);
            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<UsuarioDTO>("pesquisar_login_usuarios", p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    usuario = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Pesquisar pelo login do Usuario");

            }

            return usuario;
        }


        #endregion

        #region Perfil
        public IEnumerable<PerfilDTO> Listar_Perfil()
        {
            var perfis = new List<PerfilDTO>();

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<PerfilDTO>("listar_perfil", commandType: CommandType.StoredProcedure).ToList();

                    perfis = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Listar perfil");

            }
            return perfis;
        }

        public Int32 Edita_Inseri_Perfil(PerfilDTO perfil)
        {
            var resp = 0;
            var p = new DynamicParameters();
            p.Add("vPerfilID", perfil.PerfilId);
            p.Add("vNomePerfil", perfil.NomePerfil);
            p.Add("vresp", dbType: DbType.Int32, direction: ParameterDirection.Output);


            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("editar_inserir_perfi",
                      p, commandType: CommandType.StoredProcedure);

                    resp = p.Get<int>("vresp");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Editar perfil");

            }

            return resp;
        }
        #endregion

        #region Municipio

        public IEnumerable<MunicipiosDTO> Listar_Municipios()
        {
            var municipios = new List<MunicipiosDTO>();

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<MunicipiosDTO>("listar_municipio", commandType: CommandType.StoredProcedure).ToList();

                    municipios = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao listar municipio");

            }

            return municipios;
        }
        #endregion

        #region Dash

        public IEnumerable<ChartDTO> Dash_Pedido_operador(int dia)
        {
            var operador = new List<ChartDTO>();

            var p = new DynamicParameters();
            p.Add("vdia", dia);


            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<ChartDTO>("dash_pedidos_operador", p, commandType: CommandType.StoredProcedure).ToList();

                    operador = retorno;
                    if (operador.Count == 0)
                    {
                        var chart = new ChartDTO();
                        chart.Item = "Não existe";
                        chart.Valor = 0;
                        chart.Valor2 = 0;
                        operador.Add(chart);

                    }

                    var menor = operador.OrderBy(x => x.Valor).FirstOrDefault();
                    var maior = operador.OrderByDescending(x => x.Valor).FirstOrDefault();

                    operador.Add(menor);
                    operador.Add(maior);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao listar pedido operador");

            }


            return operador;
        }

        public IEnumerable<ChartDTO> Dash_Volume_Cliente(int dia)
        {
            var cliente = new List<ChartDTO>();

            var p = new DynamicParameters();
            p.Add("vdia", dia);


            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<ChartDTO>("dash_volume_cliente", p, commandType: CommandType.StoredProcedure).ToList();

                    cliente = retorno;

                    if (cliente.Count == 0)
                    {
                        var chart = new ChartDTO();
                        chart.Item = "Não existe";
                        chart.Valor = 0;
                        chart.Valor2 = 0;
                        cliente.Add(chart);

                    }

                    var menor = cliente.OrderBy(x => x.Valor).FirstOrDefault();
                    var maior = cliente.OrderByDescending(x => x.Valor).FirstOrDefault();

                    cliente.Add(menor);
                    cliente.Add(maior);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em Dash_Volume_Cliente");

            }


            return cliente;
        }
        #endregion

        #region produto
        //================================ Produtos ======================================
        public IEnumerable<ProdutosDTO> PesquisarProdutosDal(Int32? id_produto, Int32? id_cliente, String sku)
        {
            var lista_retorno = new List<ProdutosDTO>();

            try
            {

                var p = new DynamicParameters();
                p.Add("vId", id_produto);
                p.Add("vIdCliente", id_cliente);
                p.Add("vSku", sku);

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {

                    var retorno = connection.Query<ProdutosDTO>("pesquisa_produto", p, commandType: CommandType.StoredProcedure).ToList();
                    lista_retorno = retorno;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em ListaProdutosDal");

            }

            return lista_retorno;
        }

        public Int32 InserProdutoDal(ProdutosDTO prod, Int32 cli)
        {
            var resp = 0;
            var p = new DynamicParameters();
            p.Add("vId_cliente", cli);
            p.Add("vdescricao", prod.Descricao);
            p.Add("vsku", prod.Sku);
            p.Add("voperador", prod.Operador);
            p.Add("vund", prod.Unidade);
            p.Add("vpeso_brt", prod.Peso_brt);
            p.Add("vpeso_liq", prod.Peso_liq);
            p.Add("vlaco", prod.Laco);
            p.Add("vAltura", prod.Altura);

            p.Add("vcod_bar1", prod.Cod_bar1);
            p.Add("vobs", prod.Obs);
            p.Add("vId_embalagem", prod.Embalagem);

            p.Add("vvolume", prod.Volume);
            p.Add("vncm", prod.Ncm);
            p.Add("vqtd_emb", prod.qtd_emb);
            p.Add("vposicoes", null);
            p.Add("vData", prod.Data_cad.Substring(0, 10));

            p.Add("vp_unit", prod.P_unit);
            p.Add("vcst", prod.Cst);
            p.Add("vresp", dbType: DbType.Int32, direction: ParameterDirection.Output);


            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("Produtos_Inserir",
                      p, commandType: CommandType.StoredProcedure);

                    resp = p.Get<int>("vresp");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em InserProdutoDal");

            }



            return resp;

        }

        public Int32 EditaProduto(ProdutosDTO prod, Int32 cli)
        {
            var resp = 0;
            var p = new DynamicParameters();
            p.Add("vId_cliente", cli);
            p.Add("vdescricao", prod.Descricao);
            p.Add("vsku", prod.Sku);
            p.Add("voperador", prod.Operador);
            p.Add("vund", prod.Unidade);
            p.Add("vpeso_brt", prod.Peso_brt);
            p.Add("vpeso_liq", prod.Peso_liq);
            p.Add("vlaco", prod.Laco);
            p.Add("vAltura", prod.Altura);

            p.Add("vcod_bar1", prod.Cod_bar1);
            p.Add("vobs", prod.Obs);
            p.Add("vId_embalagem", prod.Embalagem);

            p.Add("vvolume", prod.Volume);
            p.Add("vncm", prod.Ncm);
            p.Add("vqtd_emb", prod.qtd_emb);
            p.Add("vposicoes", null);
            var dt = String.IsNullOrEmpty(prod.Data_cad) ? DateTime.Now.ToString("yyyy-MM-dd") : prod.Data_cad.Substring(0, 10);
            p.Add("vData", dt);

            p.Add("vId_produto", prod.Id_produto);
            p.Add("vp_unit", prod.P_unit);
            p.Add("vcst", prod.Cst);
            p.Add("vresp", dbType: DbType.Int32, direction: ParameterDirection.Output);


            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("Produtos_Editar",
                      p, commandType: CommandType.StoredProcedure);

                    resp = p.Get<int>("vresp");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em EditaProduto");

            }



            return resp;
        }

        public Int32 InserProdutoDoArquivoDal(NfeItens prod, Int32 cli)
        {
            var resp = 0;
            var p = new DynamicParameters();
            p.Add("vId_cliente", cli);
            var des = (prod.Descricao_prod + new string(' ', 60)).Substring(0, 60).Trim();
            p.Add("vdescricao", des);
            p.Add("vsku", prod.Sku);
            p.Add("voperador", "Robo_Wms");
            p.Add("vund", prod.Especie);
            p.Add("vpeso_brt", 1);
            p.Add("vpeso_liq", 1);
            p.Add("vlaco", 10);
            p.Add("vAltura", 10);
            p.Add("vAltura", 10);
            p.Add("vcod_bar1", prod.Ean);
            p.Add("vobs", "Inserido Automaticamente");
            p.Add("vId_embalagem", "1");
            p.Add("vp_unit", prod.Punit);
            p.Add("vcst", 0);

            p.Add("vvolume", 1.5M);
            p.Add("vncm", prod.Ncm);
            p.Add("vqtd_emb", 1);

            p.Add("vData", DateTime.Now.ToString("yyyy-MM-dd"));
            p.Add("vresp", dbType: DbType.Int32, direction: ParameterDirection.Output);


            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("Produtos_Inserir",
                      p, commandType: CommandType.StoredProcedure);

                    resp = p.Get<int>("vresp");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em InserProdutoDoArquivoDal");

            }



            return resp;

        }

        public List<ListaGenericaDTO> Listar_Embalagem()
        {
            var embalagens = new List<ListaGenericaDTO>();

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<ListaGenericaDTO>("listar_embalagem", commandType: CommandType.StoredProcedure).ToList();

                    embalagens = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao Listar embalagem");

            }
            return embalagens;
        }



        #endregion

        #region Nfs

        public string InserirNfe(Nfe nota)
        {
            String Retorna = "Erro";


            try
            {
                var dadosNfe = nota;

                var dias = Convert.ToInt32(_config.GetSection("Tempos:DiasEntrega").Value);

                String str = " INSERT INTO  cdoca.nfs ";
                str += " (id_cliente, ";     //1
                str += " nf_nf, ";           //2
                str += " Obs, ";              //3
                str += " peso, ";             //4
                str += " data_registro,";      //5
                str += " aberto_por, ";        //6
                str += " tp, ";             //7
                str += " tipo_nf, ";           //8
                str += " destino, ";           //9
                str += " endDest, ";           //10
                str += " qtd_itens, ";        //11
                str += " status_nf, ";        //12
                str += " valor, ";            //13
                str += " data_emissao, ";     //16
                str += " origem_nf, ";         //17
                str += " obs_email, ";        //19
                str += " cnpj_dest, ";         //21
                                               //  str += " nr_embarque, ";      //24
                str += " CODIN_REMETENTE, ";      //25
                str += " nf_edi, ";          //26
                str += " data_entrada, ";      //27
                str += " chaveNfe, ";         //34
                str += " serie_nf, ";           //36
                str += " dt_final_entrega, "; //38
                str += " FISJUR, ";
                str += " cod_municipio) ";
                str += " VALUES ";

                str += " (@vId_cliente   ";           //1
                str += ", @vNrNf1";             //2
                str += ", 'Sem OBS'";                   //3
                str += ",@vPeso";                 //4
                str += ", Now()";    //5
                str += ", 'ROBO_API_WMS' ";                     //6
                str += ",@tp ";                          //7
                str += ",  @remessa";                        //8
                str += ", @vRazaoDest";  //9
                str += ",@vEndDest";         //10
                str += ",@vQtdVol ";                 //11
                str += ", 1";                          //12
                str += ",@vValorCobrado ";           //13
                str += ",@vDtEmissao";  //16
                str += ", 'N'";                         //17
                str += ", 'Sem OBS'";                   //19
                str += ",@vCnpjDest";        //21
                                             //  str += ", @vNrEmbarque";    //24
                str += ",@vId_Remetente ";          //25
                str += ", @vNrNfEdi";       //26
                str += ", @DtEntrada";    //27
                str += ", @chave";      //34
                str += ",@vSerieNf";           //36 
                str += ",@DtFinalEntrega";
                str += ",@fisjur";
                str += ",@cod_municipio)";
                var p = new DynamicParameters();
                var agora = DateTime.Now;

                p.Add("@vId_cliente", dadosNfe.Id_cliente);
                p.Add("@vNrNf1", dadosNfe.Nf_Wms);
                p.Add("@tp", 0);
                p.Add("@vPeso", 0);
                String CnpjEntrada = _config.GetSection("Cnpj_entrada_Wms").Value;
                if (CnpjEntrada == dadosNfe.Head.Cnpj_destino)
                {
                    p.Add("@remessa", "I");
                }
                else
                {
                    p.Add("@remessa", "O");
                }
                p.Add("@vRazaoDest", dadosNfe.Head.Nome_destino.ToUpper());
                p.Add("@vEndDest", $"{dadosNfe.Head.Endereco_destino}/{dadosNfe.Head.Cidade_destino}/{dadosNfe.Head.Estado_destino}{new String(' ', 80)}".Substring(0, 80).Trim().ToUpper());
                p.Add("@vQtdVol", dadosNfe.ListaItens.Sum(x => x.Qtd));
                p.Add("@vValorCobrado", dadosNfe.Head.Valor_Total);
                p.Add("@vDtEmissao", dadosNfe.Head.Data_emissao);
                p.Add("@vCnpjDest", dadosNfe.Head.Cnpj_destino);


                p.Add("@vId_Remetente", dadosNfe.Id_rementente);
                p.Add("@vNrNfEdi", dadosNfe.Head.Nr_original_cliente);

                p.Add("@DtEntrada", agora.ToString("yyyy-MM-dd HH:mm:ss"));
                p.Add("@chave", new String('0', 44));
                p.Add("@vSerieNf", dadosNfe.Head.Serie);
                p.Add("@DtFinalEntrega", DateTime.Now.AddDays(dias));

                p.Add("@fisjur", dadosNfe.Head.Fis_jur_destino);

                p.Add("@cod_municipio", dadosNfe.Head.Id_Cidade_destino);
                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    connection.Execute(str, p);


                    Retorna = "OK";
                }


            }
            catch (Exception ex)
            {

                Retorna = "Erro";
                _logger.LogError(ex.Message, "Erro em InserirNfe");

            }

            return Retorna;
        }


        public string InserirNfeCrossDock(Nfe nota)
        {
            String Retorna = "Erro";


            try
            {
                var dadosNfe = nota;

                var dias = Convert.ToInt32(_config.GetSection("Tempos:DiasEntrega").Value);

                String str = " INSERT INTO  cdoca.nfs ";
                str += " (id_cliente, ";     //1
                str += " nf_nf, ";           //2
                str += " Obs, ";              //3
                str += " peso, ";             //4
                str += " data_registro,";      //5
                str += " aberto_por, ";        //6
                str += " tp, ";             //7
                str += " tipo_nf, ";           //8
                str += " destino, ";           //9
                str += " endDest, ";           //10
                str += " qtd_itens, ";        //11
                str += " status_nf, ";        //12
                str += " valor, ";            //13
                str += " data_emissao, ";     //16
                str += " origem_nf, ";         //17
                str += " obs_email, ";        //19
                str += " cnpj_dest, ";         //21
                                               //  str += " nr_embarque, ";      //24
                str += " CODIN_REMETENTE, ";      //25
                str += " nf_edi, ";          //26
                str += " data_entrada, ";      //27
                str += " chaveNfe, ";         //34
                str += " serie_nf, ";           //36
                str += " dt_final_entrega, "; //38
                str += " FISJUR, ";
                str += " cod_municipio) ";
                str += " VALUES ";

                str += " (@vId_cliente   ";           //1
                str += ", @vNrNf1";             //2
                str += ", 'Sem OBS'";                   //3
                str += ",@vPeso";                 //4
                str += ", Now()";    //5
                str += ", 'ROBO_API_WMS' ";                     //6
                str += ",@tp ";                          //7
                str += ",  @remessa";                        //8
                str += ", @vRazaoDest";  //9
                str += ",@vEndDest";         //10
                str += ",@vQtdVol ";                 //11
                str += ", 1";                          //12
                str += ",@vValorCobrado ";           //13
                str += ",@vDtEmissao";  //16
                str += ", 'N'";                         //17
                str += ", 'Sem OBS'";                   //19
                str += ",@vCnpjDest";        //21
                                             //  str += ", @vNrEmbarque";    //24
                str += ",@vId_Remetente ";          //25
                str += ", @vNrNfEdi";       //26
                str += ", @DtEntrada";    //27
                str += ", @chave";      //34
                str += ",@vSerieNf";           //36 
                str += ",@DtFinalEntrega";
                str += ",@fisjur";
                str += ",@cod_municipio)";
                var p = new DynamicParameters();
                var agora = DateTime.Now;

                p.Add("@vId_cliente", dadosNfe.Id_cliente);
                p.Add("@vNrNf1", dadosNfe.Nf_Wms);
                p.Add("@tp", 0);
                p.Add("@vPeso", 0);
                String CnpjEntrada = _config.GetSection("Cnpj_entrada_Wms").Value;
                if (CnpjEntrada == dadosNfe.Head.Cnpj_destino)
                {
                    p.Add("@remessa", "I");
                }
                else
                {
                    p.Add("@remessa", "O");
                }
                p.Add("@vRazaoDest", dadosNfe.Head.Nome_destino.ToUpper());
                p.Add("@vEndDest", $"{dadosNfe.Head.Endereco_destino}/{dadosNfe.Head.Cidade_destino}/{dadosNfe.Head.Estado_destino}{new String(' ', 80)}".Substring(0, 80).Trim().ToUpper());
                p.Add("@vQtdVol", dadosNfe.ListaItens.Sum(x => x.Qtd));
                p.Add("@vValorCobrado", dadosNfe.Head.Valor_Total);
                p.Add("@vDtEmissao", dadosNfe.Head.Data_emissao);
                p.Add("@vCnpjDest", dadosNfe.Head.Cnpj_destino);


                p.Add("@vId_Remetente", dadosNfe.Id_rementente);
                p.Add("@vNrNfEdi", dadosNfe.Head.Nr_original_cliente);

                p.Add("@DtEntrada", agora.ToString("yyyy-MM-dd HH:mm:ss"));
                var ch = dadosNfe.Head.Chave.Length == 44 ? dadosNfe.Head.Chave : new String('0', 44);

                p.Add("@chave", ch);
                p.Add("@vSerieNf", dadosNfe.Head.Serie);
                p.Add("@DtFinalEntrega", DateTime.Now.AddDays(dias));

                p.Add("@fisjur", dadosNfe.Head.Fis_jur_destino);

                p.Add("@cod_municipio", dadosNfe.Head.Id_Cidade_destino);


                if (nota.Id_cliente== 2216) //cepera
                {
                    var listaCepera = TrocaSkuCepera(nota.ListaItens);

                    if (listaCepera.Count>0)
                    {
                        nota.ListaItens = listaCepera;
                    }

                }


                // criar o sql do nf_cros_dock

                String str2 = "INSERT INTO `cdoca`.`nf_cros_dock` ";
                str2 += " (peso_br,";
                str2 += " `nr_nf`,";
                str2 += " `data_emissao`,";
                str2 += " `destino`,";
                str2 += " `id_cliente`,";
                str2 += " `sku`,";
                str2 += " `qtd`,";
                str2 += " `p_unit`,";
                str2 += " `descricao`,";
                str2 += " `cnpj_dest`,";
                str2 += " `ean`,";
                str2 += " `NrNf_cliente`,";
                str2 += " `und`)";
                str2 += " VALUES ";

                int cont = 0;
                foreach (var iteml in nota.ListaItens)
                {
                    if (cont > 0) { str2 += ","; }
                    
                    str2 += $"('{dadosNfe.Head.Peso_brt.ToString().Replace(".", ",")}',";
                    str2 += $" '{dadosNfe.Nf_Wms}',";
                    str2 += $"'{dadosNfe.Head.Data_emissao.ToString("dd/MM/yyyy")}',";
                    str2 += $"'{dadosNfe.Head.Nome_destino}',";
                    str2 += $" {dadosNfe.Id_cliente} ,";
                    str2 += $"'{iteml.Sku.Trim().ToUpper()}',";
                    str2 += $"'{iteml.Qtd.ToString().Replace(".", ",")}',";

                    var pu = iteml.Punit == null ? 0 : iteml.Punit;

                    str2 += $"{pu.ToString().Replace(",", ".")},";
                    str2 += $"'{iteml.Descricao_prod}',";
                    str2 += $"'{dadosNfe.Head.Cnpj_destino}',";
                    str2 += $"'{iteml.Ean}',";
                    str2 += $" {dadosNfe.Head.Nr_original_cliente} ,";

                    str2 += $"'{iteml.Especie}')";


                    cont++;
                }

                if (nota.ListaItens.Count != cont)
                {
                    return Retorna;
                }

                String Str3 = $"{str};{str2}";


                //criar a transação

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {



                    connection.Open();
                    using (var trans = connection.BeginTransaction())
                    {


                        try
                        {
                            connection.Execute(Str3, p, transaction: trans);
                            trans.Commit();

                            Retorna = "OK";
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Erro em Inserir pedido DAl Rollback: {ex.Message}");
                            trans.Rollback();
                        }


                    }


                    Retorna = "OK";
                }


            }
            catch (Exception ex)
            {

                Retorna = "Erro";
                _logger.LogError(ex.Message, "Erro em InserirNfe");

            }

            return Retorna;
        }
        List<NfeItens> TrocaSkuCepera(List<NfeItens> listaIni)
        {
            var listaFinal = new List<NfeItens>();
            var listaCepera = ListaCeprera();

            foreach (var item in listaIni)
            {
                var itemFinal = new NfeItens();
                try
                {
                    var itemCepera = listaCepera.Where(x => x.Sku_atual == item.Sku).FirstOrDefault();
                   
                    if (itemCepera != null)
                    {
                        itemFinal = item;
                        itemFinal.Sku = itemCepera.Sku_old;
                        itemFinal.Descricao_prod = $"*{itemCepera.Sku_atual}* - {item.Descricao_prod}";
                        

                    }
                    else
                    {
                        itemFinal = item;
                    }
                }
                catch (Exception)
                {

                    itemFinal = item;
                }
                listaFinal.Add(itemFinal);
            }

            return listaFinal;
        }

        List<SkuCepera> ListaCeprera()
        {
            var _retorno = new List<SkuCepera>();

            String str = "SELECT `sku_cepera`.`Sku_old`,";
                str += "`sku_cepera`.`Sku_atual`,";
                str += "`sku_cepera`.`Descricao`";
                str += " FROM `cdoca`.`sku_cepera`;";
                try
                {

                    using (IDbConnection connection = new MySqlConnection(connMysql))
                    {
                        _retorno = connection.Query<SkuCepera>(str).ToList();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, "Erro em PesquisaNfe");
                }

                return _retorno;

             

        }


        public String PesquisaNfe(Int32 id_cliente, String nr_nf)
        {
            String _retorno = null;

            String str = "Select nf_nf from cdoca.nfs WHERE nf_nf = @nf and id_cliente=@id_cliente";
            var p = new DynamicParameters();
            p.Add("@nf", nr_nf);
            p.Add("@id_cliente", id_cliente);
            try
            {

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<String>(str, p).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em PesquisaNfe");
            }

            return _retorno;

        }

        public List<nfeCdoca> ListaNfe(Int32 dias, String tipo, Int32 oc)
        {
            var _retorno = new List<nfeCdoca>();

            var p = new DynamicParameters();
            p.Add("vDias", dias);
            p.Add("vOrdCarga", oc);
            p.Add("vTipo", tipo);

            try
            {

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<nfeCdoca>("listas_nfe", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em PesquisaNfe");
            }

            return _retorno;

        }

        public List<nfeCdoca> ListarNfeSaidaCdoca(Int32 id_cliente, String dt_ini, String dt_fim)
        {
            var _retorno = new List<nfeCdoca>();

            var p = new DynamicParameters();
            p.Add("vIdCliente", id_cliente);
            p.Add("vDtIni", dt_ini);
            p.Add("vDtFim", dt_fim);

            try
            {

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<nfeCdoca>("listar_nfe_cdoca", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em PesquisaNfe Cdoca");
            }

            return _retorno;

        }

        public List<nfeCdoca> ListarNfeSaidaCdocaPorNf(String vNrNf)
        {
            var _retorno = new List<nfeCdoca>();

            var p = new DynamicParameters();
            p.Add("vNrNf", vNrNf);

            try
            {

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<nfeCdoca>("listar_nfe_CrossDock", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em PesquisaNfe Cdoca por nfe");
            }

            return _retorno;

        }







        public List<NfeCrossDock> BuscaItensListaSeparacaoDal(String filtro)
        {
            var _retorno = new List<NfeCrossDock>();

            var p = new DynamicParameters();
            p.Add("vNfs", filtro);
          

            try
            {

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<NfeCrossDock>("lista_Sep_bulk2", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em PesquisaNfe");
            }

            return _retorno;


        }


        public List<NfeCrossDock> ListaNfCrossDockDal(String filtro)
        {
            var lista = new List<NfeCrossDock>();
            String SrtSQL_a = " SELECT `nf_cros_dock`.`id_cliente`,";

            SrtSQL_a += " `tab_cliente`.`nome_cli`,";
            SrtSQL_a += " `nf_cros_dock`.`nr_nf` Notas, ";
            SrtSQL_a += " `nf_cros_dock`.`data_emissao`, ";
            SrtSQL_a += " `nf_cros_dock`.`destino`, ";
            SrtSQL_a += " `nf_cros_dock`.`peso_br` peso_item,";

            SrtSQL_a += " `nf_cros_dock`.`sku`, ";
            SrtSQL_a += " `nf_cros_dock`.`descricao`, ";
            SrtSQL_a  += " `nf_cros_dock`.`qtd` Qtd_item, ";
            SrtSQL_a  += " `tab_cod_auxiliar`.`cod_aux`,";

            SrtSQL_a  += " `nf_cros_dock`.`ean` ,";

            SrtSQL_a += " `nf_cros_dock`.`und` ";
            SrtSQL_a += "  FROM cdoca.`nf_cros_dock` ";

            SrtSQL_a += "  Join cdoca.`nfs` on  `nf_cros_dock`.`nr_nf` = `nfs`.nf_nf and  `nf_cros_dock`.`id_cliente`=`nfs`.`id_cliente`";

            SrtSQL_a += "  join cdoca.`tab_cliente` on ";

            SrtSQL_a  += "  `nf_cros_dock`.`id_cliente`=`tab_cliente`.`id_cliente`";
            SrtSQL_a  += "  Left Join cdoca.`tab_cod_auxiliar` on ";

            SrtSQL_a  += "  `nf_cros_dock`.`sku`=`tab_cod_auxiliar`.`sku` ";

            SrtSQL_a += $" WHERE  `nfs`.id in ({filtro})";

            try
            {

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    lista = connection.Query<NfeCrossDock>(SrtSQL_a).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro em Listar nf crossDock");
            }


            return lista;
        }
        #endregion

        #region Transportadora
        public IEnumerable<ListaGenericaDTO> Listar_transportadora()
        {
            var cs = new List<ListaGenericaDTO>();

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<ListaGenericaDTO>("Listar_transportadora", commandType: CommandType.StoredProcedure).ToList();

                    cs = retorno;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Transportadora: " + ex.Message);
            }
            return cs;
        }
        #endregion

        #region Veiculos
        public IEnumerable<ListaGenericaDTO> Listar_tipo_veiculo()
        {
            var cs = new List<ListaGenericaDTO>();

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<ListaGenericaDTO>("Listar_tipo_veiculo", commandType: CommandType.StoredProcedure).ToList();

                    cs = retorno;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar Tipo veiculo: " + ex.Message);
            }
            return cs;
        }
        #endregion

        #region OrdCarga
        public IEnumerable<ListaGenericaDTO> Listar_status_oc()
        {
            var cs = new List<ListaGenericaDTO>();

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<ListaGenericaDTO>("listar_stat_evento", commandType: CommandType.StoredProcedure).ToList();

                    cs = retorno;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao Listar status oc: " + ex.Message);
            }
            return cs;
        }

        public Ordem_CargaDTO Pesquisa_Orde_Carga( int id)
        {
            Ordem_CargaDTO oc = new Ordem_CargaDTO();
            var p = new DynamicParameters();

          
            p.Add("vId", id);
         
            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<Ordem_CargaDTO>("Pesquisar_ord_carga", p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    oc = retorno;
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, "Erro ao listar ordem de carga");
            }

            return oc;
        }

        public List<Ordem_CargaDTO> Lista_Orde_Carga(string dt_ini, string dt_fim, int status, int tp, string io)
        {
            List<Ordem_CargaDTO> lista = new List<Ordem_CargaDTO>();
            var p = new DynamicParameters();

            p.Add("vdt_ini", dt_ini);
            p.Add("vdt_fim", dt_fim);
            p.Add("vStatEvento", status);
            p.Add("vTp", tp);
            p.Add("vIo", io);
            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<Ordem_CargaDTO>("listar_ord_carga", p, commandType: CommandType.StoredProcedure).ToList();

                    lista = retorno;
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, "Erro ao listar ordem de carga");
            }

            return lista;
        }






        #endregion

        #region Posicao

        public IEnumerable<PosicaoDTO> Listar_Posicoes()
        {
            var itens = new List<PosicaoDTO>();

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    var retorno = conection.Query<PosicaoDTO>("listar_posicoes", commandType: CommandType.StoredProcedure).ToList();

                    itens = retorno;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao Listar posicaoes {ex.Message}");

            }

            return itens;
        }



        #endregion

        #region Estoque
        public IEnumerable<EstoqueDTO> ListaEstoqueProduto(Int32? id_produto, Int32? id_cliente)
        {
            var _retorno = new List<EstoqueDTO>();

            try
            {


                var p = new DynamicParameters();
                p.Add("vId_cliente", id_cliente);
                p.Add("vId_produto", id_produto);

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<EstoqueDTO>("Listar_Estoque", p, commandType: CommandType.StoredProcedure).ToList();

                }



            }
            catch (Exception ex)
            {

                _logger.LogError($"Erro em ListaEstoqueProduto: {ex.Message}");
            }

            return _retorno;
        }

        public String BaixaEstoque(List<EstoqueDTO> itens, String pedido, Int32 id_cliente, Int32 id_pedido)
        {
            String resp = "OK";

            String pedidostr = $"'{pedido}'";
            try
            {
                //============================= Edita Ordem_cli ===================================
                String str = "UPDATE  solae.ordem_cli ";

                str += " SET Qtd_Saida = (CASE id_cod  ";

                var cont = "";

                foreach (var item in itens)
                {

                    str += $"  WHEN {item.id} THEN Qtd_Saida+{item.Qtd_baixar.ToString().Replace(',', '.')} ";

                    if (cont != "")
                    {
                        cont += ",";
                    }
                    cont += $"{item.id}";
                }
                str += "END) ";
                str += $" WHERE id_cod IN({cont})";

                //================================= Insere Mov_ord ================================================================================
                var nr_ped_saida = CapturaNrPedido();
                var data_saida = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                String Str1 = "INSERT INTO solae.mov_ord ";
                Str1 += " ( nr_nf_ent, ";
                Str1 += " nr_nf_saida, ";
                Str1 += " qtd_ent, ";
                Str1 += " data_saida, ";
                Str1 += " qtd_saida, ";
                Str1 += " validade,";
                Str1 += " operador, ";
                Str1 += " obs, ";
                Str1 += " posicao, ";
                Str1 += " sku, ";
                Str1 += " lote, ";
                Str1 += " tipo_mov, ";
                Str1 += " id_cliente, ";
                Str1 += " p_unit, ";
                Str1 += " data_reg, ";
                Str1 += " nr_documento, ";
                Str1 += " id_Ordem_cli, ";
                Str1 += " icms_ent, ";
                Str1 += " Id_produto) ";

                Str1 += "VALUES ";

                var cont1 = 0;

                foreach (var iteml in itens)
                {
                    if (cont1 > 0) { Str1 += ","; }

                    Str1 += $"('{iteml.Nr_nf}',";
                    Str1 += $"'{nr_ped_saida}',";
                    Str1 += "0,";
                    Str1 += $"'{data_saida}',";
                    Str1 += $"{iteml.Qtd_baixar.ToString().Replace(',', '.')},";
                    Str1 += $"'{iteml.Validade.ToString("dd/MM/yyyy")}',";
                    Str1 += "'ROBO_Wms',";
                    Str1 += "'Saida de produtos',";
                    Str1 += $"'{iteml.Posicao}',";
                    Str1 += $"'{iteml.Sku}',";
                    Str1 += $"'{iteml.Lote}',";
                    Str1 += "'Saida',";
                    Str1 += $"{iteml.Id_cliente},";
                    Str1 += $"{iteml.P_unit.ToString().Replace(',', '.')},";
                    Str1 += "Now(),";
                    Str1 += $"{pedidostr},";
                    Str1 += $"{iteml.id},";
                    Str1 += "0,";
                    Str1 += $"{iteml.Id_produto})";


                    cont1++;
                }

                //  var ped = PesquisaPedidosProcessadosDal(pedidostr, id_cliente).FirstOrDefault();

                String Str4 = "Update solae.nf_cli_head SET processado='S',";
                Str4 += $" nf_pelog= '{nr_ped_saida}',";
                Str4 += " dt_processado=current_timestamp()";
                Str4 += $" WHERE indice = {id_pedido}";

                String Str5 = "Update solae.nf_cli_item SET processado='S',";
                Str5 += $" nf_pelog = '{nr_ped_saida}'";
                Str5 += $" WHERE Id_head = {id_pedido}";


                String Str2 = "UPDATE solae.nr_nfiscal SET nr_nf= nr_nf+1 WHERE cod=1";




                String Str3 = $"{str};{Str1};{Str2};{Str4};{Str5}";

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {

                    connection.Open();
                    using (var trans = connection.BeginTransaction())
                    {


                        try
                        {
                            connection.Execute(Str3, null, commandTimeout: 60, transaction: trans);
                            trans.Commit();

                            resp = "OK";


                        }
                        catch (Exception ex)
                        {

                            trans.Rollback();
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"Erro em BaixaEstoque: {ex.Message}");
            }

            return resp;
        }

        public String BaixaEstoqueBulk(List<EstoqueDTO> itens)
        {
            //  String vOperador = HttpContext.Session.GetString("UsrLogado") == null ? "" : HttpContext.Session.GetString("UsrLogado").ToString();
            String resp = "OK";
            String id_pedido = "0";
            String pedidostr = $"";
            var dadosUsr = Global.Dados_user;
            try
            {
                //============================= Edita Ordem_cli ===================================
                String str = "UPDATE  solae.ordem_cli ";

                str += " SET Qtd_Saida = (CASE id_cod  ";

                var cont = "";

                foreach (var item in itens)
                {

                    str += $"  WHEN {item.id} THEN Qtd_Saida+{item.Qtd_baixar.ToString().Replace(',', '.')} ";

                    if (cont != "")
                    {
                        cont += ",";
                    }
                    cont += $"{item.id}";
                }
                str += "END) ";
                str += $" WHERE id_cod IN({cont})";

                var Nr_ped_group = itens.GroupBy(x => x.Ped_cli).ToList();
                //================================= Insere Mov_ord ================================================================================

                var data_saida = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                String Str1 = "INSERT INTO solae.mov_ord ";
                Str1 += " ( nr_nf_ent, ";
                Str1 += " nr_nf_saida, ";
                Str1 += " qtd_ent, ";
                Str1 += " data_saida, ";
                Str1 += " qtd_saida, ";
                Str1 += " validade,";
                Str1 += " operador, ";
                Str1 += " obs, ";
                Str1 += " posicao, ";
                Str1 += " sku, ";
                Str1 += " lote, ";
                Str1 += " tipo_mov, ";
                Str1 += " id_cliente, ";
                Str1 += " p_unit, ";
                Str1 += " data_reg, ";
                Str1 += " nr_documento, ";
                Str1 += " id_Ordem_cli, ";
                Str1 += " icms_ent, ";
                Str1 += " Id_produto) ";

                Str1 += "VALUES ";
                String Str4 = "";
                String Str5 = "";
                String Str2 = "";

                var nr_ped_saida = CapturaNrPedido();
                var cont2 = 0;
                var peds = "";
                var nrPeds = "";
                var movPed = 1;
                foreach (var pedido in Nr_ped_group)
                {
                    
                    var itensPed = itens.Where(x => x.Ped_cli == pedido.Key).ToList();
                    var cont1 = 0;

                    foreach (var iteml in itensPed)
                    {

                        if (cont1 > 0)
                        {
                            Str1 += ",";
                        }
                        else { id_pedido = iteml.Id_Head_item != null ? iteml.Id_Head_item.ToString() : "0"; }

                        Str1 += $"('{iteml.Nr_nf}',";
                        Str1 += $"'{nr_ped_saida}',";
                        Str1 += "0,";
                        Str1 += $"'{data_saida}',";
                        Str1 += $"{iteml.Qtd_baixar.ToString().Replace(',', '.')},";
                        Str1 += $"'{iteml.Validade.ToString("dd/MM/yyyy")}',";
                        Str1 += $"'{dadosUsr.NomeUsuario}',";
                        Str1 += "'Saida de produtos',";
                        Str1 += $"'{iteml.Posicao}',";
                        Str1 += $"'{iteml.Sku}',";
                        Str1 += $"'{iteml.Lote}',";
                        Str1 += "'Saida',";
                        Str1 += $"{iteml.Id_cliente},";
                        Str1 += $"{iteml.P_unit.ToString().Replace(',', '.')},";
                        Str1 += "Now(),";
                        Str1 += $"'{iteml.Ped_cli}',";
                        Str1 += $"{iteml.id},";
                        Str1 += "0,";
                        Str1 += $"{iteml.Id_produto})";


                        cont1++;




                    }
                    //========================== Atualiza os dados em nf_Cli_head e Item
                    //  var ped = PesquisaPedidosProcessadosDal(pedidostr, id_cliente).FirstOrDefault();

                    if(Nr_ped_group.Count()> movPed)
                        Str1 += ",";
                    movPed++;
                    if (cont2 > 0) { peds += ","; nrPeds += ",";  }
                    peds += $"{ nr_ped_saida.ToString()}|{id_pedido}";
                    nrPeds += $"{ id_pedido}";
                    nr_ped_saida++;
                    cont2++;
                }

                var ped = peds.Split(',');
                Str4 = "Update solae.nf_cli_head SET ";
                Str4 += " processado = CASE indice ";
                String Str4a = $" nf_pelog = CASE indice ";
                String Str4b = $" dt_processado = CASE indice ";

                Str5 = "Update solae.nf_cli_item SET ";
                Str5 += " processado = CASE Id_head ";
                String Str5a = $" nf_pelog = CASE Id_head ";

                foreach (var item in ped)
                {
                    var dd = item.Split('|');
                    Str4 += $" WHEN {dd[1]} THEN 'S' ";
                    Str4a += $" WHEN {dd[1]} THEN '{dd[0]}' ";
                    Str4b += $" WHEN {dd[1]} THEN  current_timestamp() ";

                    Str5 += $" WHEN {dd[1]} THEN 'S' ";
                    Str5a += $" WHEN {dd[1]} THEN '{dd[0]}' ";

                }
                Str4 += " END, ";

                Str4 += $"{Str4a} END,";
                Str4 += $"{Str4b} END ";
                Str4 += $" WHERE indice IN ({nrPeds}) ";

                Str5 += " END, ";
                Str5 += $"{Str5a} END ";
                Str5 += $" WHERE Id_head IN ({nrPeds}) ";



                
                Str2 = $"UPDATE solae.nr_nfiscal SET nr_nf= {nr_ped_saida} WHERE cod=1 ";




                String Str3 = $"{str};{Str1};{Str2};{Str4};{Str5}";

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {

                    connection.Open();
                    using (var trans = connection.BeginTransaction())
                    {


                        try
                        {
                            connection.Execute(Str3, null, commandTimeout: 60, transaction: trans);
                            trans.Commit();

                            resp = "OK";


                        }
                        catch (Exception ex)
                        {

                            trans.Rollback();
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"Erro em BaixaEstoque: {ex.Message}");
            }

            return resp;
        }





        public IEnumerable<EstoqueDTO> ListaEstoqueEstagio(String sku, Int32? id_cliente, String posicao, String lote, String pos05, String saldo)
        {


            var _retorno = new List<EstoqueDTO>();

            try
            {


                var p = new DynamicParameters();
                p.Add("vIdCliente", id_cliente);
                p.Add("vSku", sku);

                p.Add("vPosicao", posicao);
                p.Add("vLote", lote);

                p.Add("vPos05", pos05);
                p.Add("vSaldo", saldo);





                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<EstoqueDTO>("lista_estoque_estagio", p, commandType: CommandType.StoredProcedure).ToList();

                }



            }
            catch (Exception ex)
            {

                _logger.LogError($"Erro em ListaEstoqueEstagio: {ex.Message}");
            }

            return _retorno;


        }

        /// <summary>
        /// Pesquisa os itens em estoque pela chave do pedido de entrada
        /// </summary>
        /// <param name="id_pedido"></param>
        /// <returns></returns>
        public IEnumerable<EstoqueDTO> PesquisaPedidoEstoque(Int32 id_pedido)
        {
            IEnumerable<EstoqueDTO> pedido = new List<EstoqueDTO>();
            var p = new DynamicParameters();

            p.Add("vIdHead", id_pedido);

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    pedido = conection.Query<EstoqueDTO>("PesquisaPedidoEstoque", p, commandType: CommandType.StoredProcedure);


                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao pesquisar pedido");

            }

            return pedido;

        }

        public Int32 GetNewOrderNr()
        {
            Int32 resp = 0;
            var p = new DynamicParameters();
            p.Add("novoValor", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {


                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("CapturaNrOrdem",
                      p, commandType: CommandType.StoredProcedure);

                    resp = p.Get<int>("novoValor");

                }

            }
            catch (Exception ex)
            {

                var err = ex.Message;
            }
            return resp;

        }

        public String InsertEstoqueBulk(List<EstoqueDTO> itens)
        {
            String resp = "Erro|0";
            var json = JsonConvert.SerializeObject(itens);

            var cont = 0;
            var p = new DynamicParameters();

            p.Add("json", json);



            p.Add("cont", dbType: DbType.Int32, direction: ParameterDirection.Output);


            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("inserir_bulk_Estoque",
                      p, commandType: CommandType.StoredProcedure);

                    cont = p.Get<int>("cont");

                    resp = $"OK|{cont}";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro em InsertEstoqueBulk {ex.Message}");

            }





            return resp;
        }

        public String AtualizaEstoqueItem(EstoqueDTO iten)
        {
            String resp = "Erro|0";

            if (String.IsNullOrWhiteSpace(iten.Operador))
            {
                var dadosUsr = Global.Dados_user;
                iten.Operador = dadosUsr.NomeUsuario;
            }



            var json = JsonConvert.SerializeObject(iten);
            var cont = 0;
            var p = new DynamicParameters();

            p.Add("json", json);
            p.Add("msg", dbType: DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                using (var conection = new MySqlConnection(connMysql))
                {
                    conection.ExecuteScalar<int>("inserir_lote_validade",
                      p, commandType: CommandType.StoredProcedure);

                    cont = p.Get<int>("msg");

                    resp = $"OK|{cont}";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao Ao Atualizar lote validade {ex.Message}");

            }

            return resp;
        }

        public EstoqueDTO PesquisaEstoqueIdItem(Int32 id)
        {
            EstoqueDTO reg = new EstoqueDTO();
            var p = new DynamicParameters();

            p.Add("vId", id);

            try
            {
                using (var conection = new MySqlConnection(connMysql))

                {
                    reg = conection.Query<EstoqueDTO>("PesquisaRegistroEstoque", p, commandType: CommandType.StoredProcedure).FirstOrDefault();


                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Erro ao pesquisar pedido");

            }

            return reg;

        }


        #endregion

        #region Pelog consultas montadas em outro programa que podem ser utilizadas.

        //================================ Pedidos Pelog  ======================================



        //================================ Destino Pelog  ======================================
        public DestinoDTO PesquisaDestinoDal(String filtro, String fisJur)
        {
            var _retorno = new DestinoDTO();


            try
            {


                String str = "SELECT `CODIGO` as Id_destino ,";
                str += " `NOME` as nome";

                str += " FROM cdoca.tab_destino";
                if (fisJur.ToUpper() == "F")
                {
                    str += $" Where `CPF` = '{filtro}'";
                }
                else
                {
                    str += $" Where `CNPJ` = '{filtro}'"; ;
                }


                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<DestinoDTO>(str).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro em PesquisaDestinoDal: {ex.Message}");
            }

            return _retorno;
        }

        public String InsereDestinoDal(List<DestinoDTO> lista)
        {
            String resp = "Erro";

            try
            {


                if (lista.Count() > 0 && !String.IsNullOrEmpty(connMysql))
                {

                    String Str = "INSERT INTO cdoca.tab_destino ";
                    Str += " ( CNPJ, ";
                    Str += " CPF, ";
                    Str += " NOME, ";
                    Str += " FANTASIA, ";
                    Str += " ENDERECO,";
                    Str += " NUMERO, ";
                    Str += " COMPLEMENTO, ";
                    Str += " BAIRRO, ";
                    Str += " MUNICIPIO, ";
                    Str += " ESTADO, ";
                    Str += " CEP, ";
                    Str += " TELEFONE, ";
                    Str += " CONTATO, ";
                    Str += " EMAIL, ";
                    Str += " FISJUR, ";
                    Str += " INSCREST, ";
                    Str += " DATACADASTRO,";
                    Str += " CFOP,";
                    Str += " ALICOTA,";
                    Str += " COD_MUNICIPIO,";
                    Str += " OPERADOR)";
                    Str += "VALUES";

                    var cont = 0;

                    foreach (var iteml in lista)
                    {
                        if (cont > 0) { Str += ","; }
                        if (iteml.tipo_pessoa == "J") { Str += $" ('{iteml.cpf_cnpj}',"; } else { Str += " (null,"; }
                        if (iteml.tipo_pessoa == "F") { Str += $"'{iteml.cpf_cnpj}',"; } else { Str += " null,"; }
                        Str += $"'{iteml.nome.Trim().ToUpper()}',";
                        Str += $"'{iteml.nome.ToUpper() + new String(' ', 20).Substring(0, 20).Trim()}',";
                        Str += $"'{iteml.endereco.Trim().ToUpper()}',";
                        Str += $"'{iteml.numero.Trim().ToUpper() + new String(' ', 10).Substring(0, 10).Trim()}',";
                        Str += $"'{iteml.complemento.Trim().ToUpper()}',";
                        Str += $"'{iteml.bairro.Trim().ToUpper()}',";
                        Str += $"'{iteml.cidade.Trim().ToUpper()}',";
                        Str += $"'{iteml.uf.Trim().ToUpper()}',";
                        Str += $"'{iteml.cep.Trim().ToUpper()}',";
                        Str += $"'{iteml.fone.Trim().ToUpper()}',";
                        Str += $"'{iteml.nome.ToUpper() + new String(' ', 20).Substring(0, 20).Trim()}',";
                        Str += $"'{iteml.email.Trim().ToUpper()}',";
                        Str += $"'{iteml.tipo_pessoa.Trim().ToUpper()}',";
                        if (iteml.tipo_pessoa == "J") { Str += $" '{iteml.ie}',"; } else { Str += "'ISENTO',"; }


                        Str += " Now(),";
                        Str += "'5352',";
                        Str += " 0.12,";
                        Str += $"{iteml.Cod_municipio},";
                        Str += " 'ROBO_API_SK')";

                        cont++;
                    }
                    Str += $" ON DUPLICATE KEY UPDATE DATACADASTRO = VALUES(DATACADASTRO)";

                    using (IDbConnection connection = new MySqlConnection(connMysql))
                    {
                        connection.Execute(Str);
                    }
                    resp = "OK";
                }
            }
            catch (Exception ex)
            {
                resp = "Erro";
                _logger.LogError($"Erro em InsereDestinoDal: {ex.Message}");
            }



            return resp;

        }

        //================================= Cidades ===================================

        public MunicipiosDTO PesquisarCidadeDal(String cidade, String estado)
        {
            String str = " SELECT CODIN_MUNICIPIO as Id ,codigo as Cod_municipio FROM cdoca.tabmunicipio ";
            str += $" WHERE UPPER(municipio) ='{Funcoes.RemoveAccents(cidade.Trim()).ToUpper()}' and uf='{estado.Trim().ToUpper()}'";


            var _retorno = new MunicipiosDTO();

            try
            {
                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<MunicipiosDTO>(str).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro em PesquisarCidadeDal: {ex.Message}");
            }

            return _retorno;
        }








        private Int32 CapturaNrPedido()
        {
            Int32 resp = 0;

            String str = "Select nr_nf from solae.nr_nfiscal where cod=1";
            try
            {
                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    resp = connection.Query<Int32>(str).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Erro em CapturaNrPedido: {ex.Message}");
            }

            return resp;
        }

        public List<PedidoDTO> PesquisaPedidosProcessadosDal(String filtro, Int32 id_cliente)
        {
            var _retorno = new List<PedidoDTO>();


            try
            {


                String str = "SELECT `nr_documento` as Nr_pedido ,";
                str += " `id_cliente` as Id_cliente,";
                str += " `nome_cli` as nome, ";
                str += " `indice` as Indice ";
                str += " FROM nf_cli_head";
                str += $" Where `nr_documento` in ({filtro}) and id_cliente={id_cliente} and remessa='O' and cancelado='N'";

                using (IDbConnection connection = new MySqlConnection(connMysql))
                {
                    _retorno = connection.Query<PedidoDTO>(str).ToList();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro em PesquisaPedidosProcessadosDal: {ex.Message}");
            }

            return _retorno;
        }


        #endregion


    }
}
