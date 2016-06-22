angular.module("ApprisalSPA", ["ngRoute"])
.config(['$routeProvider', function ($routeProvider) {
    console.log("----");
    $routeProvider.
     when('/Section/:sec/:employee', {
         templateUrl: '../scripts/appraisal/views/Section1.html',
         controller: 'SectionController', caseInsensitiveMatch: true
     }).
         when('/Candidate/Details/For/:employee', {
             templateUrl: '../scripts/appraisal/views/Details.html',
             controller: 'DetailController', caseInsensitiveMatch: true
         }).
    otherwise({ redirectTo: "/" })
}]);

angular.module("ApprisalReview", ["ApprisalSPA"])
.config([ function () {
}]);