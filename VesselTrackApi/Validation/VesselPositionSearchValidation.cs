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

            RuleFor(x => x.Latitude)
                .MustHaveLatValidValue<VesselPositionSearch,Searchable<Between<decimal?>>>()
                .MustHasOneInterval()
                .When(x => x.Latitude != null);

            RuleFor(x => x.Longitude)
                .MustHaveLonValidValue<VesselPositionSearch,Searchable<Between<decimal?>>>()
                .MustHasOneInterval()
                .When(x => x.Longitude != null);

            RuleFor(x => x.Timestamp)
                .MustHasOneInterval()
                .When(x => x.Timestamp != null)
                .MustDateTimeAfterGreaterBefore<VesselPositionSearch,Searchable<Between<DateTime?>>>()
                .When(x=>x.Timestamp?.Value.From != null && x.Timestamp.Value.To != null);
        }
    }
}