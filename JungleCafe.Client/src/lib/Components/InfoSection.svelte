<script>
    export let openingHours = {
        weekdays: "13:00 - 19:00",
        weekend: "13:00 - 20:00"
    };
    export let address = {
        street: "Chmielna 69",
        city: "00-801 Warszawa",
        country: "Poland"
    };
</script>

<div class="info-section">
    <div class="text-container">
        <h2 class="text-heading">Opening Hours:</h2>
        <p class="text-subheading">Monday - Friday: {openingHours.weekdays}</p>
        <p class="text-subheading">Saturday - Sunday: {openingHours.weekend}</p>

        <h2 class="text-heading">Our Location:</h2>
        <p class="text-subheading">
            {address.street}<br>
            {address.city}<br>
            {address.country}
        </p>
    </div>
    <div>
        <iframe
                class="map"
                loading="lazy"
                referrerpolicy="no-referrer-when-downgrade"
                src="https://www.google.com/maps/embed/v1/place?q=Varso+Tower&key=AIzaSyBFw0Qbyq9zTFTd-tUY6dZWTgaQzuU17R8"
        ></iframe>
    </div>
</div>