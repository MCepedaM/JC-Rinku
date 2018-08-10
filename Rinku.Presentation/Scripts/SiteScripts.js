$.fn.pageMe = function (opts) {
    var $this = this,
        defaults = {
            perPage: 10,
            showPrevNext: false,
            hidePageNumbers: false
        },
        settings = $.extend(defaults, opts);

    var listElement = $this;
    var perPage = settings.perPage;
    var children = listElement.children();
    var pager = $('.pager');

    if (typeof settings.childSelector != "undefined") {
        children = listElement.find(settings.childSelector);
    }

    if (typeof settings.pagerSelector != "undefined") {
        pager = $(settings.pagerSelector);
    }

    var numItems = children.size();
    var numPages = Math.ceil(numItems / perPage);

    pager.data("curr", 0);

    if (settings.showPrevNext) {
        $('<li><a href="#" class="prev_link">«</a></li>').appendTo(pager);
    }

    var curr = 0;
    while (numPages > curr && (settings.hidePageNumbers == false)) {
        $('<li><a href="#" class="page_link">' + (curr + 1) + '</a></li>').appendTo(pager);
        curr++;
    }

    if (settings.showPrevNext) {
        $('<li><a href="#" class="next_link">»</a></li>').appendTo(pager);
    }

    pager.find('.page_link:first').addClass('active');
    pager.find('.prev_link').hide();
    if (numPages <= 1) {
        pager.find('.next_link').hide();
    }
    pager.children().eq(1).addClass("active");

    children.hide();
    children.slice(0, perPage).show();

    pager.find('li .page_link').click(function () {
        var clickedPage = $(this).html().valueOf() - 1;
        goTo(clickedPage, perPage);
        return false;
    });
    pager.find('li .prev_link').click(function () {
        previous();
        return false;
    });
    pager.find('li .next_link').click(function () {
        next();
        return false;
    });

    function previous() {
        var goToPage = parseInt(pager.data("curr")) - 1;
        goTo(goToPage);
    }

    function next() {
        goToPage = parseInt(pager.data("curr")) + 1;
        goTo(goToPage);
    }

    function goTo(page) {
        var startAt = page * perPage,
            endOn = startAt + perPage;

        children.css('display', 'none').slice(startAt, endOn).show();

        if (page >= 1) {
            pager.find('.prev_link').show();
        }
        else {
            pager.find('.prev_link').hide();
        }

        if (page < (numPages - 1)) {
            pager.find('.next_link').show();
        }
        else {
            pager.find('.next_link').hide();
        }

        pager.data("curr", page);
        pager.children().removeClass("active");
        pager.children().eq(page + 1).addClass("active");

    }
};

function Mensaje(titulo, mensaje) {
    var _modal = "<div id='mjs' class='modal fade'><div class='modal-dialog'><div class='modal-content'>" +
      "<div class='modal-header bg-success'><button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button><h4 class='modal-title'>" + titulo + "</h4></div>" +
      "<div class='modal-body'><strong><p>" + mensaje + "</p></strong></div>" +
      "<div class='modal-footer'><button type='button' class='btn btn-success' onclick=\"$('#mjs').modal('hide');\"><span class='glyphicon glyphicon-ok'></span>&nbsp;<strong>Aceptar</strong></button></div>" +
      "</div></div></div>";



    $("body").append(_modal);

    $('#mjs').on('hidden.bs.modal', function () {
        $("#mjs").remove();
    });

    $('#mjs').modal('show');
}

function Informacion(titulo, datos) {
    var _modal = "<div id='info' class='modal fade'><div class='modal-dialog'><div class='modal-content'>" +
      "<div class='modal-header bg-info'><button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button><h4 class='modal-title'>" + titulo + "</h4></div>" +
      "<div class='modal-body'><strong><p>" + datos + "</p></strong></div>" +
      "<div class='modal-footer'><button type='button' class='btn btn-info' onclick=\"$('#info').modal('hide');\">Cerrar</button></div>" +
      "</div></div></div>";

    $("body").append(_modal);

    $('#info').on('hidden.bs.modal', function () {
        $("#info").remove();
    });

    $('#info').modal('show');
}

