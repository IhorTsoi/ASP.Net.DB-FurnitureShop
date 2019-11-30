using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Documents
{

    using MigraDoc.DocumentObjectModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace WebApplication1.Documents
    {
        public static class ParagraphExtension
        {
            //
            // Class which extands MigraDoc.Paragraph methods.
            //

            public static void AddLineBreaks(this Paragraph paragraph, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    paragraph.AddLineBreak();
                }
            }

            public static void AddTextLine(this Paragraph paragraph, string textLine)
            {
                paragraph.AddText(textLine);
                paragraph.AddLineBreak();
            }

            public static void AddTextLines(this Paragraph paragraph, params string[] textLines)
            {
                foreach (string textLine in textLines)
                {
                    paragraph.AddText(textLine);
                    paragraph.AddLineBreak();
                }
            }
        }
    }
}
