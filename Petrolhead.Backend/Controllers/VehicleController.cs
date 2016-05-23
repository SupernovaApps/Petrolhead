using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Petrolhead.Backend.DataObjects;
using Petrolhead.Backend.Models;
using System.Security.Claims;
using AutoMapper;
using System.Collections.Generic;
using System;

namespace Petrolhead.Backend.Controllers
{
    [Authorize]
    public class VehicleController : TableController<VehicleDTO>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new PetrolheadAppContext();
            DomainManager = new SimpleMappedEntityDomainManager<VehicleDTO, Vehicle>(context, Request);
        }
        PetrolheadAppContext context;

        // GET tables/Vehicle
        [QueryableExpand("Expenses")]
        [QueryableExpand("Refuels")]
        [QueryableExpand("Repairs")]
        public IQueryable<VehicleDTO> GetAllVehicleDTO()
        {
            // Get the SID of the current user.
            var claimsPrincipal = this.User as ClaimsPrincipal;
            string sid = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            // filter vehicle list by SID
            var vehicles = Query().Where(v => v.UserID == sid);
            return vehicles;
        }

        // GET tables/Vehicle/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<VehicleDTO> GetVehicleDTO(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Vehicle/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<VehicleDTO> PatchVehicleDTO(string id, Delta<VehicleDTO> patch)
        {
            Vehicle currentVehicle = context.Vehicles.Include("Expenses")
                .Include("Repairs")
                .Include("Refuels")
                .First(v => v.Id == id);

            VehicleDTO updatedEntity = patch.GetEntity();

            ICollection<ExpenseDTO> updatedExpenses = null;
            ICollection<RepairDTO> updatedRepairs = null;
            ICollection<RefuelDTO> updatedRefuels = null;

            bool requestContainsRelatedEntities = (patch.GetChangedPropertyNames().Contains("Expenses") || patch.GetChangedPropertyNames().Contains("Refuels") || patch.GetChangedPropertyNames().Contains("Repairs"));

            if (requestContainsRelatedEntities)
            {
                if (patch.GetChangedPropertyNames().Contains("Expenses"))
                {
                    for (int i = 0; i < currentVehicle.Expenses.Count && updatedEntity.Expenses != null; i++)
                    {
                        ExpenseDTO expenseDTO = updatedEntity.Expenses.FirstOrDefault(v => (v.Id == currentVehicle.Expenses.ElementAt(i).Id));

                        if (expenseDTO != null)
                        {
                            this.context.Expenses.Remove(currentVehicle.Expenses.ElementAt(i));
                        }

                    }
                    Mapper.Map<VehicleDTO, Vehicle>(updatedEntity, currentVehicle);
                    updatedExpenses = updatedEntity.Expenses;
                }
                else
                {
                    VehicleDTO vehicleDTOUpdated = Mapper.Map<Vehicle, VehicleDTO>(currentVehicle);
                    patch.Patch(vehicleDTOUpdated);
                    Mapper.Map<VehicleDTO, Vehicle>(vehicleDTOUpdated, currentVehicle);
                    updatedExpenses = vehicleDTOUpdated.Expenses;
                }

                if (updatedExpenses != null)
                {
                    currentVehicle.Expenses = new List<Expense>();

                    foreach (var expenseDTO in updatedExpenses)
                    {
                        Expense existingItem = this.context.Expenses.FirstOrDefault(v => (v.Id == expenseDTO.Id));

                        if (expenseDTO.CreatedAt == null)
                            expenseDTO.CreatedAt = existingItem.CreatedAt ?? DateTimeOffset.Now;
                         
                        existingItem = Mapper.Map<ExpenseDTO, Expense>(expenseDTO);
                        existingItem.Vehicle = currentVehicle;
                        currentVehicle.Expenses.Add(existingItem);
                    }
                }
            }

            return UpdateAsync(id, patch);
        }

        // POST tables/Vehicle
        public async Task<IHttpActionResult> PostVehicleDTO(VehicleDTO item)
        {
            var claimsPrincipal = this.User as ClaimsPrincipal;
            string sid = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            item.UserID = sid;

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
