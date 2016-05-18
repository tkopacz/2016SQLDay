using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using _2016ODataInEF.Models;

namespace _2016ODataInEF.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using _2016ODataInEF.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
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
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: odata/Products(5)
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(product => product.ProductID == key));
        }

        // PUT: odata/Products(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Product> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Put(product);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }

        // POST: odata/Products
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Created(product);
        }

        // PATCH: odata/Products(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Patch(product);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }

        // DELETE: odata/Products(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Product product = await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Products(5)/BillOfMaterials
        [EnableQuery]
        public IQueryable<BillOfMaterial> GetBillOfMaterials([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.BillOfMaterials);
        }

        // GET: odata/Products(5)/BillOfMaterials1
        [EnableQuery]
        public IQueryable<BillOfMaterial> GetBillOfMaterials1([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.BillOfMaterials1);
        }

        // GET: odata/Products(5)/ProductCostHistories
        [EnableQuery]
        public IQueryable<ProductCostHistory> GetProductCostHistories([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.ProductCostHistories);
        }

        // GET: odata/Products(5)/ProductDocument
        [EnableQuery]
        public SingleResult<ProductDocument> GetProductDocument([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(m => m.ProductID == key).Select(m => m.ProductDocument));
        }

        // GET: odata/Products(5)/ProductInventories
        [EnableQuery]
        public IQueryable<ProductInventory> GetProductInventories([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.ProductInventories);
        }

        // GET: odata/Products(5)/ProductListPriceHistories
        [EnableQuery]
        public IQueryable<ProductListPriceHistory> GetProductListPriceHistories([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.ProductListPriceHistories);
        }

        // GET: odata/Products(5)/ProductModel
        [EnableQuery]
        public SingleResult<ProductModel> GetProductModel([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(m => m.ProductID == key).Select(m => m.ProductModel));
        }

        // GET: odata/Products(5)/ProductProductPhotoes
        [EnableQuery]
        public IQueryable<ProductProductPhoto> GetProductProductPhotoes([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.ProductProductPhotoes);
        }

        // GET: odata/Products(5)/ProductReviews
        [EnableQuery]
        public IQueryable<ProductReview> GetProductReviews([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.ProductReviews);
        }

        // GET: odata/Products(5)/ProductSubcategory
        [EnableQuery]
        public SingleResult<ProductSubcategory> GetProductSubcategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(m => m.ProductID == key).Select(m => m.ProductSubcategory));
        }

        // GET: odata/Products(5)/ProductVendors
        [EnableQuery]
        public IQueryable<ProductVendor> GetProductVendors([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.ProductVendors);
        }

        // GET: odata/Products(5)/PurchaseOrderDetails
        [EnableQuery]
        public IQueryable<PurchaseOrderDetail> GetPurchaseOrderDetails([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.PurchaseOrderDetails);
        }

        // GET: odata/Products(5)/ShoppingCartItems
        [EnableQuery]
        public IQueryable<ShoppingCartItem> GetShoppingCartItems([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.ShoppingCartItems);
        }

        // GET: odata/Products(5)/SpecialOfferProducts
        [EnableQuery]
        public IQueryable<SpecialOfferProduct> GetSpecialOfferProducts([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.SpecialOfferProducts);
        }

        // GET: odata/Products(5)/TransactionHistories
        [EnableQuery]
        public IQueryable<TransactionHistory> GetTransactionHistories([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.TransactionHistories);
        }

        // GET: odata/Products(5)/UnitMeasure
        [EnableQuery]
        public SingleResult<UnitMeasure> GetUnitMeasure([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(m => m.ProductID == key).Select(m => m.UnitMeasure));
        }

        // GET: odata/Products(5)/UnitMeasure1
        [EnableQuery]
        public SingleResult<UnitMeasure> GetUnitMeasure1([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(m => m.ProductID == key).Select(m => m.UnitMeasure1));
        }

        // GET: odata/Products(5)/WorkOrders
        [EnableQuery]
        public IQueryable<WorkOrder> GetWorkOrders([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.WorkOrders);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int key)
        {
            return db.Products.Count(e => e.ProductID == key) > 0;
        }
    }
}
