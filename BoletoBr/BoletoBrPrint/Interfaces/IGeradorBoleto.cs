﻿using BoletoBr;
using System;

namespace BoletoBrPrint.Interfaces
{
    public interface IGeradorBoleto
    {
        void EmitirBoleto(Boleto document, string path, string fileName);
    }
}