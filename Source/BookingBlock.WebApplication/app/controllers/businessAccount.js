angular.module('enterpriseproject').controller('businessAccount', function ($scope, $http, $injector, $location, $resource, calendarWebApiService, $compile, $timeout, uiCalendarConfig, ShareDataService, $route) {
    // defining current business as soto's cuban business until can determine who is logged in
    // NB - user name: jane@hotmail.com password: Password1!
    // haye's cuban business id:
    $scope.userID = null; //"5ee96767-37bf-45ac-8d1b-46fcaab38054";
    var validBus = $scope.currentBusinessId;
    // this is the id of the current business of the user.
    $scope.currentBusinessId = null; //"5ee96767-37bf-45ac-8d1b-46fcaab38054";
    $scope.createLogoDownloadUrl = function()
    {
        return "https://bookingblock.azurewebsites.net/api/businesses/logo-download?id=" + $scope.currentBusinessId;
    }
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    $scope.changeTo = 'Hungarian';
    /* event source that pulls from google.com */
    $scope.eventSource = {
        cache: true,
        url: calendarWebApiService.getBusinessCalendarUrl( "5ee96767-37bf-45ac-8d1b-46fcaab38054" ),
        type: 'GET', // Send post data
        rendering: 'background',
        error: function() {
            //alert('There was an error while fetching events.');
        },
        success: function(){
            //$scope.$apply();
        }
    };
    /* event source that contains custom events on the scope */
    $scope.events = [
    //   {title: 'All Day Event',start: new Date(y, m, 1), rendering: 'background'},
    //   {title: 'Long Event',start: new Date(y, m, d - 5),end: new Date(y, m, d - 2), rendering: 'background'},
    //   {id: 999,title: 'Repeating Event',start: new Date(y, m, d - 3, 16, 0),allDay: false, rendering: 'background'},
    //   {id: 999,title: 'Repeating Event',start: new Date(y, m, d + 4, 16, 0),allDay: false, rendering: 'background'},
    //   {title: 'Birthday Party',start: new Date(y, m, d + 1, 19, 0),end: new Date(y, m, d + 1, 22, 30),allDay: false, rendering: 'background'},
    //   {title: 'Click for Google',start: new Date(y, m, 28),end: new Date(y, m, 29),url: 'http://google.com/', rendering: 'background'}
    ];
    /* event source that calls a function on every view switch */
    $scope.eventsF = function (start, end, timezone, callback) {
      var s = new Date(start).getTime() / 1000;
      var e = new Date(end).getTime() / 1000;
      var m = new Date(start).getMonth();
      var events = [{title: 'Feed Me ' + m,start: s + (50000),end: s + (100000),allDay: false, className: ['customFeed']}];
      callback(events);
    };
    $scope.calEventsExt = {
       color: '#f00',
       textColor: 'yellow',
       events: [ 
          {type:'party',title: 'Lunch',start: new Date(y, m, d, 12, 0),end: new Date(y, m, d, 14, 0),allDay: false, rendering: 'background'},
          {type:'party',title: 'Lunch 2',start: new Date(y, m, d, 12, 0),end: new Date(y, m, d, 14, 0),allDay: false, rendering: 'background'},
          {type:'party',title: 'Click for Google',start: new Date(y, m, 28),end: new Date(y, m, 29),url: 'http://google.com/', rendering: 'background'}
        ]
    };
    /* alert on eventClick */
    $scope.alertOnEventClick = function( date, jsEvent, view){
        $scope.alertMessage = (date.title + ' was clicked ');
    };
    /* alert on Drop */
     $scope.alertOnDrop = function(event, delta, revertFunc, jsEvent, ui, view){
       $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
    };
    /* alert on Resize */
    $scope.alertOnResize = function(event, delta, revertFunc, jsEvent, ui, view ){
       $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
    };
    /* add and removes an event source of choice */
    $scope.addRemoveEventSource = function(sources,source) {
      var canAdd = 0;
      angular.forEach(sources,function(value, key){
        if(sources[key] === source){
          sources.splice(key,1);
          canAdd = 1;
        }
      });
      if(canAdd === 0){
        sources.push(source);
      }
    };
    /* add custom event*/
    $scope.addEvent = function() {
      $scope.events.push({
        title: 'Open Sesame',
        start: new Date(y, m, 28),
        end: new Date(y, m, 29),
        className: ['openSesame']
      });
    };
    /* remove event */
    $scope.remove = function(index) {
      $scope.events.splice(index,1);
    };
    /* Change View */
    $scope.changeView = function(view,calendar) {
      uiCalendarConfig.calendars[calendar].fullCalendar('changeView',view);
    };
    /* Change View */
    $scope.renderCalender = function(calendar) {
      if(uiCalendarConfig.calendars[calendar]){
        uiCalendarConfig.calendars[calendar].fullCalendar('render');
      }
    };
     /* Render Tooltip */
    $scope.eventRender = function( event, element, view ) { 
        element.attr({'tooltip': event.title,
                     'tooltip-append-to-body': true});
        $compile(element)($scope);
    };
    /* config object */
    $scope.uiConfig = {
      calendar:{
        height: 450,
        editable: true,
        droppable: true, 
        header:{
          left: 'title',
          center: '',
          right: 'today prev,next'
        },
        eventClick: $scope.alertOnEventClick,
        eventDrop: $scope.alertOnDrop,
        eventResize: $scope.alertOnResize,
        eventRender: $scope.eventRender
      }
    };
    $scope.changeLang = function() {
      if($scope.changeTo === 'Hungarian'){
        $scope.uiConfig.calendar.dayNames = ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"];
        $scope.uiConfig.calendar.dayNamesShort = ["Vas", "Hét", "Kedd", "Sze", "Csüt", "Pén", "Szo"];
        $scope.changeTo= 'English';
      } else {
        $scope.uiConfig.calendar.dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        $scope.uiConfig.calendar.dayNamesShort = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        $scope.changeTo = 'Hungarian';
      }
    };
    /* event sources array*/
    $scope.eventSources = [$scope.events, $scope.eventSource, $scope.eventsF];
    $scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];
    /*
    $(document).ready(function(){
        $('#calendar').fullCalendar({
            // put your options and callbacks here
            events: {
                cache: true,
                url: 'https://bookingblock.azurewebsites.net/api/businesses/' +  $scope.test + '/Calendar',
                type: 'GET', // Send post data
                error: function() {
                    alert('There was an error while fetching events.');
                }
            }
        });
    }
    $('#calendar').fullCalendar({
        // put your options and callbacks here
        events: {
            cache: true,
            url: 'https://bookingblock.azurewebsites.net/api/businesses/' +  $scope.test + '/Calendar',
            //url: calendarWebApiService.getBusinessCalendarUrl(ShareDataService.getId()),
            type: 'GET', // Send post data
            error: function() {
                alert('There was an error while fetching events.');
            },
            success: function(){
                alert("working calendar");
                alert("data = " + JSON.stringify($scope.test));
                //$scope.$apply();
            }
        }
    });
    */
    $scope.LoadByBusinessId = function(businessId) {
        // set the current business address to the one passed into the load function.
        $scope.currentBusinessId = businessId;
        // alert("BUSINESS ID: " + businessId);  
        $scope.LOGOURL = $scope.createLogoDownloadUrl();
        $scope.LOGOURLUPLOAD = "https://bookingblock.azurewebsites.net/api/businesses/logo-upload?id=" + businessId;
        var validBusiness = businessId;
        // Relates to manage account tab to display list of available services
        $scope.serviceList = undefined;
        var listOfServices = undefined;
        // get the business' details from the database
        if(validBusiness) {
            $http.get('https://bookingblock.azurewebsites.net/api/Businesses/' + validBusiness)
                .success(function (data, status, headers, config) {
                    // alert("Success - and business results are: " + JSON.stringify(data));
                    // display business' details on screen
                    $scope.setBusDisplay(data);
                }).error(function(data, status, headers, config) {
                });
            // get and display list of opening times associated with business
            $http.get('https://bookingblock.azurewebsites.net/api/BusinessOpeningTimes/' + validBusiness)
                .success(function(data, status, headers, config) {
                    //alert("Success and opening time results are" + JSON.stringify(data));
                    //display opening times on screen
                    $scope.setOpeningTimes(data);
                }).error(function(data, status, headers, config) {
                });
            // get and display list of services associated with business
            $http.get('https://bookingblock.azurewebsites.net/api/services/'+validBusiness+'/list')
                .success(function(data, status, headers, config) {
                    //alert("success and list of services is: " + JSON.stringify(data));
                    listOfServices=data;
                    $scope.serviceList = listOfServices;
                }).error(function(data, status, headers, config) {
                //alert("BAD" + JSON.stringify(data));
                });
            // Displaying current bookings
            $scope.bookings = null;
            $http.get("https://bookingblock.azurewebsites.net/api/Bookings?api_key="+ validBusiness)
                .success(function(data, status, headers, config) {
                    var bookingList=data;
                    $scope.bookings = bookingList;
                    //alert("bookingList = " + JSON.stringify(bookingList));
                }).error(function(data, status, headers, config) {
                    //alert("BAD" + JSON.stringify(data));
                });
        }
    };
    $scope.LoadByUserId = function(userId) {
        $scope.userID = userId;
        $http.get('https://bookingblock.azurewebsites.net/api/businesses/my-business')
            .success(function (data, status, headers, config) {
                $scope.LoadByBusinessId(data["Id"]);
            }).error(function(data, status, headers, config) {
                //alert("ERROR LOADING BUSINESS ID");
                $location.path('/CreateBusiness');
            });
        };
    $scope.Load = function() {
        $http.get('https://bookingblock.azurewebsites.net/api/identity/who')
            .success(function (data, status, headers, config) {
                $scope.LoadByUserId(data);
            }).error(function(data, status, headers, config) {
                //alert("ERROR LOADING USER ID");
            });
        };
    $scope.Load();
    // GET THE FILE INFORMATION.
    $scope.getFileDetails = function (e) {
        $scope.files = [];
        $scope.$apply(function () {
            // STORE THE FILE OBJECT IN AN ARRAY.
            for (var i = 0; i < e.files.length; i++) {
                $scope.files.push(e.files[i])
            }
        });
    };
    $scope.LOGOURL = "https://bookingblock.azurewebsites.net/api/businesses/logo-download?id=1cb540f4-9f58-4efb-9aad-ded842862b3e";
    $scope.LOGOURLUPLOAD = "https://bookingblock.azurewebsites.net/api/businesses/logo-upload?id=1cb540f4-9f58-4efb-9aad-ded842862b3e";
    // NOW UPLOAD THE FILES.
    $scope.uploadFiles = function () {
        //FILL FormData WITH FILE DETAILS.
        var data = new FormData();
        for (var i in $scope.files) {
            data.append("uploadedFile", $scope.files[i]);
        }
        // ADD LISTENERS.
        var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);
            // SEND FILE DETAILS TO THE API.
            objXhr.open("POST", $scope.LOGOURLUPLOAD);
            objXhr.send(data);
        }
        // UPDATE PROGRESS BAR.
        function updateProgress(e) {
            if (e.lengthComputable) {
                document.getElementById('pro').setAttribute('value', e.loaded);
                document.getElementById('pro').setAttribute('max', e.total);
            }
        }
        // CONFIRMATION.
        function transferComplete(e) {
            alert("Files uploaded successfully.");
        }
        // setting initial display of business details on manage account tab
        $scope.setBusDisplay = function(dataIn){
        // setting business details to display
        // alert("set Data function data is " + JSON.stringify(dataIn));
        $scope.businessName = dataIn.Name;
        //  alert("businessType name is " + JSON.stringify(dataIn.BusinessType.Name));
        $scope.businessType = /*dataIn.BusinessType.Name;*/ "Business Type Throwing error - cannot find in data returned";
        $scope.ownerName = "Owner Name to check";
        $scope.emailAddress = dataIn.EmailAddress;
        $scope.businessWebsite = /*dataIn.Website*/ "Throwing error - Website is undefined";
        $scope.faxNumber = dataIn.FaxNumber;
        $scope.contactNumber = dataIn.PhoneNumber;
        //$scope.address1 = dataIn.Address;
        $scope.address1 = dataIn.Address.split(',')[0];
        $scope.address2 = dataIn.Address.split(',')[1];
        $scope.addressTown = dataIn.Address.split(',')[2];
        $scope.postcode = dataIn.Address.split(',')[3];
        //$scope.country = $scope.options = [{ name: "a", id: 1 }, { name: "b", id: 2 }];
        //$scope.country = 6;
        //$scope.monOpen = new Date(dataIn.OpeningTimeMonday);
        //alert("data is here: " + dataIn.OpeningTimeMonday.stringify());
    };
    // Need to fix this
    $scope.setOpeningTimes = function(dataIn){
    // alert("opening times method called" + JSON.stringify(dataIn));
    //  alert("testing something" + dataIn.ClosingTimeFriday);
        var temp1 = dataIn.OpeningTime.split(':')[0];
        var temp2 = dataIn.OpeningTime.split(':')[1];
        $scope.monOpen = temp1 + ":" + temp2;
        $scope.tueOpen = temp1 + ":" + temp2;
        $scope.wedOpen = temp1 + ":" + temp2;
        $scope.thuOpen = temp1 + ":" + temp2;
        $scope.friOpen = temp1 + ":" + temp2;
        $scope.satOpen = temp1 + ":" + temp2;
        $scope.sunOpen = temp1 + ":" + temp2;
        temp1 = dataIn.ClosingTime.split(':')[0];
        temp2 = dataIn.ClosingTime.split(':')[1];
        $scope.monClose = temp1 + ":" + temp2;
        $scope.tueClose = temp1 + ":" + temp2;
        $scope.wedClose = temp1 + ":" + temp2;
        $scope.thuClose = temp1 + ":" + temp2;
        $scope.friClose = temp1 + ":" + temp2;
        $scope.satClose = temp1 + ":" + temp2;
        $scope.sunClose = temp1 + ":" + temp2;
    };
    // function to update business with new data in html form - modified from createBusiness.js
    $scope.updateBusiness = function(){
            $scope.performUpdate($scope.businessName,$scope.businessType,$scope.ownerName,$scope.emailAddress,$scope.businessWebsite,$scope.faxNumber,$scope.contactNumber,
            $scope.address1,$scope.address2,$scope.addressTown,$scope.postcode,$scope.country,$scope.monOpen,$scope.monClose,$scope.tusOpen,$scope.tueClose,
            $scope.wedOpen,$scope.wedClose,$scope.thuOpen,$scope.thuClose,$scope.friOpen,$scope.friClose,$scope.satOpen,$scope.satClose,$scope.sunOpen,$scope.sunClose);
        };
        // Function to create the new business using the information provided in the registration form
        $scope.performUpdate = function(name, type, ownerName, email, website, fax, contact, add1, add2, town,
            postcode, country, monO, monC, tueO, tueC, wedO, wedC, thuO, thuC, friO, friC, satO,
            satC, sunO, sunC) {
            var monOfix = "2016-05-30T" + monO + ":17.50Z";
            var monCfix = "2016-05-30T" + monC + ":17.50Z";
            var tueOfix = "2016-05-30T" + tueO + ":17.50Z";
            var tueCfix = "2016-05-30T" + tueC + ":17.50Z";
            var wedOfix = "2016-05-30T" + wedO + ":17.50Z";
            var wedCfix = "2016-05-30T" + wedC + ":17.50Z";
            var thuOfix = "2016-05-30T" + thuO + ":17.50Z";
            var thuCfix = "2016-05-30T" + thuC + ":17.50Z";
            var friOfix = "2016-05-30T" + friO + ":17.50Z";
            var friCfix = "2016-05-30T" + friC + ":17.50Z";
            var satOfix = "2016-05-30T" + satO + ":17.50Z";
            var satCfix = "2016-05-30T" + satC + ":17.50Z";
            var sunOfix = "2016-05-30T" + sunO + ":17.50Z";
            var sunCfix = "2016-05-30T" + sunC + ":17.50Z";
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
            // alert("update called");
            // alert("monopen = "+ JSON.stringify(monO));
            // put request with the new business to the api server awaiting the result
            $http.put('https://bookingblock.azurewebsites.net/api/businesses/' + validBus)
            .success(function (data, status, headers, config) {
                //alert("OK: business updated. " + JSON.stringify(data));
                // only redirect when everything is ok.
                // redirect to the business' account page
                $location.path('/BusinessAccount');
            }).error(function(data, status, headers, config) {
               //alert("BAD" + JSON.stringify(data));
            });
        };
        // function to delete business - TO BE TESTED
        $scope.deleteBusiness = function(){
            alert("This will delete your business!");
            var del = confirm("Are you sure you want to delete your business?");
            if(del==true){
                $http.delete('https://bookingblock.azurewebsites.net/api/Businesses/' + validBus);
            }
            else{
                // do nothing
            }
        };   
    // function to show/hide new service div - modified from http://www.aspsnippets.com/Articles/AngularJS-Show-Hide-Toggle-HTML-DIV-on-Button-click-using-ng-show-and-ng-hide.aspx
    //This will hide the DIV by default.
    $scope.IsHidden = true;
    $scope.ShowHide = function () {
        //If DIV is hidden it will be visible and vice versa.
        $scope.IsHidden = $scope.IsHidden ? false : true;
    };
    $scope.newName = null;
    $scope.newDescription = "";
    $scope.newCost = null;
    $scope.newDuration = null;
    // function to add a new service to business
    $scope.addService = function(){
        $scope.performAddService($scope.newName,$scope.newCost,$scope.newDuration);
    };
    $scope.performAddService = function(name,cost,duration){
        alert(duration);
        var newService = {
            "BusinessId": $scope.currentBusinessId,
            "Name": name,
            "Description": "Description",
            "Cost": cost,
            "Duration": duration
        };
        $http.post('https://bookingblock.azurewebsites.net/api/services/create', newService)
            .success(function (data, status, headers, config) {
                //alert("OK: service created. " + JSON.stringify(data));
                // only redirect when everything is ok.
                // redirect to the business' account page - to refresh page and display business
                $route.reload();
            }).error(function(data, status, headers, config) {
                //alert("BAD" + JSON.stringify(data));
            });
    };
});










    
    
    
    