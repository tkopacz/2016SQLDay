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
    builder.EntitySet<abc2noncluster>("abc2noncluster");
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class abc2nonclusterController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/abc2noncluster
        [EnableQuery]
        public IQueryable<abc2noncluster> Getabc2noncluster()
        {
            return db.abc2noncluster;
        }

        // GET: odata/abc2noncluster(5)
        [EnableQuery]
        public SingleResult<abc2noncluster> Getabc2noncluster([FromODataUri] int key)
        {
            return SingleResult.Create(db.abc2noncluster.Where(abc2noncluster => abc2noncluster.id == key));
        }

        // PUT: odata/abc2noncluster(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<abc2noncluster> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            abc2noncluster abc2noncluster = await db.abc2noncluster.FindAsync(key);
            if (abc2noncluster == null)
            {
                return NotFound();
            }

            patch.Put(abc2noncluster);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!abc2nonclusterExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(abc2noncluster);
        }

        // POST: odata/abc2noncluster
        public async Task<IHttpActionResult> Post(abc2noncluster abc2noncluster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.abc2noncluster.Add(abc2noncluster);
            await db.SaveChangesAsync();

            return Created(abc2noncluster);
        }

        // PATCH: odata/abc2noncluster(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<abc2noncluster> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            abc2noncluster abc2noncluster = await db.abc2noncluster.FindAsync(key);
            if (abc2noncluster == null)
            {
                return NotFound();
            }

            patch.Patch(abc2noncluster);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!abc2nonclusterExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(abc2noncluster);
        }

        // DELETE: odata/abc2noncluster(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            abc2noncluster abc2noncluster = await db.abc2noncluster.FindAsync(key);
            if (abc2noncluster == null)
            {
                return NotFound();
            }

            db.abc2noncluster.Remove(abc2noncluster);
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

        private bool abc2nonclusterExists(int key)
        {
            return db.abc2noncluster.Count(e => e.id == key) > 0;
        }
    }
}
