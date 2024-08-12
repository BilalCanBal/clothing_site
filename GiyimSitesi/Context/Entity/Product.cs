namespace GiyimSitesi.Context.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public int Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime Created_at { get; set; }

    }
}
