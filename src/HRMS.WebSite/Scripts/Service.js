angular.module('MyTest', ['ui.bootstrap', 'ngGrid']).factory('SectionService', ['$http', function ($http) {
    return {
        Get: function () {
            var a = [{ Id: "1", Name: "First", Type: "1" }, { Id: "2", Name: "Second", Type: "2" }];
            return a;
            //return $http.get('/api/Module/Get/');
        },
        Add: function (data) {
            return $http.post('/api/Module/Create/', data);
        },
        Update: function (data) {
            //debugger;
            return $http.post('/api/Module/Update/', data);
        },
        Delete: function (data) {
            //debugger;
            return $http.post('/api/Module/Delete/', data);
        }
    };
}]).factory('QuestionService', ['$http', function ($http) {
    return {
        Get: function () {
            var a = [{ Id: "1", Name: "FirstQuestion" }, { Id: "2", Name: "SecondQuestion" }];
            return a;
            //return $http.get('/api/Module/Get/');
        },
        Add: function (data) {
            return $http.post('/api/Module/Create/', data);
        },
        Update: function (data) {
            //debugger;
            return $http.post('/api/Module/Update/', data);
        },
        Delete: function (data) {
            //debugger;
            return $http.post('/api/Module/Delete/', data);
        }
    };
}]).factory('YearSectionService', ['$http', function ($http) {
    return {
        Get: function () {
            var a = [{ year: "2013-2014", Id: "1", Name: "FirstSection", Order: "2" }, { year: "2013-2014", Id: "2", Name: "SecondSection", Order: "1" }];
            return a;
            //return $http.get('/api/Module/Get/');
        },
        GetSection: function () {
            var a = [{ Id: "1", Name: "FirstSection" }, { Id: "2", Name: "SecondSection" }];
            return a;
            //return $http.get('/api/Module/Get/');
        },
        Add: function (data) {
            return $http.post('/api/Module/Create/', data);
        },
        Update: function (data) {
            //debugger;
            return $http.post('/api/Module/Update/', data);
        },
        Delete: function (data) {
            //debugger;
            return $http.post('/api/Module/Delete/', data);
        }
    };
}]).factory('EmployeeService', ['$http', function ($http) {
    return {
        Get: function () {
            return $http.get('/AppraisalAdmin/GetList');
        },
        GetSection: function () {
            var a = [{ Id: "1", Name: "FirstSection" }, { Id: "2", Name: "SecondSection" }];
            return a;
            //return $http.get('/api/Module/Get/');
        },
        Add: function (data) {
            return $http.post('/api/Module/Create/', data);
        },
        Update: function (data) {
            //debugger;
            return $http.post('/api/Module/Update/', data);
        },
        Delete: function (data) {
            //debugger;
            return $http.post('/api/Module/Delete/', data);
        }
    };
}]);