var app = angular.module("ApprisalSPA");

app.controller("BaseController", ["$scope", "$http", "$location", function ($scope, $http, $location) {
    $scope.MenuData = [];

    var d = $location.$$path.split("/");
    console.log(d);
    var d1 = d[d.length - 1];
    $scope.section = d[d.length - 2];
    if (d1=="0"||parseInt(d1)) {
        $http
            .get("/api/AppraisalReview/Menu/" + d1)
            .success(function (data) {
                $scope.MenuData = data;
                $scope.MenuData[0]["class"] = "selected";
            });
    }
    //$scope.MenuData = [
    //        { sectionID: 1, text: "Section 1", url: "#/Section/1" },
    //        { sectionID: 2, text: "Section 2", url: "#/Section/2" },
    //        { sectionID: 3, text: "Section 3", url: "#/Section/3" }
    //];
}])