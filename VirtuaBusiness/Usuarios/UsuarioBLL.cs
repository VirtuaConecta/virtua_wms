using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virtua.Utilities;
using VirtuaDTO;
using VirtuaRepository;


namespace VirtuaBusiness.Usuarios
{
    public class UsuarioBLL : IusuarioBLL
    {
        private IDataConnection _data;
        private ILogger<UsuarioBLL> _log;
        public UsuarioBLL(IDataConnection db , ILogger<UsuarioBLL> log)
        {
            _log = log;
            _data = db;
        }
        public IEnumerable<UsuarioDTO> Listar_Usauario()
        {
            return _data.Listar_Usauario();
        }

        public Int32 Edita_Inseri_Usuario(UsuarioDTO usuario)
        {
            return _data.Edita_Inseri_Usuario(usuario);
        }

        public UsuarioDTO Pesquisar_Login_Usuario(string login)
        {
            return _data.Pesquisar_Login_Usuario(login);
        }

        public Boolean AutenticaUsr(string login, string password)
        {
            //O Default da senha é false
            Boolean eValido = false;

            try
            {

                //transforma a senha vida da tela em Md5
                if (!String.IsNullOrWhiteSpace(login) && !String.IsNullOrWhiteSpace(password))
                {

                    var crypyo = MD5Crypto.getMD5Hash(password);


                    //Retorna dd Repositorio a senha do respectivo login
                    UsuarioDTO DadosUsr = _data.Pesquisar_Login_Usuario(login);



                    if (DadosUsr != null && DadosUsr.Senha !=null)
                    {
                        //Se a senha digitada na tela em Md5 for igual a retornada pelo db entao retorna True
                        if (DadosUsr != null && crypyo.ToString() == DadosUsr.Senha.ToString().ToUpper())
                        {
                            eValido = true;
                        } 
                    }


                }
            }
            catch (Exception ex)
            {

                _log.LogError($"Erro em AutenticaUsr {ex.Message} ");
            }

            return eValido;

        }

        public Boolean ValidaCpf(string cpf)
        {
            var valida = false;

            var user = _data.Listar_Usauario().Where(u => u.Cpf == cpf).FirstOrDefault();
            if (user != null)
                valida = true;

            return valida;
        }

        public Boolean ValidaUsuario(string usuario)
        {
            var valida = false;

            var user = _data.Listar_Usauario().Where(u => u.Login == usuario).FirstOrDefault();
            if (user != null)
                valida = true;

            return valida;
        }

        public Boolean ValidaEmail(string email)
        {
            var valida = false;

            var user = _data.Listar_Usauario().Where(u => u.Email == email).FirstOrDefault();
            if (user != null)
                valida = true;

            return valida;
        }
    }
}
