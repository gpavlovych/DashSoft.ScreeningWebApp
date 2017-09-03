'use strict';

angular.module('screeningApp.parent', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/parent', {
    templateUrl: 'parent/parent.html',
    controller: 'ParentCtrl'
  });
}])

.controller('ParentCtrl', [function() {

}]);