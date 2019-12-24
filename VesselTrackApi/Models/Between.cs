using System;

namespace VesselTrackApi.Models
{
    [Serializable]
    public class Between<T>
    {
        public T From { get; set; }
        public T To { get; set; }

        private Between(T @from, T @to)
        {
            From = @from;
            To = @to;
        }

        public static Between<T> Create(T @from, T @to)
        {
            return (@from == null && @to == null) 
                ? null 
                : new Between<T>(@from ,@to);
        }
    }
}