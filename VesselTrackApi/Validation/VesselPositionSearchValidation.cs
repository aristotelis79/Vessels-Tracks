using System;
using FluentValidation;
using VesselTrackApi.Models;

namespace VesselTrackApi.Validation
{
    public class VesselPositionSearchValidation : AbstractValidator<VesselPositionSearch>
    {
        public VesselPositionSearchValidation()
        {
            RuleFor(x => x.Mmsi)
                .MustHasOne()
                .When(x => x.Mmsi != null);

            RuleFor(x => x.Lat)
                .MustHaveLatValidValue<VesselPositionSearch,Searchable<Between<decimal?>>>()
                .MustHasOneInterval()
                .When(x => x.Lat != null);

            RuleFor(x => x.Lon)
                .MustHaveLonValidValue<VesselPositionSearch,Searchable<Between<decimal?>>>()
                .MustHasOneInterval()
                .When(x => x.Lon != null);

            RuleFor(x => x.Timestamp)
                .MustHasOneInterval()
                .When(x => x.Timestamp != null);
        }

    }
}