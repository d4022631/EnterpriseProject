angular.module('enterpriseproject')
  .controller('businessDisplay', function ($scope, $http, $location, ShareDataService, reviewsWebApiService, calendarWebApiService, uiGmapGoogleMapApi, $compile, $timeout, uiCalendarConfig, $localStorage){
  uiGmapGoogleMapApi.then(function(maps) {
  });
  $scope.marker = [ ];
  $scope.map = { center: { latitude: 54, longitude: 54 }, zoom: 8 };
  $scope.markers = $scope.mapMarkers;
  var businessToDisplay = ShareDataService.getSearchResults();
  $scope.results = undefined;
  $scope.BNname = null;
  $scope.bid = businessToDisplay;
  $scope.businessLogo = null;
  $(document).ready(function(){
      $('#datetimepicker').datepick();
  });
  $('#calendar').fullCalendar({
        // put your options and callbacks here
        contentHeight:600,
        defaultView: "agendaDay",
        events: {
            cache: true,
            url: calendarWebApiService.getBusinessCalendarUrl(ShareDataService.getId()),
        type: 'GET', // Send post data
        error: function() {
            //alert('There was an error while fetching events.');
        },
        success: function(){
           //$scope.$apply();
        }
    }
  });
  $scope.getServicesAsync = function(businessId, successCallback)
  {
      $http.get('https://bookingblock.azurewebsites.net/api/services/'+ businessId +'/list')
      .success(function(data, status, headers, config) {
          successCallback(data);
      }).error(function(data, status, headers, config) {
          //alert("BAD" + JSON.stringify(data));
      });
  };
  $scope.getBusinessAsync = function(businessId, successCallback)
  {
  $http.get('https://bookingblock.azurewebsites.net/api/Businesses/' + businessToDisplay)
      .success(function (data, status, headers, config) {
          successCallback(data);
          //alert("COOL BEANS: " + JSON.stringify(data));
          //alert("Worked.   " + data["Name"]);
          $scope.results = data["Name"];
          //alert("Latitude is " + data["Latitude"]);
          // 
          }).error(function(data, status, headers, config) {
          // alert("Not worked" + JSON.stringify(data));
          });
  };
  $http.get('https://bookingblock.azurewebsites.net/api/Businesses/' + businessToDisplay)
      .success(function (data, status, headers, config) {
        var temp = data["OpeningTimeMonday"];
        // alert("this is a thingy: " + temp);
        // alert("COOL BEANS: " + JSON.stringify(data));
        //alert("Worked.   " + data["Name"]);
        $scope.results = data["OpeningTimes"];
        //alert("Latitude is " + data["Latitude"]);
      }).error(function(data, status, headers, config) {
        // alert("Not worked" + JSON.stringify(data));
      });     
  $scope.getBusinessLogoAsync = function(businessId)
  {
    return  "https://bookingblock.azurewebsites.net/api/businesses/logo-download?id=" + businessId;
  };
  var baseurl = "https://enterpriseproject-d4022631.c9users.io/app/resources/images/"
  if(businessToDisplay) {
      // alert("and the results are: " + JSON.stringify(businessToDisplay));
      $scope.getBusinessAsync(businessToDisplay, function(data){
          $scope.map = { center: { latitude: data["Latitude"] , longitude: data["Longitude"] }, zoom: 16 };
          $scope.marker = [ ];
          $scope.marker.push({
              idKey: 1,
              latitude:  data["Latitude"],
              longitude: data["Longitude"]
          });
      });
      $scope.getServicesAsync(businessToDisplay, function(data) {
      $scope.serviceList = data;
  });
  $scope.businessLogo = $scope.getBusinessLogoAsync(businessToDisplay);
  }
  //alert ("test" + ShareDataService.getId());
  // function to navigate to selected business' booking page
  $scope.createBooking = function(){
  // alert ("Button clicked");
  // checking the correct object is to be sent.
  //  alert("id = " + JSON.stringify(ShareDataService.getId()));
  // redirect to businessDisplay page
  $location.path('/Booking');
  };
  $scope.createReview = function(){
      //stuff here.
      //alert("review created.");
      reviewsWebApiService.createReview(ShareDataService.getId(), 10, "I LIKE CATS",
        function(e){
          //alert ("OK");
      },
      function(e){
         // alert("bad");
      });
  };
  // Relates to html container to display list of available services
  $scope.serviceList = undefined;
  var listOfServices = undefined;
  // To request a booking (one service at a time at the moment)
  // this should be an ID not a name
  $scope.serviceRequested = undefined;
  $scope.timeRequested = undefined;
  $scope.yearRequested = undefined;
  $scope.monthRequested = undefined;
  $scope.dayRequested = undefined;
  $scope.priceRequested = undefined;
  // in controller
  $scope.init = function () {
  };
  var date = new Date();
  var d = date.getDate();
  var m = date.getMonth();
  var y = date.getFullYear();
  $scope.changeTo = 'Hungarian';
  /* event source that pulls from google.com */
  $scope.eventSource = {
      cache: true,
      url: calendarWebApiService.getBusinessCalendarUrl(ShareDataService.getId()),
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
      element.attr({'tooltip': event.title,'tooltip-append-to-body': true});
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
  if(businessToDisplay) {
      // to display business name
      $http.get('https://bookingblock.azurewebsites.net/api/Businesses/' + businessToDisplay)
          .success(function (data, status, headers, config) {
              $scope.businessName = data["Name"];
          }).error(function(data, status, headers, config) {
      });
      // get and display list of services associated with business
      $http.get('https://bookingblock.azurewebsites.net/api/services/'+businessToDisplay+'/list')
          .success(function(data, status, headers, config) {
              listOfServices=data;
              $scope.serviceList = listOfServices;
          }).error(function(data, status, headers, config) {
              //alert("BAD" + JSON.stringify(data));
          });
      }
  // To set service required - one at a time at the moment
  // To request a booking (one service at a time at the moment)
  // called when the booking button is clicked - serviceIn is the service ID
  $scope.requestBooking = function(serviceIn, costIn){
      $scope.serviceRequested = serviceIn;
      $scope.priceRequested = costIn;
      //alert("service id = " + JSON.stringify($scope.serviceRequested));
      // changes background colour of the relevant button to green to show service has been selected
      document.getElementById("serviceButton").style.backgroundColor='Green';
  };
  // creates a new booking object and posts to the api service
  $scope.postBooking = function(){
      $scope.time = $scope.timeRequested;
      $scope.year = $scope.yearRequested;
      $scope.month = $scope.monthRequested;
      $scope.day = $scope.dayRequested;
      $scope.cost = $scope.priceRequested;
      //alert($scope.year + "-" + $scope.month + "-" + $scope.day + "T" + $scope.time + ":17.957");
      var newBooking = {
          "ServiceId": $scope.serviceRequested,
          "DateTime": $scope.year + "-" + $scope.month + "-" + $scope.day + "T" + $scope.time + ":17.957",
          "Cost": $scope.cost
      };
      //alert("posting request");
      $http.post('https://bookingblock.azurewebsites.net/api/bookings/create',newBooking)
          .success(function(data, status, headers, config){
              //alert("new booking requested" + JSON.stringify(data));
          }).error(function(data, status,headers,config){
              //alert("BAD" + JSON.stringify(data));
              $localStorage['pageID'] = true;
              //var temp = $localStorage['pageID'];
              //alert(JSON.stringify(temp));
              $location.path('/Register');
      });
  };
});