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
    builder.EntitySet<vSalesPersonSalesByFiscalYear>("vSalesPersonSalesByFiscalYears");
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class vSalesPersonSalesByFiscalYearsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/vSalesPersonSalesByFiscalYears
        [EnableQuery(PageSize =10,
            AllowedArithmeticOperators =AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.All,
            AllowedLogicalOperators =AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All
            )]
        public IQueryable<vSalesPersonSalesByFiscalYear> GetvSalesPersonSalesByFiscalYears()
        {
            return db.vSalesPersonSalesByFiscalYears;
        }

        // GET: odata/vSalesPersonSalesByFiscalYears(5)
        [EnableQuery]
        public SingleResult<vSalesPersonSalesByFiscalYear> GetvSalesPersonSalesByFiscalYear([FromODataUri] string key)
        {
            return SingleResult.Create(db.vSalesPersonSalesByFiscalYears.Where(vSalesPersonSalesByFiscalYear => vSalesPersonSalesByFiscalYear.JobTitle == key));
        }

        // PUT: odata/vSalesPersonSalesByFiscalYears(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<vSalesPersonSalesByFiscalYear> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            vSalesPersonSalesByFiscalYear vSalesPersonSalesByFiscalYear = await db.vSalesPersonSalesByFiscalYears.FindAsync(key);
            if (vSalesPersonSalesByFiscalYear == null)
            {
                return NotFound();
            }

            patch.Put(vSalesPersonSalesByFiscalYear);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSalesPersonSalesByFiscalYearExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(vSalesPersonSalesByFiscalYear);
        }

        // POST: odata/vSalesPersonSalesByFiscalYears
        public async Task<IHttpActionResult> Post(vSalesPersonSalesByFiscalYear vSalesPersonSalesByFiscalYear)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vSalesPersonSalesByFiscalYears.Add(vSalesPersonSalesByFiscalYear);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (vSalesPersonSalesByFiscalYearExists(vSalesPersonSalesByFiscalYear.JobTitle))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(vSalesPersonSalesByFiscalYear);
        }

        // PATCH: odata/vSalesPersonSalesByFiscalYears(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<vSalesPersonSalesByFiscalYear> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            vSalesPersonSalesByFiscalYear vSalesPersonSalesByFiscalYear = await db.vSalesPersonSalesByFiscalYears.FindAsync(key);
            if (vSalesPersonSalesByFiscalYear == null)
            {
                return NotFound();
            }

            patch.Patch(vSalesPersonSalesByFiscalYear);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSalesPersonSalesByFiscalYearExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(vSalesPersonSalesByFiscalYear);
        }

        // DELETE: odata/vSalesPersonSalesByFiscalYears(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            vSalesPersonSalesByFiscalYear vSalesPersonSalesByFiscalYear = await db.vSalesPersonSalesByFiscalYears.FindAsync(key);
            if (vSalesPersonSalesByFiscalYear == null)
            {
                return NotFound();
            }

            db.vSalesPersonSalesByFiscalYears.Remove(vSalesPersonSalesByFiscalYear);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vSalesPersonSalesByFiscalYearExists(string key)
        {
            return db.vSalesPersonSalesByFiscalYears.Count(e => e.JobTitle == key) > 0;
        }
    }
}
