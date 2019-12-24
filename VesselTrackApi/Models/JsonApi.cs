using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VesselTrackApi.Models
{
    public interface IJsonApi<T> where T : struct
    {
        T Id { get; set; }
    }

    [Serializable]
    public class JsonApi<T> where T : struct
    {
        public JsonApi(IJsonApi<T> entity)
        {
            Id = entity.Id;
            Type = (entity.GetType().Name).ToLower() + "s";
            Data = entity;
        }

        [DataMember(Name = "id")]
        public T Id { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "data")]
        public IJsonApi<T> Data { get; set; }
    }

    [Serializable]
    public class JsonErrorApi 
    {
        public JsonErrorApi(string error, int status)
        {
            Error = error;
            Status = status;
            TraceId = Guid.NewGuid();
        }

        public JsonErrorApi(List<string> errors, int status)
        {
            Error = errors.Aggregate((x,y) => $"{x},{y}");
            Status = status;
            TraceId = Guid.NewGuid();
        }

        [DataMember(Name = "error")]
        public string Error { get; set; }
        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "traceId")]
        public Guid TraceId { get; }

        public static ObjectResult Error500 => new ObjectResult(new JsonErrorApi("Internal server Error", StatusCodes.Status500InternalServerError))
                                                                {
                                                                    StatusCode = StatusCodes.Status500InternalServerError,
                                                                }; 

        public static ObjectResult Error404 => new NotFoundObjectResult(new JsonErrorApi("Not Found",StatusCodes.Status404NotFound))
                                                                {
                                                                    StatusCode = StatusCodes.Status404NotFound,
                                                                    DeclaredType = new TypeDelegator(typeof(NotFoundObjectResult))
                                                                };


        public static ObjectResult Error400(IList<ValidationFailure> failures)
        {
            var errors = failures.Select(s => s.ErrorMessage).ToList();

            return new BadRequestObjectResult(
                new JsonErrorApi(errors, StatusCodes.Status400BadRequest))
            {
                StatusCode = StatusCodes.Status400BadRequest,
                DeclaredType = new TypeDelegator(typeof(BadRequestObjectResult))
            };
        }
    }
}