


$('.nav-links .dropdown').click(function () {
    if ($('nav-links .dropdown').hasClass('open')) {
        $(this).removeClass('open');
    }
    else {
        $(this).addClass('open');
    }
});



$(function () {
    $(".navigation li a").each(function () {
        if ($(this).attr("href") == window.location.pathname) {
            $(this).addClass("current");
        }
    });
});



/* sidebar*/



$('.navigation').find('li.active').parents('li').addClass('active');
$('.navigation').find('li').not('.active').has('ul').children('ul').addClass('hidden-ul');
$('.navigation').find('li').has('ul').children('a').parent('li').addClass('has-ul');


$(document).on('click', '.sidebar-toggle', function (e) {
    e.preventDefault();

    $('body').toggleClass('sidebar-narrow');

    if ($('body').hasClass('sidebar-narrow')) {
        $('.navigation').children('li').children('ul').css('display', '');

        $('.sidebar-content').hide().delay().queue(function () {
            $(this).show().addClass('animated fadeIn').clearQueue();
        });
    }

    else {
        $('.navigation').children('li').children('ul').css('display', 'none');
        $('.navigation').children('li.active').children('ul').css('display', 'block');

        $('.sidebar-content').hide().delay().queue(function () {
            $(this).show().addClass('animated fadeIn').clearQueue();
        });
    }
});



