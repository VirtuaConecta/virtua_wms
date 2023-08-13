using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Pedidos
{
    public  interface IpedidoBLL
    {
   
        PedidoDTO Pesquisa_Pedido(String NrPed, Int32 id_cliente, Int32? id_pedido);
        IEnumerable<PedidoItemDTO> Listar_Itens_Ped(Int32 id_ped);
        IEnumerable<DocumentoClienteHeadDTO> Listar_Doc_Por_StatusBLL(String st);
        IEnumerable<PedidoDTO> ListaPedidoPendente(String vRemessa, String vCancelado, String vProcessado);
        Int32 Edita_Inserir_PedidoBLL(PedidoDTO pedido, DestinoDTO dest);
        RetornoPedidoDTO ValidaPedManual(String Nr_doc, Int32 Id_cliente, String vOpera, String vIp);
        String InserePedidoDoArquivo(Nfe pedido);
        IEnumerable<PedidoDTO> ListaPedidoGeralBLL(String DtIni, String DtFim, String Remessa, Int32 idCliente);
        IEnumerable<Ped_entrada_saidaDTO> Listar_Ped_entrada_saida(int Id_cliente, DateTime Dt_fim, DateTime Dt_ini);
        String InsereArquivosDaPastaBll(List<String> listarArquivos, String localArquivos, String baixa);
        String Inserir_PedidoManualBLL(Nfe tela);
        IEnumerable<PedidoItemDTO> ListaItensPedidoTemp(List<NfeItens> itens);
        String Inserir_PedidoTempItensBLL(List<PedidoItemDTO> itensPed);
        String InserePedidoNoEstoqueBll(Int32 id_pedido, String remessa, String Operador);
        IEnumerable<PedidoItemDTO> ListaItensPedidosAgrupados(String notas);

        String BaixarPedidoAgrupado(PedAgrupadoDTO ped);
    }
}
