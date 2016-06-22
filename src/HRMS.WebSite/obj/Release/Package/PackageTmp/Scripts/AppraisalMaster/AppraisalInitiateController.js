//Array.prototype.AddNewPropertyToCollection = function (objArr) {
//    var dis = this;
//    for (var current in dis) {
//        console.log(JSON.stringify(current));
//        for (var obj in objArr) {
//            if (obj != "AddNewPropertyToCollection")
//                dis[current][objArr[obj]["prop"]] = objArr[obj]["value"];
//        }
//    }
//    return dis;

//}

var app = angular.module('HRMS', ['ngGrid']);
app.controller('MyCtrl', ["$scope", "AppraislFactory", "$http",function ($scope, AppraislFactory,$http) {
    // $scope.statuses = AppraislFactory.getDropDownData.Appr1;
    AppraislFactory.getDropDownData()
        .success(function (data, status, headers) {
            $scope.statuses = data.Appr1;
        }).error(function (data) {
            $scope.message = data.Message;

            $scope.msgType = "label-danger";
        });

    $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="city as city.Text for city in  statuses" ng-blur="updateEntity(row)" />';
    $scope.filterOptions = {
        filterText: ''
    };
    $scope.gridOptions = {
        data: 'myData',
        enableRowSelection: false,
        enableCellEditOnFocus: true,
        enableCellEditOnFocus: 'row.entity.editable',
        checkboxHeaderTemplate: '<input class="ngSelectionHeader" type="checkbox" ng-model="allSelected" ng-change="toggleSelectAll(allSelected)" ng-click="selectAll(allSelected)"/>',
        showSelectionCheckbox: true,
        checkboxCellTemplate: "<div class=\"ngSelectionCell\"><input style=\"display:inline\" tabindex=\"-1\" class=\"ngSelectionCheckbox\" type=\"checkbox\" ng-checked=\"row.selected\" ng-click=\"checkedIndex(row)\"/></div>",
        multiSelect: true,
        plugins: [new ngGridCsvExportPlugin(null,$http)],
        showFooter: true,
            filterOptions: $scope.filterOptions,
            //showFilter:true,filterText= '<EName 1>:<lit 1>;<RPoolName 2>:<lit a>;<DUName 3>:<lit A>'
        columnDefs: [
            //{
            //    field: 'checker',
            //    displayName: '',
            //    enableCellEdit: false,
            //    cellTemplate: '<input type="checkbox" ng-model="COL_FIELD" ng-click="checkedIndex(row)">'
            //},
        { field: "EC", width: "*", displayName: 'Employee Code', enableCellEdit: false },
            //{ field: "EID", width: "*" , displayName: 'ID' },
        { field: "EName", width: "*", displayName: 'Name', enableCellEdit: false },
        { field: "App1", width: "*", displayName: 'Appraiser 1', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"Appr1"' },
        { field: "App2", width: "*", displayName: 'Appraiser 2', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"Appr2"' },
        { field: "Rv1", width: "*", displayName: 'Reviewer 1', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"Revr1"' },
        { field: "RV2", width: "*", displayName: 'Reviewer 2', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"Revr2"' },
        { field: "GHID", width: "*", displayName: 'Group Head', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"GroupHeadID"' },
        { field: "IDF", width: "*", displayName: 'IDF', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"IDFId"' },
        { field: "IDFE1", width: "*", displayName: 'IDF Escalation 1', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"IDFEsc1"' },
        { field: "IDFE2", width: "*", displayName: 'IDF Escalation 2', enableCellEditOnFocus: true, editableCellTemplate: $scope.cellSelectEditableTemplate, cellFilter: 'mapStatus:"IDFEsc2"' },
        { field: "RPoolName", width: "*", displayName: 'Resource Pool',enableCellEdit: false },
        { field: "DUName", width: "*", displayName: 'Delivery Unit', enableCellEdit: false }]
    };

    AppraislFactory.getGridData().success(function (data, status, headers) {
        $scope.myData = data;
       //$scope.gridOptions.$gridScope.columns[11].toggleVisible();
       //$scope.gridOptions.$gridScope.columns[12].toggleVisible();
    }).error(function (data) {
        $scope.message = data.Message;

        $scope.msgType = "label-danger";
    });
    var saveEmployeeList = [];
    $scope.updateEntity = function (row) {
        if (saveEmployeeList.indexOf(row.entity) === -1) {
            row.entity.checker = true;
            for (var i in row.entity) {
                if (typeof (row.entity[i]) == "object") {
                    row.entity[i] = row.entity[i].Value;
                }
            }
            //saveEmployeeList.push(row.entity);
        }
        else {
            //saveEmployeeList.splice(saveEmployeeList.indexOf(row.entity), 1);
            row.entity.checker = true;
            for (var i in row.entity) {
                if (typeof (row.entity[i]) == "object") {
                    row.entity[i] = row.entity[i].Value;
                }
            }
           // saveEmployeeList.push(row.entity);
        }
    };
    //$scope.doSomething = function (id) {
    //    alert("foo");
    //    location.href = 'http://localhost:25000/EmployeeDetails/EmployeeDetails?employeeId=NPAoVwFgc0/2AdfFFeNSsU+xiibdosdWC5N+Xu3indHHv1DBcXgO6Q=='
    //}
    $scope.checkedIndex = function (row) {
        if (saveEmployeeList.indexOf(row.entity) === -1) {
            saveEmployeeList.push(row.entity);
        }
        else {
            saveEmployeeList.splice(saveEmployeeList.indexOf(row.entity), 1);
        }
    }
    $scope.saveGrid = function () {
        if (saveEmployeeList.length>0){
            $scope.myDiv = true;
            AppraislFactory.SaveGridData(saveEmployeeList)
                  .success(function (data, status, headers) {
                      if (data == '1') {
                          alert("Data is Ready to initiate");
                          window.location.reload();
                      }
                  }).error(function (data) {
                      $scope.myDiv = false;
                      $scope.message = data.Message;

                      $scope.msgType = "label-danger";
                  });
        }

        else {
            alert("Select atleast one record!")
        }
    }
    $scope.selectAll = function (rowCheck) {
        angular.forEach($scope.gridOptions.$gridScope.renderedRows, function (elem) {
            if (rowCheck == false) {
                saveEmployeeList.splice(saveEmployeeList.indexOf(elem.entity), 1);
            }
            else if (saveEmployeeList.indexOf(elem.entity) === -1) {
                saveEmployeeList.push(elem.entity);
            }
        })
    }

      $scope.filterName = function () {
          var filterText = 'EName:' + $scope.nameFilter;
          if (filterText !== 'EName:') {
              $scope.filterOptions.filterText = filterText;
          } else {
              $scope.filterOptions.filterText = '';
          }
  };

    $scope.filterRpool = function () {
        var filterText = 'RPoolName:' +$scope.poolFilter;
        if (filterText !== 'RPoolName:') {
            $scope.filterOptions.filterText = filterText;
           }
       else {
            $scope.filterOptions.filterText = '';
         }
    };

    $scope.filterDU = function () {
        var filterText = 'DUName:' + $scope.DUFilter;
          if (filterText !== 'DUName:') {
              $scope.filterOptions.filterText = filterText;
              } else {
              $scope.filterOptions.filterText = ''; }
        };
}])

.directive('ngBlur', function () {
    return function (scope, elem, attrs) {
        elem.bind('blur', function () {
            scope.$apply(attrs.ngBlur);
        });
    };
})

.filter('mapStatus', function (AppraislFactory) {
    var dataSaveed = [];
    AppraislFactory.getDropDownData()
         .success(function (data, status, headers) {
             dataSaveed = data;
         }).error(function (data) {
             $scope.message = data.Message;

             $scope.msgType = "label-danger";
         });
    return function (input, option) {
        if (input != null) {
            var totalEmployee = typeof (dataSaveed[option]) == "object" ? dataSaveed[option] : dataSaveed[dataSaveed[option]];
            if (totalEmployee) {
                var emplyeeMatched = totalEmployee.filter(function (emp) {
                    if (input.Value == undefined) {
                        return emp.Value == input;
                    }
                    else {
                        return emp.Value == input.Value;
                    }
                });

                if (emplyeeMatched.length == 1)
                    return emplyeeMatched[0].Text;
                else
                    return 'Select'
            }
        }
        else
            return 'Select';
    };
})

 .factory('AppraislFactory', ['$http', function ($http) {
     var gridData = function () {
         return $http({
             method: 'GET',
             url: '/api/AppraisalReview/GetAppraisalList',
         })
     }

     //var doRequest = { "Appr1": [{ "Value": 61, "Text": "HCM  ADMIN" }, { "Value": 142, "Text": "Mukul Navinchandra Shah" }, { "Value": 152, "Text": "Manoj Manoharan Payakkal" }, { "Value": 153, "Text": "Sameer Prafuchandra Thakar" }, { "Value": 157, "Text": "Amit deepakbhai Rawal" }, { "Value": 160, "Text": "Uddesh Subhash Kondekar" }, { "Value": 161, "Text": "Vikesh Vijay Sonkar" }, { "Value": 165, "Text": "Narendra Pandurang Patkar" }, { "Value": 167, "Text": "Shilesh PNKumaran SK" }, { "Value": 168, "Text": "Rajneesh Baldev Makhija" }, { "Value": 169, "Text": "Partho Sukeschandra Pal" }, { "Value": 170, "Text": "Shailendra Sheena Kotian" }, { "Value": 174, "Text": "Lucy Lonappan" }, { "Value": 175, "Text": "Dinesh  Pushpan" }, { "Value": 179, "Text": "Pushpal S K Mishra" }, { "Value": 183, "Text": "SAnilKumar RamaKrishna S" }, { "Value": 187, "Text": "Anjan Arun Kumar Chatterjee" }, { "Value": 188, "Text": "Kapil R Varadharajan" }, { "Value": 190, "Text": "Jude Jacinto" }, { "Value": 196, "Text": "Ajit Venkateswarlu Jasti" }, { "Value": 200, "Text": "Nikish Kiritkumar Parikh" }, { "Value": 205, "Text": "Mohan Khanna V Yadavalli" }, { "Value": 207, "Text": "B. Sriram" }, { "Value": 208, "Text": "Manish BibhutiPrasad Kumar" }, { "Value": 209, "Text": "Rashesh Anilkumar Mehta" }, { "Value": 216, "Text": "Chetan Nalin Parikh" }, { "Value": 228, "Text": "Srinivasu Ramarao Nekkanti" }, { "Value": 229, "Text": "Devireddy Narasimha Seshireddy" }, { "Value": 248, "Text": "James Fredwin Myabo" }, { "Value": 256, "Text": "Jigna  Makhija" }, { "Value": 259, "Text": "Sairam  Oduru" }, { "Value": 290, "Text": "Hiren Ramesh Doshi" }, { "Value": 292, "Text": "Brijen Arvindbhai Patel" }, { "Value": 324, "Text": "Binoy Krishnankutty" }, { "Value": 326, "Text": "Rickson Canute Borges" }, { "Value": 342, "Text": "Dhaval Rajnikant Shah" }, { "Value": 343, "Text": "Tejal  Shah" }, { "Value": 363, "Text": "Milind Rajaram Boritkar" }, { "Value": 368, "Text": "Gururaj R B Sheth" }, { "Value": 369, "Text": "Ganeshan Venkateshan A" }, { "Value": 388, "Text": "Niladri  Panigrahi" }, { "Value": 403, "Text": "Vikram Narendra Shah" }, { "Value": 405, "Text": "Ritamber Shashi Jha" }, { "Value": 412, "Text": "Blessy  Antony" }, { "Value": 467, "Text": "Anushree  Tirwadkar" }, { "Value": 479, "Text": "Pallavi Sunil Gharat" }, { "Value": 482, "Text": "Raj  Thaker" }, { "Value": 483, "Text": "Vivek RajaRam Rawat" }, { "Value": 485, "Text": "Amol Naresh Karambat" }, { "Value": 489, "Text": "Ratan Radheshyam Yadav" }, { "Value": 490, "Text": "Radheshyam Hemraj Yadav" }, { "Value": 516, "Text": "Abhishek Virendra Singh" }, { "Value": 530, "Text": "Davinder  Luthra" }, { "Value": 552, "Text": "Vinod Balkrishna Mankar" }, { "Value": 554, "Text": "Delkumar Kishanlal Pal" }, { "Value": 556, "Text": "Swati Prakash Patil" }, { "Value": 558, "Text": "Tushar Vinayak Suvarnkar" }, { "Value": 573, "Text": "Asmita Nandakumar More" }, { "Value": 578, "Text": "Yogesh Pandurang Mhatre" }, { "Value": 618, "Text": "Shivangni  Rawal" }, { "Value": 637, "Text": "Soniya Arvind Tiwari" }, { "Value": 639, "Text": "Snehal Surendranath Bapardekar" }, { "Value": 640, "Text": "Sandeep Test Singh Test Sahni Test" }, { "Value": 647, "Text": "Ashutosh Janardhan Singh" }, { "Value": 670, "Text": "Jayavant Tulsidas Desai" }, { "Value": 671, "Text": "Pinky Lalpratap Singh" }, { "Value": 672, "Text": "Swapnil Ramdas Borse" }, { "Value": 673, "Text": "Amit Vijay Gandhi" }, { "Value": 689, "Text": "Rakhee Dada Khute" }, { "Value": 698, "Text": "Priyanka Vitthal Lokhande" }, { "Value": 1339, "Text": "Harikrishnan  C" }, { "Value": 1343, "Text": "Sunil Balwan Kumar" }, { "Value": 1344, "Text": "Neelesh  Rathore" }, { "Value": 1353, "Text": "Saranya  Kartik" }, { "Value": 1357, "Text": "Swati Viraj Gadhave" }, { "Value": 1364, "Text": "Neha  Suvarna" }, { "Value": 1366, "Text": "Rahul Hemkant Chevale" }, { "Value": 1384, "Text": "Roshani Dashrath Kamble" }, { "Value": 1385, "Text": "Sandeep  Chaudhari" }, { "Value": 1390, "Text": "Shashin  Chheda" }, { "Value": 1391, "Text": "Sanket Subhash Patil" }, { "Value": 1398, "Text": "Vijay  Kumar P" }, { "Value": 1403, "Text": "Sandeep Ramchandra Sane" }, { "Value": 1406, "Text": "Sukesh Soman Panikar" }, { "Value": 1407, "Text": "Subhash  Mohan" }, { "Value": 1453, "Text": "Devendra  Gupta" }, { "Value": 1455, "Text": "Gaurav Mukesh Upadhyay" }, { "Value": 1465, "Text": "Amit Kumar Sharma" }, { "Value": 1476, "Text": "Narasimharao  Pati" }, { "Value": 1482, "Text": "Nirmal Bhanushali Bhanushali" }, { "Value": 1487, "Text": "Prasad Seetaram Chaugule" }, { "Value": 1493, "Text": "Dipanjali  Mohanty" }, { "Value": 1503, "Text": "RohiniTest Akhilesh Kulkarni" }, { "Value": 1504, "Text": "Manisha Sudhakar Bagal" }, { "Value": 1507, "Text": "Anway Anand Kathale" }, { "Value": 1522, "Text": "Nainita Karunakara Shettyii" }, { "Value": 1526, "Text": "Harshapaul Marian Pinto" }, { "Value": 1563, "Text": "Shubhangi Bharat Pathare" }, { "Value": 1564, "Text": "Samir Navinchandra Savla" }, { "Value": 1570, "Text": "Charly Mathew Varghese" }, { "Value": 1597, "Text": "Suresh Ramswaroop Rawat" }, { "Value": 1609, "Text": "Priya Prakash Bhandarkar" }, { "Value": 1616, "Text": "Priyankaygjh Sunil Kadamjh" }, { "Value": 1626, "Text": "Vineed Vikram Pillai" }, { "Value": 1628, "Text": "Vishal  Laghate" }, { "Value": 1662, "Text": "Saikat Phanindra Mukutmani" }, { "Value": 1679, "Text": "Rahul  Majumder" }, { "Value": 1680, "Text": "Santwana  Bhati" }, { "Value": 1683, "Text": "Ajit Kumar Tiwari" }, { "Value": 1689, "Text": "Manish  Abraham" }, { "Value": 1704, "Text": "Nikhil Mohan Juikar" }, { "Value": 1705, "Text": "Supriya Shekhar Bhandari" }, { "Value": 1713, "Text": "Pooja  Saini" }, { "Value": 1716, "Text": "Kim Sayagi Patil" }, { "Value": 1724, "Text": "Sanjay Anthony Rajkumar" }, { "Value": 1738, "Text": "Neha Milind Kulkarni" }, { "Value": 1739, "Text": "Prashant Prabhakaran Nair" }, { "Value": 1740, "Text": "Namrata Chandrakant Shetkar" }, { "Value": 1750, "Text": "Prashant Manohar Kadam" }, { "Value": 1753, "Text": "Gargi  Sakhare" }, { "Value": 1765, "Text": "Dhyanesh Premjesus Singh" }, { "Value": 1779, "Text": "Madhuri Rajesh Mahadik" }, { "Value": 1780, "Text": "Nimmy Chakkunny Chowalloor" }, { "Value": 1786, "Text": "Poonam  Sharma" }, { "Value": 1789, "Text": "Ramesh  Gundala" }, { "Value": 1791, "Text": "Neelam  Himanshu Mistry" }, { "Value": 1797, "Text": "Shrikant Vitthal Haralayya" }, { "Value": 1798, "Text": "Charles Albert Paul" }, { "Value": 1818, "Text": "Swaminathan Kumbakonam Seshadri" }, { "Value": 1820, "Text": "Ritesh Tarunkant Shah" }, { "Value": 1822, "Text": "Alok  Anand" }, { "Value": 1828, "Text": "Nitin  Bhatnagar" }, { "Value": 1831, "Text": "Reenu  Rathore" }, { "Value": 1832, "Text": "Umesh Babulal Mishra" }, { "Value": 1838, "Text": "Saurabh  Chowdhury" }, { "Value": 1851, "Text": "Sarita Rampyar Singh" }, { "Value": 1854, "Text": "Pranav Prabhakar Kamble" }, { "Value": 1859, "Text": "Sneha Susanta Dolui" }, { "Value": 1871, "Text": "Rishikesh Ramesh Yawalkar" }, { "Value": 1872, "Text": "Rishikesh Ram Gugale" }, { "Value": 1876, "Text": "yashodharan prakash dhavale" }, { "Value": 1887, "Text": "Devika Parag Naik" }, { "Value": 1895, "Text": "Pratibha  Krishna Sakpal" }, { "Value": 1896, "Text": "Sumitra  Krishnamurthy" }, { "Value": 1897, "Text": "Prasadi Kiran Shigwan" }, { "Value": 1907, "Text": "Harikrishna  kanta" }, { "Value": 1913, "Text": "Tanmay Subhash Sawant" }, { "Value": 1917, "Text": "kiran shankar shelke" }, { "Value": 1919, "Text": "Jayaraj Mariadhas Nadar" }, { "Value": 1960, "Text": "Nidhi  Moghe" }, { "Value": 1962, "Text": "Sachin Govind Kumkar" }, { "Value": 1968, "Text": "Kavita Anand Gaikwad" }, { "Value": 1970, "Text": "Saroj  Singh" }, { "Value": 1973, "Text": "Kundan Sikandar Kumar" }, { "Value": 1976, "Text": "Vinita Umesh Prasad" }, { "Value": 1985, "Text": "Pruthvi P Paghdal" }, { "Value": 1986, "Text": "Shital Shantilal Dedhia" }, { "Value": 1987, "Text": "Sagar Sarjerao Gaikwad" }, { "Value": 1989, "Text": "Veerabhadra Rao  Kasina" }, { "Value": 2007, "Text": "Rohini Rahul Deshmukh" }, { "Value": 2014, "Text": "Pooja  Iyer" }, { "Value": 2015, "Text": "Ashish Kamalakar Pateel" }, { "Value": 2030, "Text": "siddharth appa dilpak" }, { "Value": 2038, "Text": "Pravin Baban Dongare" }, { "Value": 2051, "Text": "Anish  Vijayan" }, { "Value": 2056, "Text": "Deepa  Chaudhary" }, { "Value": 2061, "Text": "Ashish  Ojha" }, { "Value": 2064, "Text": "Nikhil  Kandalkar" }, { "Value": 2065, "Text": "Makarand  Patil" }, { "Value": 2067, "Text": "Tushar  Gite" }, { "Value": 2069, "Text": "Vidyadhar Namdev Dighe" }, { "Value": 2078, "Text": "Nagendar  Reddy" }, { "Value": 2079, "Text": "Pooja  Nimbkar" }, { "Value": 2082, "Text": "Ashish  Patil" }, { "Value": 2083, "Text": "Sneha Sadanand Sawant" }, { "Value": 2093, "Text": "Gorakh Balwant Mali" }, { "Value": 2098, "Text": "Nivedita  Sinha" }, { "Value": 2101, "Text": "Purva  Paldhe" }, { "Value": 2110, "Text": "Murali  Gorantla" }, { "Value": 2113, "Text": "Siddhesh  Shivtarkar" }, { "Value": 2133, "Text": "Vibrant Web Two" }, { "Value": 2138, "Text": "Siddharth  Rajpoot" }, { "Value": 2164, "Text": "Mrunalini Francis Navgire" }, { "Value": 2167, "Text": "Vinayak Pandurang Patil" }, { "Value": 2169, "Text": "Divya  Rohit" }, { "Value": 2170, "Text": "Bindya  Shah" }, { "Value": 2172, "Text": "Ashutosh  Tawade" }, { "Value": 2175, "Text": "Ansu  Thomas" }, { "Value": 2179, "Text": "Sudhakar Reddy Yasa" }, { "Value": 2180, "Text": "Rosemary  Varkey" }, { "Value": 2190, "Text": "Preeti Rohit Pandit" }, { "Value": 2191, "Text": "Darshana Anant Pawar" }, { "Value": 2198, "Text": "Harleen  Rekhi" }, { "Value": 2199, "Text": "Kamal  Malik" }, { "Value": 2200, "Text": "Vijay  Panchal" }, { "Value": 2208, "Text": "Jasbir  Singh" }, { "Value": 2209, "Text": "Ramalaxmi  Nagarajan" }, { "Value": 2212, "Text": "Ashika  Suresh" }, { "Value": 2213, "Text": "Vidya Vijay Raju" }, { "Value": 2218, "Text": "Glanda Meera Nazareth" }, { "Value": 2219, "Text": "Neha Rajiv Gugnani" }, { "Value": 2220, "Text": "Arbind Kumar Singh" }, { "Value": 2221, "Text": "Vaishali Rameshkumar Tikare" }, { "Value": 2223, "Text": "Baljeet Singh Chambal" }, { "Value": 2224, "Text": "Yashashwini  Somisetty" }, { "Value": 2226, "Text": "Manoj Ravishankar Mishra" }, { "Value": 2227, "Text": "Tushar Balakrishna Kulkarni" }, { "Value": 2230, "Text": "Kiran Kumar Pillai" }, { "Value": 2231, "Text": "Nilesh Sadashiv Zemse" }, { "Value": 2233, "Text": "Tomy Solomen Nediyodi" }, { "Value": 2237, "Text": "Gaurav  Sharma" }, { "Value": 2240, "Text": "Ankush RaviBhushan Handa" }, { "Value": 2241, "Text": "Umesh Muralidharan Nair" }, { "Value": 2243, "Text": "Vikram Vijay Bangera" }, { "Value": 2244, "Text": "Wilson Joaquim Dsilva" }, { "Value": 2249, "Text": "Tushar Harnarayan Malani" }, { "Value": 2252, "Text": "Arjun Kishore Rao" }, { "Value": 2254, "Text": "Karamveer  Kaur" }, { "Value": 2255, "Text": "Sheetal Kunal Gada" }, { "Value": 2256, "Text": "Snehal Vithal Bhavsar" }, { "Value": 2257, "Text": "Dinesh  Eswarawaka" }, { "Value": 2259, "Text": "Nagarajan  Packiarajan" }, { "Value": 2269, "Text": "Jaspreet Mankoo" }, { "Value": 2274, "Text": "Deepika  Bawangade" }, { "Value": 2275, "Text": "Neha Nitin Potdar" }, { "Value": 2278, "Text": "Sushma Birendra Pathak" }, { "Value": 2282, "Text": "kunal  devadiga" }, { "Value": 2283, "Text": "Neha  kapoor" }, { "Value": 2284, "Text": "Mamta  Chawla" }, { "Value": 2288, "Text": "Pratiksha  Damle" }, { "Value": 2289, "Text": "Shweta  Pandith" }, { "Value": 2298, "Text": "Milauni  Pujara" }, { "Value": 2301, "Text": "Sharanya  Nair" }, { "Value": 2302, "Text": "Jui  Bhatawdekar" }, { "Value": 2305, "Text": "Swatirani  Senapati" }, { "Value": 2308, "Text": "Vivek  Bhitale" }, { "Value": 2310, "Text": "Praveen  Singh" }, { "Value": 2313, "Text": "Avishek  Das" }, { "Value": 2316, "Text": "Ganesh  Janaki" }, { "Value": 2317, "Text": "Shreekanth  Menon" }, { "Value": 2319, "Text": "Tejaswi  Kamble" }, { "Value": 2324, "Text": "Punit  Aggarwal" }, { "Value": 2330, "Text": "RajendraPrasad  Penugonda" }, { "Value": 2331, "Text": "Subhash  Mondal" }, { "Value": 2332, "Text": "Amol  Jiwane" }, { "Value": 2334, "Text": "Manish Ramesh Sharma" }, { "Value": 2337, "Text": "Sagar Shivaji Vetal" }, { "Value": 2358, "Text": "Ravi  Srivastava" }], "Appr2": "Appr1", "Revr1": "Appr1", "Revr2": "Appr1", "GroupHeadID": "Appr1", "IDFId": "Appr1", "IDFEsc1": "Appr1", "IDFEsc2": "Appr1" }

     var doRequest = function () {
         return $http({
             method: 'GET',
             url: '/api/AppraisalReview/GetSetupList',
         })
     }

     var saveRequest = function (saveList) {
         //debugger;
         return $http({
             method: 'POST',
             url: '/api/AppraisalReview/SaveAppraisalList',
             data: saveList,
             dataType: "json"
         });
     }
     return {
         getDropDownData: function () { return doRequest() },
         getGridData: function () { return gridData() },
         SaveGridData: function (saveList) { return saveRequest(saveList); }
     }
 }])