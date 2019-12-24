using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using Nest;
using Swashbuckle.AspNetCore.SwaggerGen;

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

        public static Searchable<T> Create(T value)
        {
            return value == null || IsEmptyIEnumerable(value)
                ? null 
                : new Searchable<T>(value);
        }


        private static bool IsEmptyIEnumerable(T value)
        {
            if (!typeof(T).IsEnumerable(out var typeofValue)) return false;
            return !((IEnumerable) value).Any();
        }
    }
}