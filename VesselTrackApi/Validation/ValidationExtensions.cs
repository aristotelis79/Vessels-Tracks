using System.Collections.Generic;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using VesselTrackApi.Models;

namespace VesselTrackApi.Validation
{
    public static class ValidationExtensions
    {

        public static IRuleBuilderOptions<T, Searchable<Between<decimal?>>> MustHaveLatValidValue<T,TIn>(this IRuleBuilder<T, Searchable<Between<decimal?>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x.Value.From >= -90 && x.Value.From <= 90 
                           && x.Value.To >= -90 && x.Value.To <= 90)
                .WithMessage("latitude out of range");
        }

        public static IRuleBuilderOptions<T, Searchable<Between<decimal?>>> MustHaveLonValidValue<T,TIn>(this IRuleBuilder<T, Searchable<Between<decimal?>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x.Value.From >= -180 && x.Value.From <= 180 
                           && x.Value.To >= -180 && x.Value.To <= 180)
                .WithMessage("longtitude out of range");
        }

        public static IRuleBuilderOptions<T, Searchable<Between<TIn>>> MustHasOneInterval<T,TIn>(this IRuleBuilder<T, Searchable<Between<TIn>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x.Value.From != null || x.Value.To != null)
                .WithMessage("Must at least one value");
        }

        public static IRuleBuilderOptions<T, Searchable<IList<TIn>>> MustHasOne<T,TIn>(this IRuleBuilder<T, Searchable<IList<TIn>>> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x.Value.Any())
                .WithMessage("Must at least one value");
        }
    }
}