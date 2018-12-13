using BoletoBr;
using System;

namespace BoletoBrPrint.Interfaces
{
    public interface IGeradorBoleto
    {
        void GerarBoleto(Boleto document, string path, string fileName);
    }
}