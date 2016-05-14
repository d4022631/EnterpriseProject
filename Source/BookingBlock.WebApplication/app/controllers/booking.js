angular.module('enterpriseproject')
  .controller('booking', function ($scope, $http, $location, ShareDataService, calendarWebApiService, $compile, $timeout, uiCalendarConfig, $localStorage) {
    // id for Soto's Cuban to test calendar
    $scope.test = "909252bf-140a-e611-ad82-9cd21ea8ce10";
    var businessToDisplay = ShareDataService.getId();
    $scope.results = undefined;
    $scope.BNname = null;
    // Relates to html container to display list of available services
    $scope.serviceList = undefined;
    var listOfServices = undefined;
    // To request a booking (one service at a time at the moment)
    // this should be an ID not a name
    $scope.serviceRequested = undefined;
    $scope.timeRequested = "2016-04-28T12:39:17.958Z";
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
    $scope.requestBooking = function(serviceIn){
      $scope.serviceRequested = serviceIn;
      //alert("service id = " + JSON.stringify($scope.serviceRequested));
      // changes background colour of the relevant button to green to show service has been selected
      document.getElementById("serviceButton").style.backgroundColor='Green';
      $scope.postBooking(serviceIn, $scope.timeRequested);
    };
    // creates a new booking object and posts to the api service
    $scope.postBooking = function(service, time){
      var newBooking = {
      "ServiceId": service,
      "DateTime": time
    };
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
  