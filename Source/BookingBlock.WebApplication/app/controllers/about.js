angular.module('enterpriseproject')
  .controller('WebAPiCtrl', function ($scope, $http) {
    var token = sessionStorage.getItem("token");
    var at = JSON.parse(token)['access_token'];
    $scope.claims = null;
    $scope.httpCall = function() {
      var config = {headers:  {
      }
    };
    $http.get('https://bookingblock.azurewebsites.net/api/identity/claims', config).success(function (data, status, headers, config) {
    		$scope.claims = data;
    	}).error(function(data, status, headers, config) {
    	});
    };
  });
  