'use strict';

angular.module('screeningApp.teacher', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/teacher', {
    templateUrl: 'teacher/teacher.html',
    controller: 'TeacherCtrl'
  });
}])

.controller('TeacherCtrl', [function() {

}]);