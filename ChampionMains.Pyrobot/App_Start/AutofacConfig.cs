﻿using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using ChampionMains.Pyrobot.Data;
using ChampionMains.Pyrobot.Services;
using ChampionMains.Pyrobot.Services.Reddit;
using ChampionMains.Pyrobot.Services.Riot;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;

namespace ChampionMains.Pyrobot
{
    public class AutofacConfig
    {
        public static void Register(ContainerBuilder builder)
        {
            var s = ConfigurationManager.AppSettings;

            // MVC controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Web API
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // DI model binders
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // Web abstractions
            builder.RegisterModule<AutofacWebTypesModule>();
            
            // Property injection in view pages
            builder.RegisterSource(new ViewRegistrationSource());

            // Property injection in action filters
            builder.RegisterFilterProvider();

            // Config

            builder.Register(context => new ApplicationConfiguration
            {
                FlairBotVersion = s["bot.version"],
                RiotUpdateMin = TimeSpan.Parse(s["website.riotUpdateMin"]),
                RiotUpdateMax = TimeSpan.Parse(s["website.riotUpdateMax"]),
                FlairUpdateMin = TimeSpan.Parse(s["website.flairUpdateMin"]),
                FlairUpdateMax = TimeSpan.Parse(s["website.flairUpdateMax"])
            }).SingleInstance();

            // Services
            builder.RegisterType(typeof(UserService)).InstancePerDependency();
            builder.RegisterType(typeof(SummonerService)).InstancePerDependency();
            builder.RegisterType(typeof(SubredditService)).InstancePerDependency();
            builder.RegisterType(typeof(FlairService)).InstancePerDependency();
            // above use db, below do not
            builder.RegisterType(typeof(RedditService)).SingleInstance();
            builder.RegisterType(typeof(ValidationService)).SingleInstance();
            builder.Register(context => new RoleService(
                    ConfigurationManager.AppSettings["security.admins"].Split(',').Select(x => x.Trim()).ToList())
            ).SingleInstance();
            builder.Register(context => new RiotService
            {
                WebRequester = new RiotWebRequester(s["riot.rateLimit"])
                {
                    ApiKey = s["riot.apiKey"],
                    MaxAttempts = int.Parse(s["riot.maxAttempts"]),
                    RetryInterval = TimeSpan.Parse(s["riot.retryInterval"])
                }
            }).SingleInstance();
            builder.Register(context => new WebJobService(
                s["AzureWebJobsStorage"],
                s["webjob.wakeup.username"],
                s["webjob.wakeup.password"],
                s["webjob.wakeup.url"],
                s["userAgent"]));
            builder.Register(context => new RedditWebRequester(
                s["reddit.script.clientId"], 
                s["reddit.script.clientSecret"],
                s["reddit.modUserName"], 
                s["reddit.modPassword"],
                s["userAgent"])).SingleInstance();

            //// Jobs
            //builder.RegisterType(typeof (SummonerUpdateJob)).InstancePerLifetimeScope();
            //builder.RegisterType(typeof (BulkFlairUpdateJob)).InstancePerLifetimeScope();
            //builder.RegisterType(typeof (FlairUpdateJob)).InstancePerLifetimeScope();
            //builder.RegisterType(typeof (BulkLeagueUpdateJob)).InstancePerLifetimeScope();
            //builder.RegisterType(typeof (ConfirmRegistrationMailJob)).InstancePerLifetimeScope();
            //builder.RegisterType(typeof (ConfirmFlairUpdatedMailJob)).InstancePerLifetimeScope();

            // Data persistance
            builder.RegisterType(typeof(UnitOfWork)).InstancePerDependency();

            Configure(builder.Build());
        }

        private static void Configure(IContainer container)
        {
            // Replace the MVC dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Replace the WebAPI dependency resolver
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Replace the Hang-fire activator
            //ConfigureHang-fire(Hang-fire.GlobalConfiguration.Configuration, container);
        }

        //private static void ConfigureHang-fire(IGlobalConfiguration config, IContainer container)
        //{
        //    config.UseAutofacActivator(container);
        //}
    }
}