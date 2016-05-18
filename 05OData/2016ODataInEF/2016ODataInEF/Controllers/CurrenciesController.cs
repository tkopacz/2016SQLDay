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
    builder.EntitySet<Currency>("Currencies");
    builder.EntitySet<CountryRegionCurrency>("CountryRegionCurrencies"); 
    builder.EntitySet<CurrencyRate>("CurrencyRates"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CurrenciesController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/Currencies
        [EnableQuery]
        public IQueryable<Currency> GetCurrencies()
        {
            return db.Currencies;
        }

        // GET: odata/Currencies(5)
        [EnableQuery]
        public SingleResult<Currency> GetCurrency([FromODataUri] string key)
        {
            return SingleResult.Create(db.Currencies.Where(currency => currency.CurrencyCode == key));
        }

        // PUT: odata/Currencies(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Currency> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Currency currency = await db.Currencies.FindAsync(key);
            if (currency == null)
            {
                return NotFound();
            }

            patch.Put(currency);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(currency);
        }

        // POST: odata/Currencies
        public async Task<IHttpActionResult> Post(Currency currency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Currencies.Add(currency);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CurrencyExists(currency.CurrencyCode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(currency);
        }

        // PATCH: odata/Currencies(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Currency> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Currency currency = await db.Currencies.FindAsync(key);
            if (currency == null)
            {
                return NotFound();
            }

            patch.Patch(currency);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(currency);
        }

        // DELETE: odata/Currencies(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            Currency currency = await db.Currencies.FindAsync(key);
            if (currency == null)
            {
                return NotFound();
            }

            db.Currencies.Remove(currency);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Currencies(5)/CountryRegionCurrencies
        [EnableQuery]
        public IQueryable<CountryRegionCurrency> GetCountryRegionCurrencies([FromODataUri] string key)
        {
            return db.Currencies.Where(m => m.CurrencyCode == key).SelectMany(m => m.CountryRegionCurrencies);
        }

        // GET: odata/Currencies(5)/CurrencyRates
        [EnableQuery]
        public IQueryable<CurrencyRate> GetCurrencyRates([FromODataUri] string key)
        {
            return db.Currencies.Where(m => m.CurrencyCode == key).SelectMany(m => m.CurrencyRates);
        }

        // GET: odata/Currencies(5)/CurrencyRates1
        [EnableQuery]
        public IQueryable<CurrencyRate> GetCurrencyRates1([FromODataUri] string key)
        {
            return db.Currencies.Where(m => m.CurrencyCode == key).SelectMany(m => m.CurrencyRates1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CurrencyExists(string key)
        {
            return db.Currencies.Count(e => e.CurrencyCode == key) > 0;
        }
    }
}
