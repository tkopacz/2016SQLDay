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
    builder.EntitySet<abc3insert>("abc3insert");
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class abc3insertController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/abc3insert
        [EnableQuery]
        public IQueryable<abc3insert> Getabc3insert()
        {
            return db.abc3insert;
        }

        // GET: odata/abc3insert(5)
        [EnableQuery]
        public SingleResult<abc3insert> Getabc3insert([FromODataUri] int key)
        {
            return SingleResult.Create(db.abc3insert.Where(abc3insert => abc3insert.id == key));
        }

        // PUT: odata/abc3insert(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<abc3insert> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            abc3insert abc3insert = await db.abc3insert.FindAsync(key);
            if (abc3insert == null)
            {
                return NotFound();
            }

            patch.Put(abc3insert);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!abc3insertExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(abc3insert);
        }

        // POST: odata/abc3insert
        public async Task<IHttpActionResult> Post(abc3insert abc3insert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.abc3insert.Add(abc3insert);
            await db.SaveChangesAsync();

            return Created(abc3insert);
        }

        // PATCH: odata/abc3insert(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<abc3insert> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            abc3insert abc3insert = await db.abc3insert.FindAsync(key);
            if (abc3insert == null)
            {
                return NotFound();
            }

            patch.Patch(abc3insert);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!abc3insertExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(abc3insert);
        }

        // DELETE: odata/abc3insert(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            abc3insert abc3insert = await db.abc3insert.FindAsync(key);
            if (abc3insert == null)
            {
                return NotFound();
            }

            db.abc3insert.Remove(abc3insert);
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

        private bool abc3insertExists(int key)
        {
            return db.abc3insert.Count(e => e.id == key) > 0;
        }
    }
}
