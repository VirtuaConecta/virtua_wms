using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;
using VirtuaBusiness.Pedidos;

namespace VirtuaBusiness.Estoque
{
    public class EstoqueBLL : IEstoqueBll
    {
        private readonly ILogger<EstoqueBLL> logger;
        private readonly IConfiguration config;
        private readonly IDataConnection db;
        private readonly IpedidoBLL pedidos;

        public EstoqueBLL(ILogger<EstoqueBLL> logger, IConfiguration config, IDataConnection db)
        {
            this.logger = logger;
            this.config = config;
            this.db = db;
         
        }

        public IEnumerable<EstoqueDTO> ListaEstoqueProdutoBll(Int32? id_produto, Int32? id_cliente)
        {

            return db.ListaEstoqueProduto(id_produto, id_cliente);

        }

        public IEnumerable<EstoqueDTO> ListaEstoqueEstagioBll(String Sku, Int32? IdCliente, String Posicao, String Lote,
            String Pos05, String Saldo)
        {
            return db.ListaEstoqueEstagio(Sku, IdCliente, Posicao, Lote, Pos05, Saldo);
        }


        public String BaixaEstoqueBll(Nfe pedido)
        {

            String resp = "OK";
            var nr_pedido = pedido.Nf_Wms;
            var id_cliente = pedido.Id_cliente;
            var listaItens = pedido.ListaItens;


            var id_pedido = db.Pesquisa_Pedido(nr_pedido, id_cliente, null).indice;

            try
            {
                var listaEst = new List<EstoqueDTO>();
                foreach (var item in listaItens)
                {
                    var itemEstoque = new EstoqueDTO();
                    var qtd = item.Qtd;

                    if (item.Id_Produto > 0)
                    {

                        //Busca o SKU para o cliente
                        var prod = ListaEstoqueProdutoBll(item.Id_Produto, id_cliente).Where(x => x.Posicao.Substring(0, 2) != "05").ToList();


                        //totaliza o SKU
                        var SaldoDisponivel = SomaProduto(prod, item.Id_Produto);

                        //caso Haja qtd suficiente 
                        if (SaldoDisponivel >= qtd)
                        {

                            foreach (var itemprod in prod)
                            {

                                // Se um unico registro contiver o Saldo
                                if (itemprod.Saldo >= qtd)
                                {
                                    itemEstoque = itemprod;
                                    itemEstoque.Qtd_baixar = Convert.ToInt32(qtd); //valor a baixar
                                    qtd = 0; // Marca zero para sair
                                }
                                else if (itemprod.Saldo < qtd) // precisará completar com outro registro no porx loop
                                {
                                    itemEstoque = itemprod;
                                    itemEstoque.Qtd_baixar = itemprod.Saldo;
                                    qtd = qtd - itemprod.Saldo;


                                }
                                listaEst.Add(itemEstoque);
                                if (qtd == 0)
                                {
                                    break;
                                }


                            }


                        }
                        else
                        {
                            resp = "Falta_saldo";
                            logger.LogError("Erro em baixar pedido: Falta Saldo");
                            return resp;
                        }

                    }
                    else
                    {
                        logger.LogError("Erro em baixar pedido: Id do produto = 0");
                    }

                }
                // executar a baixa
                // salvar a movimentação
                // Encerrar o pedido
                var baixa = db.BaixaEstoque(listaEst, nr_pedido, id_cliente, id_pedido);




            }
            catch (Exception ex)
            {
                resp = ex.Message;

                logger.LogError($"Erro em baixar pedido: {ex.Message}");
            }


            return resp;


        }

        public Decimal SomaProduto(List<EstoqueDTO> lista, Int32 idProduto)
        {
            Decimal valor = 0;
            try
            {


                if (lista.Count > 0)
                {
                    var valores = (from o in lista
                                   where o.Id_produto == idProduto
                                   group o by new { o.Id_produto } into g
                                   select new
                                   {
                                       Id_produto = g.Key.Id_produto,
                                       Saldo = g.Sum(x => x.Saldo)

                                   }).FirstOrDefault();
                    valor = valores.Saldo;
                }
            }
            catch (Exception ex)
            {
                var e = ex.Message;

            }

            return valor;


        }

        /// <summary>
        /// Pesquisa o pedido pelo id do head do pedido trazendo os itens
        /// </summary>
        /// <param name="id_pedido"></param>
        /// <returns></returns>
        public IEnumerable<EstoqueDTO> PesquisaPedidoEstoqueBLL(Int32 id_pedido)
        {
            return db.PesquisaPedidoEstoque(id_pedido);
        }

        public Int32 CapturaNrOrdem()
        {
            return db.GetNewOrderNr();


        }

        public String InserirEstoqueBulkBLL(List<EstoqueDTO> lista)
        {


            return db.InsertEstoqueBulk(lista);

        }


        public String InserirLoteValidade(EstoqueDTO Item, List<EstoqueDTO> lista)
        {
            //Sucesso
            String StRetorno = "OK";

            if (String.IsNullOrEmpty(Item.Lote) == true)
            {
                StRetorno = "Lote vazio";
            }
            else if (Item.Qtd_entrada==0)
            {
                StRetorno = "Quantidade de Entrada /r/n não foi escolhida";
            }
            else if (String.IsNullOrEmpty(Item.Validade.ToString()) == true)
            {
                StRetorno = "Validade não foi definda";
            }
             else // Se as validações estão corretas fazer a inserção do Lote/Validade
            {
                             
                //Se encontrou cadastro do produto
                if (Item.Id_produto>0)
                {
                   
                    Item.Obs = "Alterado Lote/Validade";
                    //Item.Posicao = "01000000";
                    Item.Data_entrada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Item.Data_entrada2 = DateTime.Now;

                    //Captura qtd total do item do pedido e qtd entrada
                    var itemSelected = lista.Where(x => x.Id_Head_item == Item.Id_Head_item).ToList();
                   
                    var resultado = itemSelected.GroupBy(c => c.Id_Head_item)
                        .Select(gp => new
                        {
                            id = gp.Key,
                            total_ped = gp.Sum(c => c.Qtd_ped),
                            total_ent = gp.Sum(c => c.Qtd_entrada)
                        }).Single();
                    //define a qtd disponível para entrada
                    var qtd_ped = resultado.total_ped - resultado.total_ent;

                    Item.Qtd_ped = qtd_ped;
                    Item.Dt_registro = DateTime.Now;
                    if (Item.Qtd_entrada > Item.Qtd_ped)
                    {
                        return StRetorno = $"Quantidade de Entrada maior que qdt do pedido faltante: {Item.Qtd_ped}";
                    }

                    var resp = db.AtualizaEstoqueItem(Item).Split('|');

                    if (resp[0]=="OK")
                    {
                        StRetorno = "OK";
                    }
                    else
                    {
                        StRetorno = "Erro ao Atualizar DB";
                    }

                }
                else
                {
                    StRetorno = "Produto não encontrado";
                }
            }

            return StRetorno;

        }


        public EstoqueDTO CapturaRegistroEstoquePorIDBll(Int32 id)
        {
            return db.PesquisaEstoqueIdItem(id);

        }


    }
}
