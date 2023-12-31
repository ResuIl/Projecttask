﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projecttask.Models;
using System.Reflection.Emit;

namespace Projecttask.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Orders> Orders { get; set; }
    public DbSet<UserTag> UserTag { get; set; }
    public DbSet<Tag> Tag { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

		builder.Entity<Orders>()
			.HasOne(o => o.Employer)
			.WithMany()
			.HasForeignKey(o => o.EmployerId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Entity<Orders>()
			.HasOne(o => o.Worker)
			.WithMany()
			.HasForeignKey(o => o.WorkerId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
