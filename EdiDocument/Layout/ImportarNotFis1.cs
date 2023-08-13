using EdiDocument.DTO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiDocument.Layout
{
    public class ImportarNotFis1 : NotFisBase
    {
        private IArquivos _arquivos;
        public ImportarNotFis1()
        {
            _arquivos = new Arquivos();
        }



        public override List<Nfe> CapturaNotFis(String Arquivo)
        {
            var pedidos = new List<Nfe>();


            var cabecalho = new NfeHead();
            var itemPedido = new List<NfeItens>();
            //Captura dados do Arquivo
            var linhas = _arquivos.LeArquivo(Arquivo);
            var nota = 0;
            try
            {
                Int32 nrDePedidos = ContaPedidos(linhas);

                //Se o arquivo tem linhas
                if (linhas.Count > 0)
                {
                    foreach (var item in linhas)
                    {

                        if (item.Substring(0, 3) == "000")
                        {
                            var dado = item.Substring(73, 6);
                            var dt = $"{dado.Substring(0, 2)}/{dado.Substring(2, 2)}/{dado.Substring(4, 2)}";
                            if (!String.IsNullOrWhiteSpace(item.Substring(3, 35).Trim()))
                                cabecalho.Nome_cliente = item.Substring(3, 35).Trim();

                        }
                        else if (item.Substring(0, 3) == "310")
                        {
                            if (!String.IsNullOrWhiteSpace(item.Substring(3, 14).Trim()))
                                cabecalho.Nr_doc_transporte = item.Substring(3, 14).Trim();
                        }
                        else if (item.Substring(0, 3) == "311") //dados cliente
                        {
                            var len = item.Length;
                            if (len > 29)
                            {
                                cabecalho.IE_cliente = item.Substring(17, 15).Trim();


                                cabecalho.Cnpj_cli = item.Substring(3, 14).Trim();
                            }
                            if (len > 133)
                            {

                                cabecalho.Endereco_cli = item.Substring(32, 40).Trim();
                                cabecalho.Cidade_cli = item.Substring(72, 35).Trim();
                                cabecalho.Cep_cli = item.Substring(107, 9).Trim();
                                cabecalho.Estado_cli = item.Substring(116, 9).Trim();

                                var dado = item.Substring(125, 8);
                                cabecalho.Data_embarque = Validacoes.ValidaData(dado, "ddmmyyyy");




                            }

                        }
                        else if (item.Substring(0, 3) == "312") // Dados destino
                        {
                            if (nota > 0)
                            {

                                var pedido = new Nfe();

                                pedido.Head = cabecalho;
                                pedido.ListaItens = itemPedido;

                                pedidos.Add(pedido);

                                cabecalho = new NfeHead();
                                cabecalho.Nome_cliente = pedido.Head.Nome_cliente;
                                cabecalho.Endereco_cli = pedido.Head.Endereco_cli;
                                cabecalho.Cidade_cli = pedido.Head.Cidade_cli;
                                cabecalho.Cep_cli = pedido.Head.Cep_cli;
                                cabecalho.Estado_cli = pedido.Head.Estado_cli;
                                cabecalho.IE_cliente = pedido.Head.IE_cliente;
                                cabecalho.Cnpj_cli = pedido.Head.Cnpj_cli;
                                cabecalho.Nr_doc_transporte = pedido.Head.Nr_doc_transporte;

                                itemPedido = new List<NfeItens>();
                            }




                            cabecalho.Nome_destino = item.Substring(3, 40).Trim();
                            cabecalho.Cnpj_destino = item.Substring(43, 14).Trim();
                            cabecalho.IE_destino = item.Substring(57, 15).Trim();
                            cabecalho.Endereco_destino = item.Substring(72, 40).Trim();

                            cabecalho.Bairro_destino = item.Substring(112, 20).Trim();
                            cabecalho.Cidade_destino = item.Substring(132, 35).Trim();
                            cabecalho.Cep_destino = item.Substring(167, 9).Trim();

                            var id_cidade = item.Substring(176, 9).Trim();
                            if (!String.IsNullOrWhiteSpace(id_cidade) && Information.IsNumeric(id_cidade))
                                cabecalho.Id_Cidade_destino = Convert.ToInt32(id_cidade);
                            cabecalho.Estado_destino = item.Substring(185, 9).Trim();

                            if (item.Length >= 235)
                            {
                                cabecalho.Telefone_destino = item.Substring(198, 35).Trim();

                                var fj = item.Substring(233, 1).Trim();
                                if (Information.IsNumeric(fj))
                                    cabecalho.Fis_jur_destino = fj == "1" ? "J" : "F"; //1 = CGC – CAD. GERAL DE CONTRIBUITES 2 = CPF – CAD. PESSOAS FÍSICAS
                            }

                            nota++;
                        }
                        else if (item.Substring(0, 3) == "313") // dados da nota
                        {
                            var nr = item.Substring(3, 15).Trim();
                            var rota = item.Substring(18, 7).Trim();
                            var meio = item.Substring(25, 1).Trim();
                            var ttrans = item.Substring(26, 1).Trim();
                            var tcarga = item.Substring(27, 1).Trim();
                            var ConFrete = item.Substring(28, 1).Trim();
                            var Especie = item.Substring(63, 15).Trim();
                            var QtdVol = item.Substring(78, 7).Trim();
                            var ValorT = item.Substring(85, 15).Trim();
                            var Pbruto = item.Substring(100, 7).Trim();
                            var Placas = item.Substring(144, 7).Trim();
                            var acao = item.Substring(212, 1).Trim();
                            var EspeciePed = item.Substring(63, 15).Trim();

                            cabecalho.Especie = !String.IsNullOrWhiteSpace(EspeciePed) ? EspeciePed : "VOLUME(S)";

                            if (!String.IsNullOrWhiteSpace(acao))
                                cabecalho.Acao_doc = acao == "I" ? "I-INCLUIR" : "E-EXCLUIR"; //1 = CGC – CAD. GERAL DE CONTRIBUITES 2 = CPF – CAD. PESSOAS FÍSICAS

                            if (Information.IsNumeric(nr))
                                cabecalho.Nr_romaneio = Convert.ToInt32(nr);

                            if (!String.IsNullOrWhiteSpace(rota))
                                cabecalho.Cod_rota =rota;

                            if (Information.IsNumeric(meio))
                                cabecalho.Meio_transporte = Validacoes.ValidaMeioTransporte(Convert.ToInt32(meio));

                            if (Information.IsNumeric(ttrans))
                                cabecalho.Meio_transporte = Validacoes.ValidaTipoTransporteCarga(Convert.ToInt32(ttrans));

                            if (Information.IsNumeric(tcarga))
                                cabecalho.Tipo_carga = Validacoes.ValidaTipoCarga(Convert.ToInt32(tcarga));

                            if (!String.IsNullOrWhiteSpace(ConFrete))
                                cabecalho.Cond_frete = Validacoes.ValidaCifFob(ConFrete);

                            cabecalho.Serie = item.Substring(29, 3).Trim();
                            cabecalho.Nr_original_cliente = item.Substring(32, 8).Trim();

                            var dt = item.Substring(40, 8).Trim();
                            cabecalho.Data_emissao = Validacoes.ValidaData(dt, "ddmmyyyy");

                            if (!String.IsNullOrWhiteSpace(Especie))
                                cabecalho.Especie = Especie;

                            if (!String.IsNullOrWhiteSpace(QtdVol))
                                cabecalho.Nr_volumes = Validacoes.ValidaStringDecimal(QtdVol, 5, 2);


                            if (Information.IsNumeric(ValorT))
                                cabecalho.Valor_Total = Validacoes.ValidaStringDecimal(ValorT, 5, 2);


                            if (Information.IsNumeric(Pbruto))
                                cabecalho.Peso_brt = Validacoes.ValidaStringDecimal(Pbruto, 5, 2);

                            if (!String.IsNullOrWhiteSpace(Placas))
                                cabecalho.Placas = Placas;

                            if (item.Length >= 282)
                            {
                                var chave = item.Substring(237, 44).Trim();

                                if (!String.IsNullOrWhiteSpace(chave) && Information.IsNumeric(chave) && chave.Length == 44)
                                {
                                    cabecalho.Chave = chave;
                                }
                                else
                                {
                                    cabecalho.Chave = new String('0', 44);
                                }


                            }
                            if (item.Length >= 283)
                            {
                                var td = item.Substring(281, 9).Trim();
                                cabecalho.Tipo_pedido = td;

                            }


                        }
                        else if (item.Substring(0, 3) == "314")
                        {

                            // dados dos itens da nota estão em 4 colunas de dados


                            for (int i = 0; i <= 3; i++)
                            {
                                // grupo1 
                                var qtd = item.Substring(3 + (52 * i), 7).Trim();
                                var esp = item.Substring(10 + (52 * i), 15).Trim();
                                var sku = item.Substring(25 + (52 * i), 30).Trim();


                                if (Information.IsNumeric(qtd) && Convert.ToDecimal(qtd) > 0)
                                {
                                    itemPedido.Add(new NfeItens
                                    {
                                        Qtd = Validacoes.ValidaStringDecimal(qtd, 7, 0),
                                        Especie = esp,
                                        Sku = sku
                                    });
                                }


                            }






                        }


                    }

                    if (nota == 1 || nota == nrDePedidos)
                    {

                        var pedido = new Nfe();
                        pedido.Head = cabecalho;
                        pedido.ListaItens = itemPedido;
                        pedidos.Add(pedido);


                    }


                }


            }
            catch (Exception ex)
            {

                var err = ex.Message;
            }





            return pedidos;
        }

        public override string GeraNotFis(string NomeDoc)
        {
            throw new NotImplementedException();
        }

        private Int32 ContaPedidos(List<string> lista)
        {
            int cont = 0;
            if (lista.Count > 0)
            {
                foreach (var linha in lista)
                {

                    if (linha.Substring(0, 3) == "313")
                    {
                        cont++;

                    }



                }

            }

            return cont;
        }

    }
}
