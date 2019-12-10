using System;

namespace VesselTrackApi.Models
{
    [Serializable]
    public class Searchable<T>
    {
        public Searchable(T value)
        {
            Value = value;
        }
        public T Value { get; set; }
    }
}