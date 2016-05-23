using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;

using Owin;
using AutoMapper;
using Petrolhead.Backend.DataObjects;
using Petrolhead.Backend.Models;

namespace Petrolhead.Backend
{
  
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            Mapper.Initialize((cfg) =>
            {
                cfg.CreateMap<Vehicle, VehicleDTO>()
                .ForMember(vehicleDTO => vehicleDTO.Expenses, map => map.MapFrom(vehicle => vehicle.Expenses))
                .ForMember(vehicleDTO => vehicleDTO.Refuels, map => map.MapFrom(vehicle => vehicle.Refuels))
                .ForMember(vehicleDTO => vehicleDTO.Repairs, map => map.MapFrom(vehicle => vehicle.Repairs));

                cfg.CreateMap<VehicleDTO, Vehicle>()
                .ForMember(vehicle => vehicle.Expenses, map => map.MapFrom(vehicleDTO => vehicleDTO.Expenses))
                .ForMember(vehicle => vehicle.Refuels, map => map.MapFrom(vehicleDTO => vehicleDTO.Refuels))
                .ForMember(vehicle => vehicle.Repairs, map => map.MapFrom(vehicleDTO => vehicleDTO.Repairs));

                cfg.CreateMap<Expense, ExpenseDTO>();
                cfg.CreateMap<ExpenseDTO, Expense>();
                cfg.CreateMap<Repair, RepairDTO>()
                .ForMember(repairDTO => repairDTO.Components, map => map.MapFrom(repair => repair.Components));
                cfg.CreateMap<RepairDTO, Repair>()
                .ForMember(repair => repair.Components, map => map.MapFrom(repairDTO => repairDTO.Components));
                cfg.CreateMap<Component, ComponentDTO>();
                cfg.CreateMap<ComponentDTO, Component>();
                cfg.CreateMap<Refuel, RefuelDTO>();
                cfg.CreateMap<RefuelDTO, Refuel>();
                cfg.CreateMap<FuelStation, FuelStationDTO>();
                cfg.CreateMap<FuelStationDTO, FuelStation>();

            });
            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new PetrolheadInitializer());

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    // This middleware is intended to be used locally for debugging. By default, HostName will
                    // only have a value when running in an App Service application.
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            app.UseWebApi(config);
        }
    }

    public sealed class PetrolheadInitializer : DropCreateDatabaseIfModelChanges<PetrolheadAppContext>
    {
        protected override void Seed(PetrolheadAppContext context)
        {
            var vehicle = new Vehicle()
            {
                Name = "Test",
                UserID = "abc",
                Total = 0,
                Description = "Lorem ipsum",
                Manufacturer = "SomeCompany",
                Model = "SomeCar",
                ModelIdentifier = "200XL",
                NextWarrantDate = DateTime.Today,
                NextRegoRenewal = DateTime.Today,
                BudgetMax = 2000,
                IsOverBudget = false,
                Id = Guid.Empty.ToString(),
                HumanTotal = "$0.00",

            };


            var expenses = new List<Expense>()
            {
                new Expense()
                {
                    Name = "Expense",
                    Cost = 0,
                    HumanCost = "$0.00",
                    Description = "Lorem ipsum",
                    TransactionDate = DateTime.Today,
                    HumanTransactionDate = DateTime.Today.ToShortDateString(),
                    Mileage = 2000,
                    Vehicle = vehicle,
                    Id = Guid.NewGuid().ToString()
                }
            };
            vehicle.Expenses = expenses;
            context.Expenses.AddRange(expenses);

            List<Refuel> refuels = new List<Refuel>()
            {
                new Refuel()
                {
                    Name = "Expense",
                    Cost = 0,
                    HumanCost = "$0.00",
                    Description = "Lorem ipsum",
                    TransactionDate = DateTime.Today,
                    HumanTransactionDate = DateTime.Today.ToShortDateString(),
                    Mileage = 2000,
                    Vehicle = vehicle,
                    Id = Guid.NewGuid().ToString(),
                    
                }
            };

            var fuelStation = new FuelStation()
            {
                Location = "Auckchurch Z",
            };
            refuels[0].Location = fuelStation;

            vehicle.Refuels = refuels;
            context.Refuels.AddRange(refuels);

            List<Repair> repairs = new List<Repair>()
            {
                new Repair()
                {
                     Name = "Expense",
                    Cost = 0,
                    HumanCost = "$0.00",
                    Description = "Lorem ipsum",
                    TransactionDate = DateTime.Today,
                    HumanTransactionDate = DateTime.Today.ToShortDateString(),
                    Mileage = 2000,
                    Vehicle = vehicle,
                    Id = Guid.NewGuid().ToString(),
                    
                }
            };

            List<Component> components = new List<Component>()
                    {
                        new Component()
                        {
                            Name = "Engine",
                            Expense = expenses[0],
                            Cost = 0,
                            Description = "Lorem ipsum",
                            DateRepaired = DateTimeOffset.Now,
                            HumanCost = "$0.00"
                        }
                    };
            context.Components.AddRange(components);
            repairs[0].Components = components;

            context.Repairs.AddRange(repairs);
            base.Seed(context);
        }
    }
   
}

