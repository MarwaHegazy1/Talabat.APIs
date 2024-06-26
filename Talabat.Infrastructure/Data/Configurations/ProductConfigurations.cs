﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;

namespace Talabat.Infrastructure.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(P => P.Name).IsRequired().HasMaxLength(100);
			builder.Property(P => P.Description).IsRequired();
			builder.Property(P => P.PictureUrl).IsRequired();

			builder.Property(P => P.Price).HasColumnType("decimal(18, 2)");


			builder.HasOne(P => P.Brand)
			.WithMany()
			.HasForeignKey(P => P.ProductBrandId);


			builder.HasOne(P => P.Category)
			.WithMany()
			.HasForeignKey(P => P.ProductCategoryId);
		}
	}
}
