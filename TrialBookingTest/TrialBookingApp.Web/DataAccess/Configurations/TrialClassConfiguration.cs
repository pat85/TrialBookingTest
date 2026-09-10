using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess.Configurations
{
    public class TrialClassConfiguration
        : IEntityTypeConfiguration<TrialClass>
    {
        public void Configure(EntityTypeBuilder<TrialClass> builder)
        {
            builder.HasKey(x => x.TrialClassId);

            builder.Property(x => x.TrialClassTitle)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.TrialClassStartDate)
                .IsRequired();

            builder.Property(x => x.TrialClassEndDate)
                .IsRequired();

            builder.Property(x => x.TrialClassCapacity)
                .IsRequired();

            builder.Property(x => x.ConfirmedBookingCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.HasMany(x => x.TrialClassBookings)
                .WithOne(x => x.TrialClass)
                .HasForeignKey(x => x.TrialClassId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TrialClassStartDate);
        }
    }
}
