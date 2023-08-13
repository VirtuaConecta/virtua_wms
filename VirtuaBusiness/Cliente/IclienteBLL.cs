using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaBusiness.Cliente
{
    public  interface IclienteBLL
    {
        ClienteDTO Pesquisar_Cliente(int id);
        List<ClienteDTO> Listar_Cliente();
        int Edita_Inseri_Cliente(ClienteDTO cliente);
        ClienteDTO Pesquisar_ClienteCnpj(String Cnpj);
    }
}
