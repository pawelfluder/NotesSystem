namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();

            products.Add(new Product { Name = "test1", CategoryName = "CategoryTest1" });
            products.Add(new Product { Name = "test2", CategoryName = "CategoryTest2" });
            products.Add(new Product { Name = "test3", CategoryName = "CategoryTest2" });
            products.Add(new Product { Name = "test4", CategoryName = "CategoryTest3" });

            var newObject01 = GetNewObject01(products);
            var newObject02 = GetNewObject02(products);
        }

        static public List<(string, int)> GetNewObject01(List<Product> products)
        {
            var categories = products.Select(x => x.CategoryName).Distinct();
            var newObject02 = categories.Select(x => (x, products.Count(y => y.CategoryName == x))).ToList();

            return newObject02;
        }

        static public List<(string, int)> GetNewObject02(List<Product> products)
        {
            var categories = products.Select(x => x.CategoryName).Distinct();
            var newObject02 = categories.Select(x => (x, products.Count(y => y.CategoryName == x))).ToList();

            return newObject02;
        }

        internal class Product
        {
            public string Name { get; set; }
            public string CategoryName { get; set; }
        }
    }
}