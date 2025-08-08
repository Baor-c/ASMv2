// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Product card functionality
document.addEventListener('DOMContentLoaded', function() {
    // Check for product cards and ensure they're visible
    const productCards = document.querySelectorAll('.product-card');
    
    if (productCards.length > 0) {
        console.log(`Found ${productCards.length} product cards`);
        
        // Add animation and ensure visibility for each product card
        productCards.forEach((card, index) => {
            // Ensure card is visible
            card.style.opacity = '1';
            
            // Add a small delay for each card to create a staggered effect
            setTimeout(() => {
                card.classList.add('visible');
            }, index * 100);
            
            // Check if product image exists
            const productImg = card.querySelector('.product-img');
            if (productImg) {
                // Add error handling for images
                productImg.onerror = function() {
                    console.log('Product image failed to load, applying placeholder');
                    this.src = '/images/placeholder-food.jpg';
                    this.onerror = null; // Prevent infinite loop
                };
            }
        });
    }
});
