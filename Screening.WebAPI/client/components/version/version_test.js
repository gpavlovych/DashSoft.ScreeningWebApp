'use strict';

describe('screeningApp.version module', function() {
  beforeEach(module('screeningApp.version'));

  describe('version service', function() {
    it('should return current version', inject(function(version) {
      expect(version).toEqual('0.1');
    }));
  });
});
