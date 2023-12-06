using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebShopOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "HomePage",
                url: "home",
                defaults: new { controller = "Home", action = "Index", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "Intro",
                url: "intro",
                defaults: new { controller = "Intro", action = "Index", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "Contact", action = "Index", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "CustomerInfor",
                url: "account/customerinfor-{name}",
                defaults: new { controller = "Account", action = "CustomerInfor", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "ShoppingCart",
                url: "cart",
                defaults: new { controller = "ShoppingCart", action = "Index", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "CheckOut",
                url: "payment",
                defaults: new { controller = "ShoppingCart", action = "CheckOut", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "DetailProduct",
                url: "product/detail/{alias}-{id}",
                defaults: new { controller = "Products", action = "Detail", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "DetailNews",
                url: "news/detail/{id}",
                defaults: new { controller = "News", action = "NewsDetail", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "ProductCategory",
                url: "product-category/{alias}-{id}",
                defaults: new { controller = "Products", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "Products",
                url: "product",
                defaults: new { controller = "Products", action = "Index", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "News",
                url: "news",
                defaults: new { controller = "News", action = "Index", Alias = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebShopOnline.Controllers" }
            );
        }
    }
}
