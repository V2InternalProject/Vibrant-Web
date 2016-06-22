angular.module('Appraisal').controller('sectionController', ['$scope', 'SectionService', '$filter', function ($scope, SectionService, $filter) {
    $scope.types = [{ ID: 1, Name: "V" }, { ID: 2, Name: "FR" }, { ID: 3, Name: "FC" }, { ID: 4, Name: "VC" }];

    $scope.tabClick = function () {
        $scope.sectionName = "";
        $scope.selectedOption = 1;
        $scope.submitted = false;
    };

    SectionService.Get().then(function (response) {
        $scope.section = response.data.Data;
    });

    $scope.cancle = function (Id, index) {
        var i = index;
        SectionService.Get().then(function (response) {
            $scope.newSection = response.data.Data;
            $scope.section[i] = $scope.newSection[i];
        });
    };

    $scope.submit = function () {
        if ($scope.sectionForm.$valid) {
            $scope.myDiv = true;
            var sectionType = $scope.selectedOption;
            var sectionName = $scope.sectionName;
            var sections = { 'sectionId': null, 'sectionName': sectionName, 'sectionType': sectionType };
            SectionService.Add(sections).then(function (response) {
                var msg = response.data.Message;
                $scope.sectionName = "";
                $scope.selectedOption = 1;
                $scope.submitted = false;
                SectionService.Get().then(function (response) {
                    $scope.section = response.data.Data;
                });
                $scope.myDiv = false;
                alert(msg);
            });
        }
        else {
            $scope.submitted = true;
        }
    };

    $scope.update = function (ID, data, index) {
        $scope.myDiv = true;
        var Id = ID;
        var i = index;
        var sectionType = data.sectionType;
        var sectionName = data.sectionName;
        var sections = { 'sectionId': Id, 'sectionName': sectionName, 'sectionType': sectionType };
        SectionService.Update(sections).then(function (response) {
            var msg = response.data.Message;
            SectionService.Get().then(function (response) {
                $scope.newSection = response.data.Data;
                $scope.section[i] = $scope.newSection[i];
            });
            $scope.myDiv = false;
            alert(msg);
        });
    };

    $scope.getObjectWithId = function (id) {
        var found = $filter('getById')($scope.types, id);
        $scope.selected = JSON.stringify(found);
    }
}]).controller('questionController', ['$scope', 'QuestionService', function ($scope, QuestionService) {
    $scope.questionDataTypes = ['Text', 'Date', 'Integer', 'Boolean'];
    $scope.questionControlTypes = ['Autonum', 'Agree','Text', 'Label', 'Rating', 'Date'];
    $scope.visibleToStageList = [{ "id": 1, Name: "Appraisee" }, { "id": 2, Name: "Appraiser 1" }, { "id": 3, Name: "Appraiser 2" }, { "id": 4, Name: "Reviewer 1" }, { "id": 5, Name: "Reviewer 2" }, { "id": 6, Name: "Group Head" }, { "id": 7, Name: "IDF" }, { "id": 8, Name: "IDF Escalation 1" }, { "id": 9, Name: "IDF Escalation 2" }];

    $scope.tabClick = function () {
        $scope.questionName = "";
        $scope.yourSelect = "Text";
        $scope.questionParam = "";
        $scope.questionAbbr = "";
        $scope.editStageId = "";
        $scope.yourSelect1 = "Text";
        $scope.validation = "";
        $scope.submitted = false;
    };

    QuestionService.Get().then(function (response) {
        $scope.question = response.data.Data;
    });

    $scope.cancle = function (Id, index) {
        var i = index;
        QuestionService.Get().then(function (response) {
            $scope.newQuestion = response.data.Data;
            $scope.question[i] = $scope.newQuestion[i];
        });
    };

    $scope.submit = function () {
        if ($scope.questionForm.$valid) {
            $scope.myDiv = true;
            //debugger;
            var questionName = $scope.questionName;
            var datatype = $scope.yourSelect;
            var questionParam = $scope.questionParam;
            var questionAbbr = $scope.questionAbbr;
            var controltype = $scope.yourSelect1;
            var validation = $scope.validation;
            var editStageId = $scope.editStageId;

            var questions = { 'questionId': null, 'questionText': questionName, 'dataType': datatype, 'questionParam': questionParam, 'questionAbbr': questionAbbr, 'controlType': controltype, 'validation': validation, 'editStageId': editStageId };
            QuestionService.Add(questions).then(function (response) {
                var msg = response.data.Message;
                $scope.questionName = "";
                $scope.yourSelect = "Text";
                $scope.questionParam = "";
                $scope.questionAbbr = "";
                $scope.editStageId = "";
                $scope.yourSelect1 = "Text";
                $scope.validation = "";
                $scope.submitted = false;
                QuestionService.Get().then(function (response) {
                    $scope.question = response.data.Data;
                });
                $scope.myDiv = false;
                alert(msg);
            });
        }
        else {
            $scope.submitted = true;
        }
    };

    $scope.update = function (ID, data, index) {
        $scope.myDiv = true;
        var Id = ID;
        var i = index;
        var questionName = data.questionText;
        var datatype = data.dataType;
        var questionParam = data.questionParam;
        var questionAbbr = data.questionAbbr;
        var controltype = data.controlType;
        var validation = data.validation;
        var editStageId = data.editStageId;
        var questions = { 'questionId': Id, 'questionText': questionName, 'dataType': datatype, 'questionParam': questionParam, 'questionAbbr': questionAbbr, 'controlType': controltype, 'validation': validation, 'editStageId': editStageId };
        QuestionService.Update(questions).then(function (response) {
            var msg = response.data.Message;
            QuestionService.Get().then(function (response) {
                $scope.newQuestion = response.data.Data;
                $scope.question[i] = $scope.newQuestion[i];
            });
            $scope.myDiv = false;
            alert(msg);
        });
    };

    $scope.getObjectWithId = function (array, id) {
        var countt1 = array.length;
        var match = null;
        for (var n = 0; n < countt1; n++) {
            var ii = array[n].id;
            if (id === ii) {
                match = array[n].Name;
            }
        }
        return match;
    };
}]).controller('yearController', ['$scope', 'YearSectionService', '$filter', function ($scope, YearSectionService, $filter) {
    var ID = null;

    $scope.visibleToStageList = [{ "id": 1, Name: "Appraisee" }, { "id": 2, Name: "Appraiser 1" }, { "id": 3, Name: "Appraiser 2" }, { "id": 4, Name: "Reviewer 1" }, { "id": 5, Name: "Reviewer 2" }, { "id": 6, Name: "Group Head" }, { "id": 7, Name: "IDF" }, { "id": 8, Name: "IDF Escalation 1" }, { "id": 9, Name: "IDF Escalation 2" }];
    $scope.member = { visibleToStageList: [] };
    $scope.selected_items = [];

    $scope.tabClick = function () {
        $scope.selectedYear = "";
        $scope.selectedSection = "";
        $scope.selectedOrder = "";
        $scope.visibleToStage = "";
        $scope.selected_items = [];
        $scope.sectionDD = false;
        $scope.orderDD = false;
        $scope.submitted = false;
        $scope.submittedSelect = false;
    };

    $scope.RequiredList = [{ ID: 0, Name: "Yes" }, { ID: 1, Name: "No" }]

    YearSectionService.GetList(ID).then(function (response) {
        $scope.yearSectionList = response.data.Data;
        $scope.yearSectionListCopy = angular.copy($scope.yearSectionList);
    });
    var yearSectionListCount = null;
    YearSectionService.Get().then(function (response) {
        $scope.years = response.data.Data;
    });

    $scope.changeYear = function () {
        ID = $scope.selectedYear;
        YearSectionService.GetList(ID).then(function (response) {
            $scope.yearSectionList = response.data.Data;
            YearSectionService.GetSection().then(function (response) {
                $scope.sectionList = response.data.Data;
                $scope.sectionListCopy = angular.copy($scope.sectionList);
                var count = $scope.yearSectionList.length;
                yearSectionListCount = count;
                for (var i = 0; i < count; i++) {
                    var name = $scope.yearSectionList[i].SectionName;
                    var count1 = $scope.sectionList.length;
                    for (var k = 0; k < count1; k++) {
                        var name1 = $scope.sectionList[k].sectionName;
                        if (name === name1) {
                            $scope.sectionList.splice(k, 1);
                            k--;
                            count1--;
                        }
                    }
                }
                var a = $scope.sectionList.length;
                if (a === 0) {
                    alert("There are no more section to mapping..");
                    $scope.selectedYear = "";
                    $scope.selectedSection = "";
                    $scope.selectedOrder = "";
                    $scope.visibleToStage = "";
                    $scope.selected_items = [];
                }
            });
        });

        $scope.forOrder = true;
    };

    $scope.changeSetion = function () {
        $scope.button = true;
    };

    $scope.submit = function () {
        if ($scope.submityearSectionform.$valid) {
            var mapping = "S"
            var yearId = $scope.selectedYear;
            var sectionId = $scope.selectedSection;
            var isRequired = $scope.IsRequired;
            var visibleToStage = null;
            var visibleToStage1 = null;
            var visible = $scope.selected_items.length;
            if (visible === 0) {
                $scope.submittedSelect = true;
            }
            else {
                for (var b = 0; b < visible; b++) {
                    if (visibleToStage === null) {
                        visibleToStage = $scope.selected_items[b];
                    }
                    else {
                        visibleToStage = visibleToStage + ',' + $scope.selected_items[b];
                    }
                }
                var visibleToStage1 = visibleToStage;
                $scope.myDiv = true;
                $scope.submittedSelect = false;
                var order = yearSectionListCount + 1;
                var data = { 'MappingType': mapping, 'YearID': yearId, 'SectionId': sectionId, 'Order': order, 'IsRequired': isRequired, 'Stages': visibleToStage1 };
                YearSectionService.Add(data).then(function (response) {
                    var msg = response.data.Message;
                    $scope.selectedYear = "";
                    $scope.selectedSection = "";
                    $scope.selectedOrder = "";
                    $scope.visibleToStage = "";
                    $scope.selected_items = [];
                    ID = null;
                    $scope.submitted = false;
                    $scope.submittedSelect = false;
                    YearSectionService.GetList(ID).then(function (response) {
                        $scope.yearSectionList = response.data.Data;
                    });
                    $scope.myDiv = false;
                    alert(msg);
                });
            }
        }
        else {
            $scope.submitted = true;
        }
    };

    $scope.update = function () {
        $scope.myDiv = true;
        var data = $scope.yearSectionList;
        YearSectionService.Update(data).then(function (response) {
            $scope.selectedYear = "";
            $scope.selectedSection = "";
            $scope.forOrder = false;
            ID = null;
            YearSectionService.GetList(ID).then(function (response) {
                $scope.yearSectionList = response.data.Data;
            });
            $scope.myDiv = false;
        });
    };

    $scope.upOrder = function (data) {
        var index;
        var index1;
        $scope.Temp = {};
        $scope.Temp1 = {};
        var Order = data.Order;
        var Order1 = Order - 1;
        var count = $scope.yearSectionList.length;
        for (var a = 0 ; a < count; a++) {
            var orders = $scope.yearSectionList[a].Order;
            if (orders === Order) {
                $scope.Temp = $scope.yearSectionList[a];
                index = a;
                $scope.Temp.Order = Order1;
            }
            else if (orders === Order1) {
                $scope.Temp1 = $scope.yearSectionList[a];
                index1 = a;
                $scope.Temp1.Order = Order;
            }
        }
        $scope.yearSectionList[index] = $scope.Temp;
        $scope.yearSectionList[index1] = $scope.Temp1;
        $scope.forUpdate = true;
    };

    $scope.DownOrder = function (data) {
        var index;
        var index1;
        $scope.Temp = {};
        $scope.Temp1 = {};
        var Order = data.Order;
        var Order1 = Order + 1;
        var count = $scope.yearSectionList.length;
        for (var a = 0 ; a < count; a++) {
            var orders = $scope.yearSectionList[a].Order;
            if (orders === Order) {
                $scope.Temp = $scope.yearSectionList[a];
                index = a;
                $scope.Temp.Order = Order1;
            }
            else if (orders === Order1) {
                $scope.Temp1 = $scope.yearSectionList[a];
                index1 = a;
                $scope.Temp1.Order = Order;
            }
        }
        $scope.yearSectionList[index] = $scope.Temp;
        $scope.yearSectionList[index1] = $scope.Temp1;
        $scope.forUpdate = true;
    };

    $scope.cancleOrder = function () {
        $scope.yearSectionList = angular.copy($scope.yearSectionListCopy);
        $scope.forUpdate = false;
    };

    $scope.updateData = function (data, select, index) {
        var l = select.length;
        var visibleToStage1 = null;
        if (l === 0) {
            var mappingId = data.MappingId;
            var isRequired = data.isRequired;
            visibleToStage1 = data.Stages;
            var data = { 'MappingId': mappingId, 'IsRequired': isRequired, 'Stages': visibleToStage1 };
            YearSectionService.UpdateMapping(data).then(function (response) {
                $scope.selectedYear = "";
                $scope.selectedSection = "";
                $scope.selected_items = [];
                $scope.forOrder = false;
                ID = null;
                YearSectionService.GetList(ID).then(function (response) {
                    $scope.yearSectionList = response.data.Data;
                });
            });
        }
        else {
            for (var b = 0; b < l; b++) {
                if (visibleToStage1 === null) {
                    visibleToStage1 = select[b];
                }
                else {
                    visibleToStage1 = visibleToStage1 + ',' + select[b];
                }
            }
            var mappingId = data.MappingId;
            var isRequired = data.isRequired;
            var data = { 'MappingId': mappingId, 'IsRequired': isRequired, 'Stages': visibleToStage1 };
            YearSectionService.UpdateMapping(data).then(function (response) {
                $scope.selectedYear = "";
                $scope.selectedSection = "";
                $scope.selected_items = [];
                $scope.forOrder = false;
                ID = null;
                YearSectionService.GetList(ID).then(function (response) {
                    $scope.yearSectionList = response.data.Data;
                });
            });
        }
    };

    $scope.cancle = function (index) {
        var i = index;
        ID = $scope.selectedYear;
        YearSectionService.GetList(ID).then(function (response) {
            $scope.newyearSectionList = response.data.Data;
            $scope.yearSectionList[i] = $scope.newyearSectionList[i];
        });
    };

    $scope.getObjectWithIds = function (id) {
        var found = $filter('getById')($scope.RequiredList, id);
        $scope.selected = JSON.stringify(found);
    }

    $scope.getObjectWithId = function (array, id) {
        $scope.tempData = [];
        var match = null;
        $scope.splitArray = id.split(',');
        var countt = $scope.splitArray.length;
        for (var m = 0; m < countt; m++) {
            var ii = parseInt($scope.splitArray[m]);
            var countt1 = array.length;
            for (var n = 0; n < countt1; n++) {
                var id = array[n].id;
                if (id === ii) {
                    $scope.tempData.push(array[n].Name);
                    if (match === null) {
                        match = array[n].Name;
                    }
                    else {
                        match = match + ',' + array[n].Name;
                    }
                }
            }
            var str = match;
        }
        match = null;
        return str;
    };
}]).controller('mapingController', ['$scope', 'YearService', '$filter', function ($scope, YearService, $filter) {
    $scope.RequiredList = [{ ID: 0, Name: "Yes" }, { ID: 1, Name: "No" }]

    $scope.visibleToStageList = [{ "id": 1, Name: "Appraisee" }, { "id": 2, Name: "Appraiser 1" }, { "id": 3, Name: "Appraiser 2" }, { "id": 4, Name: "Reviewer 1" }, { "id": 5, Name: "Reviewer 2" }, { "id": 6, Name: "Group Head" }, { "id": 7, Name: "IDF" }, { "id": 8, Name: "IDF Escalation 1" }, { "id": 9, Name: "IDF Escalation 2" }];
    $scope.member = { visibleToStageList: [] };
    $scope.selected_items = [];

    $scope.tabClick = function () {
        $scope.selectedYear = "";
        $scope.selectedSection = "";
        $scope.selectedQuestion = "";
        $scope.visibleToStage = "";
        $scope.selected_items = [];
        $scope.forOrder = false;
        $scope.submitted = false;
        $scope.submittedSelect = false;
    };

    YearService.Get().then(function (response) {
        $scope.yearList = response.data.Data;
    });

    var ID = null;
    var section1 = null;
    var mappingId = null;
    var QuestionOrder = null;
    YearService.GetList(ID, section1).then(function (response) {
        $scope.AllList = response.data.Data;
        $scope.AllListCopy = angular.copy($scope.AllList);
    });

    $scope.changeYear = function () {
        var yearId = $scope.selectedYear;
        YearService.GetSection(yearId).then(function (response) {
            $scope.sectionList = response.data.Data;
        });

        ID = yearId;
        YearService.GetList(ID, section1).then(function (response) {
            $scope.AllList = response.data.Data;
            $scope.AllListCopy = angular.copy($scope.AllList);
        });
    };

    $scope.changeSetion = function () {
        $scope.selectedQuestion = "";
        var year = $scope.selectedYear;
        var section = $scope.selectedSection;
        YearService.GetMappingId(year, section).then(function (response) {
            $scope.mappingId = response.data.Data;
            mappingId = $scope.mappingId[0].MappingId;
        });

        YearService.GetList(ID, section).then(function (response) {
            $scope.AllList = response.data.Data;
            $scope.AllListCopy = angular.copy($scope.AllList);

            YearService.GetQuestion().then(function (response) {
                $scope.questionList = response.data.Data;
                var count = $scope.AllList.length;
                QuestionOrder = count;
                for (var i = 0; i < count; i++) {
                    var name = $scope.AllList[i].QuestionName;
                    var count1 = $scope.questionList.length;
                    for (var k = 0; k < count1; k++) {
                        var name1 = $scope.questionList[k].questionText;
                        if (name === name1) {
                            $scope.questionList.splice(k, 1);
                            k--;
                            count1--;
                        }
                    }
                }
                var a = $scope.questionList.length;
                if (a === 0) {
                    alert("There are no more question to mapping..");
                    $scope.selectedYear = "";
                    $scope.selectedSection = "";
                    $scope.selectedQuestion = "";
                    $scope.visibleToStage = "";
                    $scope.selected_items = [];
                }
            });
        });
        $scope.forOrder = true;
    };

    $scope.cancleOrder = function () {
        $scope.AllList = angular.copy($scope.AllListCopy);
        $scope.forUpdate = false;
    };

    $scope.cancle = function (index) {
        var i = index;
        ID = $scope.selectedYear;
        var section = $scope.selectedSection;
        YearService.GetList(ID, section).then(function (response) {
            $scope.NewAllList = response.data.Data;
            $scope.AllList[i] = $scope.NewAllList[i];
        });
    };

    $scope.submit = function () {
        if ($scope.submitmappingform.$valid) {
            var mapping = "Q"
            var questionId = $scope.selectedQuestion;
            var order = QuestionOrder + 1;
            var visibleToStage1 = null;
            var isRequired = $scope.IsRequired;
            var visibleToStage = null;
            var visible = $scope.selected_items.length;
            if (visible === 0) {
                $scope.submittedSelect = true;
            }
            else {
                for (var b = 0; b < visible; b++) {
                    if (visibleToStage === null) {
                        visibleToStage = $scope.selected_items[b];
                    }
                    else {
                        visibleToStage = visibleToStage + ',' + $scope.selected_items[b];
                    }
                }
                visibleToStage1 = visibleToStage;
                $scope.myDiv = true;
                var data = { 'MappingType': mapping, 'QuestionId': questionId, 'Order': order, 'MappingId': mappingId, 'IsRequired': isRequired, 'Stages': visibleToStage1 };
                YearService.Add(data).then(function (response) {
                    var msg = response.data.Message;
                    $scope.selectedYear = "";
                    $scope.selectedSection = "";
                    $scope.selectedQuestion = "";
                    $scope.visibleToStage = "";
                    $scope.selected_items = [];
                    $scope.forOrder = false;
                    $scope.submitted = false;
                    $scope.submittedSelect = false;
                    ID = null;
                    YearService.GetList(ID).then(function (response) {
                        $scope.AllList = response.data.Data;
                    });
                    $scope.myDiv = false;
                    alert(msg);
                });
            }
        }
        else {
            $scope.submitted = true;
        }
    };

    $scope.update = function () {
        $scope.myDiv = true;
        var data = $scope.AllList;
        YearService.Update(data).then(function (response) {
            $scope.selectedYear = "";
            $scope.selectedSection = "";
            $scope.selectedQuestion = "";
            $scope.forOrder = false;
            ID = null;
            YearService.GetList(ID).then(function (response) {
                $scope.AllList = response.data.Data;
            });
            $scope.myDiv = false;
        });
    };

    $scope.updateData = function (data, select, index) {
        var l = select.length;
        var visibleToStage1 = null;
        if (l === 0) {
            var mappingId = data.QMappingId;
            var isRequired = data.isRequired;
            visibleToStage1 = data.Stages;
            var data = { 'MappingId': mappingId, 'IsRequired': isRequired, 'Stages': visibleToStage1 };
            YearService.UpdateMapping(data).then(function (response) {
                $scope.selectedYear = "";
                $scope.selectedSection = "";
                $scope.selectedQuestion = "";
                $scope.selected_items = [];
                $scope.forOrder = false;
                ID = null;
                YearService.GetList(ID).then(function (response) {
                    $scope.AllList = response.data.Data;
                });
            });
        }
        else {
            for (var b = 0; b < l; b++) {
                if (visibleToStage1 === null) {
                    visibleToStage1 = select[b];
                }
                else {
                    visibleToStage1 = visibleToStage1 + ',' + select[b];
                }
            }
            var mappingId = data.QMappingId;
            var isRequired = data.isRequired;
            var data = { 'MappingId': mappingId, 'IsRequired': isRequired, 'Stages': visibleToStage1 };
            YearService.UpdateMapping(data).then(function (response) {
                $scope.selectedYear = "";
                $scope.selectedSection = "";
                $scope.selectedQuestion = "";
                $scope.selected_items = [];
                $scope.forOrder = false;
                ID = null;
                YearService.GetList(ID).then(function (response) {
                    $scope.AllList = response.data.Data;
                });
            });
        }
    };

    $scope.upOrder = function (data) {
        var index;
        var index1;
        $scope.Temp = {};
        $scope.Temp1 = {};
        var Order = data.Order;
        var Order1 = Order - 1;
        var count = $scope.AllList.length;
        for (var a = 0 ; a < count; a++) {
            var orders = $scope.AllList[a].Order;
            if (orders === Order) {
                $scope.Temp = $scope.AllList[a];
                index = a;
                $scope.Temp.Order = Order1;
            }
            else if (orders === Order1) {
                $scope.Temp1 = $scope.AllList[a];
                index1 = a;
                $scope.Temp1.Order = Order;
            }
        }
        $scope.AllList[index] = $scope.Temp;
        $scope.AllList[index1] = $scope.Temp1;
        $scope.forUpdate = true;
    };

    $scope.DownOrder = function (data) {
        var index;
        var index1;
        $scope.Temp = {};
        $scope.Temp1 = {};
        var Order = data.Order;
        var Order1 = Order + 1;
        var count = $scope.AllList.length;
        for (var a = 0 ; a < count; a++) {
            var orders = $scope.AllList[a].Order;
            if (orders === Order) {
                $scope.Temp = $scope.AllList[a];
                index = a;
                $scope.Temp.Order = Order1;
            }
            else if (orders === Order1) {
                $scope.Temp1 = $scope.AllList[a];
                index1 = a;
                $scope.Temp1.Order = Order;
            }
        }
        $scope.AllList[index] = $scope.Temp;
        $scope.AllList[index1] = $scope.Temp1;
        $scope.forUpdate = true;
    };

    $scope.getObjectWithId = function (array, id) {
        $scope.tempData = [];
        var match = null;
        $scope.splitArray = id.split(',');
        var countt = $scope.splitArray.length;
        for (var m = 0; m < countt; m++) {
            var ii = parseInt($scope.splitArray[m]);
            var countt1 = array.length;
            for (var n = 0; n < countt1; n++) {
                var id = array[n].id;
                if (id === ii) {
                    $scope.tempData.push(array[n].Name);
                    if (match === null) {
                        match = array[n].Name;
                    }
                    else {
                        match = match + ',' + array[n].Name;
                    }
                }
            }
            var str = match;
        }
        match = null;
        return str;
    };

    $scope.getObjectWithIds = function (id) {
        var found = $filter('getById')($scope.RequiredList, id);
        $scope.selected = JSON.stringify(found);
    }
}]).filter('getById', function () {
    return function (input, id) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (+input[i].ID == +id) {
                return input[i].Name;
            }
        }
        return null;
    }
}).directive('dropdownMultiselect', function () {
    return {
        restrict: 'E',
        scope: {
            model: '=',
            options: '=',
            pre_selected: '=preSelected'
        },
        template: "<div class='btn-group' data-ng-class='{open: open}'>" +
         "<button class='btn btn-small'>Select</button>" +
                 "<button class='btn btn-small dropdown-toggle' data-ng-click='open=!open;openDropdown()'><span class='caret'></span></button>" +
                 "<ul class='dropdown-menu' aria-labelledby='dropdownMenu'>" +
                     "<li><a data-ng-click='selectAll()'><i class='glyphicon glyphicon-ok-sign'></i>  Check All</a></li>" +
                     "<li><a data-ng-click='deselectAll();'><i class='glyphicon glyphicon-remove-sign'></i>  Uncheck All</a></li>" +
                     "<li class='divider'></li>" +
                     "<li data-ng-repeat='option in options'> <a data-ng-click='setSelectedItem()'>{{option.Name}}<span data-ng-class='isChecked(option.id)'></span></a></li>" +
                 "</ul>" +
             "</div>",
        controller: function ($scope) {
            $scope.openDropdown = function () {
                $scope.selected_items = [];
                for (var i = 0; i < $scope.pre_selected.length; i++) {
                    $scope.selected_items.push($scope.pre_selected[i].id);
                }
            };

            $scope.selectAll = function () {
                $scope.model = _.pluck($scope.options, 'id');
                console.log($scope.model);
            };
            $scope.deselectAll = function () {
                $scope.model = [];
                console.log($scope.model);
            };
            $scope.setSelectedItem = function () {
                var id = this.option.id;
                if (_.contains($scope.model, id)) {
                    $scope.model = _.without($scope.model, id);
                } else {
                    $scope.model.push(id);
                }
                console.log($scope.model);
                return false;
            };
            $scope.isChecked = function (id) {
                if (_.contains($scope.model, id)) {
                    return 'glyphicon glyphicon-ok pull-right';
                }
                return false;
            };
        }
    }
});