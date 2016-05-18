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
    builder.EntitySet<PersonCreditCard>("PersonCreditCards");
    builder.EntitySet<CreditCard>("CreditCards"); 
    builder.EntitySet<Person>("People"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PersonCreditCardsController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/PersonCreditCards
        [EnableQuery]
        public IQueryable<PersonCreditCard> GetPersonCreditCards()
        {
            return db.PersonCreditCards;
        }

        // GET: odata/PersonCreditCards(5)
        [EnableQuery]
        public SingleResult<PersonCreditCard> GetPersonCreditCard([FromODataUri] int key)
        {
            return SingleResult.Create(db.PersonCreditCards.Where(personCreditCard => personCreditCard.BusinessEntityID == key));
        }

        // PUT: odata/PersonCreditCards(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<PersonCreditCard> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PersonCreditCard personCreditCard = await db.PersonCreditCards.FindAsync(key);
            if (personCreditCard == null)
            {
                return NotFound();
            }

            patch.Put(personCreditCard);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonCreditCardExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(personCreditCard);
        }

        // POST: odata/PersonCreditCards
        public async Task<IHttpActionResult> Post(PersonCreditCard personCreditCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PersonCreditCards.Add(personCreditCard);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonCreditCardExists(personCreditCard.BusinessEntityID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(personCreditCard);
        }

        // PATCH: odata/PersonCreditCards(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<PersonCreditCard> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PersonCreditCard personCreditCard = await db.PersonCreditCards.FindAsync(key);
            if (personCreditCard == null)
            {
                return NotFound();
            }

            patch.Patch(personCreditCard);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonCreditCardExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(personCreditCard);
        }

        // DELETE: odata/PersonCreditCards(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            PersonCreditCard personCreditCard = await db.PersonCreditCards.FindAsync(key);
            if (personCreditCard == null)
            {
                return NotFound();
            }

            db.PersonCreditCards.Remove(personCreditCard);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/PersonCreditCards(5)/CreditCard
        [EnableQuery]
        public SingleResult<CreditCard> GetCreditCard([FromODataUri] int key)
        {
            return SingleResult.Create(db.PersonCreditCards.Where(m => m.BusinessEntityID == key).Select(m => m.CreditCard));
        }

        // GET: odata/PersonCreditCards(5)/Person
        [EnableQuery]
        public SingleResult<Person> GetPerson([FromODataUri] int key)
        {
            return SingleResult.Create(db.PersonCreditCards.Where(m => m.BusinessEntityID == key).Select(m => m.Person));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonCreditCardExists(int key)
        {
            return db.PersonCreditCards.Count(e => e.BusinessEntityID == key) > 0;
        }
    }
}
