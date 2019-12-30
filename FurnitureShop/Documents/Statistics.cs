using FurnitureShop.Documents.WebApplication1.Documents;
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
    public class Statistics
    {
        private Document _document = new Document();

        public Statistics()
        {
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
