var app = angular.module("ApprisalSPA");

app.controller("DetailController", ['$scope', '$routeParams', '$http', 'ApprisalDataFactory', function ($scope, $routeParams, $http, adf) {
    if ($scope.$parent.MenuData.length > 0) {
        for (var i = 0; i < $scope.$parent.MenuData.length; i++) {
            $scope.$parent.MenuData[i].class = "";
            $scope.$parent.MenuData[0].class = "selected";
        }
    }

    /*
    every node in viewData will havre following members

    SectionID: int,
    SectionName: string,
    Sectiondata: []
    isdataAvailable: bool

    */

    var viewData = [];

    /*
     Every node of Post data will contains following data

    SectionId: int,
    SectionComment: string
    */

    //if ($scope.$parent.MenuData.length > 0) {
    //    for (var i = 0; i < $scope.$parent.MenuData.length; i++) {
    //        $scope.$parent.MenuData[i]["class"] = "";
    //    }
    //}
    //$scope.$parent.MenuData[0]["class"] = "selected"

    var postdata = []
    var eid = $routeParams.employee;
    if (eid.length > 0) {
        adf.getFrom("/api/appraisalReview/GetAppraiseeDetails/" + eid)
            .success(function (data) {
                $scope.data = data;
            })
            .error(function (err) { })
    }
    else {
        $scope.viewData = "ERROR, Employee ID is not provided";
    }
}]);