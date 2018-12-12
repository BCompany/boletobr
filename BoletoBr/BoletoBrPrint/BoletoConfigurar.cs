using BoletoBr;
using iTextSharp.text;

namespace BoletoBrPrint
{
    public class BoletoConfigurar
    {
        public string NumeroBanco { get; private set; }
        public Image Logotipo { get; private set; }
        public string PagavelPreferencialmente { get; private set; }

        private readonly ModeloImplementacao modelo;

        public BoletoConfigurar(ModeloImplementacao modelo)
        {
            this.modelo = modelo;           

            this.CustomizarModelo();

            this.PagavelPreferencialmente = string.Join(" ", "LOCAL DE PAGAMENTO: PAGAVEL PREFERENCIALMENTE NO BANCO", modelo.EnumDescricao());
        }

        private void CustomizarModelo()
        {
            switch (this.modelo)
            {
                case ModeloImplementacao.Bradesco:
                    this.NumeroBanco = "237-2";
                    this.Logotipo = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoBradescoPNG);
                    break;

                case ModeloImplementacao.Itau:
                    this.NumeroBanco = "341-7";
                    this.Logotipo = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoItauPNG);
                    break;

                case ModeloImplementacao.Santander:
                    this.NumeroBanco = "033-7";
                    this.Logotipo = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoSantanderPNG);
                    break;

                default:
                    this.NumeroBanco = "000-0";
                    break;
            }
        }
    }
}
