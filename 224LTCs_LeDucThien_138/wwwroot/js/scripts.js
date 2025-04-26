/*!
    * Start Bootstrap - SB Admin v7.0.7 (https://startbootstrap.com/template/sb-admin)
    * Copyright 2013-2023 Start Bootstrap
    * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-sb-admin/blob/master/LICENSE)
    */

$(window).on('load', function () {
    // Gọi hàm loadLayout khi trang đã tải xong
    $(window).on('load', function () {
        loadLayout(); // Bắt đầu tải các phần
    });

    // Lắng nghe sự kiện click trên nút sidebarToggle
    $('#sidebarToggle').on('click', function (event) {
        event.preventDefault();
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        $('body').toggleClass('sb-sidenav-toggled');
        localStorage.setItem('sb|sidebar-toggle', $('body').hasClass('sb-sidenav-toggled'));
    });
});
