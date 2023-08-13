using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtuaDTO;
using VirtuaRepository;

namespace VirtuaBusiness.Cliente
{
    public class ClienteBLL : IclienteBLL
    {
        IDataConnection _db;
        public ClienteBLL(IDataConnection db)
        {
            _db = db;
        }

        public ClienteDTO Pesquisar_Cliente(int id)
        {
            return _db.Listar_Cliente(id).FirstOrDefault();
        }

        public ClienteDTO Pesquisar_ClienteCnpj(String Cnpj)
        {

            return _db.Listar_Cliente(null).Where(x=>x.Cnpj== Cnpj).FirstOrDefault();
        }

        public List<ClienteDTO> Listar_Cliente()
        {
            return _db.Listar_Cliente(null);
        }

        public int Edita_Inseri_Cliente(ClienteDTO cliente)
        {
            return _db.Edita_Inseri_Cliente(cliente);
        }
    }
}
