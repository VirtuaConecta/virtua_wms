using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaBusiness.Estoque;
using VirtuaBusiness.Nfs_cdoca;
using VirtuaBusiness.Produto;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Pedidos
{
    public class PedidoBll : IpedidoBLL
    {
        IDataConnection _db;
        IConfiguration _config;
        private IprodutoBLL _prod;
        ArquivoFactory _arq;
        InfsCdoca _nota;
        private readonly IEstoqueBll estoque;

        public ILogger<PedidoBll> Logger { get; }

        public PedidoBll(ILogger<PedidoBll> logger, IDataConnection db, IprodutoBLL prod, IConfiguration config,
            ArquivoFactory arq, InfsCdoca nota,IEstoqueBll estoque)
        {
            Logger = logger;
            _db = db;
            _prod = prod;
            _config = config;
            _arq = arq;
            _nota = nota;
            this.estoque = estoque;
        }


        public PedidoDTO Pesquisa_Pedido(String NrPed, Int32 id_cliente, Int32? id_pedido)
        {
        
            return _db.Pesquisa_Pedido(NrPed, id_cliente,id_pedido);
        }

        public IEnumerable<PedidoItemDTO> Listar_Itens_Ped(Int32 id_ped)
        {
            return _db.Listar_Itens_Ped(id_ped);
        }

        public IEnumerable<DocumentoClienteHeadDTO> Listar_Doc_Por_StatusBLL(String st)
        {
            return _db.Listar_Doc_Por_Status(st);
        }

        public IEnumerable<PedidoDTO> ListaPedidoPendente(String vRemessa, String vCancelado, String vProcessado)
        {
            return _db.ListaPedidoPendente(vRemessa, vCancelado, vProcessado);
        }

        public Int32 Edita_Inserir_PedidoBLL(PedidoDTO pedido, DestinoDTO dest)
        {
            return _db.Edita_Inserir_Pedido(pedido, dest);
        }

        public IEnumerable<Ped_entrada_saidaDTO> Listar_Ped_entrada_saida(int Id_cliente, DateTime Dt_fim, DateTime Dt_ini)
        {
            return _db.Listar_Ped_entrada_saida(Id_cliente, Dt_fim, Dt_ini);
        }
        public IEnumerable<PedidoDTO> ListaPedidoGeralBLL(String DtIni, String DtFim, String Remessa, Int32 idCliente)
        {
            return _db.ListaPedidoGeral(DtIni, DtFim, Remessa, idCliente);
        }


        public IEnumerable<PedidoItemDTO> ListaItensPedidosAgrupados (String notas)
        {

            if (!String.IsNullOrEmpty(notas))
            {

                var itens = notas.Split(',');
                int[] indices = new int[itens.Count()];

                for (int i = 0; i < itens.Count(); i++)
                {
                    indices[i] = Convert.ToInt32(itens[i]);
                }

                var envio = JsonConvert.SerializeObject(indices);

                return _db.Listar_Ped_itensAgrupados(envio);

            }
            return null;
        }
        public String InserePedidoDoArquivo(Nfe pedido)
        {//Inseri item no nfs tambem
            var id_cliente = pedido.Id_cliente;
            //verifca os itens do pedidos
            var novaLista = new List<NfeItens>();
            foreach (var item in pedido.ListaItens)
            {
                var idprod = _prod.PesquisarProdutoBLL(null, id_cliente, item.Sku).FirstOrDefault();

                if (idprod != null && idprod.Id_produto > 0)
                {
                    item.Id_Produto = idprod.Id_produto;
                }
                else
                {

                    var id = _prod.InsereProdutoDoPedido(item, id_cliente);
                    if (id > 0)
                    {
                        item.Id_Produto = id;
                    }
                    else
                    {
                        item.Id_Produto = 0;
                    }
                }


                novaLista.Add(item);

            }

            pedido.ListaItens = novaLista;

            return _db.InserePedidoDal(pedido);

        }

        public RetornoPedidoDTO ValidaPedManual(String Nr_doc, Int32 Id_cliente, String vOpera, String vIp)
        {
            RetornoPedidoDTO Retorno = new RetornoPedidoDTO();

            Int32 resultado = 0;
            //pesquisa por id_cliente e nr_documento
            Int32 Nr_ID = 0;
            var pedido = _db.Pesquisa_Pedido(Nr_doc, Id_cliente,null);

            if (pedido != null && !String.IsNullOrEmpty(pedido.Nr_documento))
            {
                
                    resultado = 4;
                    //resultado = "Pedidos Já existe no DB Operador: " + pedido.Operador;
                
            }
            Retorno.Retorno = resultado;
            return Retorno;
        }

        public string InsereArquivosDaPastaBll(List<string> listarArquivos, string localArquivos,String baixa)
        {
            String resp = null;
            try
            {

                var ArquivoOk = "S";

                if (listarArquivos.Count > 0)
                {
                    //valida se o Db esta conectando
                    // if (_ikeep.DbIsOn())
                    //  {

                    foreach (var item in listarArquivos)
                    {

                        var arq = item.ToString().Split('.');

                        var ext = arq[1].ToUpper().Replace(".", "");
                        var nomeArq = item;

                        if (ext == ".ZIP")
                        {
                            var arquivoDescopactado = Virtua.Utilities.Arquivos.DescompactaZIP(nomeArq, localArquivos);
                            if (arquivoDescopactado == "OK")
                            {
                                Virtua.Utilities.Arquivos.MoveZip(nomeArq, localArquivos);
                            }
                        }

                        String extensoes = _config.GetSection("Arquivos:Extensoes").Value;
                        String Cnpj_Wms = _config.GetSection("Cnpj_entrada_Wms").Value;
                        // Aqui acrescentamos todas as extensões que queremos inserir
                        if (extensoes.IndexOf(ext) > -1)
                        {
                            var lista_nfes = new List<Nfe>();
                            //formata dados do Arquivo
                            lista_nfes = _arq.CapturaDadosArquivoFactory(ext).InsereArquivo(localArquivos, nomeArq);

                            if (lista_nfes.Count() == 0)
                            {
                                ArquivoOk = "Erro";
                                var retornoMover = Virtua.Utilities.Arquivos.MoveArquivo(ArquivoOk, nomeArq, localArquivos);
                                resp = "Nenhum arquivo encontrado";
                            }

                            foreach (var ped in lista_nfes)
                            {

                                try
                                {

                                    //valida pedidos existentes
                                    var pedencontrado = Pesquisa_Pedido(ped.Nf_Wms, ped.Id_cliente,null);
                                    if (pedencontrado == null || String.IsNullOrEmpty(pedencontrado.Nr_documento))
                                    {


                                        //Insere o pedido
                                        ArquivoOk = InserePedidoDoArquivo(ped);

                                        String respB = null;
                                        //fazer a baixa de estoque
                                        if (ArquivoOk == "OK" && baixa=="S" && ped.Head.Cnpj_destino != Cnpj_Wms)
                                        {
                                          respB = estoque.BaixaEstoqueBll(ped);

                                        }

                                        String notaOK = null;
                                        //Inserir nf no cdoca nfs
                                        if (ArquivoOk == "OK")
                                        {  notaOK = _nota.InserirNfsBLL(ped);
                                            
                                        }

                               
                                        if(ArquivoOk == "OK"  && notaOK == "OK")
                                        { resp = "OK"; }
                                        else if (ArquivoOk != "OK")
                                        { resp = "Erro ao baixar pedido"; }
                                        else if (ArquivoOk == "OK" && notaOK != "OK")
                                        { resp = "Erro ao inserir nota no cdoca"; }
                                        

                                        if (baixa=="S" && respB != "OK")
                                        {
                                            resp = "Erro ao Baixar PEdido";
                                        }
                                    
                                    }
                                    else
                                    {
                                        //move o pedido
                                        ArquivoOk = "Erro";
                                        resp = "Pedido ja existe";
                                    }




                                    var retornoMover = Virtua.Utilities.Arquivos.MoveArquivo(ArquivoOk, nomeArq, localArquivos);

                                }
                                catch (Exception ex)
                                {
                                    resp = "Erro";
                                    //  Task.Delay(2000).Wait();
                                    Logger.LogError(ex.Message, "Erro em ImportarArquivosDoDiretorio");
                                }

                            }




                        }
                    }

                }
            }
            catch (Exception ex)
            {
                resp = "Erro";
                Logger.LogError(ex.Message, "Erro em InsereArquivosDaPastaBll");

            }


            return resp;
        }

        private String baixarEstoque(Nfe ped)
        {
            String resp = "Erro";

            try
            {
                resp = estoque.BaixaEstoqueBll(ped);
            }
            catch (Exception ex)
            {

                Logger.LogError($"Erro em baixarEstoque PedidoBll: {ex.Message}");
            }


           

            return resp;
        }

        public String Inserir_PedidoManualBLL(Nfe Tela)
        {
            // a model Nfe é usada para cria pedidos novos
            // a Model PedidoDTO é usada para manipular pedidos existentes

            String resp = "Erro|0";
            var pedExiste = Pesquisa_Pedido(Tela.Nf_Wms, Convert.ToInt32(Tela.Id_cliente),null);
            if (pedExiste != null && !String.IsNullOrWhiteSpace(pedExiste.Nr_documento)) // Pedido Existe
            {
                return "Pedido já existe|0";
            }
            else
            {
                var ret = InserePedidoDoArquivo(Tela);

                if (ret == "OK")
                    resp = $"OK|{ret}";
            }
            return resp; 
        }

        public String Inserir_PedidoTempItensBLL(List<PedidoItemDTO> itensPed)
        {
            var listaFim = new List<Nfe>();
            String resp = "Erro|0";
            try
            {
                var nfe = new Nfe();
                //var pedExiste = Pesquisa_Pedido(pedido.Nr_documento, Convert.ToInt32(pedido.Id_cliente));
                //if (pedExiste != null && !String.IsNullOrWhiteSpace(pedExiste.Nr_documento)) // Pedido Existe
                //{
                //    return "Pedido já existe|0";
                //}
                //else
                //{
                //  

                //    nfe.IdPed = Guid.NewGuid().ToString();
                //    nfe.Id_cliente = Convert.ToInt32(pedido.Id_cliente);
                //    nfe.Id_rementente = Convert.ToInt32(pedido.Id_remetente);
                //    nfe.Nf_Wms = pedido.Nr_documento;

                //    var localPedHead = new NfeHead();
                //    localPedHead.Nome_cliente = pedido.NomeCliente;
                //    localPedHead.Nr_original_cliente = pedido.Nr_doc_origem;
                //    localPedHead.Cnpj_cli = pedido.Cpf_cnpj_cliente;
                //    if (pedido.Chave == "0")
                //    {
                //        localPedHead.Chave = new String('0', 44);
                //    }
                //    else
                //    { localPedHead.Chave = pedido.Chave; }
                //    localPedHead.Especie = pedido.Especie;
                //    localPedHead.Fis_jur_destino = pedido.Fis_Jur;

                //    localPedHead.Nome_destino = dest.nome.ToUpper();
                //    localPedHead.Bairro_destino = dest.bairro.ToUpper();
                //    localPedHead.Cep_destino = dest.cep;
                //    //localPedHead.Cidade_destino = dest.cidade.ToUpper();
                //    localPedHead.Cnpj_destino = dest.cpf_cnpj;
                //    localPedHead.IE_destino = dest.ie.ToUpper();
                //    localPedHead.Endereco_destino = dest.endereco.ToUpper();
                //    //localPedHead.Estado_destino = dest.uf.ToUpper();
                //    localPedHead.Id_Cidade_destino = dest.id_cidade;


                //    nfe.Head = localPedHead;
                //    listaFim.Add(nfe);
                //    Global.PedidosTemporarios = listaFim;

                resp = $"OK|{ nfe.IdPed }";

              //  }






            }
            catch (Exception ex)
            {
                resp = $"{ex.Message}|0";
            }



            return resp; ;
        }
        public IEnumerable<PedidoItemDTO> ListaItensPedidoTemp(List<NfeItens> itens)
        {
            var lista = new  List<PedidoItemDTO>();
            foreach (var item in itens)
            {
                var reg = new PedidoItemDTO();

                reg.id_produto = Convert.ToString(item.Id_Produto);
                reg.Sku = item.Sku;
                reg.Qtd = item.Qtd;
                reg.Ncm = item.Ncm;
                reg.Descricao = item.Descricao_prod;

                lista.Add(reg);
            }




            return lista;
        }

     
        public String InserePedidoNoEstoqueBll(Int32 id_pedido,String remessa,String Operador)
        {
            String rsp = "erro|0";

            var posicao = "01000000";

            var PedidoEncontrado = estoque.PesquisaPedidoEstoqueBLL(id_pedido);

            var nr_ordem = estoque.CapturaNrOrdem();
            var dt_Ordem = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //Se não encontrar o pedido faz a entrada
            if (PedidoEncontrado.Count()==0)
            {
                var DadosEncontrado = Pesquisa_Pedido(null, 0, id_pedido);
                var ItensEncontrador = Listar_Itens_Ped(id_pedido);

            
                if (ItensEncontrador.Count() >0)
                {
                    List<EstoqueDTO> itensAentrar = new List<EstoqueDTO>();
                    foreach (var item in ItensEncontrador)
                    {
                        var itemEstoque = new EstoqueDTO();

                        itemEstoque.Id_cliente = Convert.ToInt32(DadosEncontrado.Id_cliente);
                        itemEstoque.Id_Head_item = item.Id;
                        itemEstoque.Id_produto = Convert.ToInt32(item.id_produto);
                        itemEstoque.Nr_nf = DadosEncontrado.Nr_documento;
                        itemEstoque.Qtd_ped = Convert.ToDecimal(item.Qtd);
                        itemEstoque.Posicao = posicao;
                        itemEstoque.Nr_Ordem = nr_ordem;
                        itemEstoque.Data_ordem = dt_Ordem;
                        itemEstoque.Sku = item.Sku;
                        itemEstoque.Dt_registro = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        itemEstoque.Tipo_ped = DadosEncontrado.tipo_doc;
                        itemEstoque.Operador = Operador;
                        itemEstoque.P_unit = item.P_unit == null?0 : Convert.ToDecimal(item.P_unit);
                        itemEstoque.Remessa = remessa;
                        itensAentrar.Add(itemEstoque);
                    }

                    // executar o insert do pedido

                    var itensIseridos = estoque.InserirEstoqueBulkBLL(itensAentrar).Split('|'); ;

                    if (itensIseridos[0]=="OK")
                    {
                        rsp = $"Foram inseridos {itensIseridos[1]} registros!|1";
                    }

                }
                else
                {
                    rsp = "Não foi encontrado item para o pedido!|0";
                }




               // rsp = "OK";
            }
            else
            {
                rsp = "Pedido já existe no sistema!|0";
            }



            return rsp;
        }

        /// <summary>
        /// Captura o pedido agrupado na tela e prepara/ executa a baixa
        /// </summary>
        /// <param name="ped"></param>
        /// <returns></returns>
        public String BaixarPedidoAgrupado(PedAgrupadoDTO ped)
        {
            String resposta = "erro";
            try
            {
                //Nr dos pedidos da baixa
                var pedidos_nr = ped.pedidos.Split(',');
                // registros de estoque co as qtd de baixa
                var itens = ped.items;

                // gera lista com todos os items do pedido 
                List<PedidoItemDTO> Pedidos = new List<PedidoItemDTO>() ;

                //montamos a lista que recebera os itens e qtd por pedido separadamente
                var list_itens_pedido_Mmv_estoque = new List<EstoqueDTO>();
                foreach (var nr_pedido in pedidos_nr)
                {
                    var ped1 = Convert.ToInt32(nr_pedido);
                    var ped_selecionado = Listar_Itens_Ped(ped1);

                    foreach (var item in ped_selecionado)
                    {
                        Pedidos.Add(item);
                    }
                   

                }

                // agrupa por id de produto
                var id_prod_group = Pedidos.GroupBy(x => x.id_produto).ToList();

                //montamos a lista que recebera os itens e qtd por pedido separadamente
                //var list_itens_pedido_Mmv_estoque = new List<EstoqueDTO>();

                //busca por cada id dos produtos agrapados
                foreach (var id_prod in id_prod_group)
                {
                    //Lista dos itens do pedido filtrada pelo id do produto
                    var lista_Id_prod = Pedidos.Where(x => x.id_produto == id_prod.Key).ToList();

                    //Filtra os registros selecionados na tela por id de produto
                    var registros_selecionados = itens.Where(x=>x.Id_produto.ToString() == id_prod.Key).OrderByDescending(y=>y.Qtd).ToList();

                    
                    //Varre os itens dos pedidos pelo id do produto
                    foreach (var item_produto_Pedido in lista_Id_prod)
                    {
                        var it = new EstoqueDTO();
                        //qtd no pedido para o id produto
                        var qtdPed = item_produto_Pedido.Qtd;
                        Decimal qtd_falta = Convert.ToDecimal(qtdPed);
                        var registros_selecionados_editado = new List<Item>(); 
                        registros_selecionados_editado = registros_selecionados;
                        foreach (var item in registros_selecionados_editado)
                        {
                            it = estoque.CapturaRegistroEstoquePorIDBll(Convert.ToInt32(item.Id));
                            it.Ped_cli = item_produto_Pedido.Nr_documento;
                            it.Tipo_ped = item_produto_Pedido.Tipo_doc;
                            it.Id_Head_item = item_produto_Pedido.Id_head;
                            if (item.Qtd_baixada <= item.Qtd)
                            {
                                var saldo_item = item.Qtd - item.Qtd_baixada;
                                if (saldo_item>0)
                                {

                                    if (qtd_falta > saldo_item)
                                    {
                                        it.Qtd_baixar = saldo_item;

                                        qtd_falta = qtd_falta - saldo_item;
                                        list_itens_pedido_Mmv_estoque.Add(it);
                                        registros_selecionados_editado = EditaLista(registros_selecionados_editado, item.Id, it.Qtd_baixar);
                                    }
                                    else
                                    {

                                        it.Qtd_baixar = qtd_falta;
                                        list_itens_pedido_Mmv_estoque.Add(it);
                                        registros_selecionados_editado = EditaLista(registros_selecionados_editado, item.Id, it.Qtd_baixar);
                                        break;
                                    }

                                }
                                //
                                

                            }
                            
                        }

                    }   



                }

                                
                var ped_final = list_itens_pedido_Mmv_estoque;

                resposta = _db.BaixaEstoqueBulk(ped_final);
                //Aqui tem que validar se os dados estão corretos e depois fazer a baixa



            }
            catch (Exception)
            {

                throw;
            }



            return resposta;
        }



        private List<Item> EditaLista(List<Item> lista,String id,Decimal qtd)
        {
            var lista_ret = new List<Item>();

            foreach (var item in lista)
            {
                var it_lista = new Item();
                it_lista = item;
                if (item.Id == id)
                {
                    it_lista.Qtd_baixada = it_lista.Qtd_baixada + qtd;
                }

                lista_ret.Add(it_lista);
            }


            return lista_ret;
        }

    }



}
