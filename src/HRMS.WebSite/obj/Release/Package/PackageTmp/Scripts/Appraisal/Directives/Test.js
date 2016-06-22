var app = angular.module("ApprisalSPA");

app.directive('control', ['$compile', 'ControlTemplate', 'Templates', function ($compile, ControlTemplate, Templates) {
    return {
        restrict: 'A',
        scope: {
            value: "=",
            model: '=mymodel'
        },
        template: function (tElement, tAttrs) {
            console.log(tAttrs);
            return "<input type='text' ng-model='model." + tAttrs["value"] + "' />";
        },
        compile: function (element, attr) {
            return function (scope, element, attr) {
                //element.parent().html("<div><input type='text' ng-model='model.Name'/><div>");
                // $compile(element.contents())(scope);
            }
        }
    }
}

]);

app.directive('control1', ['ControlTemplate', 'Templates', function (ControlTemplate, Templates) {
    return {
        restrict: 'A',
        template: "<div><input type='text' ng-model='model.Name'/><div>",
        scope: {
            value: "=",
            model: '=mymodel'
        }
    }
}

]);