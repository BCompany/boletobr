using iTextSharp.text;

namespace BoletoBrPrint
{
    public class BancoBoleto
    {
        public string NumeroBanco { get; private set; }
        public string PagavelPreferencialmente { get; private set; }
        public Image Logotipo { get; private set; }

        private ModeloImpressao modeloImpressao;

        public BancoBoleto(ModeloImpressao modeloImpressao)
        {
            this.modeloImpressao = modeloImpressao;           

            this.PreencherNumeroBanco();
            this.PreencherLogotipo();
            this.PreencherPreferenciaPagamento();
        }

        private void PreencherNumeroBanco()
        {
            switch (this.modeloImpressao)
            {
                case ModeloImpressao.BancoDoBrasil:
                    this.NumeroBanco = "001-9";
                    break;

                case ModeloImpressao.Bradesco:
                    this.NumeroBanco = "237-2";
                    break;

                case ModeloImpressao.CEF:
                    this.NumeroBanco = "104-0";
                    break;

                case ModeloImpressao.Itau:
                    this.NumeroBanco = "341-7";
                    break;

                case ModeloImpressao.Santander:
                    this.NumeroBanco = "033-7";
                    break;

                default:
                    this.NumeroBanco = "000-0";
                    break;
            }
        }

        private void PreencherLogotipo()
        {
            switch (this.modeloImpressao)
            {
                case ModeloImpressao.BancoDoBrasil:
                    this.Logotipo = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoBancoBrasilPNG);
                    break;

                case ModeloImpressao.Bradesco:
                    this.Logotipo = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoBradescoPNG);
                    break;

                case ModeloImpressao.CEF:
                    this.Logotipo = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoCaixaPNG);
                    break;

                case ModeloImpressao.Itau:
                    this.Logotipo = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoItauPNG);
                    break;

                case ModeloImpressao.Santander:
                    this.Logotipo = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoSantanderPNG);
                    break;

                default:
                    this.Logotipo = null;
                    break;
            }
        }

        private void PreencherPreferenciaPagamento()
        {
            this.PagavelPreferencialmente = string.Join(" ", "LOCAL DE PAGAMENTO: PAGAVEL PREFERENCIALMENTE NO BANCO", modeloImpressao.EnumDescricao());
        }
    }
}
