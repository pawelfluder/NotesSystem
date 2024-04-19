using System;
using System.Collections.Generic;
using System.Linq;
using TextHeaderAnalyzerCoreProj;

namespace TextHeaderAnalyzerFrameProj
{
   public class Header : IEquatable<Header>, INotesContainer
    {
      public string Name { get; }

      public List<INotesContainer> SubHeaders { get; }

      public List<string> Content { get; }

      public Header(List<Header> subHeaders)
      {
         SubHeaders = subHeaders.Select(x=> x as INotesContainer).ToList();
         Content = new List<string>();
      }

      public Header(List<string> content)
      {
         SubHeaders = new List<Header>().Select(x => x as INotesContainer).ToList();
         Content = content;
      }

      public Header(string name, List<string> content, List<Header> subHeaders)
      {
         Name = name;
         SubHeaders = subHeaders.Select(x => x as INotesContainer).ToList();
         Content = content;
      }

      public Header(string name, List<Header> subHeaders)
      {
         Name = name;
         SubHeaders = subHeaders.Select(x => x as INotesContainer).ToList();
         Content = new List<string>();
      }

      public Header(string name)
      {
         Name = name;
         SubHeaders = new List<INotesContainer>();
         Content = new List<string>();
      }

      public Header(string name, List<string> content)
      {
         Name = name;
         SubHeaders = new List<INotesContainer>();
         Content = content;
      }

      public Header(string name, params string[] content)
      {
         Name = name;
         Content = content.ToList();
         SubHeaders = new List<INotesContainer>();
      }

      public void AddSubHeaders(List<Header> subHeaders)
      {
         SubHeaders.AddRange(subHeaders);
      }

        public void AddSubHeaders(List<INotesContainer> subHeaders)
        {
            SubHeaders.AddRange(subHeaders);
        }

        public void AddContent(List<string> lines)
        {
            Content.AddRange(lines);
        }

        public void AddSubHeaderByPosition(Header subHeader, params int[] inputPositions)
      {
            Header temp = this;
            (var last, var positions) = GetLastAndPostitions(inputPositions);

            foreach (var pos in positions)
            {
                var temp2 = temp.SubHeaders.ElementAt(pos);
                temp = temp2 as Header;
            }

            temp.AddSubHeaderAtPosition(last, subHeader);
      }
        public (int, List<int>) GetLastAndPostitions(int[] inputPositions)
        {
            if (inputPositions[0] == 1)
            {
                var last = inputPositions.ElementAt(inputPositions.Length - 1) - 1;
                var temp = inputPositions.ToList();
                var positions = temp.Select(x => x - 1).ToList();
                positions.RemoveAt(positions.Count - 1);
                positions.RemoveAt(0);

                return (last, positions);
            }

            return (default, default);
        }

            public void AddSubHeader(Header subHeader)
        {
            SubHeaders.Add(subHeader);
        }

        public void AddSubHeaderAtPosition(int index, Header subHeader)
        {
            SubHeaders.Insert(index, subHeader);
        }

        public void AddSubHeader(string tittle, params string[] content)
      {
         var subHeader = new Header(tittle, content.ToList());
         SubHeaders.Add(subHeader);
      }

      public void AddSubHeaders(List<string> content)
      {
         Content.AddRange(content);
      }

      public static bool operator ==(Header obj1, Header obj2)
      {
         return obj1.Equals(obj2);
      }

      public static bool operator !=(Header obj1, Header obj2)
      {
         return !obj1.Equals(obj2);
      }

      //public override int GetHashCode()
      //{
      //   return Header.GetHashCode();
      //}

      public bool Equals(Header other)
      {
         if (!NamesAreEqual(this, other))
         {
            return false;
         }

         if (!ContentAreEqual(this, other))
         {
            return false;
         }

         if (!SubHeadersAreEqual(this, other))
         {
            return false;
         }

         return true;
      }

      private bool SubHeadersAreEqual(Header header, Header other)
      {
         var subHeaders = header.SubHeaders;
         var otherHeaders = other.SubHeaders;

         if (!(subHeaders.Count == otherHeaders.Count))
         {
            return false;
         }

         return subHeaders.IsClassListEqual(otherHeaders);
      }

      private bool ContentAreEqual(Header header, Header other)
      {
         var headerCount = header.Content.Count;
         var otherCount = other.Content.Count;
         if (headerCount == otherCount)
         {
            var equal = header.Content.IsClassListEqual(other.Content);
            return equal;
         }

         return false;
      }

      private bool NamesAreEqual(Header header, Header other)
      {
         var equal = header.Name == other.Name;
         return equal;
      }

      public override bool Equals(object obj)
      {
         if (!(obj is Header))
         {
            return false;
         }

         var other = (Header)obj;

         return this.Equals(other);
      }
   }
}
