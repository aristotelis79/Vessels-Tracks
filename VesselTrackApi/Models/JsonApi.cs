using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace VesselTrackApi.Models
{
    public interface IJsonApi
    {
        long Id { get; set; }
    }

    [Serializable]
    public class JsonApi
    {
        public JsonApi(IJsonApi entity)
        {
            Id = entity.Id;
            Type = (entity.GetType().Name).ToLower() + "s";
            Data = entity;
        }

        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "data")]
        public IJsonApi Data { get; set; }
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

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "traceId")]
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