function addToWishlist(productId, element) {
    const wishlistDiv = element.closest('.tf-product-btn-wishlist');

    fetch('/Wishlist/Toggle', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId })
    })
        .then(res => res.json())
        .then(data => {
            if (data.success && data.isAdded) {
                wishlistDiv.classList.add('active');
                wishlistDiv.querySelector('.icon-heart').style.display = 'none';
                wishlistDiv.querySelector('.icon-delete').style.display = 'inline-block';
                updateWishlistCount();
                showNotification('Added to wishlist');
            }
        })
        .catch(error => console.error(error));
}

function removeFromWishlist(productId, element) {
    const wishlistDiv = element.closest('.tf-product-btn-wishlist');

    fetch('/Wishlist/Remove', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId })
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                wishlistDiv.classList.remove('active');
                wishlistDiv.querySelector('.icon-heart').style.display = 'inline-block';
                wishlistDiv.querySelector('.icon-delete').style.display = 'none';
                updateWishlistCount();
                showNotification('Removed from wishlist');
            }
        })
        .catch(error => console.error(error));
}

function updateWishlistCount() {
    fetch('/Wishlist/GetCount')
        .then(res => res.json())
        .then(data => {
            const countEl = document.querySelector('.count-box');
            if (countEl) {
                countEl.textContent = data.count;
                countEl.style.display = data.count > 0 ? 'inline-block' : 'none';
            }
        })
        .catch(error => console.error(error));
}

function initializeWishlistForAllProducts() {
    document.querySelectorAll('.tf-product-btn-wishlist').forEach(btn => {
        const productId = btn.dataset.productId;
        fetch(`/Wishlist/IsInWishlist?productId=${productId}`)
            .then(res => res.json())
            .then(data => {
                const heartIcon = btn.querySelector('.icon-heart');
                const deleteIcon = btn.querySelector('.icon-delete');

                if (data.isInWishlist) {
                    btn.classList.add('active');
                    heartIcon.style.display = 'none';
                    deleteIcon.style.display = 'inline-block';
                } else {
                    btn.classList.remove('active');
                    heartIcon.style.display = 'inline-block';
                    deleteIcon.style.display = 'none';
                }
            });
    });
}

function showNotification(message) {
    console.log(message);
    alert(message); 
}

document.addEventListener("DOMContentLoaded", () => {
    initializeWishlistForAllProducts(); 
    updateWishlistCount();               
});