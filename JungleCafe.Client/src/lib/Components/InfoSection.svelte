<script>
    export let features = [
        {
            title: "Unique Atmosphere",
            description: "Enjoy your coffee in the company of amazing exotic animals"
        },
        {
            title: "Premium Coffee",
            description: "Carefully selected beans and expert baristas"
        },
        {
            title: "Animal Welfare",
            description: "All our animals are properly cared for by professionals"
        }
    ];
</script>

<section class="py-16 px-4 md:px-8">
    <div class="max-w-7xl mx-auto">
        <div class="grid md:grid-cols-2 gap-12 items-center">
            <div class="space-y-8">
                <h2 class="text-3xl md:text-4xl font-bold text-gray-800">
                    Why Choose Jungle Café?
                </h2>
                <div class="space-y-6">
                    {#each features as feature}
                        <div class="flex items-start">
                            <div class="bg-jungle-green/10 p-3 rounded-full">
                                <svg
                                        class="h-6 w-6 text-jungle-green"
                                        fill="none"
                                        stroke="currentColor"
                                        viewBox="0 0 24 24"
                                        xmlns="http://www.w3.org/2000/svg"
                                >
                                    <path
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                            stroke-width="2"
                                            d="M5 13l4 4L19 7"
                                    ></path>
                                </svg>
                            </div>
                            <div class="ml-4">
                                <h3 class="text-xl font-bold text-gray-800">{feature.title}</h3>
                                <p class="text-gray-600">{feature.description}</p>
                            </div>
                        </div>
                    {/each}
                </div>
            </div>
            <div class="space-y-6 bg-white p-8 rounded-xl shadow-lg">
                <div class="space-y-2">
                    <h3 class="text-xl font-bold text-gray-800">Opening Hours</h3>
                    <p class="text-gray-600">Monday - Friday: 13:00 - 19:00</p>
                    <p class="text-gray-600">Saturday - Sunday: 13:00 - 20:00</p>
                </div>
                <div class="space-y-2">
                    <h3 class="text-xl font-bold text-gray-800">Location</h3>
                    <p class="text-gray-600">
                        Chmielna 69<br>
                        00-801 Warszawa<br>
                        Poland
                    </p>
                </div>
                <iframe title="Jungle Café Location"
                        class="w-full h-64 rounded-lg mt-4"
                        loading="lazy"
                        referrerpolicy="no-referrer-when-downgrade"
                        src="https://www.google.com/maps/embed/v1/place?q=Varso+Tower&key=AIzaSyBFw0Qbyq9zTFTd-tUY6dZWTgaQzuU17R8"
                ></iframe>
            </div>
        </div>
    </div>
</section>