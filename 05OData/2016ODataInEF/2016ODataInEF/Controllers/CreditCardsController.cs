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
    builder.EntitySet<CreditCard>("CreditCards");
    builder.EntitySet<PersonCreditCard>("PersonCreditCards"); 
    builder.EntitySet<SalesOrderHeader>("SalesOrderHeaders"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CreditCardsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/CreditCards
        [EnableQuery]
        public IQueryable<CreditCard> GetCreditCards()
        {
            return db.CreditCards;
        }

        // GET: odata/CreditCards(5)
        [EnableQuery]
        public SingleResult<CreditCard> GetCreditCard([FromODataUri] int key)
        {
            return SingleResult.Create(db.CreditCards.Where(creditCard => creditCard.CreditCardID == key));
        }

        // PUT: odata/CreditCards(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<CreditCard> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreditCard creditCard = await db.CreditCards.FindAsync(key);
            if (creditCard == null)
            {
                return NotFound();
            }

            patch.Put(creditCard);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreditCardExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(creditCard);
        }

        // POST: odata/CreditCards
        public async Task<IHttpActionResult> Post(CreditCard creditCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CreditCards.Add(creditCard);
            await db.SaveChangesAsync();

            return Created(creditCard);
        }

        // PATCH: odata/CreditCards(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<CreditCard> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreditCard creditCard = await db.CreditCards.FindAsync(key);
            if (creditCard == null)
            {
                return NotFound();
            }

            patch.Patch(creditCard);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreditCardExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(creditCard);
        }

        // DELETE: odata/CreditCards(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            CreditCard creditCard = await db.CreditCards.FindAsync(key);
            if (creditCard == null)
            {
                return NotFound();
            }

            db.CreditCards.Remove(creditCard);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CreditCards(5)/PersonCreditCards
        [EnableQuery]
        public IQueryable<PersonCreditCard> GetPersonCreditCards([FromODataUri] int key)
        {
            return db.CreditCards.Where(m => m.CreditCardID == key).SelectMany(m => m.PersonCreditCards);
        }

        // GET: odata/CreditCards(5)/SalesOrderHeaders
        [EnableQuery]
        public IQueryable<SalesOrderHeader> GetSalesOrderHeaders([FromODataUri] int key)
        {
            return db.CreditCards.Where(m => m.CreditCardID == key).SelectMany(m => m.SalesOrderHeaders);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CreditCardExists(int key)
        {
            return db.CreditCards.Count(e => e.CreditCardID == key) > 0;
        }
    }
}
