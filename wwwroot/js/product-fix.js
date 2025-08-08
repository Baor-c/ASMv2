// Đảm bảo sản phẩm luôn hiển thị
document.addEventListener('DOMContentLoaded', function() {
    // Đảm bảo các sản phẩm luôn hiển thị ngay lập tức
    const productCards = document.querySelectorAll('.product-card');
    productCards.forEach(card => {
        card.style.opacity = '1';
    });
    
    // Xử lý AOS nếu được sử dụng
    if (typeof AOS !== 'undefined') {
        // Khởi tạo AOS với tùy chọn
        AOS.init({
            once: true,
            duration: 800,
            offset: 100
        });
    } else {
        // Bỏ qua các hiệu ứng AOS nếu thư viện không được tải
        document.querySelectorAll('[data-aos]').forEach(el => {
            el.removeAttribute('data-aos');
            el.removeAttribute('data-aos-delay');
        });
    }
});
