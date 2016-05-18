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
    builder.EntitySet<CurrencyRate>("CurrencyRates");
    builder.EntitySet<Currency>("Currencies"); 
    builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CurrencyRatesController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/CurrencyRates
        [EnableQuery]
        public IQueryable<CurrencyRate> GetCurrencyRates()
        {
            return db.CurrencyRates;
        }

        // GET: odata/CurrencyRates(5)
        [EnableQuery]
        public SingleResult<CurrencyRate> GetCurrencyRate([FromODataUri] int key)
        {
            return SingleResult.Create(db.CurrencyRates.Where(currencyRate => currencyRate.CurrencyRateID == key));
        }

        // PUT: odata/CurrencyRates(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<CurrencyRate> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CurrencyRate currencyRate = await db.CurrencyRates.FindAsync(key);
            if (currencyRate == null)
            {
                return NotFound();
            }

            patch.Put(currencyRate);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyRateExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(currencyRate);
        }

        // POST: odata/CurrencyRates
        public async Task<IHttpActionResult> Post(CurrencyRate currencyRate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CurrencyRates.Add(currencyRate);
            await db.SaveChangesAsync();

            return Created(currencyRate);
        }

        // PATCH: odata/CurrencyRates(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<CurrencyRate> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CurrencyRate currencyRate = await db.CurrencyRates.FindAsync(key);
            if (currencyRate == null)
            {
                return NotFound();
            }

            patch.Patch(currencyRate);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyRateExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(currencyRate);
        }

        // DELETE: odata/CurrencyRates(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            CurrencyRate currencyRate = await db.CurrencyRates.FindAsync(key);
            if (currencyRate == null)
            {
                return NotFound();
            }

            db.CurrencyRates.Remove(currencyRate);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CurrencyRates(5)/Currency
        [EnableQuery]
        public SingleResult<Currency> GetCurrency([FromODataUri] int key)
        {
            return SingleResult.Create(db.CurrencyRates.Where(m => m.CurrencyRateID == key).Select(m => m.Currency));
        }

        // GET: odata/CurrencyRates(5)/Currency1
        [EnableQuery]
        public SingleResult<Currency> GetCurrency1([FromODataUri] int key)
        {
            return SingleResult.Create(db.CurrencyRates.Where(m => m.CurrencyRateID == key).Select(m => m.Currency1));
        }

        // GET: odata/CurrencyRates(5)/SalesOrderHeaders
        [EnableQuery]
        public IQueryable<SalesOrderHeader> GetSalesOrderHeaders([FromODataUri] int key)
        {
            return db.CurrencyRates.Where(m => m.CurrencyRateID == key).SelectMany(m => m.SalesOrderHeaders);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CurrencyRateExists(int key)
        {
            return db.CurrencyRates.Count(e => e.CurrencyRateID == key) > 0;
        }
    }
}
