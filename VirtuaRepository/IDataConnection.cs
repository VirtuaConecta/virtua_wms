using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using VirtuaDTO;

namespace VirtuaRepository
{
    public  interface IDataConnection
    {
        #region Clientes
       // ClienteDTO Pesquisar_Cliente(int id);
        List<ClienteDTO> Listar_Cliente(int? id);
        int Edita_Inseri_Cliente(ClienteDTO cliente);
       // ClienteDTO PesquisaClientesDal(String cnpj);
        #endregion

        #region Cfop
        IEnumerable<CfopDTO> Listar_Cfop();
        #endregion

        #region Usuario

        IEnumerable<UsuarioDTO> Listar_Usauario();
        Int32 Edita_Inseri_Usuario(UsuarioDTO usuario);

        UsuarioDTO Pesquisar_Login_Usuario(string login);
        #endregion

        #region Perfil
        IEnumerable<PerfilDTO> Listar_Perfil();
        Int32 Edita_Inseri_Perfil(PerfilDTO perfil);
        #endregion

        #region Pedido

        IEnumerable<PedidoItemDTO> Listar_Ped_itensAgrupados(String id_pedidos);
        IEnumerable<DocumentoClienteHeadDTO> Listar_Doc_Por_Status(String st);
        PedidoDTO Pesquisa_Pedido(String NrPed, Int32? id_cliente, Int32? id_pedido);
        IEnumerable<PedidoItemDTO> Listar_Itens_Ped(Int32 idHead);
        IEnumerable<PedidoDTO> ListaPedidoPendente(String vRemessa, String vCancelado, String vProcessado);
        Int32 Edita_Inserir_Pedido(PedidoDTO pedido, DestinoDTO dest);
        String InserePedidoDal(Nfe Pedido);
        IEnumerable<Ped_entrada_saidaDTO> Listar_Ped_entrada_saida(int Id_cliente, DateTime Dt_fim, DateTime Dt_ini);

        IEnumerable<PedidoDTO> ListaPedidoGeral(String DtIni, String DtFim, String Remessa, Int32 idCliente);
        #endregion

        #region Produtos
        IEnumerable<ProdutosDTO> PesquisarProdutosDal(Int32? id_produto, Int32? id_cliente, String sku);
        Int32 InserProdutoDal(ProdutosDTO prod, Int32 cli);
        Int32 InserProdutoDoArquivoDal(NfeItens prod, Int32 cli);
        Int32 EditaProduto(ProdutosDTO prod, Int32 cli);
        List<ListaGenericaDTO> Listar_Embalagem();
        #endregion

        #region Municipio

        IEnumerable<MunicipiosDTO> Listar_Municipios();
        #endregion

        #region Dash
        IEnumerable<ChartDTO> Dash_Pedido_operador(int dia);
        IEnumerable<ChartDTO> Dash_Volume_Cliente(int dia);
        #endregion

        #region Nfs
        String PesquisaNfe(Int32 id_cliente, String nr_nf);
        string InserirNfe(Nfe nota);
        List<nfeCdoca> ListaNfe(Int32 dias, String tipo, Int32 oc);
        List<nfeCdoca> ListarNfeSaidaCdoca(Int32 id_cliente, String dt_ini, String dt_fim);
        string InserirNfeCrossDock(Nfe nota);
        List<NfeCrossDock> BuscaItensListaSeparacaoDal(String filtro);
        List<nfeCdoca> ListarNfeSaidaCdocaPorNf(String vNrNf);
        List<NfeCrossDock> ListaNfCrossDockDal(String filtro);
        #endregion

        #region Estoque
        IEnumerable<EstoqueDTO> ListaEstoqueProduto(Int32? id_produto, Int32? id_cliente);
        String BaixaEstoque(List<EstoqueDTO> itens, String pedido, Int32 id_cliente, Int32 id_pedido);
        IEnumerable<EstoqueDTO> ListaEstoqueEstagio(String sku, Int32? id_cliente, String posicao, String lote, String pos05, String saldo);
        IEnumerable<EstoqueDTO> PesquisaPedidoEstoque(Int32 id_pedido);
        Int32 GetNewOrderNr();
        String InsertEstoqueBulk(List<EstoqueDTO> itens);
        String AtualizaEstoqueItem(EstoqueDTO iten);
        EstoqueDTO PesquisaEstoqueIdItem(Int32 id);
        String BaixaEstoqueBulk(List<EstoqueDTO> itens);
        #endregion

        #region Transportadora
        IEnumerable<ListaGenericaDTO> Listar_transportadora();
        #endregion
        #region Veiculos
        IEnumerable<ListaGenericaDTO> Listar_tipo_veiculo();
        #endregion

        #region OrdCarga
        IEnumerable<ListaGenericaDTO> Listar_status_oc();
        Ordem_CargaDTO Pesquisa_Orde_Carga(int id);
        List<Ordem_CargaDTO> Lista_Orde_Carga(string dt_ini, string dt_fim, int status, int tp, string io);
        #endregion
        #region Posicao
        IEnumerable<PosicaoDTO> Listar_Posicoes();
        #endregion

    }
}
