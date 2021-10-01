using BoletoBr;
using BoletoBrPrint.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;

namespace BoletoBrPrint.Bancos
{
    public class BoletoGeradoPadrao : IGeradorBoleto
    {
        private readonly BoletoConfigurar config;

        public BoletoGeradoPadrao(BoletoConfigurar config)
        {
            this.config = config;
        }


        public virtual void GerarBoleto(Boleto document, string path, string fileName)
        {
            var pdfDocument = new Document(PageSize.A4);
            
            PdfWriter writer = PdfWriter.GetInstance(pdfDocument, new FileStream(path + fileName, FileMode.Create));

            var receiptTable = new PdfPTable(6);
            receiptTable.SetWidthPercentage(new float[] { 15, 15, 15, 15, 15, 25 }, pdfDocument.PageSize);
            receiptTable.WidthPercentage = 100;

            PdfPCell cell;
            Chunk chunk;
            
            var smallFont = new Font(Font.FontFamily.HELVETICA, 6);
            var regularFont = new Font(Font.FontFamily.HELVETICA, 7);
            var boldFont = new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD);
            var boldMediumFont = new Font(Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD);
            var boldBigFont = new Font(Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD);

            config.Logotipo.ScalePercent(9, 8);

            cell = new PdfPCell();
            cell.AddElement(config.Logotipo);
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthRight = 0;
            receiptTable.AddCell(cell);
            
            cell = new PdfPCell(new Phrase(new String(' ', 3) + config.NumeroBanco, boldBigFont));
            cell.Colspan = 3;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthRight = 0;
            receiptTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("\n\n" + new String(' ', 30) + "RECIBO DO PAGADOR", boldMediumFont));
            cell.Colspan = 2;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthRight = 0;
            receiptTable.AddCell(cell);

            for (int line = 2; line <= 6; line++)
                _GerarLinhaBoleto(document, config, line, receiptTable);

            chunk = new Chunk("Mensagens/Instruções (Texto de Responsabilidade do Cedente) \n", smallFont);
            var messagesText = "";
            foreach (var item in document.InstrucoesDoBoleto)
                messagesText += item.TextoInstrucao + " \n";

            chunk = new Chunk(messagesText, regularFont);
            cell = new PdfPCell();
            cell.MinimumHeight = 140;
            cell.AddElement(chunk);
            cell.Colspan = 6;
            receiptTable.AddCell(cell);

            // ***** Boleto - Documento de pagamento
            var documentTable = new PdfPTable(6);
            documentTable.SetWidthPercentage(new float[] { 15, 15, 15, 15, 15, 25 }, pdfDocument.PageSize);
            documentTable.WidthPercentage = 100;

