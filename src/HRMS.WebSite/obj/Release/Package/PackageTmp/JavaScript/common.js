$(document).ready(function () {
    //    $('#tab1').addClass('colored-border');
    //    $('#tab2').addClass('tabshover');
    //    $('#tab3').addClass('tabshover');
    //    $('.add-detailsdata').show();
    //    $('.search-detailsdata').hide();
    //    $('.holiday-listdata').hide();
    $('#tab2').click(function () {
        $('#tab2').addClass('colored-border');
        $('#tab1').removeClass('colored-border');
        $('#tab3').removeClass('colored-border');
        $('#tab2').removeClass('.tabshover');
        $('#tab1').addClass('tabshover');
        $('#tab3').addClass('tabshover');
        $('.add-detailsdata').hide();
        $('.search-detailsdata').show();
        $('.holiday-listdata').hide();
    });
    $('#tab1').click(function () {
        $('#tab1').addClass('colored-border');
        $('#tab2').removeClass('colored-border');
        $('#tab3').removeClass('colored-border');
        $('#tab1').removeClass('tabshover');
        $('#tab2').addClass('tabshover');
        $('#tab3').addClass('tabshover');
        $('.add-detailsdata').show();
        $('.search-detailsdata').hide();
        $('.holiday-listdata').hide();
    });
    $('#tab3').click(function () {
        $('#tab3').addClass('colored-border');
        $('#tab2').removeClass('colored-border');
        $('#tab1').removeClass('colored-border');
        $('#tab3').removeClass('tabshover');
        $('#tab2').addClass('tabshover');
        $('#tab1').addClass('tabshover');
        $('.add-detailsdata').hide();
        $('.search-detailsdata').hide();
        $('.holiday-listdata').show();
    });

    $("nav#menu>ul>li:first-child").addClass("mm-opened");
    $('a.mm-subopen').on('click', function () {
        $('a.mm-subopen').not(this).parent().removeClass("mm-opened");
        $(this).parent().addClass("mm-opened");
    });

    function BuildGroupedPagination(current_page, total_pages, gridId) {
        var strPages = "";
        var intMaxPages = 0;
        var intMinPages = 0;
        var intPaginI = 0;
        var li;
        var link;

        var myPageRefresh = function (e) {
            var newPage = $(e.target).text();
            $("#" + gridId).trigger("reloadGrid", [{ page: newPage }]);
            e.preventDefault();
        };

        var custom_pager = $('<ul>', { id: 'custom_pager', class: 'clearfix' });

        if (total_pages > 10) {
            if (total_pages > 3) {
                intMaxPages = 3;
            }
            else {
                intMaxPages = total_pages;
            }

            for (intPaginI = 1; intPaginI <= intMaxPages; intPaginI++) {
                link = jQuery('<a>', { href: '#', click: myPageRefresh });
                link.text(String(intPaginI));

                if (intPaginI == current_page) {
                    current = 'current_page';
                }
                else {
                    current = '';
                }

                li = jQuery('<li>', { id: current }).append(link);

                jQuery(custom_pager).append(li);
            }

            if (total_pages > 3) {
                if ((current_page > 1) && (current_page < total_pages)) {
                    if (current_page > 5) {
                        li = jQuery('<li>', { 'class': 'pageMiddle' }).append('...');
                        jQuery(custom_pager).append(li);
                    }

                    if (current_page > 4) {
                        intMinPages = current_page;
                    }
                    else {
                        intMinPages = 5;
                    }

                    if (current_page < total_pages - 4) {
                        intMaxPages = current_page;
                    }
                    else {
                        intMaxPages = total_pages - 4;
                    }

                    for (intPaginI = intMinPages - 1; intPaginI <= intMaxPages + 1; intPaginI++) {
                        link = jQuery('<a>', { href: '#', click: myPageRefresh });
                        link.text(String(intPaginI));

                        if (intPaginI == current_page) {
                            current = 'current_page';
                        }
                        else {
                            current = '';
                        }

                        li = jQuery('<li>', { id: current }).append(link);

                        jQuery(custom_pager).append(li);
                    }

                    if (current_page < total_pages - 4) {
                        li = jQuery('<li>', { 'class': 'pageMiddle' }).append('...');
                        jQuery(custom_pager).append(li);
                    }
                }
                else {
                    li = jQuery('<li>', { 'class': 'pageMiddle' }).append('...');
                    jQuery(custom_pager).append(li);
                }

                for (intPaginI = total_pages - 2; intPaginI <= total_pages; intPaginI++) {
                    link = jQuery('<a>', { href: '#', click: myPageRefresh });
                    link.text(String(intPaginI));

                    if (intPaginI == current_page) {
                        current = 'current_page';
                    }
                    else {
                        current = '';
                    }

                    li = jQuery('<li>', { id: current }).append(link);

                    jQuery(custom_pager).append(li);
                }
            }
        }
        else {
            for (intPaginI = 1; intPaginI <= total_pages; intPaginI++) {
                link = jQuery('<a>', { href: '#', click: myPageRefresh });
                link.text(String(intPaginI));

                if (intPaginI == current_page) {
                    current = 'current_page';
                }
                else {
                    current = '';
                }

                li = jQuery('<li>', { id: current }).append(link);

                jQuery(custom_pager).append(li);
            }
        }
        return custom_pager;
    }
});