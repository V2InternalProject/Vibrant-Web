var app = angular.module("ApprisalSPA");

app.value("ControlTemplate", {
    Section1: {
        "sectionid": 1,
        "yearid": 2,
        "sectiontype": "V",
        "questions": {
            "pd": {
                "questionid": 1,
                "questiontext": "Project Definition",
                "datatype": "text",
                "control": "text",
                "isrequired": true,
                "validation": {
                    "testvalue": "",
                    "failureMessage": ""
                }
            },
            "achv": {
                "questionid": 2,
                "questiontext": "Achievement",
                "datatype": "text",
                "control": "text",
                "isrequired": false,
                "validation": {
                    "testvalue": "",
                    "failureMessage": ""
                }
            },
            "sd": {
                "questionid": 3,
                "questiontext": "Start Date",
                "datatype": "date",
                "control": "text",
                "isrequired": true,
                "validation": {
                    "testvalue": "noFutureDate",
                    "failureMessage": "Start Date can not be a future date."
                }
            },
            "ed": {
                "questionid": 4,
                "questiontext": "End Date",
                "datatype": "date",
                "control": "text",
                "isrequired": false,
                "validation": {
                    "testvalue": "postSD",
                    "failureMessage": "End date must be a later date than Start Date."
                }
            },
            "pm": {
                "questionid": 5,
                "questiontext": "Project Manager Name",
                "datatype": "text",
                "control": "text",
                "isrequired": true,
                "validation": {
                    "testvalue": "",
                    "failureMessage": ""
                }
            },
            "am": {
                "questionid": 6,
                "questiontext": "Appreciation Mail",
                "datatype": "text",
                "control": "text",
                "isrequired": true,
                "validation": {
                    "testvalue": "",
                    "failureMessage": ""
                }
            }
        },
        "param": {}
    },
    Section2: {
        "sectionid": 2,
        "yearid": 2,
        "sectiontype": "FR",
        "questions": {
            "sn": {
                "questionid": 1,
                "questiontext": "S No",
                "datatype": "text",
                "control": "label",
                "isrequired": false,
                "validation": {
                    "testvalue": "",
                    "failureMessage": ""
                }
            },
            "ar": {
                "questionid": 2,
                "questiontext": "Area",
                "datatype": "text",
                "control": "label",
                "isrequired": false,
                "validation": {
                    "testvalue": "",
                    "failureMessage": ""
                }
            },
            "ac": {
                "questionid": 3,
                "questiontext": "Appraisee's Comment",
                "datatype": "text",
                "control": "text",
                "isrequired": true,
                "validation": {
                    "testvalue": "",
                    "failureMessage": ""
                }
            }
        },
        "param": {
            "sn": {
                "r1": "A",
                "r2": "B",
                "r3": "C",
                "r4": "D",
                "r5": "E"
            },
            "ar": {
                "r1": "Facilitating Factors (self)",
                "r2": "Facilitating Factors (environment)",
                "r3": "Inhibiting Factors (Self related)",
                "r4": "Inhibiting Factors (Environment Related)",
                "r5": "Support Expected/ Required from Organization in Future"
            }
        }
    }
});

app.value("Templates", {
    text: "<input type='text' name='{2}' ng-model='{0}' {1} class='form-control'>",
    date: "<input type='date' ng-model='{0}' {1} class='form-control'>",
    autonum: "<label>{{{0}}}</label>",
    label: "<label>{{{0}}}</label>",
    rating: "<select ng-init='{0}' ng-model='{0}' {1} class='form-control'>" +
                "<option value='1'>1</option>"+
                "<option value='2'>2</option>"+
                "<option value='3'>3</option>"+
                "<option value='4'>4</option>"+
                "<option value='5'>5</option>"+
            "</select>",
    agree: "<select ng-init='{0}' ng-model='{0}' {1} class='form-control'>" +
                "<option value='1'>Agree</option>"+
                "<option value='2'>Disagree</option>"+
            "</select>"
});