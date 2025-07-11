﻿using AutoReels.Studio.Identity.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoReels.Studio.Identity.Data.Contexts
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity => entity.ToTable(name: "Users"));

            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));

            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));

            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));

            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));

            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));

            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));
        }
    }

    
}
