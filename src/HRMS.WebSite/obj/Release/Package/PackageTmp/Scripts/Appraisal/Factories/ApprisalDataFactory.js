var app = angular.module("ApprisalSPA");

app.factory("ApprisalDataFactory", ['$http','ControlTemplate', function ($http,ct) {
    return {
            getContentForStep: function (url) {
                return ct[url];
            },
            getFrom: function (url) {
              return   $http.get(url)
            }

        //return new Process();
    }
}])