function MessageWarning(title, data) {
    var _modal = "<div id='info' class='modal fade in' aria-hidden='false' style='display: block;'><div class='modal-dialog'><div class='modal-content'>" +
      "<div class='modal-header bg-warning'><button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button> " +
      "<h4 class='modal-title ng-binding'><strong>" + title + "</strong></h4></div>" +
      "<div class='modal-body'><strong>" + data + "</strong></div>" +
      "<div class='modal-footer'><button type='button' class='btn btn-warning' onclick=\"$('#info').modal('hide');\"><span class=\"glyphicon glyphicon-ok\"></span>&nbsp;<strong>Aceptar</strong></button></div>" +
      "</div></div></div>";

    $("body").append(_modal);

    $('#info').on('hidden.bs.modal', function () {
        $("#info").remove();
    });

    $('#info').modal('show');
}

function Error(titulo, datos) {

    var _modal = "<div id='error' class='modal fade in' role='dialog'><div class='modal-dialog'><div class='modal-content'>" +
      "<div class='modal-header bg-danger'><button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button><h4 class='modal-title'>" + titulo + "</h4></div>" +
      "<div class='modal-body'><p><div class='validation-summary-errors'>" + datos + "</div></p></div>" +
      "<div class='modal-footer'><button type='button' class='btn btn-danger' onclick=\"$('#error').modal('hide');\">Cerrar</button></div>" +
      "</div></div></div>";

    $("body").append(_modal);

    $('#error').on('hidden.bs.modal', function () {
        $("#error").remove();
    });

    $('#error').modal('show');
}

function beforeSendAjax() {

    var _progressBar = "<div class='progress progress-striped active'><div class='progress-bar progress-bar-success'  role='progressbar' style='width: 100%'>";

    var _modal = "<div id='ajax' class='modal fade'><div class='modal-dialog'><div class='modal-content'>" +
      "<div class='modal-body'><p class='text-center'><strong>Espere un momento por favor...<strong></p><p>" + _progressBar + "</p></div>" +
      "</div></div></div>";

    $("body").append(_modal);

    $('#ajax').on('hidden.bs.modal', function () {
        $("#ajax").remove();
    });

    $('#ajax').modal('show');
}

function Confirm(obj, msj, id) {
    $(obj).confirmation({ title: msj, singleton: true, onConfirm: function () { Remove(id, obj); $(obj).confirmation('hide') } });
}

function success(info) {
    if (info.isSuccess) {
        Mensaje("Éxito", info.msj);
        CleanFields();
        if(info.newurl != null || info.newurl != "")
            setTimeout(function () { window.location = info.newurl }, 3000);
    }
    else if (info.isWarning) {
        MessageWarning("Datos no validos.", info.msj);
    }
    else {
        Error("Error", info.msj);
    }
}

function error(info) {
    Error("Error", info);
}

//function Confirm(obj, id) {
//    $(obj).confirmation({ title: "¿Desea elimnar el registro?", singleton: true, onConfirm: function () { Remove(id); $(obj).confirmation('hide') } });
//}


function soloNumeros() {
    var e = event;
    e.returnValue = ((e.keyCode >= 48 && e.keyCode <= 57 || e.keyCode == 32));
}

function soloLetras() {
    var e = event;
    e.returnValue = ((e.keyCode >= 97 && e.keyCode <= 122 || e.keyCode >= 65 && e.keyCode <= 90 || e.keyCode == 32));

}

function soloNumerosyLetras() {
    var e = event;
    e.returnValue = ((e.keyCode >= 48 && e.keyCode <= 57 || e.keyCode >= 97 && e.keyCode <= 122 || e.keyCode >= 65 && e.keyCode <= 90 || e.keyCode == 32));
}

