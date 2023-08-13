using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Estoque
{
    public interface IEstoqueBll
    {
        IEnumerable<EstoqueDTO> ListaEstoqueProdutoBll(Int32? id_produto, Int32? id_cliente);
        String BaixaEstoqueBll(Nfe ped);
        IEnumerable<EstoqueDTO> ListaEstoqueEstagioBll(String Sku, Int32? IdCliente, String Posicao, String Lote,
           String Pos05, String Saldo);
        IEnumerable<EstoqueDTO> PesquisaPedidoEstoqueBLL(Int32 id_pedido);
        Int32 CapturaNrOrdem();
        String InserirEstoqueBulkBLL(List<EstoqueDTO> lista);

        String InserirLoteValidade(EstoqueDTO Item, List<EstoqueDTO> lista);
        EstoqueDTO CapturaRegistroEstoquePorIDBll(Int32 id);
    }
}
