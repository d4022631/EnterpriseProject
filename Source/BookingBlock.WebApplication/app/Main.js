'use strict';
// create the enterprise project app.
var app = angular.module('enterpriseproject',  ['ngAnimate', 'ngCookies', 'ngResource', 'ngRoute', 'ngStorage', 'ngSanitize', 'ngTouch', 'afOAuth2', 'uiGmapgoogle-maps', 'ui.calendar','ui.bootstrap' ]);
//ShareData Service
angular
.module('enterpriseproject')
.service('ShareDataService', function ShareDataService( $localStorage ){
	var obj = this;
	obj.getSearchResults = function(){
		return  $localStorage["SearchResults"];
	}
	obj.setSearchResults = function(results){
		 $localStorage["SearchResults"] = results;
	}
	obj.getName = function(){
		return  $localStorage["NAME"];
	}
	obj.setName = function(modelname){
		 $localStorage["NAME"] = modelname;
	}
	obj.getId = function(){
	  return  $localStorage["ID"];
	}
	obj.setId = function(modelid){
	  $localStorage["ID"] = modelid;
	}
	obj.getLong = function() {
	  return $localStorage["LONG"];
	}
	obj.setLong = function(longcoord) {
	  $localStorage["LONG"] = longcoord;
	}
	obj.getLat = function() {
	  return $localStorage["LAT"];
	}
	obj.setLat = function(latcoord) {
	 $localStorage["LAT"] = latcoord;
	}
	return obj;
  }
);
app.factory('httpRequestInterceptor', function ($injector) {
  return {
    request: function (config) {
try {
 // get the access token service.
    var accessTokenService = $injector.get('AccessToken');
      if(accessTokenService) {
        // get the current users token from the access token service
        var token = accessTokenService ['token'];
        if(token) {
          // get the access token from the oAuth bearer token
          var accessToken = token['access_token'];
          if(accessToken) {
          // use this to destroying other existing headers
            config.headers['X-ACCESS-TOKEN'] = accessToken;
          }
        }
      }
}
catch(err) {
}
      return config;
    }
  };
});
app.config(function ($httpProvider) {
  $httpProvider.interceptors.push('httpRequestInterceptor');
});
//  app.service('service2',['service1', function(service1) {}]);
// this is a service for making api calls.
app.service('apiServiceHelper', function() {
  // defines the site end point
  this.endpoint = "https://bookingblock.azurewebsites.net/";
  this.resourceUrl = function(urlFragment){
    return this.endpoint + urlFragment;
  };
});
// this is the register web api service. It helps with calling the WebAPI for registering a user.
app.service('registerWebApiService', function(apiServiceHelper) {
});
app.service('calendarWebApiService', function(apiServiceHelper,  $http){
  this.getBusinessCalendarUrl = function(businessId)
  {
    return apiServiceHelper.resourceUrl('api/businesses/' +  businessId + '/Calendar');
  };
});
// this service allows us to post reviews.
app.service('reviewsWebApiService', function(apiServiceHelper,  $http) {
    this.createReview = function(businessId, score, message, successFunction, errorFunction)
    {
      var newReview = {
        "BusinessId": businessId,
        "Rating": score,
        "Comments": message
      };
      var apiResourceUrl = apiServiceHelper.resourceUrl("api/reviews/create");
      $http({
        method: 'POST',
        url: apiResourceUrl,
        data: newReview
      }).then(function successCallback(response) {
          // this callback will be called asynchronously
          // when the response is available
          successFunction(response);
        }, function errorCallback(response) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          errorFunction(response);
        });
    };
});
// this is a service, they are availible application wide to provide common functionality.
// see: http://viralpatel.net/blogs/angularjs-service-factory-tutorial/
// this service used the so it is passed into the searchService function.
app.service('searchService', function(apiServiceHelper) {
  this.search = function(businessType, postcode, func2)
  {
    // show a message box with the postcode and the business type selected by the user.
    // alert("SEARCH!\r\nPostcode: " + postcode + "\r\nBusiness: " + businessType);
    var apiResourceUrl = apiServiceHelper.resourceUrl("api/Search/" + businessType + "/" + postcode + "/");
    var results = $.getJSON(apiResourceUrl, function(data) {
      func2(data);
    });
  };
    this.businessTypeComplete = function businessTypeComplete($businessType)
    {
      // check that a postcode was given otherwise the WebApi will return 404.
      if($businessType)
      {
        var apiResourceUrl = apiServiceHelper.resourceUrl("api/BusinessTypes/AutoComplete/" + $businessType);
        return $.getJSON(apiResourceUrl);
      }
      // return an empty array.
      return [];
    };
    // this function fills in the availible autocomple options as the user types there postcode
    this.postcodeAutoComplete = function postcodeAutoComplete($postcode)
    {
      // check that a postcode was given otherwise the WebApi will return 404.
      if($postcode)
      {
        var apiResourceUrl = apiServiceHelper.resourceUrl("api/Postcodes/AutoComplete/" + $postcode);
        return $.getJSON(apiResourceUrl);
      }
      // return an empty array.
      return [];
    };
});
angular.module('enterpriseproject')
  .controller('WebAPiCtrl', function ($scope, $http) {
    var token = sessionStorage.getItem("token");
    var at = JSON.parse(token)['access_token'];
    //alert(  at);
    $scope.claims = null;
    $scope.httpCall = function() {
      var config = {headers:  {
      "X-ACCESS-TOKEN" : at
    }
};
      $http.get('https://bookingblock.azurewebsites.net/api/Identity/Claims', config).success(function (data, status, headers, config) {
    		$scope.claims = data;
    	}).error(function(data, status, headers, config) {
    	});
    };
  });
