using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Produto
{
    public interface IprodutoBLL
    {
        IEnumerable<ProdutosDTO> PesquisarProdutoBLL(Int32? id_produto, Int32? id_cliente, String sku);
        Int32 InsereProdutoDoPedido(NfeItens item, Int32 id_cliente);
        Int32 InserProdutoDal(ProdutosDTO prod, Int32 cli);
        Int32 EditaProduto(ProdutosDTO prod, Int32 cli);
        List<ListaGenericaDTO> Listar_Embalagem();
        IEnumerable<ProdutosDTO> CarregaListaAutoSku(String term, Int32 Id);
    }
}
