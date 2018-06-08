using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoletoBr.Interfaces;
using BoletoBr.Arquivo.CNAB240.Retorno;

namespace BoletoBr.Bancos.Santander
{
    class LeitorRetornoCnab240Santander : ILeitorArquivoRetornoCnab240

    {

        private readonly List<string> _linhasArquivo;


        public LeitorRetornoCnab240Santander(List<string> linhasArquivo)
        {

            _linhasArquivo = linhasArquivo;
        }


        public RetornoCnab240 ProcessarRetorno()
        {


            /* Validações */
            #region Validações
            ValidaArquivoRetorno();
            #endregion

            var objetoRetorno = new RetornoCnab240();

            LoteRetornoCnab240 ultimoLoteIdentificado = null;
            DetalheRetornoCnab240 ultimoRegistroIdentificado = null;

            foreach (var linhaAtual in _linhasArquivo)
            {

                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "0")
                    objetoRetorno.Header = ObterHeader(linhaAtual);


                /* Header de Lote */
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "1")
                {
                    ultimoLoteIdentificado = new LoteRetornoCnab240();
                    
                    objetoRetorno.Lotes.Add(ultimoLoteIdentificado);

                    ultimoLoteIdentificado.HeaderLote = ObterHeaderLote(linhaAtual);
                }


                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "3")
                {
                    if (linhaAtual.ExtrairValorDaLinha(14, 14) == "T")
                    {
                        if (ultimoLoteIdentificado == null)
                            throw new Exception("Não foi encontrado header de lote para o segmento atual.");

                        ultimoRegistroIdentificado = new DetalheRetornoCnab240();
                        ultimoRegistroIdentificado.SegmentoT = ObterRegistrosDetalheT(linhaAtual);
                    }
                    if (linhaAtual.ExtrairValorDaLinha(14, 14) == "U")
                    {
                        if (ultimoLoteIdentificado == null)
                            throw new Exception("Não foi encontrado header de lote para o segmento atual.");

                        ultimoRegistroIdentificado.SegmentoU = ObterRegistrosDetalheU(linhaAtual);
                        ultimoLoteIdentificado.RegistrosDetalheSegmentos.Add(ultimoRegistroIdentificado);

                    }
                }


