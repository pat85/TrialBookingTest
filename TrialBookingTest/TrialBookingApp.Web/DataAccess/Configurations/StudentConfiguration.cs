using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.StudentId);

            builder.Property(x => x.StudentName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ParentId);
        }
    }
}
