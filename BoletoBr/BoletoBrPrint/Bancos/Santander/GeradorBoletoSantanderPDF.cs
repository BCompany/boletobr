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

        private void _GerarLinhaBoleto(Boleto document, int lineNumber, PdfPTable table)
        {
            var smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6);
            var regularFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7);
            var boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD);
            var boldBigFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD);

            PdfPCell col1, col2, col3, col4, col5, col6;
            Chunk chunk1, chunk2;

            if (lineNumber == 2)
            {
                col1 = new PdfPCell(new Phrase("Local de Pagamento: PAGAVEL PREFERENCIALMENTE NO BANCO SANTANDER", regularFont));
                col1.Colspan = 5;
                table.AddCell(col1);

                chunk1 = new Chunk("Vencimento \n", smallFont);
                chunk2 = new Chunk(new String(' ', 40) + document.DataVencimento.ToString("dd/MM/yyyy"), regularFont);
                col2 = new PdfPCell();
                col2.AddElement(chunk1);
                col2.AddElement(chunk2);
                table.AddCell(col2);


            }
            else if (lineNumber == 3)
            {
                chunk1 = new Chunk("Beneficiário \n", smallFont);
                chunk2 = new Chunk(document.CedenteBoleto.Nome + " CNPJ: " + document.CedenteBoleto.CpfCnpjFormatado + " \n" + document.CedenteBoleto.EnderecoCedente.LogradouroNumeroComplementoBairroCidadeUfConcatenado, regularFont);
                col1 = new PdfPCell();
                col1.Colspan = 5;
                col1.AddElement(chunk1);
                col1.AddElement(chunk2);
                table.AddCell(col1);

                chunk1 = new Chunk("Agência / Código Beneficiário \n", smallFont);
                chunk2 = new Chunk(new String(' ', 33) + document.CedenteBoleto.ContaBancariaCedente.Agencia + " / " + document.CedenteBoleto.CodigoCedente, regularFont);
                col2 = new PdfPCell();
                col2.AddElement(chunk1);
                col2.AddElement(chunk2);
                table.AddCell(col2);


            }
            else if (lineNumber == 4)
            {
                chunk1 = new Chunk("Data do Documento \n", smallFont);
                chunk2 = new Chunk(document.DataDocumento.ToString("dd/MM/yyyy"), regularFont);
                col1 = new PdfPCell();
                col1.AddElement(chunk1);
                col1.AddElement(chunk2);
                table.AddCell(col1);


                chunk1 = new Chunk("No Documento \n", smallFont);
                chunk2 = new Chunk(document.NumeroDocumento, regularFont);
                col2 = new PdfPCell();
                col2.AddElement(chunk1);
                col2.AddElement(chunk2);
                table.AddCell(col2);

                chunk1 = new Chunk("Espécie Doc. \n", smallFont);
                chunk2 = new Chunk(document.Especie.Sigla, regularFont);
                col3 = new PdfPCell();
                col3.AddElement(chunk1);
                col3.AddElement(chunk2);
                table.AddCell(col3);

                chunk1 = new Chunk("Aceite \n", smallFont);
                chunk2 = new Chunk("NAO ACEITO", regularFont);
                col4 = new PdfPCell();
                col4.AddElement(chunk1);
                col4.AddElement(chunk2);
                table.AddCell(col4);

                chunk1 = new Chunk("Data Processamento \n", smallFont);
                chunk2 = new Chunk(((DateTime)document.DataProcessamento).ToString("dd/MM/yyyy"), regularFont);
                col5 = new PdfPCell();
                col5.AddElement(chunk1);
                col5.AddElement(chunk2);
                table.AddCell(col5);

                chunk1 = new Chunk("Nosso Número \n", smallFont);
                chunk2 = new Chunk(new String(' ', 32) + document.NossoNumeroFormatado, regularFont);
                col6 = new PdfPCell();
                col6.AddElement(chunk1);
                col6.AddElement(chunk2);
                table.AddCell(col6);




            }
            else if (lineNumber == 5)
            {
                chunk1 = new Chunk("Uso do Banco \n", smallFont);
                chunk2 = new Chunk(" ", regularFont);
                col1 = new PdfPCell();
                col1.AddElement(chunk1);
                col1.AddElement(chunk2);
                table.AddCell(col1);


                chunk1 = new Chunk("Carteira \n", smallFont);
                chunk2 = new Chunk(document.CarteiraCobranca.Descricao, regularFont);
                col2 = new PdfPCell();
                col2.AddElement(chunk1);
                col2.AddElement(chunk2);
                table.AddCell(col2);

                chunk1 = new Chunk("Espécie Moeda \n", smallFont);
                chunk2 = new Chunk("REAL", regularFont);
                col3 = new PdfPCell();
                col3.AddElement(chunk1);
                col3.AddElement(chunk2);
                table.AddCell(col3);

                chunk1 = new Chunk("Quantidade", smallFont);
                chunk2 = new Chunk("", regularFont);
                col4 = new PdfPCell();
                col4.AddElement(chunk1);
                col4.AddElement(chunk2);
                table.AddCell(col4);

                chunk1 = new Chunk("(x) Valor \n", smallFont);
                chunk2 = new Chunk("", regularFont);
                col5 = new PdfPCell();
                col5.AddElement(chunk1);
                col5.AddElement(chunk2);
                table.AddCell(col5);

                chunk1 = new Chunk("(=)Valor Documento \n", smallFont);
                chunk2 = new Chunk(new String(' ', 43) + document.ValorBoleto.ToString("C"), regularFont);
                col6 = new PdfPCell();
                col6.AddElement(chunk1);
                col6.AddElement(chunk2);
                table.AddCell(col6);

            }
            else if (lineNumber == 6)
            {

                chunk1 = new Chunk("Pagador11 \n", smallFont);
                chunk2 = new Chunk(document.SacadoBoleto.Nome + " " + document.SacadoBoleto.CpfCnpjFormatado + " \n" + document.SacadoBoleto.EnderecoSacado.LogradouroNumeroComplementoBairroCidadeUfConcatenado, regularFont);
                col1 = new PdfPCell();
                col1.AddElement(chunk1);
                col1.AddElement(chunk2);
                col1.Colspan = 6;
                table.AddCell(col1);

            }




        }
        public void EmitirBoleto(Boleto document, string path, string fileName)
        {

            var smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6);
            var regularFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7);
            var boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD);
            var boldBigFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD);

            var pdfDocument = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(pdfDocument, new FileStream(path + fileName, FileMode.Create));

            var receiptTable = new PdfPTable(6);
            receiptTable.SetWidthPercentage(new float[] { 15, 15, 15, 15, 15, 25 }, pdfDocument.PageSize);
            receiptTable.WidthPercentage = 100;

            PdfPCell col1, col2, col3, col4, col5, col6;
            Chunk chunk1, chunk2;


            var image = Image.GetInstance(BoletoBrPrint.Resources.BoletoBrPrint.LogotipoSantanderPNG);
            image.ScalePercent(9, 8);




            // ***** Boleto - Recibo Pagador
            // header

            col1 = new PdfPCell();
            col1.AddElement(image);
            col1.BorderWidthLeft = 0;
            col1.BorderWidthTop = 0;
            col1.BorderWidthRight = 0;
            receiptTable.AddCell(col1);

            col2 = new PdfPCell(new Phrase(new String(' ', 5) + "033-7", boldBigFont));
            col2.Colspan = 3;
            col2.BorderWidthLeft = 0;
            col2.BorderWidthTop = 0;
            col2.BorderWidthRight = 0;
            receiptTable.AddCell(col2);


            col3 = new PdfPCell(new Phrase(new String(' ', 40) + "RECIBO DO PAGADOR", boldFont));
            col3.Colspan = 2;
            col3.BorderWidthLeft = 0;
            col3.BorderWidthTop = 0;
            col3.BorderWidthRight = 0;
            receiptTable.AddCell(col3);

            // line 2
            this._GerarLinhaBoleto(document, 2, receiptTable);

            // line 3 
            this._GerarLinhaBoleto(document, 3, receiptTable);

            // line 4
            this._GerarLinhaBoleto(document, 4, receiptTable);

            // line 5
            this._GerarLinhaBoleto(document, 5, receiptTable);

            // line 6
            this._GerarLinhaBoleto(document, 6, receiptTable);


            // lines 7 - messages
            chunk1 = new Chunk("Mensagens/Instruções (Texto de Responsabilidade do Cedente) \n", smallFont);
            var messagesText = "";
            foreach (var item in document.InstrucoesDoBoleto)
            {
                messagesText += item.TextoInstrucao + " \n";
            }
            chunk2 = new Chunk(messagesText);
            col1 = new PdfPCell();
            col1.MinimumHeight = 150;
            col1.AddElement(chunk1);
            col1.AddElement(chunk2);
            col1.Colspan = 6;
            receiptTable.AddCell(col1);


            // ***** Boleto - Documento de pagamento
            var documentTable = new PdfPTable(6);
            documentTable.SetWidthPercentage(new float[] { 15, 15, 15, 15, 15, 25 }, pdfDocument.PageSize);
            documentTable.WidthPercentage = 100;





            // line 1 
            col1 = new PdfPCell();
            col1.AddElement(image);
            col1.BorderWidthLeft = 0;
            col1.BorderWidthTop = 0;
            col1.BorderWidthRight = 0;
            documentTable.AddCell(col1);

            col2 = new PdfPCell(new Phrase(new String(' ', 5) + "033-7", boldBigFont));
            col2.BorderWidthLeft = 0;
            col2.BorderWidthTop = 0;
            col2.BorderWidthRight = 0;
            documentTable.AddCell(col2);

            col3 = new PdfPCell(new Phrase("\n\n" + new String(' ', 83) + document.LinhaDigitavelBoleto, regularFont));
            col3.Colspan = 4;
            col3.BorderWidthLeft = 0;
            col3.BorderWidthTop = 0;
            col3.BorderWidthRight = 0;
            documentTable.AddCell(col3);

            // line 2
            this._GerarLinhaBoleto(document, 2, documentTable);

            // line 3
            this._GerarLinhaBoleto(document, 3, documentTable);

            // line 4
            this._GerarLinhaBoleto(document, 4, documentTable);

            // line 5
            this._GerarLinhaBoleto(document, 5, documentTable);


            // Bloco Instruções, Juros, Abatimentos
            chunk1 = new Chunk("Instruções \n", smallFont);
            var messagesTextDoc = "";
            foreach (var item in document.InstrucoesDoBoleto)
            {
                messagesTextDoc += item.TextoInstrucao + " \n";
            }
            chunk2 = new Chunk(messagesText);
            col1 = new PdfPCell();
            col1.MinimumHeight = 150;
            col1.AddElement(chunk1);
            col1.AddElement(chunk2);
            col1.Colspan = 5;
            documentTable.AddCell(col1);

            // Nested Table
            var innerTable = new PdfPTable(1);
            var innerCell = new PdfPCell();

            chunk1 = new Chunk("(-) Descontos/Abatimentos \n", smallFont);
            var discount = Convert.ToDecimal(document.ValorAbatimento) + Convert.ToDecimal(document.ValorDesconto);
            var discountFmt = discount > 0 ? new String(' ', 43) + discount.ToString("C") : "";
            chunk2 = new Chunk(discountFmt, regularFont);
            innerCell.AddElement(chunk1);
            innerCell.AddElement(chunk2);
            innerTable.AddCell(innerCell);

            chunk1 = new Chunk("(+) Moras/Multa \n", smallFont);
            var fineInterest = Convert.ToDecimal(document.JurosMora) + Convert.ToDecimal(document.ValorMulta);
            var fineInterestFmt = fineInterest > 0 ? new String(' ', 43) + fineInterest.ToString("C") : "";
            chunk2 = new Chunk(fineInterestFmt, regularFont);
            innerCell = new PdfPCell();
            innerCell.AddElement(chunk1);
            innerCell.AddElement(chunk2);
            innerTable.AddCell(innerCell);

            chunk1 = new Chunk("(+) Valor Cobrado", smallFont);
            var totalCharged = Convert.ToDecimal(document.ValorCobrado);
            var totalChargedFmt = totalCharged > 0 ? new String(' ', 43) + totalCharged.ToString("C") : "";
            chunk2 = new Chunk(totalChargedFmt, regularFont);
            innerCell = new PdfPCell();
            innerCell.AddElement(chunk1);
            innerTable.AddCell(innerCell);

            col2 = new PdfPCell(innerTable);
            documentTable.AddCell(col2);


            // line 6
            this._GerarLinhaBoleto(document, 6, documentTable);

            // *** Abre o documento para poder gerar o código de baras
            pdfDocument.Open();

            // Código de Barras
            col1 = new PdfPCell();
            col1.Colspan = 6;
            col1.BorderWidth = 0;
            BarcodeInter25 code25 = new BarcodeInter25();
            code25.Code = document.CodigoBarraBoleto;
            code25.BarHeight = 30;
            code25.Font = null;
            PdfContentByte cb = writer.DirectContent;
            var imgBarcode = code25.CreateImageWithBarcode(cb, null, null);
            imgBarcode.ScalePercent(100);
            col1.AddElement(imgBarcode);
            documentTable.AddCell(col1);


            // ************** Criação de documento PDF do boleto

            pdfDocument.Add(receiptTable);
            pdfDocument.Add(new Phrase("\n\n Corte aqui " + new StringBuilder().Insert(0, "- ", 110), regularFont));
            pdfDocument.Add(documentTable);


            pdfDocument.Close();

        }

    }
}
