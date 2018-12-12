using BoletoBrPrint.Bancos;
using BoletoBrPrint.Interfaces;
namespace BoletoBrPrint
{
    public static class BoletoFabrica
    {
        public static IGeradorBoleto Criar(string codigoBanco)
        {
            var modeloImpressao = codigoBanco.ConverterParaEnumerador();

            var config = new BoletoConfigurar(modeloImpressao);
            
            return new BoletoGeradoPadrao(config);
        }
    }
}
