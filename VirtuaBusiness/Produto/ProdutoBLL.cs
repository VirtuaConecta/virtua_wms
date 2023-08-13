
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Produto

{
    public class ProdutoBLL : IprodutoBLL
    {

        IDataConnection _db;
        public ProdutoBLL(IDataConnection db)
        {
            _db = db;
        }

        public IEnumerable<ProdutosDTO> PesquisarProdutoBLL(Int32? id_produto, Int32? id_cliente, String sku)
        {
            return _db.PesquisarProdutosDal(id_produto, id_cliente, sku);
        }

        public Int32 InsereProdutoDoPedido(NfeItens item, Int32 id_cliente)
        {


            var retorno = _db.InserProdutoDoArquivoDal(item, id_cliente);


            return retorno;
        }

        public Int32 InserProdutoDal(ProdutosDTO prod, Int32 cli)
        {
            return _db.InserProdutoDal(prod, cli);
        }

        public Int32 EditaProduto(ProdutosDTO prod, Int32 cli)
        {
            return _db.EditaProduto(prod, cli);
        }
        public List<ListaGenericaDTO> Listar_Embalagem()
        {
            return _db.Listar_Embalagem();
        }

        public IEnumerable<ProdutosDTO>  CarregaListaAutoSku(String term,Int32 Id)
        {
            var lista= _db.PesquisarProdutosDal(null, Id, null).Where(x => x.Descricao.Contains(term)).ToList();
            return lista;
        }

    }
}
