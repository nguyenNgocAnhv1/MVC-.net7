using m01_Start.Models;

namespace m01_Start
{
    public class ProductService : List<ProductModel>
    {
        public ProductService()
        {
            this.AddRange(new ProductModel[]{
               new ProductModel() { Id = 1, Name = "Iphone", Description = "999" },
               new ProductModel(){Id = 2,Name = "Xiaomi",Description = "Perfomance"},
            });
        }
    };
}