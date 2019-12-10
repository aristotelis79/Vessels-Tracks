using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Helpers;

namespace VesselTrackApi.Models
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> True<T>() =>  x => true;

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp1, Expression<Func<T, bool>> exp2) =>
            Expression.Lambda<Func<T, bool>>(Expression.AndAlso(exp1.Body, Expression.Invoke(exp2, exp1.Parameters)),exp1.Parameters);

        public static Expression<Func<T, bool>> BetweenDate<T>(Searchable<Between<DateTime?>> s) where T : ITimeEntity
        {
            if (CheckNulls(s)) return True<T>();

            var epochFrom = s?.Value?.From.ToEpoch();
            var epochTo = s?.Value?.To.ToEpoch();
            
            var @from = epochFrom != null ? (x => x.Timestamp >= epochFrom) : (Expression<Func<T, bool>>) null;
            var @to = epochTo != null ? (x => x.Timestamp <= epochTo) : (Expression<Func<T, bool>>) null;

            if (@from == null) return to;
            if (@to == null) return @from;

            return @from.And(@to);
        }

        public static Expression<Func<T, bool>> BetweenLat<T>(Searchable<Between<decimal?>> s) where T : IPosition 
        {
            if (CheckNulls(s)) return True<T>();

            Expression<Func<T, bool>> @from = s?.Value?.From != null ? (x => x.Lat >= s.Value.From) : (Expression<Func<T, bool>>) null;
            Expression<Func<T, bool>> @to =  s?.Value?.To != null ? (x => x.Lat <= s.Value.To) : (Expression<Func<T, bool>>) null;
            
            if (@from == null) return @to;
            if (@to == null) return @from;

            return @from.And(@to);
        }

        public static Expression<Func<T, bool>> BetweenLon<T>(Searchable<Between<decimal?>> s) where T : IPosition
        {
            if (CheckNulls(s)) return True<T>();

            Expression<Func<T, bool>> @from = s?.Value?.From != null ? (x => x.Lon >= s.Value.From) : (Expression<Func<T, bool>>) null;
            Expression<Func<T, bool>> @to =  s?.Value?.To != null ? (x => x.Lon <= s.Value.To) : (Expression<Func<T, bool>>) null;
            
            if (@from == null) return @to;
            if (@to == null) return @from;

            return @from.And(@to);
        }

        public static Expression<Func<T, bool>> In<T>(Searchable<IList<long>> s) where T : IVesselIdentity
        {
            return s?.Value != null && s.Value.Any()
                ?   x => s.Value.Contains(x.Mmsi)
                : True<T>();
        }

        private static bool CheckNulls<T>(Searchable<Between<T?>> s) where T : struct => s?.Value?.From == null && s?.Value?.To == null;
    }
}