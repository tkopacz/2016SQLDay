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
    builder.EntitySet<abc1>("abc1");
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class abc1Controller : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/abc1
        [EnableQuery]
        public IQueryable<abc1> Getabc1()
        {
            return db.abc1;
        }

        // GET: odata/abc1(5)
        [EnableQuery]
        public SingleResult<abc1> Getabc1([FromODataUri] int key)
        {
            return SingleResult.Create(db.abc1.Where(abc1 => abc1.id == key));
        }

        // PUT: odata/abc1(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<abc1> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            abc1 abc1 = await db.abc1.FindAsync(key);
            if (abc1 == null)
            {
                return NotFound();
            }

            patch.Put(abc1);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!abc1Exists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(abc1);
        }

        // POST: odata/abc1
        public async Task<IHttpActionResult> Post(abc1 abc1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.abc1.Add(abc1);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (abc1Exists(abc1.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(abc1);
        }

        // PATCH: odata/abc1(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<abc1> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            abc1 abc1 = await db.abc1.FindAsync(key);
            if (abc1 == null)
            {
                return NotFound();
            }

            patch.Patch(abc1);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!abc1Exists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(abc1);
        }

        // DELETE: odata/abc1(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            abc1 abc1 = await db.abc1.FindAsync(key);
            if (abc1 == null)
            {
                return NotFound();
            }

            db.abc1.Remove(abc1);
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

        private bool abc1Exists(int key)
        {
            return db.abc1.Count(e => e.id == key) > 0;
        }
    }
}
