var app = angular.module("ApprisalReview");

app.controller("ReviewController", ['$scope', '$location', '$http', 'ApprisalDataFactory', function ($scope, $location, $http, adf) {
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
    var postdata = []
    var url = $location.$$url;
    var eid = url.length > 0 && url != "" ? $location.$$url.split("/")[1] : "";
    if (eid.length > 0) {
        adf.getFrom("/api/appraisalReview/For/" + eid)
            .success(function (response) {
                var data = response.sections;
                $scope.appriasee = response.Appriasee;
                for (var i = 0; i < data.length; i++) {
                    var dataForSection = data[i].Data;
                    if (!(angular.isUndefined(dataForSection) || dataForSection === null))
                        viewData.push(h.serializeDataForGrid(data[i]));
                    else {
                        data[i].Data = [];
                        viewData.push(h.serializeDataForGrid(data[i]));
                    }
                }
                $scope.viewData = viewData;
            })
            .error(function (err) { })
    }
    else {
        $scope.viewData = "ERROR, Employee ID is not provided";
    }
    /* Helper class accesibility Private*/
    var h = {
        serializeDataForGrid: function (data) {
            var _output = null;
            if (data.Type == "JO")
                _output = h.pivotVerticleToHorizontal(data);
            else {
                if (data.Data.length > 0)
                    _output = h.setData(data, data.Data, true);
                else
                    _output = h.setData(data, [], false);
            }
            return _output;
        },
        pivotVerticleToHorizontal: function (data, colPrefix) {
            var _output = null;
            console.log(data.Data);
            if (isEmpty(data.Data) || isEmpty(data.param))
                _output = h.setData(data, [], false);
            else {
                var destination = [];
                destination = _serialize(data.param, destination);
                destination = _serialize(data.Data, destination);
                _output = h.setData(data, destination, true);
            }
            return _output;

            function _serialize(data, destination) {
                var _counter;
                for (var current in data) {
                    _counter = 0;
                    for (var item in data[current]) {
                        if (destination[_counter] === undefined)
                            destination.push({});
                        destination[_counter][current] = data[current][item];
                        _counter++;
                    }
                }
                return destination;
            }
        },
        setData: function (data, grid, isAvailable) {
            return { SectionID: data.SectionID, SectionName: data.SectionName, SectionQuestion:data.Questions,  SectionData: grid, isDataAvailable: isAvailable};
        }
    }

    $scope.printDiv = function () {
        window.print();
    }

    $scope.gotoPreviousPage = function () {
        window.location.href = "/AppraisalSection/AppraisalEmployee";
    }
}]);