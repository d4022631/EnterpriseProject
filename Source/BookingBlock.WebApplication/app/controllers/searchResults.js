angular.module('enterpriseproject').controller('searchResults', function (
    $timeout, $scope, $rootScope, searchService, $http, $location,
    shareSearchData, ShareDataService, uiGmapGoogleMapApi,     $sessionStorage ) {
    uiGmapGoogleMapApi.then(function(maps) {
       // alert("MAP LOADER"); 
    });
    $scope.marker = [ ];
    $scope.map = { center: { latitude: 54, longitude: 54 }, zoom: 8 };
    $scope.markers = $scope.mapMarkers;
      $http.get('https://bookingblock.azurewebsites.net/api/identity/who')
        .success(function(data, status, headers, config) {
          //alert("Success the id was found" + JSON.stringify(data))
            successCallback(data);
          }).error(function(data, status, headers, config) {
            //alert("BAD" + JSON.stringify(data));
          });
    // get the current search results.
    var currentResults = ShareDataService.getId();
    // Code for search button
    $scope.postcode = undefined;
    $scope.businessType = undefined;
    $scope.searchResult = undefined;
    $scope.results = undefined;
    if(currentResults)
    {
      $scope.results = currentResults['Results'];
      $scope.postcode = currentResults['Postcode'];
      $scope.businessType = currentResults['BusinessType'];
      $scope.map = { center: { latitude: currentResults["Latitude"] , longitude: currentResults["Longitude"] }, zoom: 8 };
      for (var i = 0; i <  $scope.results.length; i++) { 
        $scope.marker.push({
          idKey: i+1,
          latitude:  $scope.results[i]["Latitude"],
          longitude: $scope.results[i]["Longitude"]
        });
      }
    //  "Latitude": 54.6027994601504,
    // "Longitude": -1.28983126839118,
    }
    else
    {
      // Send them back to the home page.
      $location.path('/');
    }
    // define a perform search function to execute when the user presses the search button.
    // this is linked to the view via ng-click="performSearch()"
    $scope.performSearch = function() {
      var searchResult = searchService.search($scope.businessType, $scope.postcode, function(e){
        // display the updated results on the page
        $scope.displayResults(e);
      });
    };
    // function to update results with new search
    $scope.displayResults = function(newResults){
      if(newResults) {
        $scope.results = newResults['Results'];
        //$scope.results[0]["Latitude"]
        // updating the map with the new results' locations
        $scope.map = { center: { latitude: newResults["Latitude"] , longitude: newResults["Longitude"] }, zoom: 8 };
        for (var i = 0; i <  $scope.results.length; i++) { 
          $scope.marker.push({
            idKey: i+1,
            latitude:  $scope.results[i]["Latitude"],
            longitude: $scope.results[i]["Longitude"]
          });
        }
        // applying the new results to the html page
        $scope.$apply();
      }
    };
    $scope.businessTypeComplete = function businessTypeComplete($businessType)
    {
      return searchService.businessTypeComplete($businessType);
    };
    // this function fills in the availible autocomple options as the user types their postcode
    $scope.postcodeAutoComplete = function postcodeAutoComplete($postcode)
    {
      return searchService.postcodeAutoComplete($postcode);
    };
    // function to set businessId using ShareDataService and navigate to selected business' home page
    $scope.viewBusiness = function(idIn){
      //alert("This should work " + latIn + " " = longIn);
      // set the results to the shared object.
      ShareDataService.setSearchResults(idIn);
      //ShareDataService.setLat(latIn);
      //ShareDataService.setLong(longIn);
      // redirect to businessDisplay page
      $location.path('/BusinessDisplay');
    };
    // function to navigate to advanced search page when button is clicked
    $scope.advSearchNav = function(){
        $location.path('/AdvancedSearch');
    };
});