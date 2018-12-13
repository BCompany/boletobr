using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace BoletoBr
{
    public static class BoletoExtensionsMethods
    {
        public static string EnumDescricao<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type tipo = e.GetType();
                Array valor = System.Enum.GetValues(tipo);

                foreach (int val in valor)
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memberInfo = tipo.GetMember(tipo.GetEnumName(val));

                        var descriptionAttribute = memberInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                            return descriptionAttribute.Description;
                    }
            }

            return "";
        }

        public static ModeloImplementacao ConverterParaModeloImplementacao(this string codigoBanco)
        {
            switch (codigoBanco)
            {
                case "033":
                    return ModeloImplementacao.Santander;
                case "237":
                    return ModeloImplementacao.Bradesco;
                case "341":
                    return ModeloImplementacao.Itau;
                default:
                    throw new NotImplementedException("Banco código " + codigoBanco + " ainda não foi implementado.");
            }
        }
    }

    public enum ModeloImplementacao
    {
        [Description("BRADESCO")]
        Bradesco,
        [Description("CAIXA ECONOMICA FEDERAL")]
        Itau,
        [Description("SANTANDER")]
        Santander
    }
}