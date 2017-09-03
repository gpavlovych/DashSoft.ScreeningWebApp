'use strict';

angular.module('screeningApp.landing', ['ngRoute'])

    .config(['$routeProvider', function($routeProvider) {
        $routeProvider.when('/landing', {
            templateUrl: 'landing/landing.html',
            controller: 'LandingCtrl'
        });
    }])

    .controller('LandingCtrl', ['$scope', function($scope) {
        $scope.login = function() {

        };
    }]);