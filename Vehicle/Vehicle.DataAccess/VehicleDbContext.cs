using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model = Vehicle.DomainModel;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Vehicle.DataAccess
{
    public class VehicleDbContext : IdentityDbContext<Model.VehicleUser>
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options)
        {
        }
        public DbSet<Model.Vehicle> Vehicle { get; set; }
        public DbSet<Model.TypeOfVehicle> TypeOfVehicle { get; set; }
        public DbSet<Model.VehicleModel> VehicleModel { get; set; }
        public DbSet<Model.VehicleBrand> VehicleBrand { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Model.Vehicle>().HasKey(m => m.VehicleID);
            builder.Entity<Model.TypeOfVehicle>().HasKey(m => m.TypeOfVehicleID);
            builder.Entity<Model.VehicleModel>().HasKey(m => m.VehicleModelID);
            builder.Entity<Model.VehicleBrand>().HasKey(m => m.VehicleBrandID);

            // shadow properties
            //builder.Entity<Model.Vehicle>().Property<DateTime>("CreationDate");
            //builder.Entity<Model.Vehicle>().Property<DateTime>("ModifiedDate");
            
            base.OnModelCreating(builder);

        }
        public override int SaveChanges()
        {
            //Change Tracker
            ChangeTracker.DetectChanges();
            UpdatedProperty<DomainModel.Vehicle>();

            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            //Change Tracker
            ChangeTracker.DetectChanges();
            UpdatedProperty<DomainModel.Vehicle>();
            UpdatedProperty<DomainModel.VehicleBrand>();
            UpdatedProperty<DomainModel.VehicleModel>();
            UpdatedProperty<DomainModel.TypeOfVehicle>();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdatedProperty<T>() where T : class
        {
            //Add New Soruce Dedect
            var addedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added);

            foreach (var entry in addedSourceInfo)
            {
                if (entry.Property("CreationDate") != null)
                {
                    entry.Property("CreationDate").CurrentValue = DateTime.UtcNow;
                }
                if (entry.Property("ModifiedDate") != null)
                {
                    entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
                }
            }

            //Modified Soruce Dedect
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                if (entry.Property("ModifiedDate") != null)
                {
                    entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
