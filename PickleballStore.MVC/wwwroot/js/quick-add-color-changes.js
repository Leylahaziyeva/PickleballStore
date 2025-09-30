document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".popup-quickadd .variant-picker-values input").forEach(input => {
        input.addEventListener("change", function () {
            const label = this.closest(".variant-picker-values").querySelector("label[for='" + this.id + "']");
            if (!label) return;

            const modal = this.closest(".popup-quickadd");
            const imgEl = modal.querySelector(".tf-product-info-item .image img");

            // Change image if variant has an image
            const imageName = label.getAttribute("data-image");
            if (imageName && imgEl) {
                imgEl.src = "/images/products/" + imageName;
            }

            // Update selected variant label
            const variantLabel = this.closest(".variant-picker-item").querySelector(".variant-picker-label-value");
            if (variantLabel) {
                variantLabel.textContent = label.getAttribute("data-value");
            }
        });
    });
});
