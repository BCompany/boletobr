using BoletoBr.Arquivo.CNAB400.Remessa;

namespace BoletoBr.Bancos
{
    public class RemessaConfigurar
    {
        private readonly ModeloImplementacao modelo;
        private readonly RemessaCnab400 remessa;

        public RemessaConfigurar(ModeloImplementacao modelo, RemessaCnab400 remessa)
        {
            this.modelo = modelo;
            this.remessa = remessa;
        }

        private void CustomizarModelo()
        {
            switch (modelo)
            {
                case ModeloImplementacao.Bradesco:


                    break;

                case ModeloImplementacao.Itau:
                    //not necessary specific configuration
                    break;

                case ModeloImplementacao.Santander:
                    //not necessary specific configuration
                    break;

                default:
                    break;
            }
        }
    }
}
