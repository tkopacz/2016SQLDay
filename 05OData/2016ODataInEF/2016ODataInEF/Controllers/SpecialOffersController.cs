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
    builder.EntitySet<SpecialOffer>("SpecialOffers");
    builder.EntitySet<SpecialOfferProduct>("SpecialOfferProducts"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SpecialOffersController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/SpecialOffers
        [EnableQuery]
        public IQueryable<SpecialOffer> GetSpecialOffers()
        {
            return db.SpecialOffers;
        }

        // GET: odata/SpecialOffers(5)
        [EnableQuery]
        public SingleResult<SpecialOffer> GetSpecialOffer([FromODataUri] int key)
        {
            return SingleResult.Create(db.SpecialOffers.Where(specialOffer => specialOffer.SpecialOfferID == key));
        }

        // PUT: odata/SpecialOffers(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<SpecialOffer> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SpecialOffer specialOffer = await db.SpecialOffers.FindAsync(key);
            if (specialOffer == null)
            {
                return NotFound();
            }

            patch.Put(specialOffer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialOfferExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(specialOffer);
        }

        // POST: odata/SpecialOffers
        public async Task<IHttpActionResult> Post(SpecialOffer specialOffer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SpecialOffers.Add(specialOffer);
            await db.SaveChangesAsync();

            return Created(specialOffer);
        }

        // PATCH: odata/SpecialOffers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<SpecialOffer> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SpecialOffer specialOffer = await db.SpecialOffers.FindAsync(key);
            if (specialOffer == null)
            {
                return NotFound();
            }

            patch.Patch(specialOffer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialOfferExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(specialOffer);
        }

        // DELETE: odata/SpecialOffers(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            SpecialOffer specialOffer = await db.SpecialOffers.FindAsync(key);
            if (specialOffer == null)
            {
                return NotFound();
            }

            db.SpecialOffers.Remove(specialOffer);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/SpecialOffers(5)/SpecialOfferProducts
        [EnableQuery]
        public IQueryable<SpecialOfferProduct> GetSpecialOfferProducts([FromODataUri] int key)
        {
            return db.SpecialOffers.Where(m => m.SpecialOfferID == key).SelectMany(m => m.SpecialOfferProducts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpecialOfferExists(int key)
        {
            return db.SpecialOffers.Count(e => e.SpecialOfferID == key) > 0;
        }
    }
}
