'use strict';

angular.module('screeningApp.version', [
  'screeningApp.version.interpolate-filter',
  'screeningApp.version.version-directive'
])

.value('version', '0.1');
