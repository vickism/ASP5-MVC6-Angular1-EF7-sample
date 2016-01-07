(function() {
	"use strict";
	angular.module("app-trips")
		.controller("tripsController", tripsController);

	function tripsController($http) {
		var vm = this;
		vm.trips = [];
		vm.newTrip = {};
		vm.errorMessage = "";
		vm.addTrip = function() {
			vm.isBusy = true;
			vm.errorMessage = "";
			$http.post("/api/trip", vm.newTrip).then(
				function(response) {
					vm.trips.push(response.data);
					vm.newTrip = {};
				},function() {
					vm.errorMessage = "Failed to Save trip";
				}).finally(function() {
				vm.isBusy = false;
			});
		};
		vm.isBusy = true;

		$http.get("/api/trip")
			.then(function(response) {
				angular.copy(response.data, vm.trips);
				
			}, function(error) {
				vm.errorMessage = "Failed to load data " + error;
			})
		.finally(function() {
			vm.isBusy = false;
		});
	}
})();