const basketTotalPrice = document.getElementById("basketTotalPrice");
const basketContainer = document.getElementById("basketContainer");
const basketTotalCount = document.getElementById("basketTotalCount");
const basketTotalBadge = document.getElementById("basketTotalBadge");

function loadBasket() {
    fetch('/basket/getbasket', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            console.log("Basket:", data);
            basketContainer.innerHTML = "";

            if (!data.items || data.items.length === 0) {
                basketContainer.innerHTML = '<div class="tf-mini-cart-item"><p class="text-center">Your cart is empty</p></div>';
                if (basketTotalCount) basketTotalCount.innerText = 0;
                if (basketTotalBadge) basketTotalBadge.innerText = 0;

                const subtotalElement = document.querySelector('.tf-totals-total-value');
                if (subtotalElement) {
                    subtotalElement.textContent = '$0.00 USD';
                }
                return;
            }

            data.items.forEach(item => {
                let variantHtml = '';
                if (item.variant && item.variant.optionName && item.variant.optionValue) {
                    variantHtml = `<div class="meta-variant">${item.variant.optionName}: ${item.variant.optionValue}</div>`;
                }

                basketContainer.innerHTML += `
          <div class="tf-mini-cart-item">
        <div class="tf-mini-cart-image">
            <a href="/product/details/${item.productId}">
                <img src="/images/products/${item.imageName}" alt="${item.productName}">
            </a>
        </div>
        <div class="tf-mini-cart-info">
            <a class="title link" href="/product/details/${item.productId}">${item.productName}</a>
            ${variantHtml}
            <div class="price fw-6">$${item.price.toFixed(2)}</div>
            <div class="tf-mini-cart-btns">
                <div class="wg-quantity small">
                    <span class="btn-quantity minus-btn" data-variant-id="${item.variant?.id || 0}">-</span>
                    <input type="text" name="number" value="${item.quantity}" readonly>
                    <span class="btn-quantity plus-btn" data-variant-id="${item.variant?.id || 0}">+</span>
                </div>
                <div class="tf-mini-cart-remove" data-variant-id="${item.variant?.id || 0}">Remove</div>
            </div>
        </div>
    </div>`;
            });

            if (basketTotalCount) basketTotalCount.innerText = data.totalCount;
            if (basketTotalBadge) basketTotalBadge.innerText = data.totalCount;

            const subtotalElement = document.querySelector('.tf-totals-total-value');
            if (subtotalElement) {
                subtotalElement.textContent = `$${data.totalPrice.toFixed(2)} USD`;
            }
        })
        .catch(error => {
            console.error('Error loading basket:', error);
        });
}
function addToBasket(variantId, quantity = 1) {
    if (!variantId) {
        alert("Please select a variant first.");
        return;
    }

    fetch(`/basket/add?variantId=${variantId}&quantity=${quantity}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                loadBasket();
            } else {
                alert('Failed to add product to basket.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred while adding the product to the basket.');
        });
}
function removeFromBasket(variantId) {
    fetch(`/basket/remove?variantId=${variantId}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                loadBasket();
            } else {
                alert('Failed to remove product from basket.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred while removing the product from the basket.');
        });
}

function changeQuantity(variantId, change) {
    console.log(`Changing quantity for variant ${variantId} by ${change}`);

    fetch(`/basket/changequantity?variantId=${variantId}&quantity=${change}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                loadBasket();
            } else {
                alert('Failed to change quantity.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred while changing the quantity.');
        });
}

document.addEventListener("click", function (e) {
    if (e.target.classList.contains("tf-mini-cart-remove")) {
        let variantId = e.target.getAttribute("data-variant-id");
        console.log("Removing variant:", variantId);
        removeFromBasket(variantId);
    }

    if (e.target.classList.contains("plus-btn")) {
        let variantId = e.target.getAttribute("data-variant-id");
        console.log("Increasing quantity for variant:", variantId);
        changeQuantity(variantId, 1);
    }

    if (e.target.classList.contains("minus-btn")) {
        let variantId = e.target.getAttribute("data-variant-id");
        console.log("Decreasing quantity for variant:", variantId);
        changeQuantity(variantId, -1);
    }
});
function initializeVariantSelection() {
    document.querySelectorAll('input[type="radio"][name^="color-"]').forEach(radio => {
        radio.addEventListener('change', function () {
            const variantId = this.dataset.variantId;
            const radioName = this.getAttribute('name');

            let hiddenInputId;
            if (radioName.includes('quickview')) {
                const productId = radioName.split('-').pop();
                hiddenInputId = `selectedVariantId-quickview-${productId}`;
            } else {
                const productId = radioName.split('-').pop();
                hiddenInputId = `selectedVariantId-${productId}`;
            }

            const hiddenInput = document.getElementById(hiddenInputId);
            if (hiddenInput) {
                hiddenInput.value = variantId;
                console.log(`Selected variant ID: ${variantId} for ${hiddenInputId}`);
            }
        });
    });
}

document.addEventListener("DOMContentLoaded", function () {
    loadBasket();
    initializeVariantSelection();
});


//function changeQuantity(productId, change) {

//    const productIdInput = document.getElementById(`productId${productId}`);
//    const currentQuantity = parseInt(productIdInput.value);
//    const cartContainer = document.getElementById("cartContainer");

//    if (currentQuantity + change < 1) {
//        return; // Prevent quantity from going below 1
//    }

//    productIdInput.value = currentQuantity + change;

//    fetch(`/cart/changeQuantity?productId=${productId}&change=${change}`, {
//        method: 'POST'
//    })
//        .then(response => response.json())
//        .then(data => {
//            console.log(data);
//            cartContainer.innerHTML = data.cartHtml;
//            loadBasket1(data.basketViewModel);
//        })
//        .catch(error => console.error('Error:', error));
//}