                /* Trailer de Lote */
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "5")
                    if (ultimoLoteIdentificado != null)
                        ultimoLoteIdentificado.TrailerLote = ObterTrailerLote(linhaAtual);

                /* Trailer de arquivo */
                if (linhaAtual.ExtrairValorDaLinha(8, 8) == "9")
                    objetoRetorno.Trailer = ObterTrailer(linhaAtual);

            }

            return objetoRetorno;
        }

        public void ValidaArquivoRetorno()
        {

            if (_linhasArquivo == null)
                throw new Exception("Dados do arquivo de retorno estão nulos. Impossível processar.");

            if (_linhasArquivo.Count <= 0)
                throw new Exception("Dados do arquivo de retorno não estão corretos. Impossível processar.");

            if (_linhasArquivo.Count < 3)
                throw new Exception("Dados do arquivo de retorno não contém o mínimo de 3 linhas. Impossível processar.");

            var qtdLinhasHeader =
                _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(8, 8) == "0");

            if (qtdLinhasHeader <= 0)
                throw new Exception("Não foi encontrado HEADER do arquivo de retorno.");

            if (qtdLinhasHeader > 1)
                throw new Exception("Não é permitido mais de um HEADER no arquivo de retorno.");

            var qtdLinhasHeaderLote = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(8, 8) == "1");

            if (qtdLinhasHeaderLote <= 0)
                throw new Exception("Não foi encontrado HEADER do arquivo de retorno.");

            if (qtdLinhasHeaderLote > 1)
                throw new Exception("Não é permitido mais de um HEADER no arquivo de retorno.");

            var qtdLinhasDetalheSegmentoT = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(14, 14) == "T");

            if (qtdLinhasDetalheSegmentoT <= 0)
                throw new Exception("Não foi encontrado DETALHE para o Segmento T no arquivo de retorno.");

            var qtdLinhasDetalheSegmentoU = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(14, 14) == "U");

            if (qtdLinhasDetalheSegmentoU <= 0)
                throw new Exception("Não foi encontrado DETALHE para o Segmento U no arquivo de retorno.");

            var qtdLinhasTrailerLote = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(8, 8) == "5");

            if (qtdLinhasTrailerLote <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailerLote > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");

            var qtdLinhasTrailer = _linhasArquivo.Count(wh => wh.ExtrairValorDaLinha(8, 8) == "9");

            if (qtdLinhasTrailer <= 0)
                throw new Exception("Não foi encontrado TRAILER do arquivo de retorno.");

            if (qtdLinhasTrailer > 1)
                throw new Exception("Não é permitido mais de um TRAILER no arquivo de retorno.");
        }

        public HeaderRetornoCnab240 ObterHeader(string linha)
        {

            var objetoRetorno = new HeaderRetornoCnab240
            {
                CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt(),
                LoteServico = linha.ExtrairValorDaLinha(4, 7),
                CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt(),
                TipoInscricaoEmpresa = linha.ExtrairValorDaLinha(17, 17).BoletoBrToInt(),
                NumeroInscricaoEmpresa = linha.ExtrairValorDaLinha(18, 32),
                CodigoAgencia = linha.ExtrairValorDaLinha(33, 36).BoletoBrToInt(),
                DvCodigoAgencia = linha.ExtrairValorDaLinha(37, 37),
                ContaCorrente = linha.ExtrairValorDaLinha(38, 46),
                DvContaCorrente = linha.ExtrairValorDaLinha(47, 47),
                Convenio = linha.ExtrairValorDaLinha(53, 61),
                NomeDoBeneficiario = linha.ExtrairValorDaLinha(73, 102),
                NomeDoBanco = linha.ExtrairValorDaLinha(103, 132),
                CodigoRemessaRetorno = linha.ExtrairValorDaLinha(143, 143).BoletoBrToInt(),
                DataGeracaoGravacao = Convert.ToDateTime(linha.ExtrairValorDaLinha(144, 151).ToDateTimeFromDdMmAa()),
                NumeroSequencial = linha.ExtrairValorDaLinha(158, 163).BoletoBrToInt(),
                VersaoLayout = linha.ExtrairValorDaLinha(164, 166),

            };

            return objetoRetorno;

        }


        public HeaderLoteRetornoCnab240 ObterHeaderLote(string linha)
        {
            var objetoRetorno = new HeaderLoteRetornoCnab240
            {
                CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt(),
                LoteServico = linha.ExtrairValorDaLinha(4, 7),
                CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt(),
                TipoOperacao = linha.ExtrairValorDaLinha(9, 9),
                TipoServico = linha.ExtrairValorDaLinha(10, 11).BoletoBrToInt(),
                VersaoLayoutLote = linha.ExtrairValorDaLinha(14, 16).BoletoBrToInt(),
                TipoInscricaoEmpresa = linha.ExtrairValorDaLinha(18, 18).BoletoBrToInt(),
                NumeroInscricaoEmpresa = linha.ExtrairValorDaLinha(19, 33),
                Convenio = linha.ExtrairValorDaLinha(34, 42),
                CodigoAgencia = linha.ExtrairValorDaLinha(54, 57).BoletoBrToInt(),
                DvCodigoAgencia = linha.ExtrairValorDaLinha(58, 58),
                ContaCorrente = linha.ExtrairValorDaLinha(59, 67),
                DvContaCorrente = linha.ExtrairValorDaLinha(68, 68),
                NomeDoBeneficiario = linha.ExtrairValorDaLinha(74, 103),
                NumeroRemessaRetorno = linha.ExtrairValorDaLinha(184, 191),
                DataGeracaoGravacao = Convert.ToDateTime(linha.ExtrairValorDaLinha(192, 199).ToDateTimeFromDdMmAa())
            };

            return objetoRetorno;
        }

        public DetalheSegmentoTRetornoCnab240 ObterRegistrosDetalheT(string linha)
        {

            var objetoRetorno = new DetalheSegmentoTRetornoCnab240
            {
                CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt(),
                LoteServico = linha.ExtrairValorDaLinha(4, 7),
                CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt(),
                NumeroRegistro = linha.ExtrairValorDaLinha(9, 13).BoletoBrToInt(),
                CodigoSegmento = linha.ExtrairValorDaLinha(14, 14),
                CodigoMovimento = linha.ExtrairValorDaLinha(16, 17).BoletoBrToInt(),
                Agencia = linha.ExtrairValorDaLinha(18, 21).BoletoBrToInt(),
                DigitoAgencia = linha.ExtrairValorDaLinha(22, 22),
                ContaCorrente = linha.ExtrairValorDaLinha(23, 31).BoletoBrToInt(),
                DigitoContaCorrente = linha.ExtrairValorDaLinha(32, 32),
                NossoNumero = linha.ExtrairValorDaLinha(41, 53),
                CodigoCarteira = linha.ExtrairValorDaLinha(54, 54).BoletoBrToInt(),
                NumeroDocumento = linha.ExtrairValorDaLinha(55, 69),
                DataVencimento = Convert.ToDateTime(linha.ExtrairValorDaLinha(70, 77).ToDateTimeFromDdMmAaaa()),
                ValorTitulo = linha.ExtrairValorDaLinha(78, 92).BoletoBrToDecimal() / 100,
                BancoCobradorRecebedor = linha.ExtrairValorDaLinha(93, 95).BoletoBrToInt(),
                AgenciaCobradoraRecebedora = linha.ExtrairValorDaLinha(96, 99).BoletoBrToInt(),
                DvAgenciaConta = linha.ExtrairValorDaLinha(100, 100),
                IdentificacaoTituloNaEmpresa = linha.ExtrairValorDaLinha(101, 125),
                Moeda = linha.ExtrairValorDaLinha(126, 127).BoletoBrToInt(),
                TipoInscricaoSacado = linha.ExtrairValorDaLinha(128, 128).BoletoBrToInt(),
                NumeroInscricaoSacado = linha.ExtrairValorDaLinha(129, 143).BoletoBrToLong(),
                NomeSacado = linha.ExtrairValorDaLinha(144, 183),
                NumeroContrato = linha.ExtrairValorDaLinha(184, 193).BoletoBrToLong(),
                ValorTarifas = linha.ExtrairValorDaLinha(194, 208).BoletoBrToDecimal() / 100,
                MotivoOcorrencia = linha.ExtrairValorDaLinha(209, 218)

            };

            return objetoRetorno;

        }

        public DetalheSegmentoURetornoCnab240 ObterRegistrosDetalheU(string linha)
        {
            var objetoRetorno = new DetalheSegmentoURetornoCnab240
            {
                CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt(),
                LoteServico = linha.ExtrairValorDaLinha(4, 7),
                CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt(),
                NumeroRegistro = linha.ExtrairValorDaLinha(9, 13).BoletoBrToInt(),
                CodigoSegmento = linha.ExtrairValorDaLinha(14, 14),
                CodigoMovimento = linha.ExtrairValorDaLinha(16, 17).BoletoBrToInt(),
                JurosMultaEncargos = linha.ExtrairValorDaLinha(18, 32).BoletoBrToDecimal() / 100,
                ValorDescontoConcedido = linha.ExtrairValorDaLinha(33, 47).BoletoBrToDecimal() / 100,
                ValorAbatimentoConcedido = linha.ExtrairValorDaLinha(48, 62).BoletoBrToDecimal() / 100,
                ValorIofRecolhido = linha.ExtrairValorDaLinha(63, 77).BoletoBrToDecimal() / 100,
                ValorPagoPeloSacado = linha.ExtrairValorDaLinha(78, 92).BoletoBrToDecimal() / 100,
                ValorLiquidoASerCreditado = linha.ExtrairValorDaLinha(93, 107).BoletoBrToDecimal() / 100,
                ValorOutrasDespesas = linha.ExtrairValorDaLinha(108, 122).BoletoBrToDecimal() / 100,
                ValorOutrosCreditos = linha.ExtrairValorDaLinha(123, 137).BoletoBrToDecimal() / 100,
                DataOcorrencia = Convert.ToDateTime(linha.ExtrairValorDaLinha(138, 145).ToDateTimeFromDdMmAaaa()),
                DataCredito = Convert.ToDateTime(linha.ExtrairValorDaLinha(146, 153).ToDateTimeFromDdMmAaaa()),
                CodigoOcorrenciaPagador = linha.ExtrairValorDaLinha(154, 157),
                DataOcorrenciaPagador = Convert.ToDateTime(linha.ExtrairValorDaLinha(158, 165).ToDateTimeFromDdMmAa()),
                ValorOcorrenciaPagador = linha.ExtrairValorDaLinha(166, 180).BoletoBrToDecimal() / 100,
                ComplementoOcorrenciaPagador = linha.ExtrairValorDaLinha(181, 210),
                CodigoBancoCompensacao = linha.ExtrairValorDaLinha(211, 213).BoletoBrToInt(),

            };

            return objetoRetorno;
        }

        public TrailerLoteRetornoCnab240 ObterTrailerLote(string linha)
        {
            var objetoRetorno = new TrailerLoteRetornoCnab240
            {
                CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt(),
                LoteServico = linha.ExtrairValorDaLinha(4, 7),
                CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt(),
                QtdRegistrosLote = linha.ExtrairValorDaLinha(18, 23).BoletoBrToLong(),
                QtdTitulosCobrancaSimples = linha.ExtrairValorDaLinha(24, 29).BoletoBrToLong(),
                ValorTitulosCobrancaSimples = linha.ExtrairValorDaLinha(30, 46).BoletoBrToDecimal() / 100,
                QtdTitulosCobrancaVinculada = linha.ExtrairValorDaLinha(47, 52).BoletoBrToLong(),
                ValorTitulosCobrancaVinculada = linha.ExtrairValorDaLinha(53, 69).BoletoBrToDecimal() / 100,
                QtdTitulosCobrancaCaucionada = linha.ExtrairValorDaLinha(70, 75).BoletoBrToLong(),
                ValorTitulosCobrancaCaucionada = linha.ExtrairValorDaLinha(76, 92).BoletoBrToDecimal() / 100,
                QtdTitulosCobrancaDescontada = linha.ExtrairValorDaLinha(93, 98).BoletoBrToLong(),
                ValorTitulosCobrancaDescontada = linha.ExtrairValorDaLinha(99, 115).BoletoBrToDecimal() / 100,
                NumeroAvisoLancamento = linha.ExtrairValorDaLinha(116, 123)
            };

            return objetoRetorno;
        }

        public TrailerRetornoCnab240 ObterTrailer(string linha)
        {
            var objetoRetorno = new TrailerRetornoCnab240
            {
                CodigoBanco = linha.ExtrairValorDaLinha(1, 3).BoletoBrToInt(),
                LoteServico = linha.ExtrairValorDaLinha(4, 7),
                CodigoRegistro = linha.ExtrairValorDaLinha(8, 8).BoletoBrToInt(),
                QtdLotesArquivo = linha.ExtrairValorDaLinha(18, 23).BoletoBrToInt(),
                QtdRegistrosArquivo = linha.ExtrairValorDaLinha(24, 29).BoletoBrToInt(),
            };

            return objetoRetorno;
        }

    }
}
