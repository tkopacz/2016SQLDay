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
    builder.EntitySet<BusinessEntityAddress>("BusinessEntityAddresses");
    builder.EntitySet<Address>("Addresses"); 
    builder.EntitySet<AddressType>("AddressTypes"); 
    builder.EntitySet<BusinessEntity>("BusinessEntities"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class BusinessEntityAddressesController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/BusinessEntityAddresses
        [EnableQuery]
        public IQueryable<BusinessEntityAddress> GetBusinessEntityAddresses()
        {
            return db.BusinessEntityAddresses;
        }

        // GET: odata/BusinessEntityAddresses(5)
        [EnableQuery]
        public SingleResult<BusinessEntityAddress> GetBusinessEntityAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.BusinessEntityAddresses.Where(businessEntityAddress => businessEntityAddress.BusinessEntityID == key));
        }

        // PUT: odata/BusinessEntityAddresses(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<BusinessEntityAddress> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BusinessEntityAddress businessEntityAddress = await db.BusinessEntityAddresses.FindAsync(key);
            if (businessEntityAddress == null)
            {
                return NotFound();
            }

            patch.Put(businessEntityAddress);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessEntityAddressExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(businessEntityAddress);
        }

        // POST: odata/BusinessEntityAddresses
        public async Task<IHttpActionResult> Post(BusinessEntityAddress businessEntityAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BusinessEntityAddresses.Add(businessEntityAddress);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BusinessEntityAddressExists(businessEntityAddress.BusinessEntityID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(businessEntityAddress);
        }

        // PATCH: odata/BusinessEntityAddresses(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<BusinessEntityAddress> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BusinessEntityAddress businessEntityAddress = await db.BusinessEntityAddresses.FindAsync(key);
            if (businessEntityAddress == null)
            {
                return NotFound();
            }

            patch.Patch(businessEntityAddress);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessEntityAddressExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(businessEntityAddress);
        }

        // DELETE: odata/BusinessEntityAddresses(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            BusinessEntityAddress businessEntityAddress = await db.BusinessEntityAddresses.FindAsync(key);
            if (businessEntityAddress == null)
            {
                return NotFound();
            }

            db.BusinessEntityAddresses.Remove(businessEntityAddress);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/BusinessEntityAddresses(5)/Address
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.BusinessEntityAddresses.Where(m => m.BusinessEntityID == key).Select(m => m.Address));
        }

        // GET: odata/BusinessEntityAddresses(5)/AddressType
        [EnableQuery]
        public SingleResult<AddressType> GetAddressType([FromODataUri] int key)
        {
            return SingleResult.Create(db.BusinessEntityAddresses.Where(m => m.BusinessEntityID == key).Select(m => m.AddressType));
        }

        // GET: odata/BusinessEntityAddresses(5)/BusinessEntity
        [EnableQuery]
        public SingleResult<BusinessEntity> GetBusinessEntity([FromODataUri] int key)
        {
            return SingleResult.Create(db.BusinessEntityAddresses.Where(m => m.BusinessEntityID == key).Select(m => m.BusinessEntity));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BusinessEntityAddressExists(int key)
        {
            return db.BusinessEntityAddresses.Count(e => e.BusinessEntityID == key) > 0;
        }
    }
}
