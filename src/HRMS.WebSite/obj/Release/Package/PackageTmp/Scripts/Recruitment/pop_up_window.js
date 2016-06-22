/// <reference path="encoder.js" />

var alertModalWindow = {
    windowId: 'modal_alertModalWindow_container',
    templateId: 'modal_alertModalWindow',
    close: function () {
        $('#' + this.windowId).dialog('close');
    },
    open: function (message, headerMessage, focusControl) {
        $('#' + this.windowId).html('');

        var Msg = {};
        if (typeof (headerMessage) == "undefined" || typeof (headerMessage) == "null")
            Msg = [{ message: message, headerMessage: 'Looks like there\'s a problem:' }];
        else
            Msg = [{ message: message, headerMessage: headerMessage }];

        //        Encoder.EncodeType = "entity";
        //        var encodedmsg = Encoder.htmlDecode(Msg);
        // var encodedmsg = Html.Encode(Msg);

        $('#' + this.templateId).tmpl(Msg).appendTo('#' + this.windowId);

        $('.messagePara').html($('.messagePara').html().replace(/&lt;/g, "<").replace(/&gt;/g, ">"));

        var modal = $('#' + this.windowId).dialog({
            modal: 'true', position: 'center', width: 300, close: function () {
                if (typeof (focusControl) == "object") { focusControl.focus(); }
            }
        });

        //set title of dialog dynamically
        $('span#ui-dialog-title-modal_alertModalWindow_container').html($('div#alertmodaltitle').attr('title'));

        //new code
        $('modal_alertModalWindow').html(Msg);
    }
};

var V2hrmsAlert = function (message, headerMessage, control) {
    alertModalWindow.open(message, headerMessage, control);
};