using BoletoBr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace BoletoBrPrint.Bancos
{
    public class GeradorBoletoSantanderPDF
    {
        public void EmitirBoleto(Boleto document, string path, string fileName)
        {

            var smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6);
            var regularFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7);
            var boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD);

            var pdfDocument = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(pdfDocument, new FileStream(path + fileName, FileMode.Create));

            var mainTable = new PdfPTable(6);
            mainTable.SetWidthPercentage(new float[] { 15, 15, 15, 15, 15, 25 }, pdfDocument.PageSize);
            mainTable.WidthPercentage = 100;

            PdfPCell col1, col2, col3, col4, col5, col6;
            Chunk chunk1, chunk2;


            // header
            col1 = new PdfPCell(new Phrase("Santander - 033-7", boldFont));
            col1.Colspan = 4;
            col1.BorderWidthLeft = 0;
            col1.BorderWidthTop = 0;
            col1.BorderWidthRight = 0;
            mainTable.AddCell(col1);

            col2 = new PdfPCell(new Phrase(new String(' ', 40) + "RECIBO DO PAGADOR", boldFont));
            col2.Colspan = 2;
            col2.BorderWidthLeft = 0;
            col2.BorderWidthTop = 0;
            col2.BorderWidthRight = 0;
            mainTable.AddCell(col2);

            // line 2 
            col1 = new PdfPCell(new Phrase("Local de Pagamento: PAGAVEL PREFERENCIALMENTE NO BANCO SANTANDER", regularFont));
            col1.Colspan = 5;
            mainTable.AddCell(col1);

            chunk1 = new Chunk("Vencimento \n", smallFont);
            chunk2 = new Chunk(new String(' ', 40) + document.DataVencimento.ToString("dd/MM/yyyy"), regularFont);
            col2 = new PdfPCell();
            col2.AddElement(chunk1);
            col2.AddElement(chunk2);
            mainTable.AddCell(col2);

            // line 3 
            chunk1 = new Chunk("Beneficiário \n", smallFont);
            chunk2 = new Chunk(document.CedenteBoleto.Nome + " CNPJ: " + document.CedenteBoleto.CpfCnpjFormatado + " \n" + document.CedenteBoleto.EnderecoCedente.LogradouroNumeroComplementoBairroCidadeUfConcatenado, regularFont);
            col1 = new PdfPCell();
            col1.Colspan = 5;
            col1.AddElement(chunk1);
            col1.AddElement(chunk2);
            mainTable.AddCell(col1);

            chunk1 = new Chunk("Agência / Código Beneficiário \n", smallFont);
            chunk2 = new Chunk(new String(' ', 33) + document.CedenteBoleto.ContaBancariaCedente.Agencia + " / " + document.CedenteBoleto.CodigoCedente, regularFont);
            col2 = new PdfPCell();
            col2.AddElement(chunk1);
            col2.AddElement(chunk2);
            mainTable.AddCell(col2);

            // line 4
            chunk1 = new Chunk("Data do Documento \n", smallFont);
            chunk2 = new Chunk(document.DataDocumento.ToString("dd/MM/yyyy"), regularFont);
            col1 = new PdfPCell();
            col1.AddElement(chunk1);
            col1.AddElement(chunk2);
            mainTable.AddCell(col1);


            chunk1 = new Chunk("No Documento \n", smallFont);
            chunk2 = new Chunk(document.NumeroDocumento, regularFont);
            col2 = new PdfPCell();
            col2.AddElement(chunk1);
            col2.AddElement(chunk2);
            mainTable.AddCell(col2);

            chunk1 = new Chunk("Espécie Doc. \n", smallFont);
            chunk2 = new Chunk(document.Especie.Sigla, regularFont);
            col3 = new PdfPCell();
            col3.AddElement(chunk1);
            col3.AddElement(chunk2);
            mainTable.AddCell(col3);

            chunk1 = new Chunk("Aceite \n", smallFont);
            chunk2 = new Chunk("NAO ACEITO", regularFont);
            col4 = new PdfPCell();
            col4.AddElement(chunk1);
            col4.AddElement(chunk2);
            mainTable.AddCell(col4);

            chunk1 = new Chunk("Data Processamento \n", smallFont);
            chunk2 = new Chunk(((DateTime)document.DataProcessamento).ToString("dd/MM/yyyy"), regularFont);
            col5 = new PdfPCell();
            col5.AddElement(chunk1);
            col5.AddElement(chunk2);
            mainTable.AddCell(col5);

            chunk1 = new Chunk("Nosso Número \n", smallFont);
            chunk2 = new Chunk(new String(' ', 32) + document.NossoNumeroFormatado, regularFont);
            col6 = new PdfPCell();
            col6.AddElement(chunk1);
            col6.AddElement(chunk2);
            mainTable.AddCell(col6);


            // line 5
            chunk1 = new Chunk("Uso do Banco \n", smallFont);
            chunk2 = new Chunk(" ", regularFont);
            col1 = new PdfPCell();
            col1.AddElement(chunk1);
            col1.AddElement(chunk2);
            mainTable.AddCell(col1);


            chunk1 = new Chunk("Carteira \n", smallFont);
            chunk2 = new Chunk(document.CarteiraCobranca.Descricao, regularFont);
            col2 = new PdfPCell();
            col2.AddElement(chunk1);
            col2.AddElement(chunk2);
            mainTable.AddCell(col2);

            chunk1 = new Chunk("Espécie Moeda \n", smallFont);
            chunk2 = new Chunk("REAL", regularFont);
            col3 = new PdfPCell();
            col3.AddElement(chunk1);
            col3.AddElement(chunk2);
            mainTable.AddCell(col3);

            chunk1 = new Chunk("Quantidade", smallFont);
            chunk2 = new Chunk("", regularFont);
            col4 = new PdfPCell();
            col4.AddElement(chunk1);
            col4.AddElement(chunk2);
            mainTable.AddCell(col4);

            chunk1 = new Chunk("(x) Valor \n", smallFont);
            chunk2 = new Chunk("", regularFont);
            col5 = new PdfPCell();
            col5.AddElement(chunk1);
            col5.AddElement(chunk2);
            mainTable.AddCell(col5);

            chunk1 = new Chunk("(=)Valor Documento \n", smallFont);
            chunk2 = new Chunk(new String(' ', 43) + document.ValorBoleto.ToString("C"), regularFont);
            col6 = new PdfPCell();
            col6.AddElement(chunk1);
            col6.AddElement(chunk2);
            mainTable.AddCell(col6);

            // line 6
            chunk1 = new Chunk("Pagador11 \n", smallFont);
            chunk2 = new Chunk(document.SacadoBoleto.Nome + " " + document.SacadoBoleto.CpfCnpjFormatado +  " \n" + document.SacadoBoleto.EnderecoSacado.LogradouroNumeroComplementoBairroCidadeUfConcatenado, regularFont);
            col1 = new PdfPCell();
            col1.AddElement(chunk1);
            col1.AddElement(chunk2);
            col1.Colspan = 6;
            mainTable.AddCell(col1);



            pdfDocument.Open();
            pdfDocument.Add(mainTable);

            //BarcodeInter25 code25 = new BarcodeInter25();
            //code25.Code = document.CodigoBarraBoleto;
            //PdfContentByte cb = writer.DirectContent;
            //code25.Font = null;
            //pdfDocument.Add(code25.CreateImageWithBarcode(cb, null, null));

            pdfDocument.Close();

        }
    }
}
