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
        public async Task<VehicleDTO> PatchVehicleDTO(string id, Delta<VehicleDTO> patch)
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
                        var obj = expenseDTO;
                        Expense existingItem = this.context.Expenses.FirstOrDefault(v => (v.Id == expenseDTO.Id));

                        if (expenseDTO.CreatedAt == null)
                            obj.CreatedAt = existingItem?.CreatedAt ?? DateTimeOffset.Now;

                        existingItem = Mapper.Map<ExpenseDTO, Expense>(obj);
                        existingItem.Vehicle = currentVehicle;
                        currentVehicle.Expenses.Add(existingItem);
                    }
                }

                if (patch.GetChangedPropertyNames().Contains("Refuels"))
                {
                    for (int i = 0; i < currentVehicle.Refuels.Count && updatedEntity.Refuels != null; i++)
                    {
                        RefuelDTO refuelDTO = updatedEntity.Refuels.FirstOrDefault(x => (x.Id == currentVehicle.Refuels.ElementAt(i).Id));

                        if (refuelDTO != null)
                        {
                            this.context.Refuels.Remove(currentVehicle.Refuels.ElementAt(i));
                        }

                        Mapper.Map<VehicleDTO, Vehicle>(updatedEntity, currentVehicle);
                        updatedRefuels = updatedEntity.Refuels;

                    }
                }
                else
                {
                    VehicleDTO vehicleDTOUpdated = Mapper.Map<Vehicle, VehicleDTO>(currentVehicle);
                    patch.Patch(vehicleDTOUpdated);
                    Mapper.Map<VehicleDTO, Vehicle>(vehicleDTOUpdated, currentVehicle);
                    updatedRefuels = updatedEntity.Refuels;

                }

                if (updatedRefuels != null)
                {
                    currentVehicle.Refuels = new List<Refuel>();

                    foreach (var refuelDTO in updatedEntity.Refuels)
                    {
                        RefuelDTO obj = refuelDTO;
                        Refuel existingItem = context.Refuels.FirstOrDefault(r => r.Id == refuelDTO.Id);

                        
                        if (refuelDTO.CreatedAt == null)
                            obj.CreatedAt = existingItem?.CreatedAt ?? DateTimeOffset.Now;

                        existingItem = Mapper.Map<RefuelDTO, Refuel>(obj);
                        existingItem.Location = Mapper.Map<FuelStationDTO, FuelStation>(obj.Location);
                        existingItem.Vehicle = currentVehicle;
                        currentVehicle.Refuels.Add(existingItem);

                    }
                }

                
                if (patch.GetChangedPropertyNames().Contains("Repairs"))
                {
                    for (int i = 0; i < currentVehicle.Repairs.Count && updatedEntity.Repairs != null; i++)
                    {
                        RepairDTO repairDTO = updatedEntity.Repairs.FirstOrDefault(rp => rp.Id == currentVehicle.Repairs.ElementAt(i).Id);

                        if (repairDTO != null)
                        {
                            this.context.Repairs.Remove(currentVehicle.Repairs.ElementAt(i));
                        }
                        Mapper.Map<VehicleDTO, Vehicle>(updatedEntity, currentVehicle);
                        updatedRepairs = updatedEntity.Repairs;
                    }
                }
                else
                {
                    VehicleDTO vehicleDTOUpdated = Mapper.Map<Vehicle, VehicleDTO>(currentVehicle);
                    patch.Patch(vehicleDTOUpdated);
                    Mapper.Map<VehicleDTO, Vehicle>(vehicleDTOUpdated, currentVehicle);
                    updatedRepairs = updatedEntity.Repairs;
                }

                if (updatedRepairs != null)
                {
                    currentVehicle.Repairs = new List<Repair>();

                    var components = new List<Component>();
                    foreach (var repairDTO in updatedEntity.Repairs)
                    {
                        var obj = repairDTO;
                        Repair existingItem = context.Repairs.FirstOrDefault(rp => rp.Id == repairDTO.Id);
                        if (obj.CreatedAt == null)
                            obj.CreatedAt = existingItem?.CreatedAt ?? DateTimeOffset.Now;
                        existingItem = Mapper.Map<RepairDTO, Repair>(obj);
                        existingItem.Vehicle = currentVehicle;
                        currentVehicle.Repairs.Add(existingItem);

                    
                        

                        
                    }
                }
            }
            await context.SaveChangesAsync();

            var result = Mapper.Map<Vehicle, VehicleDTO>(currentVehicle);
            return result;
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
