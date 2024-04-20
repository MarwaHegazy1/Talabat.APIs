﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Infrastructure
{
	internal class SpecificaationsEvaluator<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
		{
			var query = inputQuery;

			if (spec.Criteria is not null) // P => P.Id == 1
				query = query.Where(spec.Criteria);

			if(spec.OrderBy is not null)
				query = query.OrderBy(spec.OrderBy);

			else if(spec.OrderByDesc is not null)
				query=query.OrderByDescending(spec.OrderByDesc);

			// query = _dbcontext.Set<Product>().Where(P =>P.Id == 1)
			// Includes
			// 1. P => P.Brand
			// 2. P => P.Category

			query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

			// _dbcontext.Set<Product>().Where(P =>P.Id == 1).Include(P => P.Brand)
			// _dbcontext.Set<Product>().Where(P =>P.Id == 1).Include(P => P.Brand).Include(P => P.Category)

			return query;
		}
	}
}
