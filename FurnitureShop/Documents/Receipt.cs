using FurnitureShop.Documents.WebApplication1.Documents;
using FurnitureShop.Models;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Documents
{
    public class Receipt
    {
        private Document _document = new Document();
        private OrderHeader _orderHeader;
        private string _path;

        public Receipt(OrderHeader orderHeader, string path)
        {
            _orderHeader = orderHeader;
            _path = path;
            //
            InitializeStyle();
            Fill();
        }

        public Byte[] GetDocument()
        {
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(unicode: true);
            renderer.Document = _document;
            renderer.RenderDocument();
            //
            MemoryStream stream = new MemoryStream();
            renderer.Save(stream, true);
            return stream.ToArray();
        }

        private void InitializeStyle()
        {
            Style style = _document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 15;
        }

        private void Fill()
        {
            Section section = _document.AddSection();
            Image img = section.AddImage(_path + "/img/logo/logo.png");
            img.Height = "4cm";

            Paragraph paragraph = section.AddParagraph();
            paragraph.AddLineBreaks(count: 2);

            Table table = CreateTableWithHeading("Приобретенные товары:");
            Row row = table.AddRow();
            row.Height = 25;
            row.Borders.Bottom = new Border() { Width = "1pt", Color = Colors.DarkGray };
            row.Cells[0].AddParagraph("Артикул:");
            row.Cells[1].AddParagraph("Название:");
            row.Cells[2].AddParagraph("Количество:");
            row.Cells[3].AddParagraph("Цена:");

            foreach (var od in _orderHeader.OrderDetails)
            {
                row = table.AddRow();
                row.Height = 20;
                row.Cells[0].AddParagraph(od.VendorCode);
                row.Cells[1].AddParagraph(od.Furniture.Name);
                row.Cells[2].AddParagraph($"{od.Quantity} шт.");
                row.Cells[3].AddParagraph($"{(int)od.Furniture.Price} грн");
            }
            row = table.AddRow();
            row.Borders.Top = new Border() { Width = "1pt", Color = Colors.DarkGray };
            //
            row = table.AddRow();
            row.Borders.Bottom = new Border() { Width = "1pt", Color = Colors.DarkGray };
            paragraph = row.Cells[3].AddParagraph();
            FormattedText text = paragraph.AddFormattedText("Итог:", TextFormat.Bold);
            text.Color = Colors.Black;
            row.Height = 25;
            //
            row = table.AddRow();
            row.Cells[3].AddParagraph($"{_orderHeader.OrderDetails.Aggregate(0, (seed, od) => { return seed + od.Quantity * (int)od.Furniture.Price;}).ToString()} грн");

            paragraph = section.AddParagraph();
            paragraph.AddLineBreaks(count: 4);
            paragraph.Format.Font.Color = MigraDoc.DocumentObjectModel.Color.FromCmyk(100, 30, 20, 50);
            paragraph.AddTextLines(
                $"Дата: {DateTime.Now.ToString("d")}",
                $"Покупатель: {_orderHeader.AppUser.Name}");
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            paragraph = section.AddParagraph();
            paragraph.AddLineBreaks(count: 2);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddTextLines("Будем рады Вас видеть, FurnitureShop!");
        }

        private Table CreateTableWithHeading(string heading)
        {
            Section section = _document.LastSection;
            section.AddParagraph().AddLineBreaks(count: 2);
            //
            Paragraph paragraph = section.AddParagraph();
            FormattedText text = paragraph.AddFormattedText(heading, "Heading");
            text.Color = Colors.Black;
            paragraph.AddLineBreaks(count: 2);
            //
            Table table = section.AddTable();
            Column col = table.AddColumn(); // 1 column
            col.Width = 80;
            col = table.AddColumn();        // 2 column
            col.Width = 200;
            col = table.AddColumn();        // 3 column
            col.Width = 100;
            col = table.AddColumn();        // 4 column
            col.Width = 100;
            //
            return table;
        }
    }
}
