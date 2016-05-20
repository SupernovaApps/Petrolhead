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
            // Database.SetInitializer(new MobileServiceInitializer());

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

   
}

