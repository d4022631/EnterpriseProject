angular.module('enterpriseproject').controller('example', function ($scope, $http, $injector, $location) {
    // function to call the web api when the user clicks a button.
    $scope.webApiCall = function() {
        //alert("WebAPI call");
            // "OwnerEmailAddress": "string",
            var newBusiness = {
              "Name": "ASD",
              "Type": "Plumbing",
              "ContactName": "string",
              "ContactEmail": "string",
              "ContactNumber": "string",
              "ContactFax": "string",
              "AddressLine1": "ASD",
              "AddressLine2": "string",
              "TownCity": "string",
              "Postcode": "TS23 2QH",
              "Country": "string",
              "Website": "string",
              "OpeningTimeMonday": "01:00",
              "ClosingTimeMonday": "02:00",
              "OpeningTimeTuesday": "03:00",
              "ClosingTimeTuesday": "04:00",
              "OpeningTimeWednesday": "05:00",
              "ClosingTimeWednesday": "06:00",
              "OpeningTimeThursday": "07:00",
              "ClosingTimeThursday": "08:00",
              "OpeningTimeFriday": "09:00",
              "ClosingTimeFriday": "10:00",
              "OpeningTimeSaturday": "11:00",
              "ClosingTimeSaturday": "12:00",
              "OpeningTimeSaturday": "11:00",
              "ClosingTimeSaturday": "12:00"
            };
            // post the new business to the api server awaiting the result
            $http.post('https://bookingblock.azurewebsites.net/api/businesses/regster', newBusiness)
            .success(function (data, status, headers, config) {
                //alert("OK: business registered.");
            }).error(function(data, status, headers, config) {
                //alert("BAD" + JSON.stringify(data));
            });
    };
});
