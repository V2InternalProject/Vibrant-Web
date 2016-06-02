angular.module('MyTest', []).controller('AprController', ['$scope', function ($scope) {
    $scope.appraisalData1 = {
        "empid": "Ramesh",
        "year": "2014",
        "data": [
          {
              "sectionid": "Section - 1",
              "sectiontype":"table",
              "questions": [
                {
                    "questionid": "Why do you want this job?",
                    "type": "aaa",
                    "order": "ss",
                    "rating": [
                      {
                          "stageid": "Stage Id :1",
                          "comments": "zxc",
                          "editable": false,
                          "rating": "1"
                      },
                      {
                          "stageid": "Stage Id :2",
                          "comments": "sdsd",
                          "editable": false,
                          "rating": "2"
                      },
                      {
                          "stageid": "Stage Id :1",
                          "comments": "zxc",
                          "editable": false,
                          "rating": "1"
                      }
                    ]
                }, {
                    "questionid": "Have you got any questions?",
                    "type": "aaa",
                    "order": "ss",
                    "rating": [
                      {
                          "stageid": "stage Id : 3",
                          "comments": "zxc",
                          "editable": true,
                          "rating": "1"
                      },
                      {
                          "stageid": "Stage Id :2",
                          "comments": "sdsd",
                          "editable": false,
                          "rating": "2"
                      },
                      {
                          "stageid": "Stage Id :1",
                          "comments": "zxc",
                          "editable": false,
                          "rating": "1"
                      }
                    ]
                }
              ]
          },
          {
              "sectionid": "Section - 2",
              "sectiontype": "table",
              "questions": [
                {
                    "questionid": "Describe a situation in which you led a team",
                    "type": "",
                    "order": "",
                    "rating": [
                      {
                          "stageid": "Stage Id : 1",
                          "comments": "dfsgsdg",
                          "editable": false,
                          "rating": "3"
                      },
                      {
                          "stageid": "Stage Id : 5",
                          "comments": "dsgfdsg",
                          "editable": true,
                          "rating": "4"
                      },
                      {
                          "stageid": "Stage Id :2",
                          "comments": "sdsd",
                          "editable": false,
                          "rating": "2"
                      }
                    ]
                }
              ]
          },
          {
              "sectionid": "Section - 1",
              "sectiontype": "list",              
              "questions": [
                {
                    "questionid": "Why do you want this job?",
                    "type": "aaa",
                    "order": "ss",
                    "rating": [
                      {
                          "stageid": "Stage Id :1",
                          "comments": "zxc",
                          "editable": true,
                          "rating": "1"
                      },
                      {
                          "stageid": "Stage Id :2",
                          "comments": "sdsd",
                          "editable": false,
                          "rating": "2"
                      },
                      {
                          "stageid": "Stage Id :2",
                          "comments": "sdsd",
                          "editable": false,
                          "rating": "2"
                      }
                    ]
                }, {
                    "questionid": "Have you got any questions?",
                    "type": "aaa",
                    "order": "ss",
                    "rating": [
                      {
                          "stageid": "stage Id : 3",
                          "comments": "zxc",
                          "editable": true,
                          "rating": "1"
                      }
                    ]
                }
              ]
          },
          {
              "sectionid": "Section - 2",
              "sectiontype": "list",              
              "questions": [
                {
                    "questionid": "Describe a situation in which you led a team",
                    "type": "",
                    "order": "",
                    "rating": [
                      {
                          "stageid": "Stage Id : 1",
                          "editable": false,
                          "comments": "dfsgsdg",
                          "rating": "3"
                      },
                      {
                          "stageid": "Stage Id : 5",
                          "editable": true,
                          "comments": "dsgfdsg",
                          "rating": "4"
                      }
                    ]
                }
              ]
          }
        ]
    }

    var count = $scope.appraisalData1.data.length;
    $scope.tableData = [];
    $scope.listData = [];
    for (var i = 0; i < count; i++) {
        var j = 0;
        var k = 0;
        var type = $scope.appraisalData1.data[i].sectiontype;
        if (type === "table")
        {
            $scope.tableData.push($scope.appraisalData1.data[i]);
        }
        else if (type === "list")
        {
            $scope.listData.push($scope.appraisalData1.data[i])
        }
    }

    $scope.post = {
        "year": "2014",
        "empid": "Ramesh",
        "reviewerid": "Suresh",
        "stageid": "1",
        "action": "Actions",
        "editable": false,
        "data": [
          {
              "sectionid": "1",
              "questionid": "Questions list is 1",
              "comments": "aaa",
              "rating": "1"
          },
          {
              "sectionid": "2",
              "questionid": "Questions list is 2",
              "comments": "bbb",
              "rating": "2"
          }
        ]
    }
}])