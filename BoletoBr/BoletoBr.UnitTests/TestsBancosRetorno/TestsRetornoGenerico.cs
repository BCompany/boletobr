using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BoletoBr.Bancos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoBr.UnitTests.Tests.BancosRetorno
{
    [TestClass]
    public class TestsRetornoGenerico
    {
        [TestMethod]
        public void TestHeaderArquivoRetornoBancoHsbcCarteiraCnr()
        {
            var retorno = RetornoFabrica.Criar("341");

            retorno.ProcessarArquivo(new List<string>());
        }
    }
}

