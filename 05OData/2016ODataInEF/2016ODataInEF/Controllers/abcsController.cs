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
    builder.EntitySet<abc>("abcs");
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class abcsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/abcs
        [EnableQuery(PageSize =10)]
        public IQueryable<abc> Getabcs()
        {
            return db.abcs;
        }

        // GET: odata/abcs(5)
        [EnableQuery]
        public SingleResult<abc> Getabc([FromODataUri] int key)
        {
            return SingleResult.Create(db.abcs.Where(abc => abc.id == key));
        }

        // PUT: odata/abcs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<abc> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            abc abc = await db.abcs.FindAsync(key);
            if (abc == null)
            {
                return NotFound();
            }

            patch.Put(abc);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!abcExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(abc);
        }

        // POST: odata/abcs
        public async Task<IHttpActionResult> Post(abc abc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.abcs.Add(abc);
            await db.SaveChangesAsync();

            return Created(abc);
        }

        // PATCH: odata/abcs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<abc> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            abc abc = await db.abcs.FindAsync(key);
            if (abc == null)
            {
                return NotFound();
            }

            patch.Patch(abc);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!abcExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(abc);
        }

        // DELETE: odata/abcs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            abc abc = await db.abcs.FindAsync(key);
            if (abc == null)
            {
                return NotFound();
            }

            db.abcs.Remove(abc);
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

        private bool abcExists(int key)
        {
            return db.abcs.Count(e => e.id == key) > 0;
        }
    }
}
