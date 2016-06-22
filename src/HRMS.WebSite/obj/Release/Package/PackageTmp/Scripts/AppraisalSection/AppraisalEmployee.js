angular.module('AppraisalEmp', []).controller('employeeController', ['$scope', '$filter', '$http', function ($scope, $filter, $http) {
    $http({
        method: 'GET',
        url: '/AppraisalSection/GetLogEmployeeId',
        cache: true
    }).success(function (data, status, headers, config) {
        $scope.EmployeeId = data;
        var Id = $scope.EmployeeId;
        var URL = '/api/AppraisalAdmin/EmployeeList/for/' + Id;
        $http({
            method: 'GET',
            url: URL,
            cache: true
        }).success(function (data, status, headers, config) {
            var eData = data.Data;
            //$scope.EmployeesList = data.Data;
            function _setColumn(list, column, func) {
                for (var i in list) {
                    list[i][column] = func(list[i][column]);
                }
                return list;
            }

            $scope.EmployeesList = _setColumn(eData,
                                    "StageComplete",
                                    function (val) {
                                        var totalStages = function () {
                                            return [
                                                    { key: "Appraisee", val: false },
                                                    { key: "Appraiser 1", val: false },
                                                    { key: "Appraiser 2", val: false },
                                                    { key: "Reviewer 1", val: false },
                                                    { key: "Reviewer 2", val: false },
                                                    { key: "Group Head", val: false },
                                                    { key: "IDF", val: false }
                                                   ];
                                        }

                                         var stages = totalStages();
                                        var strList = val.split(",");
                                        for (var i in strList) {
                                            var statgeValue = stages[parseInt(strList[i]) - 1];
                                            if (statgeValue != undefined) {
                                                statgeValue.val = true;
                                            }
                                        }
                                        return stages;
                                    });
        }).error(function (data, status, headers, config) {
        });
    }).error(function (data, status, headers, config) {
    });

    //function _setColumn(list, column, func) {
    //    for (var i in list) {
    //        list[i][column] = func(list[i][column]);
    //    }
    //    return list;
    //}

    //$scope.EmployeesList = _setColumn([{ "EmployeeId": 179, "EmployeeCode": 1144, "EmployeeName": "Pushpal S K Mishra", "RelationAbbr": "RV1", "RelationShip": "", "StageComplete": "1,2" },
    //                        { "EmployeeId": 292, "EmployeeCode": 1582, "EmployeeName": "Brijen Arvindbhai Patel", "RelationAbbr": "grphd", "RelationShip": "", "StageComplete": "1,5" },
    //                        { "EmployeeId": 1385, "EmployeeCode": 3112, "EmployeeName": "Sandeep  Chaudhari", "RelationAbbr": "RV1", "RelationShip": "", "StageComplete": "1,2" },
    //                        { "EmployeeId": 200, "EmployeeCode": 1260, "EmployeeName": "Nikish Kiritkumar Parikh", "RelationAbbr": "RV1", "RelationShip": "", "StageComplete": "1,4" },
    //                        { "EmployeeId": 2283, "EmployeeCode": 3875, "EmployeeName": "Neha  kapoor", "RelationAbbr": "RV2", "RelationShip": "", "StageComplete": "1,5" }],
    //                        "StageComplete",
    //                        function (val) {
    //                            var totalStages = function (length) {
    //                                var arr = [];
    //                                for (var i = 0; i < length; i++)
    //                                    arr[i] = false;
    //                                return arr;
    //                            }

    //                            var stages = totalStages(6);
    //                            var strList = val.split(",");
    //                            for (var i in strList)
    //                                stages[parseInt(strList[i])-1] = true;

    //                            return stages;
    //                        });

    $scope.cellclick = function (Id) {
        $scope.empId = Id;
        url = "/appraisal/index#/Candidate/Details/For/" + Id;
        window.location.href = url;
    };

    $scope.reviewclick = function (Id) {
        url = "/appraisal/" + Id;
        window.location.href = url;
    };
}]);