angular.module('MyTest', []).factory('testService', ['$http', function ($http) {
    return {
        Get: function () {
            debugger;
            return $http.get('/api/test/show' + 1);
        }
    };
} ]).controller('Controllers', ['testService', '$scope', function (testService, $scope) {

    testService.Get().then(function (response) {
        debugger;
        $scope.test = response;
    });

} ]);