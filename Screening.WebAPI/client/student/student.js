'use strict';

angular.module('screeningApp.student', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/student', {
    templateUrl: 'student/student.html',
    controller: 'StudentCtrl'
  });
}])

.controller('StudentCtrl', [function() {

}]);