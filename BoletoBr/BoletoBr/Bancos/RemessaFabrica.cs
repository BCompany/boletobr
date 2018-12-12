using BoletoBr.Arquivo.CNAB400.Remessa;
using BoletoBr.Bancos.Bradesco;
using BoletoBr.Bancos.Itau;
using BoletoBr.Bancos.Santander;
using BoletoBr.Interfaces;
using System;

namespace BoletoBr.Bancos
{
    public static class RemessaFabrica
    {
        public static IEscritorArquivoRemessaCnab400 GerarArquivo(string codigoBanco, RemessaCnab400 remessa)
        {
            var modelo = codigoBanco.ConverterParaEnumerador();

            var config = new RemessaConfigurar(modelo, remessa);

            switch (modelo)
            {
                case ModeloImplementacao.Bradesco:
                    return new EscritorRemessaCnab400Bradesco(remessa);

                case ModeloImplementacao.Itau:
                    return new EscritorRemessaCnab400Itau(remessa);

                case ModeloImplementacao.Santander:
                    return new EscritorRemessaCnab400Santander(remessa);

                default:
                    throw new ArgumentException("O arquivo de remessa não esta disponível para o banco código:" + remessa.Header.CodigoBanco);
            }
        }
    }
}