using iTextSharp.text;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace BoletoBrPrint
{
    public static class Utils
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

        public static ModeloImpressao ConverterParaEnumerador(this string codigoBanco)
        {
            switch (codigoBanco)
            {
                /* 001 - Banco do Brasil */
                case "001":
                    return ModeloImpressao.BancoDoBrasil;
                /* 033 - Banco Santander */
                case "033":
                    return ModeloImpressao.Santander;
                /* 104 - Caixa */
                case "104":
                    return ModeloImpressao.CEF;
                /* 237 - Bradesco */
                case "237":
                    return ModeloImpressao.Bradesco;
                /* 341 - Itaú */
                case "341":
                    return ModeloImpressao.Itau;
                default:
                    throw new NotImplementedException("Banco código " + codigoBanco + " ainda não foi implementado.");
            }
        }
    }

    public enum ModeloImpressao
    {
        [Description("BANCO DO BRASIL")]
        BancoDoBrasil,
        [Description("BRADESCO")]
        Bradesco,
        [Description("CAIXA ECONOMICA FEDERAL")]
        CEF,
        [Description("ITAU")]
        Itau,
        [Description("SANTANDER")]
        Santander
    }
}