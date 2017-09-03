'use strict';

angular.module('screeningApp.register', ['ngRoute'])

    .config(['$routeProvider', function($routeProvider) {
        $routeProvider.when('/register', {
            templateUrl: 'register/register.html',
            controller: 'RegisterCtrl'
        });
    }])

    .controller('RegisterCtrl', ['$scope', function($scope) {
        $scope.register = function(){
           alert($scope.role);
        }
    }]);