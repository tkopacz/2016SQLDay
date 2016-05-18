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
    builder.EntitySet<Person>("People");
    builder.EntitySet<BusinessEntity>("BusinessEntities"); 
    builder.EntitySet<BusinessEntityContact>("BusinessEntityContacts"); 
    builder.EntitySet<Customer>("Customers"); 
    builder.EntitySet<EmailAddress>("EmailAddresses"); 
    builder.EntitySet<Employee1>("Employees1"); 
    builder.EntitySet<Password>("Passwords"); 
    builder.EntitySet<PersonCreditCard>("PersonCreditCards"); 
    builder.EntitySet<PersonPhone>("PersonPhones"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PeopleController : ODataController
    {
        private Model1 db = new Model1();

        // GET: odata/People
        [EnableQuery]
        public IQueryable<Person> GetPeople()
        {
            return db.People;
        }

        // GET: odata/People(5)
        [EnableQuery]
        public SingleResult<Person> GetPerson([FromODataUri] int key)
        {
            return SingleResult.Create(db.People.Where(person => person.BusinessEntityID == key));
        }

        // PUT: odata/People(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Person> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Person person = await db.People.FindAsync(key);
            if (person == null)
            {
                return NotFound();
            }

            patch.Put(person);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(person);
        }

        // POST: odata/People
        public async Task<IHttpActionResult> Post(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.People.Add(person);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonExists(person.BusinessEntityID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(person);
        }

        // PATCH: odata/People(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Person> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Person person = await db.People.FindAsync(key);
            if (person == null)
            {
                return NotFound();
            }

            patch.Patch(person);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(person);
        }

        // DELETE: odata/People(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Person person = await db.People.FindAsync(key);
            if (person == null)
            {
                return NotFound();
            }

            db.People.Remove(person);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/People(5)/BusinessEntity
        [EnableQuery]
        public SingleResult<BusinessEntity> GetBusinessEntity([FromODataUri] int key)
        {
            return SingleResult.Create(db.People.Where(m => m.BusinessEntityID == key).Select(m => m.BusinessEntity));
        }

        // GET: odata/People(5)/BusinessEntityContacts
        [EnableQuery]
        public IQueryable<BusinessEntityContact> GetBusinessEntityContacts([FromODataUri] int key)
        {
            return db.People.Where(m => m.BusinessEntityID == key).SelectMany(m => m.BusinessEntityContacts);
        }

        // GET: odata/People(5)/Customers
        [EnableQuery]
        public IQueryable<Customer> GetCustomers([FromODataUri] int key)
        {
            return db.People.Where(m => m.BusinessEntityID == key).SelectMany(m => m.Customers);
        }

        // GET: odata/People(5)/EmailAddresses
        [EnableQuery]
        public IQueryable<EmailAddress> GetEmailAddresses([FromODataUri] int key)
        {
            return db.People.Where(m => m.BusinessEntityID == key).SelectMany(m => m.EmailAddresses);
        }

        // GET: odata/People(5)/Employee
        [EnableQuery]
        public SingleResult<Employee1> GetEmployee([FromODataUri] int key)
        {
            return SingleResult.Create(db.People.Where(m => m.BusinessEntityID == key).Select(m => m.Employee));
        }

        // GET: odata/People(5)/Password
        [EnableQuery]
        public SingleResult<Password> GetPassword([FromODataUri] int key)
        {
            return SingleResult.Create(db.People.Where(m => m.BusinessEntityID == key).Select(m => m.Password));
        }

        // GET: odata/People(5)/PersonCreditCards
        [EnableQuery]
        public IQueryable<PersonCreditCard> GetPersonCreditCards([FromODataUri] int key)
        {
            return db.People.Where(m => m.BusinessEntityID == key).SelectMany(m => m.PersonCreditCards);
        }

        // GET: odata/People(5)/PersonPhones
        [EnableQuery]
        public IQueryable<PersonPhone> GetPersonPhones([FromODataUri] int key)
        {
            return db.People.Where(m => m.BusinessEntityID == key).SelectMany(m => m.PersonPhones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int key)
        {
            return db.People.Count(e => e.BusinessEntityID == key) > 0;
        }
    }
}
