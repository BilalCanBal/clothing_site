namespace GiyimSitesi.Context.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public List<Product> Producs { get; set; }

    }
}
