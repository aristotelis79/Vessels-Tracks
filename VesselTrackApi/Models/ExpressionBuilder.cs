using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Helpers;

namespace VesselTrackApi.Models
{
    public static class ExpressionBuilder
    {

        public static Expression<Func<T, bool>> SearchExpression<T>(this VesselPositionSearch vps) where T : ITimeEntity, IVesselIdentity
        {
            return BetweenDate<T>(vps.Timestamp)
                    .And(In<T>(vps.Mmsi))
                    .AndWith(BetweenLat<IGeoPoint>(vps.Latitude))
                    .AndWith(BetweenLon<IGeoPoint>(vps.Longitude));
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp1, Expression<Func<T, bool>> exp2) =>
            Expression.Lambda<Func<T, bool>>(Expression.AndAlso(exp1.Body, Expression.Invoke(exp2, exp1.Parameters)),exp1.Parameters);

        public static Expression<Func<T, bool>> AndWith<T,TR>(this Expression<Func<T, bool>> exp1, Expression<Func<TR, bool>> exp2) =>
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

            return @from.And<T>(@to);
        }

        public static Expression<Func<T, bool>> BetweenLat<T>(Searchable<Between<double?>> s) where T : IGeoPoint 
        {
            if (CheckNulls(s)) return True<T>();

            Expression<Func<T, bool>> @from = s?.Value?.From != null ? (x => x.Latitude >= s.Value.From) : (Expression<Func<T, bool>>) null;
            Expression<Func<T, bool>> @to =  s?.Value?.To != null ? (x => x.Latitude <= s.Value.To) : (Expression<Func<T, bool>>) null;
            
            if (@from == null) return @to;
            if (@to == null) return @from;

            return @from.And<T>(@to);
        }

        public static Expression<Func<T, bool>> BetweenLon<T>(Searchable<Between<double?>> s) where T : IGeoPoint
        {
            if (CheckNulls(s)) return True<T>();

            Expression<Func<T, bool>> @from = s?.Value?.From != null ? (x => x.Longitude >= s.Value.From) : (Expression<Func<T, bool>>) null;
            Expression<Func<T, bool>> @to =  s?.Value?.To != null ? (x => x.Longitude <= s.Value.To) : (Expression<Func<T, bool>>) null;
            
            if (@from == null) return @to;
            if (@to == null) return @from;

            return @from.And<T>(@to);
        }

        public static Expression<Func<T, bool>> In<T>(Searchable<IEnumerable<long>> s) where T : IVesselIdentity
        {
            return s?.Value != null && s.Value.Any()
                ?   x => s.Value.Contains(x.Mmsi)
                : True<T>();
        }

        public static Expression<Func<T, bool>> True<T>() =>  x => true;

        private static bool CheckNulls<T>(Searchable<Between<T?>> s) where T : struct => s?.Value?.From == null && s?.Value?.To == null;
    }
}