            // line 1 
            cell = new PdfPCell();
            cell.AddElement(config.Logotipo);
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthRight = 0;
            documentTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(new String(' ', 5) + config.NumeroBanco, boldBigFont));
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 0;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.BorderWidthRight = 0;
            documentTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("\n\n" + new String(' ', 27) + document.LinhaDigitavelBoleto, boldMediumFont));
            cell.Colspan = 4;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthRight = 0;
            documentTable.AddCell(cell);

            for (int line = 2; line <= 5; line++)
                _GerarLinhaBoleto(document, config, line, documentTable);

            // Bloco Instruções, Juros, Abatimentos
            chunk = new Chunk("Instruções \n", smallFont);
            var messagesTextDoc = "";
            foreach (var item in document.InstrucoesDoBoleto)
                messagesTextDoc += item.TextoInstrucao + " \n";

            chunk = new Chunk(messagesText, regularFont);
            cell = new PdfPCell();
            cell.MinimumHeight = 140;
            cell.AddElement(chunk);
            cell.Colspan = 5;
            documentTable.AddCell(cell);

            var innerTable = new PdfPTable(1);
            var innerCell = new PdfPCell();

            chunk = new Chunk("(-) Descontos/Abatimentos \n", smallFont);
            var discount = Convert.ToDecimal(document.ValorAbatimento) + Convert.ToDecimal(document.ValorDesconto);
            var discountFmt = discount > 0 ? new String(' ', 43) + discount.ToString("C") : "";
            chunk = new Chunk(discountFmt, regularFont);
            innerCell.AddElement(chunk);
            innerCell.AddElement(chunk);
            innerTable.AddCell(innerCell);

            chunk = new Chunk("(+) Moras/Multa \n", smallFont);
            var fineInterest = Convert.ToDecimal(document.JurosMora) + Convert.ToDecimal(document.ValorMulta);
            var fineInterestFmt = fineInterest > 0 ? new String(' ', 43) + fineInterest.ToString("C") : "";
            chunk = new Chunk(fineInterestFmt, regularFont);
            innerCell = new PdfPCell();
            innerCell.AddElement(chunk);
            innerCell.AddElement(chunk);
            innerTable.AddCell(innerCell);

            chunk = new Chunk("(=) Valor Cobrado", smallFont);
            var totalCharged = Convert.ToDecimal(document.ValorCobrado);
            var totalChargedFmt = totalCharged > 0 ? new String(' ', 43) + totalCharged.ToString("C") : "";
            chunk = new Chunk(totalChargedFmt, regularFont);
            innerCell = new PdfPCell();
            innerCell.AddElement(chunk);
            innerTable.AddCell(innerCell);

            cell = new PdfPCell(innerTable);
            documentTable.AddCell(cell);

            // line 6
            _GerarLinhaBoleto(document, config, 6, documentTable);

            pdfDocument.Open();

            // Código de Barras
            cell = new PdfPCell();
            cell.Colspan = 6;
            cell.BorderWidth = 0;
            BarcodeInter25 code25 = new BarcodeInter25();
            code25.Code = document.CodigoBarraBoleto;
            code25.BarHeight = 40;
            code25.Font = null;
            PdfContentByte cb = writer.DirectContent;
            var imgBarcode = code25.CreateImageWithBarcode(cb, null, null);
            imgBarcode.ScalePercent(120);
            cell.AddElement(imgBarcode);
            documentTable.AddCell(cell);

            pdfDocument.Add(receiptTable);
            pdfDocument.Add(new Phrase("\n\n Corte aqui " + new StringBuilder().Insert(0, "- ", 110), regularFont));
            pdfDocument.Add(documentTable);

            pdfDocument.Close();
        }


        public MemoryStream GerarBoleto(Boleto document, MemoryStream streamReport)
        {
            //using (var streamReport = new MemoryStream())
            //{
                var pdfDocument = new Document(PageSize.A4);

                PdfWriter writer = PdfWriter.GetInstance(pdfDocument, streamReport);

                var receiptTable = new PdfPTable(6);
                receiptTable.SetWidthPercentage(new float[] { 15, 15, 15, 15, 15, 25 }, pdfDocument.PageSize);
                receiptTable.WidthPercentage = 100;

                PdfPCell cell;
                Chunk chunk;

                var smallFont = new Font(Font.FontFamily.HELVETICA, 6);
                var regularFont = new Font(Font.FontFamily.HELVETICA, 7);
                var boldFont = new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD);
                var boldMediumFont = new Font(Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD);
                var boldBigFont = new Font(Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD);

                config.Logotipo.ScalePercent(9, 8);

                cell = new PdfPCell();
                cell.AddElement(config.Logotipo);
                cell.BorderWidthLeft = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthRight = 0;
                receiptTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(new String(' ', 3) + config.NumeroBanco, boldBigFont));
                cell.Colspan = 3;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthRight = 0;
                receiptTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n\n" + new String(' ', 30) + "RECIBO DO PAGADOR", boldMediumFont));
                cell.Colspan = 2;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthRight = 0;
                receiptTable.AddCell(cell);

                for (int line = 2; line <= 6; line++)
                    _GerarLinhaBoleto(document, config, line, receiptTable);

                chunk = new Chunk("Mensagens/Instruções (Texto de Responsabilidade do Cedente) \n", smallFont);
                var messagesText = "";
                foreach (var item in document.InstrucoesDoBoleto)
                    messagesText += item.TextoInstrucao + " \n";

                chunk = new Chunk(messagesText, regularFont);
                cell = new PdfPCell();
                cell.MinimumHeight = 140;
                cell.AddElement(chunk);
                cell.Colspan = 6;
                receiptTable.AddCell(cell);

                // ***** Boleto - Documento de pagamento
                var documentTable = new PdfPTable(6);
                documentTable.SetWidthPercentage(new float[] { 15, 15, 15, 15, 15, 25 }, pdfDocument.PageSize);
                documentTable.WidthPercentage = 100;

                // line 1 
                cell = new PdfPCell();
                cell.AddElement(config.Logotipo);
                cell.BorderWidthLeft = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthRight = 0;
                documentTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(new String(' ', 5) + config.NumeroBanco, boldBigFont));
                cell.BorderWidthLeft = 0;
                cell.BorderWidthTop = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BorderWidthRight = 0;
                documentTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n\n" + new String(' ', 27) + document.LinhaDigitavelBoleto, boldMediumFont));
                cell.Colspan = 4;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthRight = 0;
                documentTable.AddCell(cell);

                for (int line = 2; line <= 5; line++)
                    _GerarLinhaBoleto(document, config, line, documentTable);

                // Bloco Instruções, Juros, Abatimentos
                chunk = new Chunk("Instruções \n", smallFont);
                var messagesTextDoc = "";
                foreach (var item in document.InstrucoesDoBoleto)
                    messagesTextDoc += item.TextoInstrucao + " \n";

                chunk = new Chunk(messagesText, regularFont);
                cell = new PdfPCell();
                cell.MinimumHeight = 140;
                cell.AddElement(chunk);
                cell.Colspan = 5;
                documentTable.AddCell(cell);

                var innerTable = new PdfPTable(1);
                var innerCell = new PdfPCell();

                chunk = new Chunk("(-) Descontos/Abatimentos \n", smallFont);
                var discount = Convert.ToDecimal(document.ValorAbatimento) + Convert.ToDecimal(document.ValorDesconto);
                var discountFmt = discount > 0 ? new String(' ', 43) + discount.ToString("C") : "";
                chunk = new Chunk(discountFmt, regularFont);
                innerCell.AddElement(chunk);
                innerCell.AddElement(chunk);
                innerTable.AddCell(innerCell);

                chunk = new Chunk("(+) Moras/Multa \n", smallFont);
                var fineInterest = Convert.ToDecimal(document.JurosMora) + Convert.ToDecimal(document.ValorMulta);
                var fineInterestFmt = fineInterest > 0 ? new String(' ', 43) + fineInterest.ToString("C") : "";
                chunk = new Chunk(fineInterestFmt, regularFont);
                innerCell = new PdfPCell();
                innerCell.AddElement(chunk);
                innerCell.AddElement(chunk);
                innerTable.AddCell(innerCell);

                chunk = new Chunk("(=) Valor Cobrado", smallFont);
                var totalCharged = Convert.ToDecimal(document.ValorCobrado);
                var totalChargedFmt = totalCharged > 0 ? new String(' ', 43) + totalCharged.ToString("C") : "";
                chunk = new Chunk(totalChargedFmt, regularFont);
                innerCell = new PdfPCell();
                innerCell.AddElement(chunk);
                innerTable.AddCell(innerCell);

                cell = new PdfPCell(innerTable);
                documentTable.AddCell(cell);

                // line 6
                _GerarLinhaBoleto(document, config, 6, documentTable);

                pdfDocument.Open();

                // Código de Barras
                cell = new PdfPCell();
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                BarcodeInter25 code25 = new BarcodeInter25();
                code25.Code = document.CodigoBarraBoleto;
                code25.BarHeight = 40;
                code25.Font = null;
                PdfContentByte cb = writer.DirectContent;
                var imgBarcode = code25.CreateImageWithBarcode(cb, null, null);
                imgBarcode.ScalePercent(120);
                cell.AddElement(imgBarcode);
                documentTable.AddCell(cell);

                pdfDocument.Add(receiptTable);
                pdfDocument.Add(new Phrase("\n\n Corte aqui " + new StringBuilder().Insert(0, "- ", 110), regularFont));
                pdfDocument.Add(documentTable);

                pdfDocument.Close();

                //var bytesFile = streamReport.ToArray();
                //return bytesFile;

                return streamReport;
            //}
        }


        private void _GerarLinhaBoleto(Boleto document, BoletoConfigurar banco, int lineNumber, PdfPTable table)
        {
            var smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6);
            var regularFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7);
            var boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD);
            var boldBigFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD);

            PdfPCell col1, col2, col3, col4, col5, col6;
            Chunk chunk1, chunk2;

            if (lineNumber == 2)
            {
                col1 = new PdfPCell(new Phrase(banco.PagavelPreferencialmente, regularFont));
                col1.Colspan = 5;
                col1.VerticalAlignment = Element.ALIGN_MIDDLE;
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
                chunk2 = new Chunk(document.CarteiraCobranca.Codigo, regularFont);
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
    }
}