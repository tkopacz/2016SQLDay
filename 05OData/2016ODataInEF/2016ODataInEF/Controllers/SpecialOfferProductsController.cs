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
    builder.EntitySet<SpecialOfferProduct>("SpecialOfferProducts");
    builder.EntitySet<Product>("Products"); 
    builder.EntitySet<SalesOrderDetail>("SalesOrderDetails"); 
    builder.EntitySet<SpecialOffer>("SpecialOffers"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SpecialOfferProductsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/SpecialOfferProducts
        [EnableQuery]
        public IQueryable<SpecialOfferProduct> GetSpecialOfferProducts()
        {
            return db.SpecialOfferProducts;
        }

        // GET: odata/SpecialOfferProducts(5)
        [EnableQuery]
        public SingleResult<SpecialOfferProduct> GetSpecialOfferProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.SpecialOfferProducts.Where(specialOfferProduct => specialOfferProduct.SpecialOfferID == key));
        }

        // PUT: odata/SpecialOfferProducts(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<SpecialOfferProduct> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SpecialOfferProduct specialOfferProduct = await db.SpecialOfferProducts.FindAsync(key);
            if (specialOfferProduct == null)
            {
                return NotFound();
            }

            patch.Put(specialOfferProduct);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialOfferProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(specialOfferProduct);
        }

        // POST: odata/SpecialOfferProducts
        public async Task<IHttpActionResult> Post(SpecialOfferProduct specialOfferProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SpecialOfferProducts.Add(specialOfferProduct);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SpecialOfferProductExists(specialOfferProduct.SpecialOfferID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(specialOfferProduct);
        }

        // PATCH: odata/SpecialOfferProducts(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<SpecialOfferProduct> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SpecialOfferProduct specialOfferProduct = await db.SpecialOfferProducts.FindAsync(key);
            if (specialOfferProduct == null)
            {
                return NotFound();
            }

            patch.Patch(specialOfferProduct);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialOfferProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(specialOfferProduct);
        }

        // DELETE: odata/SpecialOfferProducts(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            SpecialOfferProduct specialOfferProduct = await db.SpecialOfferProducts.FindAsync(key);
            if (specialOfferProduct == null)
            {
                return NotFound();
            }

            db.SpecialOfferProducts.Remove(specialOfferProduct);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/SpecialOfferProducts(5)/Product
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.SpecialOfferProducts.Where(m => m.SpecialOfferID == key).Select(m => m.Product));
        }

        // GET: odata/SpecialOfferProducts(5)/SalesOrderDetails
        [EnableQuery]
        public IQueryable<SalesOrderDetail> GetSalesOrderDetails([FromODataUri] int key)
        {
            return db.SpecialOfferProducts.Where(m => m.SpecialOfferID == key).SelectMany(m => m.SalesOrderDetails);
        }

        // GET: odata/SpecialOfferProducts(5)/SpecialOffer
        [EnableQuery]
        public SingleResult<SpecialOffer> GetSpecialOffer([FromODataUri] int key)
        {
            return SingleResult.Create(db.SpecialOfferProducts.Where(m => m.SpecialOfferID == key).Select(m => m.SpecialOffer));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpecialOfferProductExists(int key)
        {
            return db.SpecialOfferProducts.Count(e => e.SpecialOfferID == key) > 0;
        }
    }
}
