using BoletoBrPrint.Generico;
using BoletoBrPrint.Interfaces;

namespace BoletoBrPrint
{
    public static class BoletoFabrica
    {
        public static IGeradorBoleto Criar(ModeloImpressao modeloImpressao)
        {
            return new BoletoPadrao(new BancoBoleto(modeloImpressao));
        }
    }
}
