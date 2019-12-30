using FurnitureShop.Documents.WebApplication1.Documents;
using FurnitureShop.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
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
        private IEnumerable<Furniture> furnitures;

        public Statistics(IEnumerable<Furniture> furnitures)
        {
            this.furnitures = furnitures;
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
            Paragraph par = _document.LastSection.AddParagraph("Цены самых продаваемых товаров", "Header");
            par.Format.Alignment = ParagraphAlignment.Center;
            section.AddParagraph().AddLineBreaks(3);
            //
            Chart chart = new Chart();
            chart.Left = 0;
            //
            chart.Width = Unit.FromCentimeter(16);
            chart.Height = Unit.FromCentimeter(12);
            Series series = chart.SeriesCollection.AddSeries();
            series.ChartType = ChartType.Column2D;
            series.Add(furnitures.Select(f => (double)f.Price).ToArray());
            series.HasDataLabel = true;

            XSeries xseries = chart.XValues.AddXSeries();
            xseries.Add(Enumerable.Range(1, furnitures.Count()).Select(n => n.ToString()).ToArray());

            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            //
            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;

            _document.LastSection.Add(chart);
            par = _document.LastSection.AddParagraph();
            par.AddLineBreaks(2);
            par.AddTextLine("Легенда:");
            int i = 1;
            foreach (var f in furnitures)
            {
                par.AddTextLine(i.ToString() + " = " + "[" + f.VendorCode + "] " + f.Name);
                i++;
            }
        }
    }
}
