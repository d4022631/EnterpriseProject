angular.module('enterpriseproject')
    .controller('customerAccount', function ($scope, $http, $injector, $location, ShareDataService, reviewsWebApiService, calendarWebApiService, uiGmapGoogleMapApi, $compile, $timeout, uiCalendarConfig, $localStorage) {
        // definition for userEmail binding between the html and here.
        $scope.userEmail = "jane@hotmail.com";
        // definition for userID binding between the html and here.
        $scope.userID = "459439b1-0399-4342-bbb4-01da358658b1";
        $scope.trueUserID = null;
        var userIdNumber = null;
        var validCustomer = null;
        //alert("Welcome called");
        $http.get('https://bookingblock.azurewebsites.net/api/account/get-userid?email=' + $scope.userEmail)
            .success(function (data, status, headers, config) {
                // alert("returns: " + JSON.stringify(data));
                $scope.openingTimeResults = data["Name"];
            }).error(function(data, status, headers, config) {
                // alert("Not worked" + JSON.stringify(data));
        });
        $http.get('http://bookingblockproduction.azurewebsites.net/api/Bookings')
            .success(function (data, status, headers, config) {
            // alert("returns: " + JSON.stringify(data));
                $scope.openingTimeResults = data["Name"];
            }).error(function(data, status, headers, config) {
            // alert("Not worked" + JSON.stringify(data));
        });
        //get request to retrieve ID of person who is currently logged in.
        $http.get('https://bookingblock.azurewebsites.net/api/identity/who')
            // if post is successful run the following function
            .success(function(data, status, headers, config) {
               //alert("Success the id was found" + JSON.stringify(data));
               // callback to let know
               //successCallback(data);
               $scope.trueUserID = data;
               $scope.setUserInfo(data);
               $scope.displayUserInfo(data);
               userIdNumber = data;
            // if the post is not successful and encounters an error then do the following
            }).error(function(data, status, headers, config) {
               // alert("BAD" + JSON.stringify(data));
            });
        $('#calendar').fullCalendar({
            // put your options and callbacks here
            contentHeight:600,
            defaultView: "agendaDay",
             /*     events: {
                cache: true,
                url: calendarWebApiService.getBusinessCalendarUrl(ShareDataService.getId()),
                type: 'GET', // Send post data
                error: function() {
                    alert('There was an error while fetching events.');
                },
                success: function(){
                    //$scope.$apply();
                }
            }  */
        });
        $scope.bookings = null;
        // function to display list of customer's bookings
        $scope.setUserInfo = function(dataIn){
            $http.get("https://bookingblock.azurewebsites.net/api/Bookings?api_key="+ dataIn)
            .success(function(data, status, headers, config) {
                var bookingList=data;
                $scope.bookings = bookingList;
                // alert("bookingList = " + JSON.stringify(bookingList));
            }).error(function(data, status, headers, config) {
                //alert("BAD" + JSON.stringify(data));
            });
        };
        $scope.displayUserInfo = function(userIdIn){
            $http.get("https://bookingblock.azurewebsites.net/api/Users/"+userIdIn)
            .success(function(data, status, headers, config) {
                var details=data;
                $scope.firstName = details.FirstName;
                $scope.lastName = details.LastName;
                $scope.emailAddress = details.Email;
                $scope.DateOfBirth = details.DateOfBirth;
                $scope.address1 = details.Address.split(',')[0];
                $scope.address2 = details.Address.split(',')[1];
                $scope.addressTown = details.Address.split(',')[2];
                $scope.postcode = details.Postcode;
                // alert("bookingList = " + JSON.stringify(bookingList));
            }).error(function(data, status, headers, config) {
                //("BAD" + JSON.stringify(data));
            });
        };
        $scope.updateCustomer = function(){
            if ($scope.password == null){
                $scope.performUpdateNoPass($scope.firstName,$scope.lastName,$scope.emailAddress, $scope.DateOfBirth,$scope.address1,
                $scope.address2,$scope.addressTown,$scope.postcode,$scope.password,$scope.confirmPassword,$scope.mobileNumber,
                $scope.country,$scope.gender);
            }
            else{
                $scope.performUpdate($scope.firstName,$scope.lastName,$scope.emailAddress, $scope.DateOfBirth,$scope.address1,
                $scope.address2,$scope.addressTown,$scope.postcode,$scope.password,$scope.confirmPassword,$scope.mobileNumber,
                $scope.country,$scope.gender);
            };
        };
        $scope.performUpdateNoPass = function(userFirstName, userLastName,
            userEmailAddress, userDateOfBirth,
            userAddressLine1, userAddressLine2, userTownCity,
            userPostcode,userPassword,userConfirmPassword,userMobNumber,
            userCountry,userGender) {
            var updatedUser = {
                "FirstName": userFirstName,
                "LastName": userLastName,
                "EmailAddress": userEmailAddress,
                "Password": "",
                "ConfirmPassword": "",
                "DateOfBirth": userDateOfBirth,
                "MobileNumber": userMobNumber,
                "AddressLine1": userAddressLine1,
                "AddressLine2": userAddressLine2,
                "TownCity": userTownCity,
                "Postcode": userPostcode,
                "Country": userCountry,
                "Gender": userGender
            };
            //alert("user id = " + userIdNumber);
            //alert("data = " + JSON.stringify(updatedUser));
            $http.post('https://bookingblock.azurewebsites.net/api/account/Update?api_key='+userIdNumber, updatedUser)
                .success(function (data, status, headers, config) {
                //alert("OK: business updated. " + JSON.stringify(data));
                // only redirect when everything is ok.
                // redirect to the customer's account page
                $location.path('/CustomerAccount');
            }).error(function(data, status, headers, config) {
               //alert("BAD" + JSON.stringify(data));
            });
        };   
        $scope.performUpdate = function(userFirstName, userLastName,
            userEmailAddress, userDateOfBirth,
            userAddressLine1, userAddressLine2, userTownCity,
            userPostcode,userPassword,userConfirmPassword,userMobNumber,
            userCountry,userGender) {
            var updatedUser = {
                "FirstName": userFirstName,
                "LastName": userLastName,
                "EmailAddress": userEmailAddress,
                "Password": userPassword,
                "ConfirmPassword":userConfirmPassword,
                "DateOfBirth": userDateOfBirth,
                "MobileNumber": userMobNumber,
                "AddressLine1": userAddressLine1,
                "AddressLine2": userAddressLine2,
                "TownCity": userTownCity,
                "Postcode": userPostcode,
                "Country": userCountry,
                "Gender": userGender
            };
            //alert("user id = " + userIdNumber);
            //alert("data = " + JSON.stringify(updatedUser));
            $http.post('https://bookingblock.azurewebsites.net/api/account/Update?api_key='+userIdNumber, updatedUser)
                .success(function (data, status, headers, config) {
                //alert("OK: business updated. " + JSON.stringify(data));
                // only redirect when everything is ok.
                // redirect to the customer's account page
                $location.path('/CustomerAccount');
            }).error(function(data, status, headers, config) {
               //alert("BAD" + JSON.stringify(data));
            });
        };   
    });