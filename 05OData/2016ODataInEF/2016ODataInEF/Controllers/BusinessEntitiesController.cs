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
    builder.EntitySet<BusinessEntity>("BusinessEntities");
    builder.EntitySet<BusinessEntityAddress>("BusinessEntityAddresses"); 
    builder.EntitySet<BusinessEntityContact>("BusinessEntityContacts"); 
    builder.EntitySet<Person>("People"); 
    builder.EntitySet<Store>("Stores"); 
    builder.EntitySet<Vendor>("Vendors"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class BusinessEntitiesController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/BusinessEntities
        [EnableQuery]
        public IQueryable<BusinessEntity> GetBusinessEntities()
        {
            return db.BusinessEntities;
        }

        // GET: odata/BusinessEntities(5)
        [EnableQuery]
        public SingleResult<BusinessEntity> GetBusinessEntity([FromODataUri] int key)
        {
            return SingleResult.Create(db.BusinessEntities.Where(businessEntity => businessEntity.BusinessEntityID == key));
        }

        // PUT: odata/BusinessEntities(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<BusinessEntity> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BusinessEntity businessEntity = await db.BusinessEntities.FindAsync(key);
            if (businessEntity == null)
            {
                return NotFound();
            }

            patch.Put(businessEntity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessEntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(businessEntity);
        }

        // POST: odata/BusinessEntities
        public async Task<IHttpActionResult> Post(BusinessEntity businessEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BusinessEntities.Add(businessEntity);
            await db.SaveChangesAsync();

            return Created(businessEntity);
        }

        // PATCH: odata/BusinessEntities(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<BusinessEntity> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BusinessEntity businessEntity = await db.BusinessEntities.FindAsync(key);
            if (businessEntity == null)
            {
                return NotFound();
            }

            patch.Patch(businessEntity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessEntityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(businessEntity);
        }

        // DELETE: odata/BusinessEntities(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            BusinessEntity businessEntity = await db.BusinessEntities.FindAsync(key);
            if (businessEntity == null)
            {
                return NotFound();
            }

            db.BusinessEntities.Remove(businessEntity);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/BusinessEntities(5)/BusinessEntityAddresses
        [EnableQuery]
        public IQueryable<BusinessEntityAddress> GetBusinessEntityAddresses([FromODataUri] int key)
        {
            return db.BusinessEntities.Where(m => m.BusinessEntityID == key).SelectMany(m => m.BusinessEntityAddresses);
        }

        // GET: odata/BusinessEntities(5)/BusinessEntityContacts
        [EnableQuery]
        public IQueryable<BusinessEntityContact> GetBusinessEntityContacts([FromODataUri] int key)
        {
            return db.BusinessEntities.Where(m => m.BusinessEntityID == key).SelectMany(m => m.BusinessEntityContacts);
        }

        // GET: odata/BusinessEntities(5)/Person
        [EnableQuery]
        public SingleResult<Person> GetPerson([FromODataUri] int key)
        {
            return SingleResult.Create(db.BusinessEntities.Where(m => m.BusinessEntityID == key).Select(m => m.Person));
        }

        // GET: odata/BusinessEntities(5)/Store
        [EnableQuery]
        public SingleResult<Store> GetStore([FromODataUri] int key)
        {
            return SingleResult.Create(db.BusinessEntities.Where(m => m.BusinessEntityID == key).Select(m => m.Store));
        }

        // GET: odata/BusinessEntities(5)/Vendor
        [EnableQuery]
        public SingleResult<Vendor> GetVendor([FromODataUri] int key)
        {
            return SingleResult.Create(db.BusinessEntities.Where(m => m.BusinessEntityID == key).Select(m => m.Vendor));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BusinessEntityExists(int key)
        {
            return db.BusinessEntities.Count(e => e.BusinessEntityID == key) > 0;
        }
    }
}
