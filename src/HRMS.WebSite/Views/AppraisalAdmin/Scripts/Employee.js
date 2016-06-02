angular.module('MyTest').controller('Controllers', ['$scope', '$http', '$modal', 'EmployeeService', function ($scope, $http, $modal, EmployeeService) {

    //$scope.Appraiser = [{ Id: "1", Name: "Mukesh1" }, { Id: "2", Name: "Mukesh2" }, { Id: "3", Name: "Mukesh3"}];
    EmployeeService.Get().then(function (response) {
        debugger;
        $scope.Appraiser = response.data;
    });
    $scope.Reviewer = [{ Id: "1", Name: "Rakesh1" }, { Id: "2", Name: "Rakesh2" }, { Id: "3", Name: "Rakesh3"}];

    $scope.filterOptions = {
        filterText: "",
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [5, 10, 20],
        pageSize: 5,
        currentPage: 1
    };
    $scope.setPagingData = function (data, page, pageSize) {
        var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
        $scope.myData = pagedData;
        $scope.totalServerItems = data.length;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    $scope.getPagedDataAsync = function (pageSize, page, searchText) {
        setTimeout(function () {
            var data;
            if (searchText) {
                var ft = searchText.toLowerCase();
                $http.get('/AppraisalAdmin/GetList').success(function (largeLoad) {
                    data = largeLoad.filter(function (item) {
                        return JSON.stringify(item).toLowerCase().indexOf(ft) != -1;
                    });
                    $scope.setPagingData(data, page, pageSize);
                });
            } else {
                $http.get('/AppraisalAdmin/GetList').success(function (largeLoad) {
                    $scope.setPagingData(largeLoad, page, pageSize);
                });
            }
        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

    $scope.$watch('pagingOptions', function () {
        console.log("watch changed pagingOptions");
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
    }, true);
    $scope.$watch('filterOptions', function () {
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
    }, true);

    $scope.gridOptions = {
        data: 'myData',
        enablePaging: true,
        showFooter: true,
        selectWithCheckboxOnly: true,
        mltiSelect: false,

        columnDefs: [{ field: 'EmployeeId', displayName: 'Employee Name', cellTemplate: '<div class="ngCellText" ng-class="col.colIndex()"><a href="" ng-click="cellclick(row.getProperty(col.field))" ng-bind="row.getProperty(\'EmployeeName\')"></a></div>' },
                   ],

        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.selectedData = [];

    $scope.gridOptions.primaryKey = "EmployeeId";
    $scope.gridOptions.showSelectionCheckbox = true;
    $scope.gridOptions.multiSelect = true;
    $scope.gridOptions.selectedItems = $scope.selectedData;

    $scope.cellclick = function (Id) {
        debugger;
        $scope.items = { empId: Id, EmpName: "asdfghjkl", empDetaile: "qwertyuiop" };
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                items: function () {
                    return $scope.items;
                }
            }
        });
        modalInstance.result.then(function () {
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    };

} ]).controller('ModalInstanceCtrl', ['$scope', '$modalInstance', 'items', function ($scope, $modalInstance, items) {

    var items = items;
    $scope.i = items;

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
} ]);