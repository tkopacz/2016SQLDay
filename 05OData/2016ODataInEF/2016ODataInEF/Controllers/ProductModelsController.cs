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
    builder.EntitySet<ProductModel>("ProductModels");
    builder.EntitySet<ProductModelIllustration>("ProductModelIllustrations"); 
    builder.EntitySet<ProductModelProductDescriptionCulture>("ProductModelProductDescriptionCultures"); 
    builder.EntitySet<Product>("Products"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductModelsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/ProductModels
        [EnableQuery]
        public IQueryable<ProductModel> GetProductModels()
        {
            return db.ProductModels;
        }

        // GET: odata/ProductModels(5)
        [EnableQuery]
        public SingleResult<ProductModel> GetProductModel([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductModels.Where(productModel => productModel.ProductModelID == key));
        }

        // PUT: odata/ProductModels(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ProductModel> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductModel productModel = await db.ProductModels.FindAsync(key);
            if (productModel == null)
            {
                return NotFound();
            }

            patch.Put(productModel);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productModel);
        }

        // POST: odata/ProductModels
        public async Task<IHttpActionResult> Post(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductModels.Add(productModel);
            await db.SaveChangesAsync();

            return Created(productModel);
        }

        // PATCH: odata/ProductModels(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ProductModel> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductModel productModel = await db.ProductModels.FindAsync(key);
            if (productModel == null)
            {
                return NotFound();
            }

            patch.Patch(productModel);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productModel);
        }

        // DELETE: odata/ProductModels(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ProductModel productModel = await db.ProductModels.FindAsync(key);
            if (productModel == null)
            {
                return NotFound();
            }

            db.ProductModels.Remove(productModel);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProductModels(5)/ProductModelIllustrations
        [EnableQuery]
        public IQueryable<ProductModelIllustration> GetProductModelIllustrations([FromODataUri] int key)
        {
            return db.ProductModels.Where(m => m.ProductModelID == key).SelectMany(m => m.ProductModelIllustrations);
        }

        // GET: odata/ProductModels(5)/ProductModelProductDescriptionCultures
        [EnableQuery]
        public IQueryable<ProductModelProductDescriptionCulture> GetProductModelProductDescriptionCultures([FromODataUri] int key)
        {
            return db.ProductModels.Where(m => m.ProductModelID == key).SelectMany(m => m.ProductModelProductDescriptionCultures);
        }

        // GET: odata/ProductModels(5)/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.ProductModels.Where(m => m.ProductModelID == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductModelExists(int key)
        {
            return db.ProductModels.Count(e => e.ProductModelID == key) > 0;
        }
    }
}
