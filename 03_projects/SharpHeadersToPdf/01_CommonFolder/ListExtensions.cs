using System.Collections.Generic;

namespace TextHeaderAnalyzerFrameProj
{
    public static class ListExtensions
    {
        public static bool IsClassListEqual<T>(this IEnumerable<T> thisList, IEnumerable<T> secondList) where T : class
        {
            var a = thisList.GetEnumerator();
            var b = secondList.GetEnumerator();

            while (true)
            {
                if (!a.MoveNext())
                {
                    break;
                }

                b.MoveNext();

                var equal = a.Current.Equals(b.Current);
                if (!equal)
                {
                    Dispose(a, b);
                    return false;
                }
            }

            Dispose(a, b);
            return true;
        }

        public static bool IsStructListEqual<T>(this IEnumerable<T> thisList, IEnumerable<T> secondList) where T : struct
        {
            var a = thisList.GetEnumerator();
            var b = secondList.GetEnumerator();

            while (true)
            {
                if (!a.MoveNext())
                {
                    break;
                }

                b.MoveNext();

                var equal = a.Current.Equals(b.Current);
                if (!equal)
                {
                    return false;
                }
            }

            return true;
        }

        private static void Dispose<T>(IEnumerator<T> a, IEnumerator<T> b) where T : class
        {
            a.Dispose();
            b.Dispose();
        }
    }
}
