using BoletoBr.Dominio;

namespace BoletoBr.Interfaces
{
    public interface ILeitorArquivoRetornoCnab400
    {
        RetornoCnab400 ProcessarRetorno(TipoArquivo tipoArquivo);
        void ValidaArquivoRetorno();
        HeaderRetornoCnab400 ObterHeader(string linha);
        DetalheRetornoCnab400 ObterRegistrosDetalhe(string linha);
        TrailerRetornoCnab400 ObterTrailer(string linha);
    }
}
