'use strict';

/* https://github.com/angular/protractor/blob/master/docs/toc.md */

describe('my app', function() {


  it('should automatically redirect to /student when location hash/fragment is empty', function() {
    browser.get('index.html');
    expect(browser.getLocationAbsUrl()).toMatch("/student");
  });


  describe('student', function() {

    beforeEach(function() {
      browser.get('index.html#!/student');
    });


    it('should render student when user navigates to /student', function() {
      expect(element.all(by.css('[ng-view] p')).first().getText()).
        toMatch(/partial for view 1/);
    });

  });


  describe('teacher', function() {

    beforeEach(function() {
      browser.get('index.html#!/teacher');
    });


    it('should render teacher when user navigates to /teacher', function() {
      expect(element.all(by.css('[ng-view] p')).first().getText()).
        toMatch(/partial for view 2/);
    });

  });
});
