(function ($) {

    "use strict";

    var initPreloader = function () {
        $(document).ready(function ($) {
            var Body = $('body');
            Body.addClass('preloader-site');
        });
        $(window).on('load', function () {
            $('.preloader-wrapper').fadeOut();
            $('body').removeClass('preloader-site');
        });
    }

    // init Chocolat light box
    var initChocolat = function () {
        Chocolat(document.querySelectorAll('.image-link'), {
            imageSize: 'contain',
            loop: true,
        })
    }

    var initSwiper = function () {

        var swiper = new Swiper(".main-swiper", {
            speed: 500,
            pagination: {
                el: ".swiper-pagination",
                clickable: true,
            },
        });

        var category_swiper = new Swiper(".category-carousel", {
            slidesPerView: 6,
            spaceBetween: 30,
            speed: 500,
            navigation: {
                nextEl: ".category-carousel-next",
                prevEl: ".category-carousel-prev",
            },
            breakpoints: {
                0: {
                    slidesPerView: 2,
                },
                768: {
                    slidesPerView: 3,
                },
                991: {
                    slidesPerView: 4,
                },
                1500: {
                    slidesPerView: 6,
                },
            }
        });

        var brand_swiper = new Swiper(".brand-carousel", {
            slidesPerView: 4,
            spaceBetween: 30,
            speed: 500,
            navigation: {
                nextEl: ".brand-carousel-next",
                prevEl: ".brand-carousel-prev",
            },
            breakpoints: {
                0: {
                    slidesPerView: 2,
                },
                768: {
                    slidesPerView: 2,
                },
                991: {
                    slidesPerView: 3,
                },
                1500: {
                    slidesPerView: 4,
                },
            }
        });

        var products_swiper = new Swiper(".products-carousel", {
            slidesPerView: 5,
            spaceBetween: 30,
            speed: 500,
            navigation: {
                nextEl: ".products-carousel-next",
                prevEl: ".products-carousel-prev",
            },
            breakpoints: {
                0: {
                    slidesPerView: 1,
                },
                768: {
                    slidesPerView: 3,
                },
                991: {
                    slidesPerView: 4,
                },
                1500: {
                    slidesPerView: 6,
                },
            }
        });
    }

    var initProductQty = function () {
        // SỬA LỖI: Dùng event delegation để đảm bảo các nút tăng/giảm hoạt động chính xác
        // Xử lý nút cộng (+)
        $(document).on('click', '.quantity-right-plus', function (e) {
            e.preventDefault();
            var $quantityContainer = $(this).closest('.product-qty');
            var $quantityInput = $quantityContainer.find('input[name="quantity"]');
            var currentVal = parseInt($quantityInput.val());
            if (!isNaN(currentVal)) {
                $quantityInput.val(currentVal + 1);
            }
        });

        // Xử lý nút trừ (-)
        $(document).on('click', '.quantity-left-minus', function (e) {
            e.preventDefault();
            var $quantityContainer = $(this).closest('.product-qty');
            var $quantityInput = $quantityContainer.find('input[name="quantity"]');
            var currentVal = parseInt($quantityInput.val());
            if (!isNaN(currentVal) && currentVal > 1) {
                $quantityInput.val(currentVal - 1);
            }
        });
    }

    // init jarallax parallax
    var initJarallax = function () {
        jarallax(document.querySelectorAll(".jarallax"));

        jarallax(document.querySelectorAll(".jarallax-keep-img"), {
            keepImg: true,
        });
    }

    // document ready
    $(document).ready(function () {

        initPreloader();
        initSwiper();
        initProductQty();
        initJarallax();
        initChocolat();

    }); // End of a document

})(jQuery);