angular.module('enterpriseproject').controller('register', function ($scope, $http, $injector, $location, registerWebApiService, $localStorage) {
    $scope.firstName = null;
    $scope.lastName = null;
    $scope.emailAddress = null;
    $scope.password = null;
    $scope.confirmPassword = null;
    $scope.DateOfBirth = null;
    $scope.mobileNumber = null;
    $scope.address1 = null;
    $scope.addressTown = null;
    $scope.postcode = null;
    $scope.country = null;
    $scope.gender = null;
    // Checking that the user is not already logged in. If is logged in, redirects to the home page.
    var myService = $injector.get('AccessToken');
    var token = myService['token'];
    // if the token is not null.
    if(token)
    {
        // redirect the user to the home page.
        $location.path('/');
    }
    $scope.register = function(){
        $scope.performRegistration($scope.firstName,$scope.lastName,$scope.emailAddress, $scope.password,
        $scope.confirmPassword, $scope.dateOfBirth,$scope.address1,$scope.address2,$scope.mobileNumber,$scope.addressTown,$scope.postcode,$scope.country,$scope.gender);
    };
    // Function to create the new user using the information provided in the registration form
    $scope.performRegistration = function(userFirstName, userLastName,
            userEmailAddress, userPassword, userConfirmPassword, userDateOfBirth,
            userAddressLine1, userAddressLine2, userMobileNumber, userTownCity,
            userPostcode, userCountry, userGender) {
        var newUser = {
            "FirstName": userFirstName,
            "LastName": userLastName,
            "EmailAddress": userEmailAddress,
            "Password": userPassword,
            "ConfirmPassword": userConfirmPassword,
            "DateOfBirth": userDateOfBirth,
            "AddressLine1": userAddressLine1,
            "AddressLine2": userAddressLine2,
            "MobileNumber": userMobileNumber,
            "TownCity": userTownCity,
            "Postcode": userPostcode,
            "Country": userCountry,
            "Gender": userGender
        };  
        // POST request to api to create new account and add to database. If successful returns message to user and redirects to home page to allow login. If unsuccessful returns error message.
        $http.post('https://bookingblock.azurewebsites.net/api/Account/Register', newUser)
            // if post is successful run the following function
            .success(function (data, status, headers, config) {
                // alert("OK: you can now login");
                // create temporary variable to store data from localStorage pageID
                var temp = $localStorage['pageID'];
                // alert("temp is" + temp);
                // if condition to see if temp is true
                if (temp == true) {
                    // set padeID to false again so as not to redirect when not needed
                    $localStorage['pageID'] = false;
                    // set temp to false. likely not necessary but why not.
                    temp = false;
                    // redirect the user to the booking which they were previously viewing
                    $location.path('/businessDisplay');
                // if temp is not true then
                } else {
                    // redirect the user to the home page
                    $location.path('/');
                }
            // if the post is not successful and encounters an error then do the following
            }).error(function(data, status, headers, config) {
                // alert("BAD" + JSON.stringify(data));
        });
    };
});