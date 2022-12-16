namespace MyShop.Models
{
    public class ProductModel:BaseModel
    {
        public Guid Id { get; set; }
        public int CatId { get; set; }      
        public string Title { get; set; }
        public string Images { get; set; }
        public float Price { get; set; }        
        public string Desc { get; set; }
        public Dictionary<string, string> Specifications { get; set; }

    }
}
