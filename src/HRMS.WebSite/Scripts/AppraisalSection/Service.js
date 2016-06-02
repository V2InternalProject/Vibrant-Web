angular.module('Appraisal', ['ui.bootstrap']).factory('SectionService', ['$http', function ($http) {
    return {
        Get: function () {
            return $http.get('/api/AppraisalAdmin/GetAllSectionList/');
        },
        Add: function (sections) {
            return $http.post('/api/AppraisalAdmin/SaveUpdateSections/', sections);
        },
        Update: function (sections) {
            return $http.post('/api/AppraisalAdmin/SaveUpdateSections/', sections);
        }
    };
}]).factory('QuestionService', ['$http', function ($http) {
    return {
        Get: function () {
            return $http.get('/api/AppraisalAdmin/GetAllQuestionList/');
        },
        Add: function (questions) {
            return $http.post('/api/AppraisalAdmin/SaveUpdateQuestions/', questions);
        },
        Update: function (questions) {
            return $http.post('/api/AppraisalAdmin/SaveUpdateQuestions/', questions);
        }
    };
}]).factory('YearSectionService', ['$http', function ($http) {
    return {
        Get: function () {
            return $http.get('/api/AppraisalAdmin/GetAllYear/');
        },
        GetList: function (ID) {
            return $http.get('/api/AppraisalAdmin/GetYearSectionList/for/' + ID);
        },
        GetSection: function () {
            return $http.get('/api/AppraisalAdmin/GetAllSectionList/');
        },
        Add: function (data) {
            return $http.post('/api/AppraisalAdmin/SaveYearSectionMapping/', data);
        },
        Update: function (data) {
            return $http.post('/api/AppraisalAdmin/UpdateSectionOrder/', data);
        },
        UpdateMapping: function (data) {
            return $http.post('/api/AppraisalAdmin/UpdateSectionMapping/', data);
        }
    };
}]).factory('YearService', ['$http', function ($http) {
    return {
        Get: function () {
            return $http.get('/api/AppraisalAdmin/GetAllYear/');
        },
        GetList: function (year, section) {
            return $http.get('/api/AppraisalAdmin/GetYearQuestionList/' + year + '/' + section);
        },
        GetSection: function (yearId) {
            return $http.get('/api/AppraisalAdmin/GetAllSectionOfYear/for/' + yearId);
        },
        GetMappingId: function (year, section) {
            return $http.get('/api/AppraisalAdmin/GetMappingId/' + year + '/' + section);
        },
        GetQuestion: function () {
            return $http.get('/api/AppraisalAdmin/GetAllQuestionList/');
        },
        Add: function (data) {
            return $http.post('/api/AppraisalAdmin/SaveMapping/', data);
        },
        Update: function (data) {
            return $http.post('/api/AppraisalAdmin/UpdateQuestionOrder/', data);
        },
        UpdateMapping: function (data) {
            return $http.post('/api/AppraisalAdmin/UpdateQuestionMapping/', data);
        }
    };
}]);