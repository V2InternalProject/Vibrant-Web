$(document).ready(function () {
    $('#tab1').addClass('colored-border');
    $('#tab2').addClass('tabshover');
    $('#tab3').addClass('tabshover');
    $('#tab4').addClass('tabshover');
    $('#tab5').addClass('tabshover');
    $('#tab6').addClass('tabshover');
    $('.add-detailsdata').show();
    $('.search-detailsdata').hide();
    $('.holiday-listdata').hide();
    $('.tab-panel4').hide();
    $('.tab-panel5').hide();
    $('.tab-panel6').hide();
    $('#tab2').click(function () {
        $('#tab2').addClass('colored-border');
        $('#tab1').removeClass('colored-border');
        $('#tab3').removeClass('colored-border');
        $('#tab4').removeClass('colored-border');
        $('#tab5').removeClass('colored-border');
        $('#tab6').removeClass('colored-border');
        $('#tab2').removeClass('tabshover');
        $('#tab6').addClass('tabshover');
        $('#tab1').addClass('tabshover');
        $('#tab3').addClass('tabshover');
        $('#tab4').addClass('tabshover');
        $('#tab5').addClass('tabshover');
        $('.add-detailsdata').hide();
        $('.search-detailsdata').show();
        $('.holiday-listdata').hide();
        $('.tab-panel4').hide();
        $('.tab-panel5').hide();
        $('.tab-panel6').hide();
    });
    $('#tab1').click(function () {
        $('#tab1').addClass('colored-border');
        $('#tab2').removeClass('colored-border');
        $('#tab3').removeClass('colored-border');
        $('#tab4').removeClass('colored-border');
        $('#tab6').removeClass('colored-border');
        $('#tab5').removeClass('colored-border');
        $('#tab1').removeClass('tabshover');
        $('#tab2').addClass('tabshover');
        $('#tab3').addClass('tabshover');
        $('#tab4').addClass('tabshover');
        $('#tab5').addClass('tabshover');
        $('#tab6').addClass('tabshover');
        $('.add-detailsdata').show();
        $('.search-detailsdata').hide();
        $('.holiday-listdata').hide();
        $('.tab-panel4').hide();
        $('.tab-panel5').hide();
        $('.tab-panel6').hide();
    });
    $('#tab3').click(function () {
        $('#tab3').addClass('colored-border');
        $('#tab2').removeClass('colored-border');
        $('#tab1').removeClass('colored-border');
        $('#tab3').removeClass('tabshover');
        $('#tab4').removeClass('colored-border');
        $('#tab5').removeClass('colored-border');
        $('#tab6').removeClass('colored-border');
        $('#tab2').addClass('tabshover');
        $('#tab1').addClass('tabshover');
        $('#tab4').addClass('tabshover');
        $('#tab5').addClass('tabshover');
        $('#tab6').addClass('tabshover');
        $('.add-detailsdata').hide();
        $('.search-detailsdata').hide();
        $('.holiday-listdata').show();
        $('.tab-panel4').hide();
        $('.tab-panel5').hide();
        $('.tab-panel6').hide();
    });
    $('#tab4').click(function () {
        $('#tab4').addClass('colored-border');
        $('#tab2').removeClass('colored-border');
        $('#tab1').removeClass('colored-border');
        $('#tab3').removeClass('colored-border');
        $('#tab5').removeClass('colored-border');
        $('#tab6').removeClass('colored-border');
        $('#tab4').removeClass('tabshover');
        $('#tab2').addClass('tabshover');
        $('#tab1').addClass('tabshover');
        $('#tab3').addClass('tabshover');
        $('#tab5').addClass('tabshover');
        $('#tab6').addClass('tabshover');
        $('.add-detailsdata').hide();
        $('.search-detailsdata').hide();
        $('.tab-panel4').show();
        $('.holiday-listdata').hide();
        $('.tab-panel5').hide();
        $('.tab-panel6').hide();
    });
    $('#tab5').click(function () {
        $('#tab5').addClass('colored-border');
        $('#tab2').removeClass('colored-border');
        $('#tab1').removeClass('colored-border');
        $('#tab3').removeClass('colored-border');
        $('#tab4').removeClass('colored-border');
        $('#tab6').removeClass('colored-border');
        $('#tab5').removeClass('tabshover');
        $('#tab2').addClass('tabshover');
        $('#tab1').addClass('tabshover');
        $('#tab3').addClass('tabshover');
        $('#tab4').addClass('tabshover');
        $('#tab6').addClass('tabshover');
        $('.add-detailsdata').hide();
        $('.search-detailsdata').hide();
        $('.tab-panel5').show();
        $('.tab-panel4').hide();
        $('.holiday-listdata').hide();
        $('.tab-panel6').hide();
    });

    $('#tab6').click(function () {
        $('#tab6').addClass('colored-border');
        $('#tab1').removeClass('colored-border');
        $('#tab2').removeClass('colored-border');
        $('#tab3').removeClass('colored-border');
        $('#tab4').removeClass('colored-border');
        $('#tab5').removeClass('colored-border');
        $('#tab6').removeClass('tabshover');
        $('#tab1').addClass('tabshover');
        $('#tab2').addClass('tabshover');
        $('#tab3').addClass('tabshover');
        $('#tab4').addClass('tabshover');
        $('#tab5').addClass('tabshover');
        $('.add-detailsdata').hide();
        $('.search-detailsdata').hide();
        $('.holiday-listdata').hide();
        $('.tab-panel4').hide();
        $('.tab-panel5').hide();
        $('.tab-panel6').show();
    });

    $("nav#menu>ul>li:first-child").addClass("mm-opened");
    $('a.mm-subopen').on('click', function () {
        $('a.mm-subopen').not(this).parent().removeClass("mm-opened");
        $(this).parent().addClass("mm-opened");
    });

    //    Sticky menu

    $('#wrapper').scroll(function () {
        var y = $('#wrapper').scrollTop();

        if (y >= 80) {
            $(".sub-menu-colored").addClass('fixedTop');
        } else {
            $(".sub-menu-colored").removeClass('fixedTop');
        }
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
            $("#" + gridId).trigger("reloadGrid", [{ page: newPage}]);
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

    $('.emp-submenu').mouseover(function () {
        $('.emp-head').addClass('headcolorC');
    });
    $('.emp-submenu').mouseout(function () {
        $('.emp-head').removeClass('headcolorC');
    });

    $('.vb-submenu').mouseover(function () {
        $('.vb-head').addClass('headcolorC');
    });
    $('.vb-submenu').mouseout(function () {
        $('.vb-head').removeClass('headcolorC');
    });

    $('.hrp-submenu').mouseover(function () {
        $('.hrp-head').addClass('headcolorC');
    });
    $('.hrp-submenu').mouseout(function () {
        $('.hrp-head').removeClass('headcolorC');
    });

    $('.financep-submenu').mouseover(function () {
        $('.financep-head').addClass('headcolorC');
    });
    $('.financep-submenu').mouseout(function () {
        $('.financep-head').removeClass('headcolorC');
    });

    $('.adminp-submenu').mouseover(function () {
        $('.adminp-head').addClass('headcolorC');
    });
    $('.adminp-submenu').mouseout(function () {
        $('.adminp-head').removeClass('headcolorC');
    });

    $('.settings-submenu').mouseover(function () {
        $('.settings-head').addClass('headcolorC');
    });
    $('.settings-submenu').mouseout(function () {
        $('.settings-head').removeClass('headcolorC');
    });

    $('.pms-submenu').mouseover(function () {
        $('.pms-head').addClass('headcolorC');
    });
    $('.pms-submenu').mouseout(function () {
        $('.pms-head').removeClass('headcolorC');
    });

    $('.VW-submenu').mouseover(function () {
        $('.VW-head').addClass('headcolorC');
    });
    $('.VW-submenu').mouseout(function () {
        $('.VW-head').removeClass('headcolorC');
    });
});