using iTextSharp.text;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace BoletoBrPrint
{
    public static class UtilsExtensions
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