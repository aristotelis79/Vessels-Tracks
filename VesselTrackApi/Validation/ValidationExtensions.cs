using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using VesselTrackApi.Models;

namespace VesselTrackApi.Validation
{
    public static class ValidationExtensions
    {

        public static IRuleBuilderOptions<T, Searchable<Between<double?>>> MustHaveLatValidValue<T,TIn>(this IRuleBuilder<T, Searchable<Between<double?>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => CheckPoint(x,Coord.MinLat,Coord.MaxLat))
                .WithMessage("latitude out of range");
        }

        public static IRuleBuilderOptions<T, Searchable<Between<double?>>> MustHaveLonValidValue<T,TIn>(this IRuleBuilder<T, Searchable<Between<double?>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => CheckPoint(x,Coord.MinLon,Coord.MaxLon))
                .WithMessage("longtitude out of range");
        }

        private static bool CheckPoint(Searchable<Between<double?>> x, double min, double max)
        {
            var from = x.Value.From >= min && x.Value.From <= max;
            var to = x.Value.To >= min && x.Value.To <= max;
            if (x.Value.From == null) return to;
            if (x.Value.To == null) return @from;
            return @from && to;
        }

        public static IRuleBuilderOptions<T, Searchable<Between<TIn>>> MustHasOneInterval<T,TIn>(this IRuleBuilder<T, Searchable<Between<TIn>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x.Value.From != null || x.Value.To != null)
                .WithMessage("Must at least one value");
        }

        public static IRuleBuilderOptions<T, Searchable<Between<DateTime?>>> MustDateTimeAfterGreaterBefore<T,TIn>(this IRuleBuilder<T, Searchable<Between<DateTime?>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x.Value.From <= x.Value.To)
                .WithMessage("Must after greater than from");
        }

        public static IRuleBuilderOptions<T, Searchable<IEnumerable<TIn>>> MustHasOne<T,TIn>(this IRuleBuilder<T, Searchable<IEnumerable<TIn>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x.Value.Any())
                .WithMessage("Must at least one value");
        }
    }
}