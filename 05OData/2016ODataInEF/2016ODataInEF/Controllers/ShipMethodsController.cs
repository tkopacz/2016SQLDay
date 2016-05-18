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
    builder.EntitySet<ShipMethod>("ShipMethods");
    builder.EntitySet<PurchaseOrderHeader>("PurchaseOrderHeaders"); 
    builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ShipMethodsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/ShipMethods
        [EnableQuery]
        public IQueryable<ShipMethod> GetShipMethods()
        {
            return db.ShipMethods;
        }

        // GET: odata/ShipMethods(5)
        [EnableQuery]
        public SingleResult<ShipMethod> GetShipMethod([FromODataUri] int key)
        {
            return SingleResult.Create(db.ShipMethods.Where(shipMethod => shipMethod.ShipMethodID == key));
        }

        // PUT: odata/ShipMethods(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ShipMethod> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShipMethod shipMethod = await db.ShipMethods.FindAsync(key);
            if (shipMethod == null)
            {
                return NotFound();
            }

            patch.Put(shipMethod);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipMethodExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shipMethod);
        }

        // POST: odata/ShipMethods
        public async Task<IHttpActionResult> Post(ShipMethod shipMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ShipMethods.Add(shipMethod);
            await db.SaveChangesAsync();

            return Created(shipMethod);
        }

        // PATCH: odata/ShipMethods(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ShipMethod> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShipMethod shipMethod = await db.ShipMethods.FindAsync(key);
            if (shipMethod == null)
            {
                return NotFound();
            }

            patch.Patch(shipMethod);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipMethodExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shipMethod);
        }

        // DELETE: odata/ShipMethods(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ShipMethod shipMethod = await db.ShipMethods.FindAsync(key);
            if (shipMethod == null)
            {
                return NotFound();
            }

            db.ShipMethods.Remove(shipMethod);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ShipMethods(5)/PurchaseOrderHeaders
        [EnableQuery]
        public IQueryable<PurchaseOrderHeader> GetPurchaseOrderHeaders([FromODataUri] int key)
        {
            return db.ShipMethods.Where(m => m.ShipMethodID == key).SelectMany(m => m.PurchaseOrderHeaders);
        }

        // GET: odata/ShipMethods(5)/SalesOrderHeaders
        [EnableQuery]
        public IQueryable<SalesOrderHeader> GetSalesOrderHeaders([FromODataUri] int key)
        {
            return db.ShipMethods.Where(m => m.ShipMethodID == key).SelectMany(m => m.SalesOrderHeaders);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShipMethodExists(int key)
        {
            return db.ShipMethods.Count(e => e.ShipMethodID == key) > 0;
        }
    }
}
