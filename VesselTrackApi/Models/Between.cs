using System;

namespace VesselTrackApi.Models
{
    [Serializable]
    public class Between<T>
    {
        public T From { get; set; }
        public T To { get; set; }

        public Between(T @from, T @to)
        {
            From = @from;
            To = @to;
        }
    }
}