using FurnitureShop.Documents.WebApplication1.Documents;
using FurnitureShop.Models;
using MigraDoc.DocumentObjectModel;
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
        //
        //    We create a pdf document using this class.
        //    All the data should be passed to document through the constructor.
        //    Use GetDocument() method to get the document in a byte[].
        //

        private Document _document = new Document();
        private OrderHeader orderHeader;

        public Receipt(OrderHeader orderHeader)
        {
            this.orderHeader = orderHeader;
            InitializeStyle();
            Fill();
        }

        public Byte[] GetDocument()
        {
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(unicode: true);
            renderer.Document = _document;

            renderer.RenderDocument();

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
            Paragraph paragraph = section.AddParagraph();

            paragraph.AddTextLines(
                $"Дата: {DateTime.Now.ToString("d")}",
                $"Покупатель: {orderHeader.Buyer.Name}");

            paragraph.AddLineBreaks(count: 4);

            paragraph = section.AddParagraph();
            paragraph.AddTextLines(
                "Приобретенные товары:");

            foreach (var od in orderHeader.OrderDetails)
            {
                paragraph.AddTextLines(od.VendorCode, od.Furniture.Name, $"В количестве: {od.Quantity} шт.", $"По цене: {od.Furniture.Price}");
            }

            paragraph.AddLineBreaks(count: 4);

            paragraph.AddTextLines("Приходите ещё, FurnitureShop!");
        }

        //private void CreateCarUserData()
        //{
        //    Table table = CreateTableWithHeading("Car info:");

        //    Row row = table.AddRow();
        //    row.Cells[0].AddParagraph("Name:");
        //    row.Cells[1].AddParagraph(_car.Name);

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Origin:");
        //    row.Cells[1].AddParagraph(_car.Origin);

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Years:");
        //    row.Cells[1].AddParagraph(_car.Years.ToString());

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Technical state:");
        //    row.Cells[1].AddParagraph(_car.TechState.ToString());

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Horse power:");
        //    row.Cells[1].AddParagraph(_car.HorsePower.ToString());

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Weight:");
        //    row.Cells[1].AddParagraph(_car.Weight.ToString());

        //    row = table.AddRow();
        //    Paragraph paragraph = row.Cells[0].AddParagraph();
        //    FormattedText text = paragraph.AddFormattedText("Price:");
        //    text.Color = Colors.DarkBlue;
        //    row.Cells[1].AddParagraph(_car.Weight.ToString() + "$");

        //    table = CreateTableWithHeading("My personal info:");

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Name:");
        //    row.Cells[1].AddParagraph(_user.Name);

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Surname:");
        //    row.Cells[1].AddParagraph(_user.Surname);

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Adress:");
        //    row.Cells[1].AddParagraph(_user.Adress);

        //    row = table.AddRow();
        //    row.Cells[0].AddParagraph("Phone number:");
        //    row.Cells[1].AddParagraph(_user.PhoneNumber);
        //}

        //private Table CreateTableWithHeading(string heading)
        //{
        //    Section section = _document.LastSection;

        //    section.AddParagraph().AddLineBreaks(count: 2);

        //    Paragraph paragraph = section.AddParagraph();
        //    FormattedText text = paragraph.AddFormattedText(heading, "Heading");
        //    text.Color = Colors.Black;
        //    paragraph.AddLineBreaks(count: 2);

        //    Table table = section.AddTable();
        //    Column col = table.AddColumn(); // left column for names
        //    col.Width = 150;
        //    col = table.AddColumn();        // right column for values
        //    col.Width = 500;

        //    return table;
        //}
    }
}
