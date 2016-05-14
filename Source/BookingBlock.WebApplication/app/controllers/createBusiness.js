angular.module('enterpriseproject').controller('createBusiness', function ($scope, $http, $injector, $location) {
        // the name of the new business.
        $scope.businessName = null;
        // business code to be generated and added later
        // The new business' code for URL
        // $scope.businessCode = null;
        // The type of the new business
        $scope.businessType = null;
        // The name of the business' owner
        $scope.ownerName = null;
        // The business' e-mail address
        $scope.emailAddress = null;
        // The business' website address
        $scope.businessWebsite = null;
        // The business' fax number
        $scope.faxNumber = null;
        // The business' contact number
        $scope.contactNumber = null;
        // The business' first line of address
        $scope.address1 = null;
        // The business' second line of address
        $scope.address2 = null;
        // The business' town
        $scope.addressTown = null;
        // The business' postcode
        $scope.postcode = null;
        // The country the business is located in
        $scope.country = null;
        // Opening and closing times
        $scope.monOpen = "06:00";
        $scope.monClose = "18:00";
        $scope.tuesOpen = "06:00";
        $scope.tuesClose = "18:00";
        $scope.wedOpen = "06:00";
        $scope.wedClose = "18:00";
        $scope.thursOpen = "06:00";
        $scope.thursClose = "18:00";
        $scope.friOpen = "06:00";
        $scope.friClose = "18:00";
        $scope.satOpen = "06:00";
        $scope.satClose = "18:00";
        $scope.sunOpen = "06:00";
        $scope.sunClose = "18:00";
        $scope.addBusiness = function(){
            $scope.performAdd($scope.businessName,$scope.businessType,$scope.ownerName,$scope.emailAddress,$scope.businessWebsite,$scope.faxNumber,$scope.contactNumber,
            $scope.address1,$scope.address2,$scope.addressTown,$scope.postcode,$scope.country,$scope.monOpen,$scope.monClose,$scope.tuesOpen,$scope.tuesClose,
            $scope.wedOpen,$scope.wedClose,$scope.thursOpen,$scope.thursClose,$scope.friOpen,$scope.friClose,$scope.satOpen,$scope.satClose,$scope.sunOpen,$scope.sunClose);
        };
        // Function to create the new business using the information provided in the registration form
        $scope.performAdd = function(name, type, ownerName, email, website, fax, contact, add1, add2, town,
            postcode, country, monO, monC, tueO, tueC, wedO, wedC, thuO, thuC, friO, friC, satO,
            satC, sunO, sunC) {
            var monOfix = "2016-05-30T" + monO + ":17.500";
            var monCfix = "2016-05-30T" + monC + ":17.500";
            var tueOfix = "2016-05-30T" + tueO + ":17.500";
            var tueCfix = "2016-05-30T" + tueC + ":17.500";
            var wedOfix = "2016-05-30T" + wedO + ":17.500";
            var wedCfix = "2016-05-30T" + wedC + ":17.500";
            var thuOfix = "2016-05-30T" + thuO + ":17.500";
            var thuCfix = "2016-05-30T" + thuC + ":17.500";
            var friOfix = "2016-05-30T" + friO + ":17.500";
            var friCfix = "2016-05-30T" + friC + ":17.500";
            var satOfix = "2016-05-30T" + satO + ":17.500";
            var satCfix = "2016-05-30T" + satC + ":17.500";
            var sunOfix = "2016-05-30T" + sunO + ":17.500";
            var sunCfix = "2016-05-30T" + sunC + ":17.500";
            // for the opening times if either the opening time is null, or the
            // closing time is null, then the business will not be open on that day.
            var newBusiness = {
              "Name": name,
              "Type": type,
              "ContactName": ownerName,
              "ContactEmail": email,
              "ContactNumber": contact,
              "ContactFax": fax,
              "AddressLine1": add1,
              "AddressLine2": add2,
              "TownCity": town,
              "Postcode": postcode,
              "Country": country,
              "Website": website,
              "OwnerEmailAddress": email,
              "OpeningTimeMonday": monOfix,
              "ClosingTimeMonday": monCfix,
              "OpeningTimeTuesday": tueOfix,
              "ClosingTimeTuesday": tueCfix,
              "OpeningTimeWednesday": wedOfix,
              "ClosingTimeWednesday": wedCfix,
              "OpeningTimeThursday": thuOfix,
              "ClosingTimeThursday": thuCfix,
              "OpeningTimeFriday": friOfix,
              "ClosingTimeFriday": friCfix,
              "OpeningTimeSaturday": satOfix,
              "ClosingTimeSaturday": satCfix,
              "OpeningTimeSunday": sunOfix,
              "ClosingTimeSunday": sunCfix
            };
            // there is an additional object field that can only be used by Administrators:
            // "OwnerEmailAddress": "owner@example.com",
            // this allows admins to create business' on behalf of the specified user
            // otherwise the newly created business would be owned by the admin
            // that created it and not the desired user. But for the basic create
            // business page do not worry about it.
            // post the new business to the api server awaiting the result
            $http.post('https://bookingblock.azurewebsites.net/api/businesses/regster', newBusiness)
            .success(function (data, status, headers, config) {
                //alert("OK: business registered. " + JSON.stringify(data));
                // only redirect when everything is ok.
                // redirect to the business' account page - redirecting to home for now until account page is set up
                $location.path('/BusinessAccount');
            }).error(function(data, status, headers, config) {
                //alert("BAD" + JSON.stringify(data));
            });
        };
    }
);
  