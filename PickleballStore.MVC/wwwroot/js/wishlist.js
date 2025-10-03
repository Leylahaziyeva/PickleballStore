function addToWishlist(productId) {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch('/Account/AddToWishlist', { 
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: 'productId=' + productId + '&__RequestVerificationToken=' + token  
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Added to wishlist!');
            } else {
                alert('Failed to add product to wishlist.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred while adding the product to the wishlist.');
        });
}

function removeFromWishlist(productId, element) {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch('/Account/RemoveFromWishlist', { 
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: 'id=' + productId + '&__RequestVerificationToken=' + token  
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                element.closest('tr').remove();
            } else {
                alert('Failed to remove product from wishlist.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred while removing the product from the wishlist.');
        });
}
