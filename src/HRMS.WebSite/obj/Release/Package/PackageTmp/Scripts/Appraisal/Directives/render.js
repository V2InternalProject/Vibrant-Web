var app = angular.module("ApprisalSPA");

app.directive('render', ['$compile', 'Templates', function ($compile, Templates) {
    return {
        restrict: 'A',
        scope: {
            index: '=',
            value: '=',
            template: "=",
            required: '=',
            model: '=mymodel'
        },
        link: function (scope, element, attr) {
            console.log(scope.required);
            if (scope.template != undefined) {
                var template = Templates[scope.template];
                console.log(scope.template);
                if (scope.template == "label")
                    template = (template != undefined ? template : Templates["text"]).format("model.{0}".format(scope.value))
                else
                    template = (template != undefined ? template : Templates["text"]).format("model.{0}".format(scope.value), scope.required ? "required" : "", scope.value + '[' + scope.index + ']')
                element.append(angular.element(template));
                $compile(element)(scope);
            }
        },
        replace: true,
        priority: 400
    }
}
]);