using SharpFileServiceProg.Service;
using System.Linq;

namespace WpfNotesSystem.Creator
{
    public class ContentManager
    {
        private readonly IFileService fileService;

        public ContentManager(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public void Run(IContentCreator creator, string[] lines)
        {
            var tuplesList = fileService.Header.Select2.GetElements(lines);

            if (tuplesList.Any())
            {
                var imax = tuplesList.Select(x => x.Item2).Max();
                var jmax = tuplesList.Count();

                creator.CreateRowsAndColls(jmax, imax);

                for (int j = 0; j < jmax; j++)
                {
                    var lineObj = tuplesList[j];
                    for (int i = 0; i < imax; i++)
                    {
                        if (lineObj.Type == "Header" &&
                            i == lineObj.Level - 2)
                        {
                            creator.CreateHeader((j, i),
                                lineObj.Text.ToString(),
                                imax - lineObj.Level + 2);
                            continue;
                        }

                        if (lineObj.Type == "Line" &&
                            i == lineObj.Level - 1)
                        {
                            creator.CreateLines((j, i),
                                lineObj.Text,
                                imax - lineObj.Level + 1);
                            continue;
                        }

                        creator.CreateEmpty((j, i));
                    }
                }
            }
        }
    }
}
