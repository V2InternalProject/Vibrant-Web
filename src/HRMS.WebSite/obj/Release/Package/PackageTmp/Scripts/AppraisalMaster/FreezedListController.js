var app = angular.module('HRMS', ['ngGrid']);
app.controller('MyCtrl', ["$scope", "AppraislFactory","$http", function ($scope, AppraislFactory,$http) {
    $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="city as city.Text for city in  statuses" ng-blur="updateEntity(row)" />';
    $scope.filterOptions = {
        filterText: ''
    };
    AppraislFactory.getGridData().success(function (data, status, headers) {
        $scope.myData = data;
        //$scope.gridFreezedOptions.$gridScope.columns[11].toggleVisible();
        //$scope.gridFreezedOptions.$gridScope.columns[12].toggleVisible();
        AppraislFactory.getDropDownData()
       .success(function (data, status, headers) {
           $scope.statuses = data.Appr1;
       }).error(function (data) {
           $scope.message = data.Message;

           $scope.msgType = "label-danger";
       });
    }).error(function (data) {
        $scope.message = data.Message;

        $scope.msgType = "label-danger";
    });

    $scope.gridFreezedOptions = {
        data: 'myData',
        enableRowSelection: false,
        enableCellEditOnFocus: false,
        checkboxHeaderTemplate: '<input class="ngSelectionHeader" type="checkbox" ng-model="allSelected" ng-change="toggleSelectAll(allSelected)" ng-click="selectAll(allSelected)"/>',
        showSelectionCheckbox: true,
        checkboxCellTemplate: "<div class=\"ngSelectionCell\"><input style=\"display:inline\" tabindex=\"-1\" class=\"ngSelectionCheckbox\" type=\"checkbox\" ng-checked=\"row.selected\" ng-click=\"checkedIndex(row)\"/></div>",
        multiSelect: true,
        plugins: [new ngGridCsvExportPlugin(null,$http)],
        showFooter: true,
        filterOptions: $scope.filterOptions,
        columnDefs: [

        { field: "EC", width: "*", displayName: 'Employee Code', enableCellEdit: false },
        { field: "EName", width: "*", displayName: 'Name', enableCellEdit: false },
        { field: "App1", width: "*", displayName: 'Appraiser 1', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"Appr1"' },
        { field: "App2", width: "*", displayName: 'Appraiser 2', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"Appr2"' },
        { field: "Rv1", width: "*", displayName: 'Reviewer 1', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"Revr1"' },
        { field: "RV2", width: "*", displayName: 'Reviewer 2', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"Revr2"' },
        { field: "GHID", width: "*", displayName: 'Group Head', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"GroupHeadID"' },
        { field: "IDF", width: "*", displayName: 'IDF', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"IDFId"' },
        { field: "IDFE1", width: "*", displayName: 'IDF  Escalation 1', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"IDFEsc1"' },
        { field: "IDFE2", width: "*", displayName: 'IDF  Escalation 2', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"IDFEsc2"' },
        { field: "RPoolName", width: "*", displayName: 'Resource Pool', enableCellEdit: false },
        { field: "DUName", width: "*", displayName: 'Delivery Unit', enableCellEdit: false }]
    };

    var saveEmployeeList = [];
    $scope.updateEntity = function (row) {
        if (saveEmployeeList.indexOf(row.entity) === -1) {
            row.entity.checker = true;
            saveEmployeeList.push(row.entity);
        }
        else {
            saveEmployeeList.splice(saveEmployeeList.indexOf(row.entity), 1);
            row.entity.checker = true;
            saveEmployeeList.push(row.entity);
        }
    };

    $scope.checkedIndex = function (row) {
        if (saveEmployeeList.indexOf(row.entity) === -1) {
            saveEmployeeList.push(row.entity);
        }
        else {
            saveEmployeeList.splice(saveEmployeeList.indexOf(row.entity), 1);
        }
    }

    $scope.reInitiate = function () {
        if (saveEmployeeList.length > 0) {
            $scope.myDiv = true;
            AppraislFactory.reInitiateGrid(saveEmployeeList)
                  .success(function (data, status, headers) {
                      if (data == '1') {
                          alert("Data is Reinitiated");
                          window.location.reload();
                      }
                  }).error(function (data) {
                      $scope.myDiv = false;
                      $scope.message = data.Message;

                      $scope.msgType = "label-danger";
                  });
        }
        else {
            alert("Select atleast one record!")
        }
    }

    $scope.cancelFreezeGrid = function () {
        if (saveEmployeeList.length > 0) {
            $scope.myDiv = true;
            AppraislFactory.cancelGridInitiation(saveEmployeeList)
                  .success(function (data, status, headers) {
                      if (data == '1') {
                          alert("Data is back in Appraisal Initiation");
                          window.location.reload();
                      }
                  }).error(function (data) {
                      $scope.myDiv = false;
                      $scope.message = data.Message;

                      $scope.msgType = "label-danger";
                  });
        }
        else {
            alert("Select atleast one record!")
        }
    }

    $scope.selectAll = function (rowCheck) {
        angular.forEach($scope.gridFreezedOptions.$gridScope.renderedRows, function (elem) {
            if (rowCheck == false) {
                saveEmployeeList.splice(saveEmployeeList.indexOf(elem.entity), 1);
            }
            else if (saveEmployeeList.indexOf(elem.entity) === -1) {
                saveEmployeeList.push(elem.entity);
            }
        })
    }

    $scope.filterName = function () {
        var filterText = 'EName:' + $scope.nameFilter;
        if (filterText !== 'name:') {
            $scope.filterOptions.filterText = filterText;
        } else {
            $scope.filterOptions.filterText = '';
        }
    };

    $scope.filterRpool = function () {
        var filterText = 'RPoolName:' + $scope.poolFilter;
        if (filterText !== 'RPoolName:') {
            $scope.filterOptions.filterText = filterText;
        }
        else {
            $scope.filterOptions.filterText = '';
        }
    };
    $scope.filterDU = function () {
        var filterText = 'DUName:' + $scope.DUFilter;
        if (filterText !== 'DUName:') {
            $scope.filterOptions.filterText = filterText;
        } else {
            $scope.filterOptions.filterText = '';
        }
    };
}])

.directive('ngBlur', function () {
    return function (scope, elem, attrs) {
        elem.bind('blur', function () {
            scope.$apply(attrs.ngBlur);
        });
    };
})

.filter('mapStatus', function (AppraislFactory) {
    var dataSaveed = [];
    AppraislFactory.getDropDownData()
         .success(function (data, status, headers) {
             dataSaveed = data;
         }).error(function (data) {
             $scope.message = data.Message;

             $scope.msgType = "label-danger";
         });
    return function (input, option) {
        if (input != null) {
            var totalEmployee = typeof (dataSaveed[option]) == "object" ? dataSaveed[option] : dataSaveed[dataSaveed[option]];
            if (totalEmployee) {
                var emplyeeMatched = totalEmployee.filter(function (emp) {
                    if (input.Value == undefined) {
                        return emp.Value == input;
                    }
                    else {
                        return emp.Value == input.Value;
                    }
                });

                if (emplyeeMatched.length == 1)
                    return emplyeeMatched[0].Text;
                else
                    return ''
            }
        }
        else
            return '';
    };
})

 .factory('AppraislFactory', ['$http', function ($http) {
     var gridData = function () {
         return $http({
             method: 'GET',
             url: '/api/AppraisalReview/Freezed',
         })
     }

     var doRequest = function () {
         return $http({
             method: 'GET',
             url: '/api/AppraisalReview/GetSetupList',
         })
     }

     var saveRequest = function (saveList) {
         //debugger;
         return $http({
             method: 'POST',
             url: '/api/AppraisalReview/SaveAppraisalList',
             data: saveList,
             dataType: "json"
         });
     }

     var cancelRequest = function (cancelList) {
         return $http({
             method: 'POST',
             url: '/api/AppraisalReview/CancelInitiate',
             data: cancelList,
             dataType: "json"
         });
     }
     var reInitiateRequest = function (cancelList) {
         return $http({
             method: 'POST',
             url: '/api/AppraisalReview/Unfreeze',
             data: cancelList,
             dataType: "json"
         });
     }
     return {
         getDropDownData: function () { return doRequest() },
         getGridData: function () { return gridData() },
         SaveGridData: function (saveList) { return saveRequest(saveList); },
         reInitiateGrid: function (cancelList) { return reInitiateRequest(cancelList); },
         cancelGridInitiation: function (cancelList) { return cancelRequest(cancelList); }
     }
 }])