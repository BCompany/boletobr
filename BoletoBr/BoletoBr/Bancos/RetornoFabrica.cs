using BoletoBr.Arquivo.Generico.Retorno;
using BoletoBr.Bancos.Bradesco;
using BoletoBr.Bancos.Brasil;
using BoletoBr.Bancos.Itau;
using BoletoBr.Bancos.Santander;
using System;
using System.Collections.Generic;

namespace BoletoBr.Bancos
{
    public static class RetornoFabrica
    {
        public static ITargetAdapter Criar(string codigoBanco)
        {
            var modelo = codigoBanco.ConverterParaModeloImplementacao();

            switch (modelo)
            {
                case ModeloImplementacao.Bradesco:
                    return new BradescoRetornoAdapter();

                case ModeloImplementacao.Itau:
                    return new ItauRetornoAdapter();

                case ModeloImplementacao.Santander:
                    return new SantanderRetornoAdapter();

                case ModeloImplementacao.BancoBrasil:
                    return new BancoBrasilRetornoAdapter();

                default:
                    throw new ArgumentException("O arquivo de retorno não esta disponível para o banco código:" + codigoBanco);
            }
        }
    }

    #region Adapter

   //Adapter é um Design Pattern utilizado quando se existem diferentes classes com comportamentos diferentes
   //Que precisam ser reutilizadas sem duplicação de código, para isso criamos uma interface comum entre elas que implementam uma unica interface para processar o arquivo

    //Interface que será consumida pelo GOJUR
    public interface ITargetAdapter
    {
        RetornoGenerico ProcessarArquivo(List<string> fileLines);
    }

    //Implementação para o Santander
    class SantanderRetornoAdapter: ITargetAdapter
    {
        public RetornoGenerico ProcessarArquivo(List<string> fileLines)
        {
            var fileSantander = new BancoSantander();

            return fileSantander.LerArquivoRetornoLiquidacao(fileLines);
        }
    }

    //Implementação para o Itau
    class ItauRetornoAdapter : ITargetAdapter
    {
        public RetornoGenerico ProcessarArquivo(List<string> fileLines)
        {
            var fileItau = new BancoItau();

            return fileItau.LerArquivoRetornoLiquidacao(fileLines);
        }
    }

    //Implementação para o Bradesco
    class BradescoRetornoAdapter : ITargetAdapter
    {
        public RetornoGenerico ProcessarArquivo(List<string> fileLines)
        {
            var fileBradesco = new BancoBradesco();

            return fileBradesco.LerArquivoRetornoLiquidacao(fileLines);
        }
    }

    //Implementação para o Banco do Brasil
    class BancoBrasilRetornoAdapter : ITargetAdapter
    {
        public RetornoGenerico ProcessarArquivo(List<string> fileLines)
        {
            var fileBancoBrasil = new BancoBrasil();

            return fileBancoBrasil.LerArquivoRetornoLiquidacao(fileLines);
        }
    }

    #endregion
}
