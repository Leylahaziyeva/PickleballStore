document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".popup-quickview .variant-picker-values input").forEach(input => {
        input.addEventListener("change", function () {
            const label = this.closest(".variant-picker-values").querySelector("label[for='" + this.id + "']");
            const imageName = label.getAttribute("data-image");

            if (imageName) {
                const modal = this.closest(".modal-content");
                const mainImage = modal.querySelector(".tf-product-media-wrap img");

                if (mainImage) {
                    mainImage.src = "/images/products/" + imageName;
                }
            }
        });
    });
});
