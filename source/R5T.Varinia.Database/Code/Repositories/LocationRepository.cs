﻿using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using R5T.Corcyra;
using R5T.Venetia;


namespace R5T.Varinia.Database
{
    public class LocationRepository<TDbContext> : ProvidedDatabaseRepositoryBase<TDbContext>, ILocationRepository
        where TDbContext: DbContext, ILocationDbContext
    {
        public LocationRepository(DbContextOptions<TDbContext> dbContextOptions, IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextOptions, dbContextProvider)
        {
        }

        public void Delete(LocationIdentity identity)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.Locations.Where(x => x.GUID == identity.Value).Single();

                dbContext.Locations.Remove(entity);

                dbContext.SaveChanges();
            });
        }

        public bool Exists(LocationIdentity identity)
        {
            var exists = this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.Locations.Where(x => x.GUID == identity.Value).SingleOrDefault();

                var output = entity is object;
                return output;
            });

            return exists;
        }

        public LngLat GetLngLat(LocationIdentity identity)
        {
            var lngLat = this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.Locations.Where(x => x.GUID == identity.Value).Single();

                var output = new LngLat()
                {
                    Lng = entity.Longitude.Value,
                    Lat = entity.Latitude.Value,
                };
                return output;
            });

            return lngLat;
        }

        public LocationIdentity New()
        {
            var locationIdentity = LocationIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var entity = new Entities.Location()
                {
                    GUID = locationIdentity.Value,
                };

                dbContext.Locations.Add(entity);

                dbContext.SaveChanges();
            });

            return locationIdentity;
        }

        public LocationIdentity New(LngLat lngLat)
        {
            var locationIdentity = LocationIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var entity = new Entities.Location()
                {
                    GUID = locationIdentity.Value,
                    Longitude = lngLat.Lng,
                    Latitude = lngLat.Lat,
                };

                dbContext.Locations.Add(entity);

                dbContext.SaveChanges();
            });

            return locationIdentity;
        }

        public void SetLngLat(LocationIdentity identity, LngLat lngLat)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.Locations.Where(x => x.GUID == identity.Value).Single();

                entity.Longitude = lngLat.Lng;
                entity.Latitude = lngLat.Lat;

                dbContext.SaveChanges();
            });
        }
    }
}
