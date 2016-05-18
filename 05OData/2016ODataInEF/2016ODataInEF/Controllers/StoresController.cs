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
    builder.EntitySet<Store>("Stores");
    builder.EntitySet<BusinessEntity>("BusinessEntities"); 
    builder.EntitySet<Customer>("Customers"); 
    builder.EntitySet<SalesPerson>("SalesPersons"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StoresController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/Stores
        [EnableQuery]
        public IQueryable<Store> GetStores()
        {
            return db.Stores;
        }

        // GET: odata/Stores(5)
        [EnableQuery]
        public SingleResult<Store> GetStore([FromODataUri] int key)
        {
            return SingleResult.Create(db.Stores.Where(store => store.BusinessEntityID == key));
        }

        // PUT: odata/Stores(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Store> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Store store = await db.Stores.FindAsync(key);
            if (store == null)
            {
                return NotFound();
            }

            patch.Put(store);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(store);
        }

        // POST: odata/Stores
        public async Task<IHttpActionResult> Post(Store store)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stores.Add(store);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoreExists(store.BusinessEntityID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(store);
        }

        // PATCH: odata/Stores(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Store> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Store store = await db.Stores.FindAsync(key);
            if (store == null)
            {
                return NotFound();
            }

            patch.Patch(store);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(store);
        }

        // DELETE: odata/Stores(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Store store = await db.Stores.FindAsync(key);
            if (store == null)
            {
                return NotFound();
            }

            db.Stores.Remove(store);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Stores(5)/BusinessEntity
        [EnableQuery]
        public SingleResult<BusinessEntity> GetBusinessEntity([FromODataUri] int key)
        {
            return SingleResult.Create(db.Stores.Where(m => m.BusinessEntityID == key).Select(m => m.BusinessEntity));
        }

        // GET: odata/Stores(5)/Customers
        [EnableQuery]
        public IQueryable<Customer> GetCustomers([FromODataUri] int key)
        {
            return db.Stores.Where(m => m.BusinessEntityID == key).SelectMany(m => m.Customers);
        }

        // GET: odata/Stores(5)/SalesPerson
        [EnableQuery]
        public SingleResult<SalesPerson> GetSalesPerson([FromODataUri] int key)
        {
            return SingleResult.Create(db.Stores.Where(m => m.BusinessEntityID == key).Select(m => m.SalesPerson));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreExists(int key)
        {
            return db.Stores.Count(e => e.BusinessEntityID == key) > 0;
        }
    }
}
