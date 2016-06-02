angular.module('MyTest').controller('sectionController', ['$scope', 'SectionService', function ($scope, SectionService) {
    
    $scope.types = [{ ID: "1", Name: "Table" }, { ID: "2", Name: "List" }];

    $scope.section = SectionService.Get();
    //SectionService.Get().then(function (response) {
    //    $scope.section = response.data;
    //});

    $scope.submit = function () {
        if ($scope.sectionForm.$valid) {
            var sectionType = $scope.selectedOption;
            var sectionName = $scope.sectionName;
            alert(sectionType + sectionName);
        }
        else {
            $scope.submitted = true;
        }
    };
    
}]).controller('questionController', ['$scope', 'QuestionService', function ($scope, QuestionService) {

    $scope.types = [{ ID: "1", Name: "Raitting" }, { ID: "2", Name: "Description" }];

    $scope.question = QuestionService.Get();
    //QuestionService.Get().then(function (response) {
    //    $scope.question = response.data;
    //});

    $scope.submit = function () {
        if ($scope.questionForm.$valid) {
        }
        else {
            $scope.submitted = true;
        }
    };

}]).controller('yearController', ['$scope', 'YearSectionService', function ($scope, YearSectionService) {

    $scope.submit = function () {
    };

    $scope.years = [{ Id: "1", Name: "2013-2014" }, { Id: "2", Name: "2014-2015" }];

    $scope.changeYear = function () {
        $scope.sectionList = YearSectionService.GetSection();
        //YearSectionService.GetSection().then(function (response) {
        //    $scope.sectionList = response.data;
        //});
        var count = $scope.sectionList.length;
        $scope.orderList = [];
        var id =0;
        for (var i = 0; i < count; i++) {
            id = i + 1;
            $scope.orderList.push({
                Id: id,
                Name: id
            });
        }
        $scope.sectionDD = true;
        $scope.sectionsList = YearSectionService.Get();
        //YearSectionService.Get().then(function (response) {
        //    $scope.sectionsList = response.data;
        //});
    };

    $scope.changeSetion = function () {
        $scope.orderDD = true;
    };
    
    $scope.changeOrder = function () {
        $scope.button = true;
    };

}]).controller('mapingController', ['$scope', function ($scope) {

    $scope.yearList = [{ Id: "1", Name: "2013-2014" }, { Id: "2", Name: "2014-2015" }];

    $scope.changeYear = function () {
        $scope.sectionList = [{ Id: "1", Name: "FirstSection" }, { Id: "2", Name: "SecondSection" }];
        $scope.sectionDD = true;
    };

    $scope.changeSetion = function () {
        $scope.questionList = [{ Id: "1", Name: "FirstQuestion" }, { Id: "2", Name: "SecondQuestion" }];
        var count = $scope.questionList.length;
        $scope.orderList = [];
        var id = 0;
        for (var i = 0; i < count; i++) {
            id = i + 1;
            $scope.orderList.push({
                Id: id,
                Name: id
            });
        }
        $scope.QuestionDD = true;
    };

    $scope.changeQuestion = function () {
        $scope.orderDD = true;
    };

    $scope.changeOrder = function () {
        $scope.button = true;
    };

    $scope.question = [{ Id: "1", Name: "First Question", IsSection: false }, { Id: "2", Name: "Second Question", IsSection: false }];

}]);