function CleanFields() {
    /*$("input").each(function () {
        $(this).val("");
    });

    $('select').each(function () {
        if ($(this).children().length > 0) {
            $($(this).children()[0]).attr('selected', 'selected');
            $(this).change();
        }
    });*/
    $(':input').not(':button, :submit, :reset, :hidden, :checkbox, :radio').val('');
    $(':checkbox, :radio').prop('checked', false);
}


$(document).ready(function () {
    $(".navbar-toggle").click(function (event) {
        $(".navbar-collapse").toggle('in');
    });

    //$('.typeahead.input-sm').siblings('input.tt-hint').addClass('hint-small');
    //$('.typeahead.input-lg').siblings('input.tt-hint').addClass('hint-large');

    //Check to see if the window is top if not then display button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.scrollToTop').fadeIn();
        } else {
            $('.scrollToTop').fadeOut();
        }
    });

    //Click event to scroll to top
    $('.scrollToTop').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 800);
        return false;
    });

    // Remove active for all items.
    $('.page-sidebar-menu li').removeClass('active');

    // highlight submenu item
    $('li a[href="' + this.location.pathname + '"]').parent().addClass('active');

    // Highlight parent menu item.
    $('ul a[href="' + this.location.pathname + '"]').parents('li').addClass('active');



    //PAGINAR

    $.fn.pageMe = function (opts) {
        var $this = this,
        defaults = {
            perPage: 10,
            showPrevNext: false,
            hidePageNumbers: false
        },
        settings = $.extend(defaults, opts);

        var listElement = $this;
        var perPage = settings.perPage;
        var children = listElement.children();
        var pager = $('.pager');

        if (typeof settings.childSelector != "undefined") {
            children = listElement.find(settings.childSelector);
        }

        if (typeof settings.pagerSelector != "undefined") {
            pager = $(settings.pagerSelector);
        }

        var numItems = children.size();
        var numPages = Math.ceil(numItems / perPage);

        pager.data("curr", 0);

        if (settings.showPrevNext) {
            $('<li><a href="#" class="prev_link">«</a></li>').appendTo(pager);
        }

        var curr = 0;
        while (numPages > curr && (settings.hidePageNumbers == false)) {
            $('<li><a href="#" class="page_link">' + (curr + 1) + '</a></li>').appendTo(pager);
            curr++;
        }

        if (settings.showPrevNext) {
            $('<li><a href="#" class="next_link">»</a></li>').appendTo(pager);
        }

        pager.find('.page_link:first').addClass('active');
        pager.find('.prev_link').hide();
        if (numPages <= 1) {
            pager.find('.next_link').hide();
        }
        pager.children().eq(1).addClass("active");

        children.hide();
        children.slice(0, perPage).show();

        pager.find('li .page_link').click(function () {
            var clickedPage = $(this).html().valueOf() - 1;
            goTo(clickedPage, perPage);
            return false;
        });
        pager.find('li .prev_link').click(function () {
            previous();
            return false;
        });
        pager.find('li .next_link').click(function () {
            next();
            return false;
        });

        function previous() {
            var goToPage = parseInt(pager.data("curr")) - 1;
            goTo(goToPage);
        }

        function next() {
            goToPage = parseInt(pager.data("curr")) + 1;
            goTo(goToPage);
        }

        function goTo(page) {
            var startAt = page * perPage,
            endOn = startAt + perPage;

            children.css('display', 'none').slice(startAt, endOn).show();

            if (page >= 1) {
                pager.find('.prev_link').show();
            }
            else {
                pager.find('.prev_link').hide();
            }

            if (page < (numPages - 1)) {
                pager.find('.next_link').show();
            }
            else {
                pager.find('.next_link').hide();
            }

            pager.data("curr", page);
            pager.children().removeClass("active");
            pager.children().eq(page + 1).addClass("active");

        }
    };



});