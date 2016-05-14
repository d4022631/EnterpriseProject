angular.module('enterpriseproject')
  .controller('advancedSearch', function ($timeout, $scope, $rootScope, searchService, $http, $location, shareSearchData, ShareDataService, apiServiceHelper ) {
    // Code for search button
    $scope.postcode = undefined;
    $scope.businessType = undefined;
    $scope.searchResult = undefined;
    $scope.results = undefined;
    $scope.radius = undefined;
    $scope.businessTypeComplete = function businessTypeComplete($businessType)
    {
      return searchService.businessTypeComplete($businessType);
    };
    // this function fills in the availible autocomple options as the user types there postcode
    $scope.postcodeAutoComplete = function postcodeAutoComplete($postcode)
    {
      return searchService.postcodeAutoComplete($postcode);
    };
    // define a perform search function to execute when the user presses the search button.
    // this is linked to the view via ng-click="performSearch()"
    $scope.performAdvancedSearch = function() {
      var searchRadius = $scope.getDistance();
      $http.get('https://bookingblock.azurewebsites.net/api/Search/' + $scope.businessType + '/' + $scope.postcode +'?distance=' + searchRadius)
        .success(function (data, status, headers, config) {
          // set the results to the shared object.
          ShareDataService.setId(data);
          $location.path('/SearchResults');
        }).error(function(data, status, headers, config) {
      }); 
    };
    // function to get and return the selected search radius
    $scope.getDistance = function(){
      if($scope.distanceSelected == "option-1"){
        $scope.radius = 1;
      }
      else if($scope.distanceSelected == "option-2"){
        $scope.radius = 2;
      }
      else if($scope.distanceSelected == "option-3"){
        $scope.radius = 5;
      }
      else if($scope.distanceSelected == "option-4"){
        $scope.radius = 10;
      }
      else if($scope.distanceSelected == "option-5"){
        $scope.radius = 20;
      }
      else if($scope.distanceSelected == "option-6"){
        $scope.radius = 50;
      }
      else if($scope.distanceSelected == "option-7"){
        $scope.radius = 200000;
      }
      else{
        $scope.radius = 10;
      }
      return $scope.radius;
    }
});
  