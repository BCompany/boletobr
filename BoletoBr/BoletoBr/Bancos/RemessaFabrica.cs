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
        public static IEscritorArquivoRemessaCnab400 Criar(string codigoBanco)
        {
            var modelo = codigoBanco.ConverterParaModeloImplementacao();

            switch (modelo)
            {
                case ModeloImplementacao.Bradesco:
                    return new EscritorRemessaCnab400Bradesco();

                case ModeloImplementacao.Itau:
                    return new EscritorRemessaCnab400Itau();

                case ModeloImplementacao.Santander:
                    return new EscritorRemessaCnab400Santander();

                default:
                    throw new ArgumentException("O arquivo de remessa não esta disponível para o banco código:" + codigoBanco);
            }
        }
    }
}