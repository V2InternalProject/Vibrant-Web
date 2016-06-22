var app = angular.module("ApprisalSPA");

app.directive('menu', ['$compile', function ($compile) {
    return {
        restrict: 'E',
        scope: {
            model: '@mymodel'
        },
        compile: function (element, attr) {
            return function (scope, element, attr) {
                var template = "<a href='{0}'>{1}</a>";
                var menus = "<div>{0}</div>";
                var menuItems = "";
                console.log("---");
                console.log(scope.model);
                console.log("---");
                for (var i = 0; i < scope.model.length; i++)
                    menuItems+=template.format("{{model[" + i + "].URL}}", "{{model[" + i + "].Text}}");

                var el = $compile(menus.format(menuItems))(scope);
                element.replaceWith(el);
            }
        },
        transclude: "element",
        priority: 400
    }
}
]);