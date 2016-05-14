// define the home controller.
angular.module('enterpriseproject')
  .controller('HomeController', function ($scope, $rootScope, searchService, $http, $location, shareSearchData, ShareDataService ) {
    $scope.awesomeThings = [
      'HTML5 Boilerplate',
      'AngularJS',
      'Karma'
    ];
    $scope.postcode = undefined;
    $scope.businessType = undefined;
    $scope.searchResult = undefined;
    $scope.results = undefined;
    // define a perform search function to execute when the user presses the search button.
    // this is linked to the view via ng-click="performSearch()"
    $scope.performSearch = function() {
      var searchResult = searchService.search($scope.businessType, $scope.postcode, function(e){
        shareSearchData.sendData(searchResult);
          // set the results to the shared object.
          ShareDataService.setId(e);
          $location.path('/SearchResults');
      });
    };
    $scope.businessTypeComplete = function businessTypeComplete($businessType)
    {
      return searchService.businessTypeComplete($businessType);
    };
    // this function fills in the availible autocomple options as the user types there postcode
    $scope.postcodeAutoComplete = function postcodeAutoComplete($postcode)
    {
      return searchService.postcodeAutoComplete($postcode);
    };
  });