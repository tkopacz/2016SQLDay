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
    builder.EntitySet<StateProvince>("StateProvinces");
    builder.EntitySet<Address>("Addresses"); 
    builder.EntitySet<CountryRegion>("CountryRegions"); 
    builder.EntitySet<SalesTaxRate>("SalesTaxRates"); 
    builder.EntitySet<SalesTerritory>("SalesTerritories"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StateProvincesController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/StateProvinces
        [EnableQuery]
        public IQueryable<StateProvince> GetStateProvinces()
        {
            return db.StateProvinces;
        }

        // GET: odata/StateProvinces(5)
        [EnableQuery]
        public SingleResult<StateProvince> GetStateProvince([FromODataUri] int key)
        {
            return SingleResult.Create(db.StateProvinces.Where(stateProvince => stateProvince.StateProvinceID == key));
        }

        // PUT: odata/StateProvinces(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<StateProvince> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StateProvince stateProvince = await db.StateProvinces.FindAsync(key);
            if (stateProvince == null)
            {
                return NotFound();
            }

            patch.Put(stateProvince);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateProvinceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(stateProvince);
        }

        // POST: odata/StateProvinces
        public async Task<IHttpActionResult> Post(StateProvince stateProvince)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StateProvinces.Add(stateProvince);
            await db.SaveChangesAsync();

            return Created(stateProvince);
        }

        // PATCH: odata/StateProvinces(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<StateProvince> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StateProvince stateProvince = await db.StateProvinces.FindAsync(key);
            if (stateProvince == null)
            {
                return NotFound();
            }

            patch.Patch(stateProvince);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateProvinceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(stateProvince);
        }

        // DELETE: odata/StateProvinces(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            StateProvince stateProvince = await db.StateProvinces.FindAsync(key);
            if (stateProvince == null)
            {
                return NotFound();
            }

            db.StateProvinces.Remove(stateProvince);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/StateProvinces(5)/Addresses
        [EnableQuery]
        public IQueryable<Address> GetAddresses([FromODataUri] int key)
        {
            return db.StateProvinces.Where(m => m.StateProvinceID == key).SelectMany(m => m.Addresses);
        }

        // GET: odata/StateProvinces(5)/CountryRegion
        [EnableQuery]
        public SingleResult<CountryRegion> GetCountryRegion([FromODataUri] int key)
        {
            return SingleResult.Create(db.StateProvinces.Where(m => m.StateProvinceID == key).Select(m => m.CountryRegion));
        }

        // GET: odata/StateProvinces(5)/SalesTaxRates
        [EnableQuery]
        public IQueryable<SalesTaxRate> GetSalesTaxRates([FromODataUri] int key)
        {
            return db.StateProvinces.Where(m => m.StateProvinceID == key).SelectMany(m => m.SalesTaxRates);
        }

        // GET: odata/StateProvinces(5)/SalesTerritory
        [EnableQuery]
        public SingleResult<SalesTerritory> GetSalesTerritory([FromODataUri] int key)
        {
            return SingleResult.Create(db.StateProvinces.Where(m => m.StateProvinceID == key).Select(m => m.SalesTerritory));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StateProvinceExists(int key)
        {
            return db.StateProvinces.Count(e => e.StateProvinceID == key) > 0;
        }
    }
}
