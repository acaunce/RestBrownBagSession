var app = angular.module("RestApp", []);

app.controller("SpeciesController", ["$scope", "$http", function($scope, $http) {
    $http.get("http://swapi.co/api/species/")
        .then(function(r) {
            $scope.Species = r.data.results;
        });

}])