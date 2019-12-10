using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;
using TechTalk.SpecFlow;
using VesselTrackApi.Controllers;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Models;
using VesselTrackApi.Repositories;
using VesselTrackApi.Services;

namespace VesselTrackApiTests
{
    [Binding]
    public class VesselTrackApiSteps
    {
        private ILogger<VesselController> _logger;
        private IDbContext _dbContext;
        private IRepository<VesselPositionEntity, long> _repository;
        private ITrackService _trackService;
        private VesselController _vesselController;

        private ScenarioContext _scenarioContext;

        public VesselTrackApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _logger = new Logger<VesselController>(new NLogLoggerFactory());
            _dbContext = new VesselsTrackDbContext("Data Source=(local);Initial Catalog=VesselTrackApi;Integrated Security=True;Persist Security Info=False");
            _repository = new EfRepository<VesselPositionEntity, long>(_dbContext);
            _trackService =  new TrackService(_repository);
            _vesselController = new VesselController(_logger, _trackService);
        }


        [Given(@"Vessel Track Request")]
        public void GivenVesselTrackRequest(Table table)
        {
            var requestFields = table.Rows.FirstOrDefault();
            _scenarioContext["mmsi"] = Convert.ToInt64(requestFields["mmsi"]);
            _scenarioContext["lat"] = Convert.ToDecimal(requestFields["lat"]);
            _scenarioContext["lon"] = Convert.ToDecimal(requestFields["lon"]);
            _scenarioContext["timestamp"] = Convert.ToDateTime(requestFields["timestamp"]);
        }

        [When(@"I call the endpoint")]
        public void WhenICallTheEndpoint()
        { 
            var mmsi = new long[] {(long) _scenarioContext["mmsi"]};
            var minlat = (decimal?) _scenarioContext["lat"];
            var maxLat = (decimal?) _scenarioContext["lat"];
            var minLon = (decimal?) _scenarioContext["lon"];
            var maxLon = (decimal?) _scenarioContext["lon"];
            var from = (DateTime?) _scenarioContext["timestamp"];
            var to = (DateTime?) _scenarioContext["timestamp"];
            _scenarioContext["results"] = _vesselController.Search(mmsi,minlat,maxLat,minLon,maxLon,from,to).Result;
        }

        [Then(@"the result should")]
        public void ThenTheResultShould(Table table)
        {
            var exceptResult = table.Rows.FirstOrDefault();
            var result = ((List<VesselPosition>)(_scenarioContext["results"] as OkObjectResult).Value).First();
            
            Assert.That(Convert.ToInt64(exceptResult["mmsi"]), Is.EqualTo(result.Mmsi)); 
            Assert.That(Convert.ToInt32(exceptResult["status"]), Is.EqualTo(result.Status)); 
            Assert.That(Convert.ToInt32(exceptResult["stationId"]), Is.EqualTo(result.StationId)); 
            Assert.That(Convert.ToInt32(exceptResult["speed"]), Is.EqualTo(result.Speed)); 
            Assert.That(Convert.ToDecimal(exceptResult["lon"]), Is.EqualTo(result.Lon)); 
            Assert.That(Convert.ToDecimal(exceptResult["lat"]), Is.EqualTo(result.Lat)); 
            Assert.That(Convert.ToInt32(exceptResult["course"]), Is.EqualTo(result.Course)); 
            Assert.That(Convert.ToInt32(exceptResult["heading"]), Is.EqualTo(result.Heading)); 
            Assert.That(result.Rot, Is.Null); 
            Assert.That(Convert.ToInt64(exceptResult["timestamp"]), Is.EqualTo(result.Timestamp)); 
        }
    }
}
