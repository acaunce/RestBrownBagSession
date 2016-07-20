var app = angular.module("RestDemo", []);

app.controller("SpeciesController", [
    "$scope", "$http", function($scope, $http) {
        $http.get("https://swapi.co/api/species", function(res) {
            $scope.Species = res.data.results;
        });
    }
]);