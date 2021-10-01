using BoletoBr;
using System.IO;

namespace BoletoBrPrint.Interfaces
{
    public interface IGeradorBoleto
    {
        void GerarBoleto(Boleto document, string path, string fileName);
        //byte[] GerarBoleto(Boleto document);
        MemoryStream GerarBoleto(Boleto document, MemoryStream streamReport);
    }
}