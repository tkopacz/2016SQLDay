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
    builder.EntitySet<SalesOrderDetail>("SalesOrderDetails");
    builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders"); 
    builder.EntitySet<SpecialOfferProduct>("SpecialOfferProducts"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SalesOrderDetailsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/SalesOrderDetails
        [EnableQuery]
        public IQueryable<SalesOrderDetail> GetSalesOrderDetails()
        {
            return db.SalesOrderDetails;
        }

        // GET: odata/SalesOrderDetails(5)
        [EnableQuery]
        public SingleResult<SalesOrderDetail> GetSalesOrderDetail([FromODataUri] int key)
        {
            return SingleResult.Create(db.SalesOrderDetails.Where(salesOrderDetail => salesOrderDetail.SalesOrderID == key));
        }

        // PUT: odata/SalesOrderDetails(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<SalesOrderDetail> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SalesOrderDetail salesOrderDetail = await db.SalesOrderDetails.FindAsync(key);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            patch.Put(salesOrderDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderDetailExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(salesOrderDetail);
        }

        // POST: odata/SalesOrderDetails
        public async Task<IHttpActionResult> Post(SalesOrderDetail salesOrderDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SalesOrderDetails.Add(salesOrderDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesOrderDetailExists(salesOrderDetail.SalesOrderID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(salesOrderDetail);
        }

        // PATCH: odata/SalesOrderDetails(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<SalesOrderDetail> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SalesOrderDetail salesOrderDetail = await db.SalesOrderDetails.FindAsync(key);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            patch.Patch(salesOrderDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderDetailExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(salesOrderDetail);
        }

        // DELETE: odata/SalesOrderDetails(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            SalesOrderDetail salesOrderDetail = await db.SalesOrderDetails.FindAsync(key);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            db.SalesOrderDetails.Remove(salesOrderDetail);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/SalesOrderDetails(5)/SalesOrderHeader
        [EnableQuery]
        public SingleResult<SalesOrderHeader> GetSalesOrderHeader([FromODataUri] int key)
        {
            return SingleResult.Create(db.SalesOrderDetails.Where(m => m.SalesOrderID == key).Select(m => m.SalesOrderHeader));
        }

        // GET: odata/SalesOrderDetails(5)/SpecialOfferProduct
        [EnableQuery]
        public SingleResult<SpecialOfferProduct> GetSpecialOfferProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.SalesOrderDetails.Where(m => m.SalesOrderID == key).Select(m => m.SpecialOfferProduct));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalesOrderDetailExists(int key)
        {
            return db.SalesOrderDetails.Count(e => e.SalesOrderID == key) > 0;
        }
    }
}
