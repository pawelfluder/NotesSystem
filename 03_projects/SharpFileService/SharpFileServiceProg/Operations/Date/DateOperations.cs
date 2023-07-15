using SharpFileServiceProg.Operations.Index;
using System;

namespace SharpFileServiceProg.Operations.Date
{
    public class DateOperations
    {
        private IndexOperations indexOp;

        public DateOperations()
        {
            indexOp = new IndexOperations();
        }

        public string ToYear(string dateString)
        {
            var date = DateTime.Parse(dateString);
            var year = date.Year.ToString();
            return year;
        }

        public string UderscoreDate(DateTime date)
        {
            var year = indexOp.LastTwoChar(date.Year.ToString());
            var month = indexOp.IndexToString(date.Month);
            var day = indexOp.IndexToString(date.Day);
            var hour = indexOp.IndexToString(date.Hour);
            var minute = indexOp.IndexToString(date.Minute);

            var dateString = year + "-" + month + "-" + day + "_" + hour + "-" + minute;

            return dateString;
        }
    }
}
