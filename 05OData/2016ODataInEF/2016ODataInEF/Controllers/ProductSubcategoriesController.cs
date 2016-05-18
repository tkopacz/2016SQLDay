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
    builder.EntitySet<ProductSubcategory>("ProductSubcategories");
    builder.EntitySet<ProductCategory>("ProductCategories"); 
    builder.EntitySet<Product>("Products"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductSubcategoriesController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/ProductSubcategories
        [EnableQuery]
        public IQueryable<ProductSubcategory> GetProductSubcategories()
        {
            return db.ProductSubcategories;
        }

        // GET: odata/ProductSubcategories(5)
        [EnableQuery]
        public SingleResult<ProductSubcategory> GetProductSubcategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductSubcategories.Where(productSubcategory => productSubcategory.ProductSubcategoryID == key));
        }

        // PUT: odata/ProductSubcategories(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ProductSubcategory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductSubcategory productSubcategory = await db.ProductSubcategories.FindAsync(key);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            patch.Put(productSubcategory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductSubcategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productSubcategory);
        }

        // POST: odata/ProductSubcategories
        public async Task<IHttpActionResult> Post(ProductSubcategory productSubcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductSubcategories.Add(productSubcategory);
            await db.SaveChangesAsync();

            return Created(productSubcategory);
        }

        // PATCH: odata/ProductSubcategories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ProductSubcategory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductSubcategory productSubcategory = await db.ProductSubcategories.FindAsync(key);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            patch.Patch(productSubcategory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductSubcategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productSubcategory);
        }

        // DELETE: odata/ProductSubcategories(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ProductSubcategory productSubcategory = await db.ProductSubcategories.FindAsync(key);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            db.ProductSubcategories.Remove(productSubcategory);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProductSubcategories(5)/ProductCategory
        [EnableQuery]
        public SingleResult<ProductCategory> GetProductCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProductSubcategories.Where(m => m.ProductSubcategoryID == key).Select(m => m.ProductCategory));
        }

        // GET: odata/ProductSubcategories(5)/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.ProductSubcategories.Where(m => m.ProductSubcategoryID == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductSubcategoryExists(int key)
        {
            return db.ProductSubcategories.Count(e => e.ProductSubcategoryID == key) > 0;
        }
    }
}
