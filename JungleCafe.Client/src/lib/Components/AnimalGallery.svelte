<script>
    export let animals = [
        {
            image: "/img/ara_araruna.jpeg",
            name: "Mary",
            species: "Blue-and-yellow Macaw",
            description: "Our charming resident who loves to greet guests"
        },
        {
            image: "/img/chameleon.jpg",
            name: "Max",
            species: "Veiled Chameleon",
            description: "Watch him change colors throughout the day"
        },
        {
            image: "/img/hedgehog.jpeg",
            name: "Filip",
            species: "African Pygmy Hedgehog",
            description: "The cutest little ball of spikes you'll ever meet"
        }
    ];
</script>

<section class="py-16 px-4 bg-gray-50">
    <div class="max-w-7xl mx-auto">
        <h2 class="text-4xl font-display text-center text-jungle-brown mb-12">
            Meet Our Furry & Feathered Friends
        </h2>

        <div class="grid md:grid-cols-3 gap-8">
            {#each animals as animal}
                <div
                        class="bg-white rounded-xl shadow-lg overflow-hidden transform
                 transition hover:scale-105 hover:shadow-xl"
                >
                    <img
                            src={animal.image}
                            alt={animal.species}
                            class="w-full h-64 object-cover"
                    />
                    <div class="p-6">
                        <h3 class="text-xl font-medium text-jungle-brown">{animal.name}</h3>
                        <p class="text-jungle-secondary">{animal.species}</p>
                        <p class="text-gray-600 mt-2">{animal.description}</p>
                    </div>
                </div>
            {/each}
        </div>
    </div>
</section>