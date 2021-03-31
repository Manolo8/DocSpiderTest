using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Instances;

namespace Core {
    public class Init {
        public static void Register(IServiceCollection services) {
            Instantiator.Instantiate(typeof(Init).GetTypeInfo().Assembly, services);

            // services.AddSingleton<UserAuthProvider>();

            // services.AddScoped<PermissionService>();

            #region Auth

            // services.AddScoped<PermissionService>();

            #endregion

            #region Product

            // services.AddScoped<PictureRepository>();
            // services.AddScoped<ProductRepository>();
            // services.AddScoped<BrandRepository>();
            // services.AddScoped<CategoryRepository>();
            // services.AddScoped<SizeRepository>();
            // services.AddScoped<GroupRepository>();
            // services.AddScoped<CartRepository>();
            // services.AddScoped<CartItemRepository>();
            // services.AddScoped<StockRepository>();

            // services.AddScoped<PictureService>();
            // services.AddScoped<ProductService>();
            // services.AddScoped<BrandService>();
            // services.AddScoped<CategoryService>();
            // services.AddScoped<SizeService>();
            // services.AddScoped<GroupService>();
            // services.AddScoped<CartService>();
            // services.AddScoped<StockService>();

            #endregion

            #region User

            // services.AddScoped<UserRepository>();
            // services.AddScoped<UserService>();

            // services.AddScoped<UserTokenRepository>();
            // services.AddScoped<UserTokenService>();

            #endregion

            #region Picture

            // services.AddScoped<PictureRepository>();

            // services.AddScoped<PictureService>();

            #endregion

            #region Checkout

            // services.AddScoped<CheckoutRepository>();
            // services.AddScoped<CheckoutProductRepository>();

            // services.AddScoped<CheckoutService>();
            // services.AddScoped<CheckoutProductService>();

            #endregion
        }
    }
}