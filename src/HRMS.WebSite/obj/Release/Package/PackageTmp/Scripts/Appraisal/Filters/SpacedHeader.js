var app = angular.module("ApprisalSPA");

app.filter('SpacedHeader', ['$filter', function (filter) {
    return function (value) {
        return value.split('_').join(' ');
    };
}]);

angular.module('ApprisalSPA').filter('texttodate', function () {
    return function (input) {
        if (/:00Z/.test(input))
        {
            var x = new Date(input);
            return x.getMonth()+"/"+x.getDate()+"/"+x.getFullYear();
        }
        else
            return input;
    };
});

angular.module('ApprisalSPA')
    .filter('objOrder', function () {
        return function (object) {
            var array = [];
            angular.forEach(object, function (value, key) {
                array.push({ key: key, value: value });
            });
            return array;
        };
    });