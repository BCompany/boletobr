using BoletoBrPrint.Bancos;
using BoletoBrPrint.Interfaces;
using BoletoBr;

namespace BoletoBrPrint
{
    public static class BoletoFabrica
    {
        public static IGeradorBoleto Criar(string codigoBanco)
        {
            var modelo = codigoBanco.ConverterParaEnumerador();

            var config = new BoletoConfigurar(modelo);
            
            return new BoletoGeradoPadrao(config);
        }
    }
}
