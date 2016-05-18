using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using _2016ODataInMem.Models;
using Microsoft.OData.Core;
using System.Diagnostics;

namespace _2016ODataInMem.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using _2016ODataInMem.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<TKModelEntity>("TKModelEntities");
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TKModelEntitiesController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        static List<TKModelEntity> m_data = (new TKRepository()).Get();
        // GET: odata/TKModelEntities

        /*
         * Zapytania
         * http://localhost:27435/odata/TKModelEntities?$filter=Surname eq 'Surname8'
         * http://localhost:27435/odata/TKModelEntities?$filter=Surname eq 'Surname8'&$orderby=Dt desc&$skip=5&$top=3
         * http://localhost:27435/odata/TKModelEntities?$filter=startswith(Name,'Name10')&$count=true
         */
        public async Task<IHttpActionResult> GetTKModelEntities(ODataQueryOptions<TKModelEntity> queryOptions)
        {
            //TK: Write query 
            Debug.WriteLine($"---------------------------------------------------------");
            Debug.WriteLine($"Filter: {queryOptions?.Filter?.RawValue}");
            Debug.WriteLine($"OrderBy: {queryOptions?.OrderBy?.RawValue}");
            Debug.WriteLine($"SelectExpand.RawExpand: {queryOptions?.SelectExpand?.RawExpand}");
            Debug.WriteLine($"SelectExpand.RawSelect: {queryOptions?.SelectExpand?.RawSelect}");
            Debug.WriteLine($"OrderBy: {queryOptions?.OrderBy?.RawValue}");
            Debug.WriteLine($"Skip: {queryOptions?.Skip?.RawValue}");
            Debug.WriteLine($"Top: {queryOptions?.Top?.RawValue}");
            Debug.WriteLine($"RawValues: {queryOptions?.RawValues}");

            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
                IEnumerable<TKModelEntity> result = await applyFilter(queryOptions);
                return Ok<IEnumerable<TKModelEntity>>(result);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static async Task<IQueryable<TKModelEntity>> applyFilter(ODataQueryOptions<TKModelEntity> queryOptions)
        {
            IQueryable<TKModelEntity> result = null;
            queryOptions.Validate(_validationSettings);
            await Task.Run(
                () =>
                {
                    result = queryOptions.ApplyTo(m_data.AsParallel().AsQueryable()) as IQueryable<TKModelEntity>;
                    //result = result.Where(userid == 'aaaa')
                }
                ).ConfigureAwait(false);
            return result;
        }
        // GET: odata/TKModelEntities(5)
        public async Task<IHttpActionResult> GetTKModelEntity([FromODataUri] int key, ODataQueryOptions<TKModelEntity> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            // return Ok<TKModelEntity>(tKModelEntity);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/TKModelEntities(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<TKModelEntity> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(tKModelEntity);

            // TODO: Save the patched entity.

            // return Updated(tKModelEntity);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/TKModelEntities
        public async Task<IHttpActionResult> Post(TKModelEntity tKModelEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(tKModelEntity);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/TKModelEntities(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<TKModelEntity> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(tKModelEntity);

            // TODO: Save the patched entity.

            // return Updated(tKModelEntity);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/TKModelEntities(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
