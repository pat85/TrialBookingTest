using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess.Configurations
{
    public class BookingConfiguration
        : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.BookingId);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.Property(x => x.ConfirmationDate);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TrialClass)
                .WithMany(x => x.TrialClassBookings)
                .HasForeignKey(x => x.TrialClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // A student can only book a particular trial class once.
            builder.HasIndex(x => new
            {
                x.StudentId,
                x.TrialClassId
            })
            .IsUnique();

            builder.HasIndex(x => new
            {
                x.TrialClassId,
                x.Status
            });
        }
    }
}
