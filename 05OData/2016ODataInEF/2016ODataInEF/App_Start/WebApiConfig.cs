using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using _2016ODataInEF.Models;

namespace _2016ODataInEF
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<abc>("abcs");
            builder.EntitySet<abc1>("abc1");
            builder.EntitySet<abc2noncluster>("abc2noncluster");
            builder.EntitySet<abc3insert>("abc3insert");

            builder.EntitySet<PersonCreditCard>("PersonCreditCards");
            builder.EntitySet<CreditCard>("CreditCards");
            builder.EntitySet<Person>("People");

            builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders");

            builder.EntitySet<Currency>("Currencies");
            builder.EntitySet<CountryRegionCurrency>("CountryRegionCurrencies");
            builder.EntitySet<CurrencyRate>("CurrencyRates");

            builder.EntitySet<Product>("Products");
            builder.EntitySet<BillOfMaterial>("BillOfMaterials");
            builder.EntitySet<ProductCostHistory>("ProductCostHistories");
            builder.EntitySet<ProductDocument>("ProductDocuments");
            builder.EntitySet<ProductInventory>("ProductInventories");
            builder.EntitySet<ProductListPriceHistory>("ProductListPriceHistories");
            builder.EntitySet<ProductModel>("ProductModels");
            builder.EntitySet<ProductProductPhoto>("ProductProductPhotoes");
            builder.EntitySet<ProductReview>("ProductReviews");
            builder.EntitySet<ProductSubcategory>("ProductSubcategories");
            builder.EntitySet<ProductVendor>("ProductVendors");
            builder.EntitySet<PurchaseOrderDetail>("PurchaseOrderDetails");
            builder.EntitySet<ShoppingCartItem>("ShoppingCartItems");
            builder.EntitySet<SpecialOfferProduct>("SpecialOfferProducts");
            builder.EntitySet<TransactionHistory>("TransactionHistories");
            builder.EntitySet<UnitMeasure>("UnitMeasures");
            builder.EntitySet<WorkOrder>("WorkOrders");

            builder.EntitySet<SpecialOffer>("SpecialOffers");
            builder.EntitySet<SpecialOfferProduct>("SpecialOfferProducts");

            builder.EntitySet<SpecialOfferProduct>("SpecialOfferProducts");
            builder.EntitySet<Product>("Products");
            builder.EntitySet<SalesOrderDetail>("SalesOrderDetails");
            builder.EntitySet<SpecialOffer>("SpecialOffers");

            builder.EntitySet<SalesOrderDetail>("SalesOrderDetails");
            builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders");
            builder.EntitySet<SpecialOfferProduct>("SpecialOfferProducts");

            builder.EntitySet<ShipMethod>("ShipMethods");
            builder.EntitySet<PurchaseOrderHeader>("PurchaseOrderHeaders");
            builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders");

            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<BusinessEntityAddress>("BusinessEntityAddresses");
            builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders");
            builder.EntitySet<StateProvince>("StateProvinces");

            builder.EntitySet<BusinessEntityAddress>("BusinessEntityAddresses");
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<AddressType>("AddressTypes");
            builder.EntitySet<BusinessEntity>("BusinessEntities");


            builder.EntitySet<Person>("People");
            builder.EntitySet<BusinessEntity>("BusinessEntities");
            builder.EntitySet<BusinessEntityContact>("BusinessEntityContacts");
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<EmailAddress>("EmailAddresses");
            builder.EntitySet<Employee1>("Employees1");
            builder.EntitySet<Password>("Passwords");
            builder.EntitySet<PersonCreditCard>("PersonCreditCards");
            builder.EntitySet<PersonPhone>("PersonPhones");

            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Person>("People");
            builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders");
            builder.EntitySet<SalesTerritory>("SalesTerritories");
            builder.EntitySet<Store>("Stores");

            builder.EntitySet<ProductModel>("ProductModels");
            builder.EntitySet<ProductModelIllustration>("ProductModelIllustrations");
            builder.EntitySet<ProductModelProductDescriptionCulture>("ProductModelProductDescriptionCultures");
            builder.EntitySet<Product>("Products");

            builder.EntitySet<ProductSubcategory>("ProductSubcategories");
            builder.EntitySet<ProductCategory>("ProductCategories");
            builder.EntitySet<Product>("Products");

            builder.EntitySet<Store>("Stores");
            builder.EntitySet<BusinessEntity>("BusinessEntities");
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<SalesPerson>("SalesPersons");

            builder.EntitySet<vSalesPersonSalesByFiscalYear>("vSalesPersonSalesByFiscalYears");

            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
