using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess.Configurations
{
    public class PaymentAttemptConfiguration
    : IEntityTypeConfiguration<PaymentAttempt>
    {
        public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
        {
            builder.HasKey(x => x.PaymentAttemptId);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.Property(x => x.TransactionRef)
                .HasMaxLength(100);

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.PaymentAttempts)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.BookingId);
        }
    }
}
