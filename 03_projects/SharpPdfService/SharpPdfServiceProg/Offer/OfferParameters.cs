using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using PdfService.GridWorker;
using PdfService.Worker;
using PdfSharpCore.Drawing;

namespace PdfService.Offer
{
   public class PdfOfferParameters
    {
        public const string FamilyName = "Verdana";
        public const float PageWidth = 29.6926f;
        public const float PageHeight = 21.0058f;

        public static string Author => "Author";

        public TitlePageSettings TitlePage { get; set; } = new TitlePageSettings();

        public DataPageSettings DataPage { get; set; } = new DataPageSettings();

        public PdfOfferParameters(string filePath)
        {
           var doc = new XmlDocument();
           doc.Load(filePath);

           var path = doc.DocumentElement.SelectSingleNode($"{nameof(TitlePageSettings)}/{nameof(TitlePageSettings.ImagePath)}")?.InnerText;

           if (!string.IsNullOrWhiteSpace(path) && path.StartsWith("./"))
           {
              path = Path.Combine(new FileInfo(filePath).DirectoryName, path);
           }

           SetTitlePage(doc, path);
           SetDataPage(doc);
        }

        private void SetDataPage(XmlDocument doc)
        {
           DataPage = new DataPageSettings()
           {
              LeftMargin = XUnit.FromCentimeter(GetFloat(doc.DocumentElement
                 .SelectSingleNode($"{nameof(DataPageSettings)}")?.Attributes[nameof(DataPageSettings.LeftMargin)])),
              TopMargin = XUnit.FromCentimeter(GetFloat(doc.DocumentElement
                 .SelectSingleNode($"{nameof(DataPageSettings)}")?.Attributes[nameof(DataPageSettings.TopMargin)])),
              RightMargin = XUnit.FromCentimeter(GetFloat(doc.DocumentElement
                 .SelectSingleNode($"{nameof(DataPageSettings)}")?.Attributes[nameof(DataPageSettings.RightMargin)])),
              BottomMargin = XUnit.FromCentimeter(GetFloat(doc.DocumentElement
                 .SelectSingleNode($"{nameof(DataPageSettings)}")?.Attributes[nameof(DataPageSettings.BottomMargin)])),

              DataRowHeight = GetInt(doc.DocumentElement.SelectSingleNode($"{nameof(DataPageSettings)}")
                 ?.Attributes[nameof(DataPageSettings.DataRowHeight)]),
              HeaderRowHeight = GetInt(doc.DocumentElement.SelectSingleNode($"{nameof(DataPageSettings)}")
                 ?.Attributes[nameof(DataPageSettings.HeaderRowHeight)]),
              DataFontSize = GetInt(doc.DocumentElement.SelectSingleNode($"{nameof(DataPageSettings)}")
                 ?.Attributes[nameof(DataPageSettings.DataFontSize)]),
              HeaderFontSize = GetInt(doc.DocumentElement.SelectSingleNode($"{nameof(DataPageSettings)}")
                 ?.Attributes[nameof(DataPageSettings.HeaderFontSize)]),
              HeaderFontStyle = GetFontStyle(doc.DocumentElement.SelectSingleNode($"{nameof(DataPageSettings)}")
                 ?.Attributes[nameof(DataPageSettings.HeaderFontStyle)]),
              RowsPerPage = GetInt(doc.DocumentElement.SelectSingleNode($"{nameof(DataPageSettings)}")
                 ?.Attributes[nameof(DataPageSettings.RowsPerPage)]),

              DataColumns = doc.DocumentElement.SelectSingleNode($"{nameof(DataPageSettings)}")?.ChildNodes
                 .Cast<XmlNode>().Select(_ => new DataColumn
                 {
                    Left = XUnit.FromCentimeter(GetFloat(_.Attributes[nameof(DataColumn.Left)])),
                    Id = GetInt(_.Attributes[nameof(DataColumn.Id)]),
                    Header = _.InnerText,
                 }).ToList(),
           };
        }

        private void SetTitlePage(XmlDocument doc, string path)
        {
           TitlePage = new TitlePageSettings()
           {
              LeftMargin = XUnit.FromCentimeter(GetFloat(doc.DocumentElement
                 .SelectSingleNode($"{nameof(TitlePageSettings)}")?.Attributes[nameof(TitlePageSettings.LeftMargin)])),
              TopMargin = XUnit.FromCentimeter(GetFloat(doc.DocumentElement
                 .SelectSingleNode($"{nameof(TitlePageSettings)}")?.Attributes[nameof(TitlePageSettings.TopMargin)])),
              RightMargin = XUnit.FromCentimeter(GetFloat(doc.DocumentElement
                 .SelectSingleNode($"{nameof(TitlePageSettings)}")?.Attributes[nameof(TitlePageSettings.RightMargin)])),
              BottomMargin = XUnit.FromCentimeter(GetFloat(doc.DocumentElement
                 .SelectSingleNode($"{nameof(TitlePageSettings)}")
                 ?.Attributes[nameof(TitlePageSettings.BottomMargin)])),

              CompanyName = doc.DocumentElement
                 .SelectSingleNode($"{nameof(TitlePageSettings)}/{nameof(TitlePageSettings.CompanyName)}")?.ChildNodes
                 .Cast<XmlNode>()
                 .Select(_ => _.InnerText.Trim()).Where(_ => !string.IsNullOrWhiteSpace(_)).ToArray(),
              CompanyAddress = doc.DocumentElement
                 .SelectSingleNode($"{nameof(TitlePageSettings)}/{nameof(TitlePageSettings.CompanyAddress)}")
                 ?.ChildNodes.Cast<XmlNode>()
                 .Select(_ => _.InnerText.Trim()).Where(_ => !string.IsNullOrWhiteSpace(_)).ToArray(),
              CompanyContact = doc.DocumentElement
                 .SelectSingleNode($"{nameof(TitlePageSettings)}/{nameof(TitlePageSettings.CompanyContact)}")
                 ?.ChildNodes.Cast<XmlNode>()
                 .Select(_ => _.InnerText.Trim()).Where(_ => !string.IsNullOrWhiteSpace(_)).ToArray(),
              CustomTitle = doc.DocumentElement
                 .SelectSingleNode($"{nameof(TitlePageSettings)}/{nameof(TitlePageSettings.CustomTitle)}")?.InnerText,
              ImagePath = path,
           };
        }

        public static XFontStyle GetFontStyle(XmlAttribute attr, XFontStyle defaultValue = XFontStyle.Regular)
        {
            switch (attr?.Value?.ToLower())
            {
                case "regular":
                    return XFontStyle.Regular;

                case "bold":
                    return XFontStyle.Bold;

                case "italic":
                    return XFontStyle.Italic;

                case "bolditalic":
                    return XFontStyle.BoldItalic;

                default:
                    return defaultValue;
            }
        }

        public static int GetInt(XmlAttribute attr, int defaultValue = 10)
        {
            if (attr == null)
            {
                return defaultValue;
            }

            int.TryParse(attr.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out defaultValue);

            return defaultValue;
        }

        public static float GetFloat(XmlAttribute attr, float defaultValue = 1.27f)
        {
            if (attr == null)
            {
                return defaultValue;
            }

            float.TryParse(attr.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out defaultValue);

            return defaultValue;
        }
    }
}
