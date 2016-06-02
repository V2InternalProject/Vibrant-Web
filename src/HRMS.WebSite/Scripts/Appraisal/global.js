String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
    });
};

//Object.prototype.isEmpty=function isEmpty() {
//    var dis = this;
//    // null and undefined are "empty"
//    if (dis == null) return true;

//    // Assume if it has a length property with a non-zero value
//    // that that property is correct.
//    if (dis.length > 0) return false;
//    if (dis.length === 0) return true;

//    // Otherwise, does it have any properties of its own?
//    // Note that this doesn't handle
//    // toString and valueOf enumeration bugs in IE < 9
//    for (var key in dis) {
//        if (dis.hasOwnProperty.call(dis, key)) return false;
//    }

//    return true;
//}

 function isEmpty(obj) {
    var dis = obj;
    // null and undefined are "empty"
    if (dis == null) return true;

    // Assume if it has a length property with a non-zero value
    // that that property is correct.
    if (dis.length > 0) return false;
    if (dis.length === 0) return true;

    // Otherwise, does it have any properties of its own?
    // Note that this doesn't handle
    // toString and valueOf enumeration bugs in IE < 9
    for (var key in dis) {
        if (dis.hasOwnProperty.call(dis, key)) return false;
    }

    return true;
}

//Array.prototype.Remove = function (item) {
//    var dis= this;
//    for (var i = dis.length; i--;) {
//        if (dis[i] === item) {
//            dis.splice(i, 1);
//            }
//        }
//}

//Array.prototype.RemoveMultiple = function (itemArr) {
//    var dis = this;
//    for (var i = itemArr.length; i--;) {
//        dis.Remove(itemArr[i]);
//    }
//}

RemoveFromArray = function (arr,item) {
    var dis = arr;
    for (var i = dis.length; i--;) {
        if (dis[i] === item) {
            dis.splice(i, 1);
        }
    }
    return dis;
}
RemoveMultipleFromArray = function (arr,itemArr) {
    var dis = arr;
    for (var i = itemArr.length; i--;) {
        RemoveFromArray(dis,itemArr[i]);
    }
    return dis;
}