var app = angular.module("RestApp", ["ui.bootstrap"]);

app.controller("SpeciesController", ["$scope", "$http", function ($scope, $http) {
    var getFaves = function() {
        $http.get("http://localhost:50421/favourites")
                .then(function (r) {
                    var faves = r.data;

                    angular.forEach(faves, function (f) {
                        angular.forEach($scope.Species, function (s) {
                            if (s.name === f.Name) s.Favourite = true;
                        });
                    });
                });
    }
    $http.get("http://swapi.co/api/species/")
        .then(function(r) {
            $scope.Species = r.data.results;
            getFaves();
        });

    $scope.FavouriteSpecies = function(s) {
        $http.post("http://localhost:50421/favourites", { Name: s.name })
            .then(function(r) {
                getFaves();
            });
    }
}])