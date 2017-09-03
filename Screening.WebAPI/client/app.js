'use strict';

// Declare app level module which depends on views, and components
angular.module('screeningApp', [
    'ngRoute',
    'screeningApp.student',
    'screeningApp.teacher',
    'screeningApp.parent',
    'screeningApp.login',
    'screeningApp.register',
    'screeningApp.landing',
    'screeningApp.version'
]).
config(['$locationProvider', '$routeProvider', function($locationProvider, $routeProvider) {
  $locationProvider.hashPrefix('!');

  $routeProvider.otherwise({redirectTo: '/student'});
}]).
factory('authProvider', function() {
    var user;
    return {
        setUser : function(aUser){
            user = aUser;
        },
        hasRight : function(path){
            if (!user){
                return path.startsWith('/login') || path.startsWith('/landing') || path.startsWith('/register');
            }
            else {
                if (user.roleName === 'student') {
                    return path.startsWith('/login') || path.startsWith('/landing') || path.startsWith('/register') || path.startsWith('/student')
                }
                if (user.roleName === 'teacher') {
                    return path.startsWith('/login') || path.startsWith('/landing') || path.startsWith('/register') || path.startsWith('/teacher')
                }
                if (user.roleName === 'parent') {
                    return path.startsWith('/login') || path.startsWith('/landing') || path.startsWith('/register') || path.startsWith('/parent')
                }
            }
        }
    };
}).
run(['$rootScope', '$location', 'authProvider', function ($rootScope, $location, authProvider) {
    $rootScope.$on('$routeChangeStart', function (event) {
        if (!authProvider.hasRight($location.path())) {
            console.log('DENY : Redirecting to landing');
            $location.path('/landing');
        }
        else {
            console.log('ALLOW');
        }
    });
}]);