// Creating a data service to pass search results between two controllers - modified from http://www.infragistics.com/community/blogs/dhananjay_kumar/archive/2015/05/04/how-to-share-data-between-controllers-in-the-angularjs.aspx and http://excellencenodejsblog.com/angularjs-sharing-data-between-controller/
app.service('shareSearchData',function($rootScope,$timeout){
  var searchResults = {};
  searchResults.data = false;
  searchResults.sendData = function(data){
    this.data = data;
    //using $timeout for 100ms to ensure the searchResults page has loaded into memory BEFORE the broadcast event has taken place
    $timeout(function(){
      $rootScope.$broadcast('results_Shared');
    },100);
  };
  searchResults.getData = function(){
    return this.data();
  };
  return searchResults;
});
app.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/About', {
        templateUrl: 'app/Views/About.html',
        controller: 'WebAPiCtrl',
        //requireToken: true
      }).
      when('/Contact', {
        templateUrl: 'app/Views/Contact.html',
        controller: 'contact'
      }).
      when('/CreateBusiness', {
        templateUrl: 'app/Views/CreateBusiness.html',
        controller: 'createBusiness'
      }).
	  when('/FAQ', {
        templateUrl: 'app/Views/FAQ.html'
      }).
	  when('/AccountView', {
        templateUrl: 'app/Views/AccountView.html'
      }).
	  when('/AdvancedSearch', {
        templateUrl: 'app/Views/AdvancedSearch.html',
        controller: 'advancedSearch'
      }).
	  when('/Booking', {
        templateUrl: 'app/Views/Booking.html',
        controller: 'booking'
      }).
    when('/BusinessAccount',{
      templateUrl: 'app/Views/BusinessAccount.html',
      controller: 'businessAccount'
      }).
	  when('/BusinessDisplay', {
        templateUrl: 'app/Views/BusinessDisplay.html',
        controller: 'businessDisplay'
      }).
	  when('/Login', {
        templateUrl: 'app/Views/Login.html',
        controller: 'login'
      }).
    when('/ManageBookings',{
        templateUrl:'app/Views/ManageBookings.html',
        controller:'manageBookings'
      }).
	  when('/Payments', {
        templateUrl: 'app/Views/Payments.html',
        controller:'payments'
      }).
	  when('/Register', {
        templateUrl: 'app/Views/Register.html',
         controller: 'register'
      }).
	  when('/SearchResults', {
        templateUrl: 'app/Views/SearchResults.html',
        controller: 'searchResults'
      }).
    when('/CustomerAccount',{
        templateUrl: 'app/Views/CustomerAccount.html',
        controller: 'customerAccount'
      }).
    when('/AmendBooking',{
      templateUrl:'app/Views/AmendBooking.html',
      controller:'amendBooking'
    }).
  	  when('/example', {
        templateUrl: 'app/Views/example.html',
        controller: 'example'
      }).
      when('/FAQ', {
        templateUrl: 'app/Views/FAQ.html',
        controller: 'FAQ'
      }).
      otherwise({
        templateUrl: 'app/Views/Home.html',
        controller: 'HomeController'
      });
  }]);
  angular.module('enterpriseproject').controller('MainPage', function ($scope, $http, $rootScope, searchService, $location, shareSearchData, ShareDataService) {
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
        ShareDataService.setSearchResults(e);
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
    $scope.displayResults = function(newResults){
    if(newResults)
    {
      $scope.results = newResults['Results'];
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
});