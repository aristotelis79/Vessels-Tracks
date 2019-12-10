Feature: VesselTrackApi
	In order to avoid silly mistakes

@mytag
Scenario: Find rignt vessel via get search request
	Given Vessel Track Request
		| mmsi      | lon	  | lat      | timestamp					  |
		| 247039300 | 15.4415 | 42.75178 | Monday, July 1, 2013 1:06:00 PM|
	When I call the endpoint
	Then the result should 
		| mmsi      | status | stationId | speed | lon	   | lat      | course   | heading | rot | timestamp  |
		| 247039300 | 0      | 81        | 180   | 15.4415 | 42.75178 | 144      | 144     |     | 1372683960 |