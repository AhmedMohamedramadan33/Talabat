using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {

        public static async Task SeedAsync(StoreContext context)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (context.productBrands.Count() == 0)
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands?.Count > 0)
                {
                    foreach (var ob in brands)
                    {
                        context.Set<ProductBrand>().Add(ob);

                    }
                    context.SaveChanges();
                }
            }
            if (context.productCategories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
                if (categories?.Count > 0)
                {
                    foreach (var ob in categories)
                    {
                        context.Set<ProductCategory>().Add(ob);

                    }
                    context.SaveChanges();
                }
            }
            if (context.Products.Count() == 0)
            {
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products?.Count > 0)
                {
                    foreach (var ob in products)
                    {
                        context.Set<Product>().Add(ob);

                    }
                    context.SaveChanges();
                }
            }
            if (context.DeliveryMethods.Count() == 0)
            {
                var DeliveryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery1.json");
                var DeliveryDatas = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                if (DeliveryDatas?.Count > 0)
                {
                    foreach (var ob in DeliveryDatas)
                    {
                        context.Set<DeliveryMethod>().Add(ob);
                    }
                    context.SaveChanges();
                }
            }


        }
    }
}
