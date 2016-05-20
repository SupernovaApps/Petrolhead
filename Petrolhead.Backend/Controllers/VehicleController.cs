using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Petrolhead.Backend.DataObjects;
using Petrolhead.Backend.Models;

namespace Petrolhead.Backend.Controllers
{
    public class VehicleController : TableController<VehicleDTO>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            PetrolheadAppContext context = new PetrolheadAppContext();
            DomainManager = new EntityDomainManager<VehicleDTO>(context, Request);
        }

        // GET tables/Vehicle
        [QueryableExpand("Expenses")]
        [QueryableExpand("Refuels")]
        [QueryableExpand("Repairs")]
        public IQueryable<VehicleDTO> GetAllVehicleDTO()
        {
            return Query(); 
        }

        // GET tables/Vehicle/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<VehicleDTO> GetVehicleDTO(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Vehicle/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<VehicleDTO> PatchVehicleDTO(string id, Delta<VehicleDTO> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Vehicle
        public async Task<IHttpActionResult> PostVehicleDTO(VehicleDTO item)
        {
            VehicleDTO current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Vehicle/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteVehicleDTO(string id)
        {
             return DeleteAsync(id);
        }
    }
}
