using Autofac;
using Autofac.Core;
using MongoDB.Driver;
using SimplyShare.Tracker.Models;
using SimplyShare.Tracker.Operations;
using SimplyShare.Tracker.Repository;
using SimplyShare.Tracker.Validators;

namespace SimplyShare.Tracker
{
    internal class AutofacModule : Module
    {
        private CosmosOption _cosmosOption;

        public AutofacModule(CosmosOption cosmosOption)
        {
            _cosmosOption = cosmosOption;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterType<SharingOperation>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<SharingContextRepository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ShareRequestValidator>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register(ctx =>
            {
                var mongo = new MongoClient(_cosmosOption.MongoConnectionString);
                return mongo.GetDatabase(_cosmosOption.DatabaseName);
            })
                .As<IMongoDatabase>()
                .SingleInstance();
        }
    }
}