<script>
    import { fade, fly } from 'svelte/transition';
    import { backOut } from 'svelte/easing';

    // All available animals
    const allAnimals = [
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
        },
        // Additional animals for expanded view
        {
            image: "/img/african_grey_parrot.jpg",
            name: "Rio",
            species: "African Grey Parrot",
            description: "The smartest bird you'll ever meet, loves to chat"
        },
        {
            image: "/img/leopard_gecko.jpg",
            name: "Leo",
            species: "Leopard Gecko",
            description: "Our resident sun-lover with the cutest smile"
        },
        {
            image: "/img/sugar_glider.jpg",
            name: "Sugar",
            species: "Sugar Glider",
            description: "The most playful little creature you'll ever meet"
        },
        {
            image: "/img/capybara.jpg",
            name: "Cappy",
            species: "Capybara",
            description: "The world's largest rodent, but also the friendliest"
        }
        // Add more animals as needed...
    ];

    let isExpanded = false;

    // Calculate which animals to display
    $: displayedAnimals = isExpanded ? allAnimals : allAnimals.slice(0, 3);

    // Function for external control
    export function expand() {
        isExpanded = true;
    }
</script>


<section id="animals" class="py-16 px-4 bg-gray-50">
    <div class="max-w-7xl mx-auto">
        <h2 class="text-4xl font-display text-center text-jungle-brown mb-12">
            Meet Our Furry & Feathered Friends
        </h2>

        <div class="grid md:grid-cols-3 gap-8">
            {#each displayedAnimals as animal, i}
                <div
                        class="bg-white rounded-xl shadow-lg overflow-hidden transform
                 transition duration-300 hover:scale-105 hover:shadow-xl"
                        in:fly={{
            y: 50,
            duration: 600,
            delay: i * 150,
            opacity: 0,
            easing: backOut
          }}
                        out:fade={{
            duration: 200
          }}
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

        {#if !isExpanded}
            <div
                    class="text-center mt-8"
                    in:fly={{ y: 20, duration: 100, delay: 100 }}
            >
                <button
                        on:click={() => isExpanded = true}
                        class="bg-jungle-accent text-white px-8 py-3 rounded-full font-medium
                 hover:bg-jungle-secondary transition-all duration-300 shadow-md
                 hover:shadow-lg transform hover:scale-105
                 active:scale-95"
                >
                    View All Animals
                </button>
            </div>
            {:else}
            <div
                    class="text-center mt-8"
                    in:fly={{ y: 20, duration: 400, delay: 600 }}
            >
                <button
                        on:click={() => isExpanded = false}
                        class="bg-jungle-accent text-white px-8 py-3 rounded-full font-medium
                 hover:bg-jungle-secondary transition-all duration-300 shadow-md
                 hover:shadow-lg transform hover:scale-105
                 active:scale-95"
                >
                    Show Less
                </button>
            </div>
        {/if}
    </div>
</section>