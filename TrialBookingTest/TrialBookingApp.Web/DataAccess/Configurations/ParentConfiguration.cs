using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess.Configurations
{
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.HasKey(x => x.ParentId);

            builder.Property(x => x.ParentName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ParentEmail)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.HasMany(x => x.Students)
                .WithOne(x => x.Parent)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
