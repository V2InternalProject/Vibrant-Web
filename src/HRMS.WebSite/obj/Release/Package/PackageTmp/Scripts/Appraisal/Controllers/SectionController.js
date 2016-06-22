var app = angular.module("ApprisalSPA");

app.controller("SectionController", ['$scope', '$routeParams', "ControlTemplate", 'Templates', '$http', 'ApprisalDataFactory','$window', function ($scope, $routeParams, ControlTemplate, Templates, $http, adf,$window) {
    //Get current Location Parameters

    section = parseInt($routeParams.sec);

    $scope.message = "";
    $scope.isShow = false;
    $scope.msgType = "bg-info";//"bg-danger";
    //Standard data object for every grid type
    $scope.test = "";
    $scope.Data = [];
    $scope.NewRow = function () {
        $scope.Data.push(_createBlankRow($scope.DataColumns));
        //console.log($scope.Data);
    }
    $scope.markForRemoval = function (index) {
        //console.log(JSON.stringify( $scope.$removeList));
    }

    $scope.action = function (param) {
        $scope.SubmitParam = param;
    }

    $scope.submitForm = function () {
        var confirm = window.confirm("You are trying to submit your Appraisal form. Make sure you have validated everything properly, as once it is submitted, it can not be roll back");

        if (confirm) {
            $scope.myDiv = true;
            adf
                .getFrom("/api/AppraisalReview/Submit/" + $routeParams.employee)
                .success(function () {
                    $window.location.href = "/AppraisalSection/AppraisalEmployee"
                })
                .error(function (err) {
                    $scope.myDiv = false;
                    $scope.message = err;
                    $scope.msgType = "bg-danger";
                })
        }
    };

    $scope.showCommand = function (cmd) {
        if ($scope.questionConfig) {
            if (cmd == "Add" || cmd == "remove") {
                if ($scope.questionConfig.commandType == 1 || $scope.questionConfig.commandType == 2)
                    return false;
                else ($scope.IsNewRowAllowed || $scope.questionConfig.commandType == 3)
                return true;
            }
            else if (cmd == "save") {
                if ($scope.questionConfig.commandType >= 2)
                    return true;
            }
        }

        return false;
    }

    $scope.disableCommand = function (cmd) {
        if ($scope.questionConfig) {
            if (cmd == "Chk") {
                if ($scope.questionConfig.commandType == 1 || $scope.questionConfig.commandType == 2)
                    return true;
                else ($scope.IsNewRowAllowed || $scope.questionConfig.commandType == 3)
                return false;
            }
        }
        return true;
    }

    $scope.submit = function (form) {
        if (form.$valid)
            _action($scope.SubmitParam);
        else {
            $scope.message = "Please provide required values in form";
            $scope.msgType = "bg-danger";
        }
    }

    $scope.Remove = function () {
        var removeArr = [];
        //alert('Len=' + $scope.$removeList.length);

        for (var i = 0; i < $scope.$removeList.length; i++) {
            if ($scope.$removeList[i] != null && $scope.$removeList[i])
                removeArr.push($scope.Data[i]);
        }
        $scope.Data = RemoveMultipleFromArray($scope.Data, removeArr);
        _resetIndex($scope.DataColumns, $scope.Data);
        $scope.$removeList = [];
    }
    $scope.Init = function () {
        if ($scope.$parent.MenuData.length > 0) {
            for (var i = 0; i < $scope.$parent.MenuData.length; i++) {
                $scope.MenuData[0]["class"] = "";
                $scope.$parent.MenuData[i]["class"] = $routeParams.sec == $scope.$parent.MenuData[i]["SectionID"].toString() ? "selected" : "";
            }
        }
        // get config and data for current section
        $scope.message = "Retriving Section Schema";

        $scope.msgType = "bg-info";
        adf
       .getFrom("/api/AppraisalReview/Section/{0}/for/{1}".format($routeParams.sec, $routeParams.employee))
       .success(function (data) {
           console.log('data received');
           $scope.questionConfig = data;
           $scope.IsNewRowAllowed = false;
           $scope.HeaderColumns = _getHeaderColumns();
           $scope.DataColumns = _getDataColumns();
           $scope.message = "";

           $scope.message = "Retriving Section Data, if available";

           adf
               .getFrom("/api/AppraisalReview/GetAppraisalData/{2}/{0}/for/{1}".format($routeParams.sec, $routeParams.employee, $scope.questionConfig.sectionTypeParser))
               .success(function (response) {
                   $scope.Data = _getParamConfig(response.Data, $scope.questionConfig.param);
                   $scope.message = !isEmpty(response.Data) ? "Data Loaded Successfully" : "No records found for this section.";

                   $scope.msgType = "bg-success";
                   setTimeout(function () { $('.bg-success').hide(); $scope.$digest(); }, 3000);
               }).
               error(function (data) {
                   $scope.message = data.Message;

                   $scope.msgType = "bg-danger";
               });
           $scope.$removeList = [];
           $scope.CommandList = _getCommandList();
       }).
        error(function (data) {
            $scope.message = data.Message;
            $scope.msgType = "bg-danger";
        });
    }

    $scope.$watch("msgType", function () {
        $('.msgBox').show();
        setTimeout(function () { $('.msgBox').hide(); }, 3000);
    })

    //
    /*
    1. Get Location
    2. Get Section config and data
    3. |-> populate Header
    3.1|-> Populate questions if section is blocked to question limit....
    4. |-> populate Data
    5. |-> populate Actions

    */
    //Create a blank new object in $scope.Data
    function _createBlankRow(questionsArr) {
        var obj = {};
        for (var i in questionsArr)
            if (questionsArr[i].key != undefined) {
                if (questionsArr[i].template == "autonum")
                    obj[questionsArr[i].key] = ($scope.Data.length + 1).toString();
                else
                    obj[questionsArr[i].key] = "";
            }

        return obj;
    }

    //Action

    function _action(param) {
        console.log(param);
        console.log("-------");
        var finalData = undefined;
        if ($scope.IsNewRowAllowed)
            finalData = _saveHorizontal();
        else
            finalData = _saveVerticle();
        $scope.message = "Saving Data......";
        $scope.msgType = "bg-info";
        console.log(param);
        $http({
            url: param.url,
            method: param.protocol,
            data: finalData
        }).success(function () {
            $scope.message = "Data Saved Succesfully";
            $scope.msgType = "bg-success";
        })
            .error(function () {
                $scope.message = "Error Occured while saving data for Section ";
                $scope.msgType = "bg-danger";
            })
    }

    // create a list of Transactional commands
    function _getCommandList() {
        return [{ text: "Save", protocol: "post", url: "/api/AppraisalReview/Save/{2}/{0}/for/{1}".format($routeParams.sec, $routeParams.employee, $scope.questionConfig.sectionTypeParser) }]
    }

    // a very spectific function to reset index order if any row has been removed and theres a column type 'autonum'
    function _resetIndex(colConfig, Data) {
        var col = "";
        for (var i in colConfig) {
            if (colConfig[i].template == "autonum")
                col = colConfig[i].key;
        }
        for (var i in Data) {
            //Data[i][col] = parseInt(i.value) + 1;
            Data[i][col] =parseInt(parseInt(i) + parseInt(1));
        }
    }

    //create an array of static data when section grid has
    // limited questions
    function _getParamConfig(data, param) {
        var tempdata = data;
        data = data == null || !data.length ? [] : data;
        var rowCount = 0;

        if (!isEmpty(param)) {
            // ristrict user from creating new row for data entry;
            $scope.IsNewRowAllowed = false;
            for (var current in tempdata)
                param[current] = tempdata[current];
        }
        else {
            //console.log($scope.IsNewRowAllowed + "----")
            $scope.IsNewRowAllowed = true;
            //console.log($scope.IsNewRowAllowed + "----")
        }
        // get row count for preconfigured questions
        for (var i in param) {
            var rows = param[i];
            for (var row in rows) {
                rowCount++
            }
            //check it for only first element in param and break, assuming that every element will have same count, it not then
            //admin has done something wrong.
            break;
        }
        //console.log($scope.DataColumns);
        // push new rows in Scope.Data till its lenght is equals to rowcount
        if (data.length <= 0) {
            for (var i = 0; i < rowCount; i++)
                data.push(_createBlankRow($scope.DataColumns));
        }
        //console.log(_data);
        // populate questions

        for (var i in param) {
            rowCount = 0;
            var rows = param[i];
            for (var row in rows) {
                data[rowCount++][i] = rows[row];
            }
        }
        //date Hack, its is a hack as I don't have a concrete solutions
        for (var i in $scope.DataColumns) {
            if ($scope.DataColumns[i].template == "date" && $scope.DataColumns[i].dataType == "date") {
                for (var j in data) {
                    data[j][$scope.DataColumns[i].key] = new Date(data[j][$scope.DataColumns[i].key]);
                }
            }
            else if ($scope.DataColumns[i].template == "label" && $scope.DataColumns[i].dataType == "date") {
                for (var j in data) {
                    var dt = new Date(data[j][$scope.DataColumns[i].key]);
                    if (dt.getFullYear() > 2013)
                        data[j][$scope.DataColumns[i].key] = (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                    else
                        data[j][$scope.DataColumns[i].key] = "";
                }
            }
        }
        return data;
    }

    // get list of Header Columns for grid display from question config
    function _getHeaderColumns() {
        var arr = new Array($scope.questionConfig.questions.length);
        var propCount = 0;
        // QUick hack,,, as IsEmpty not working
        for (var prm in $scope.questionConfig.param)
            propCount++;
        if (propCount == 0) {
            arr[propCount++] = "";
        }
        else propCount = 0;

        for (var question in $scope.questionConfig.questions)
            arr[$scope.questionConfig.questions[question].seq + propCount - 1] = $scope.questionConfig.questions[question].questionText;

        //cleanup undefined array indexes if there are any...

        RemoveFromArray(arr,undefined);
        return arr;
    }

    // get list of Data Columns and their respactive template type from question config
    function _getDataColumns() {
        var arr = new Array($scope.questionConfig.questions.length);
        for (var question in $scope.questionConfig.questions)
            arr[$scope.questionConfig.questions[question].seq - 1] = { key: question, dataType:$scope.questionConfig.questions[question].dataType.toLowerCase(),template: $scope.questionConfig.questions[question].controlType.toLowerCase(), requiredField: $scope.questionConfig.questions[question].isRequired };

        RemoveFromArray(arr, undefined);
        return arr;
    }

    //Save Vertical data for the section type FR or FC
    function _saveVerticle() {
        var validColumns = [];
        var data = $scope.DataColumns;
        for (var col in $scope.DataColumns) {
            if (data[col]["template"] != "label")
                validColumns.push(data[col]["key"]);
        }
        var output = {};
        for (var index in validColumns) {
            var colName = validColumns[index];
            var rows = {};
            var counter = 0;
            for (var row in $scope.Data)
                rows["r" + row] = $scope.Data[row][colName] ;
            output[colName] = rows;
        }
        return output;
    }

    //Save Horizontal data for the section type V or VC
    function _saveHorizontal() {
        return $scope.Data;
    }
}])