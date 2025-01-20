<script>
    import {fade, fly} from 'svelte/transition';
    import {backOut} from 'svelte/easing';
    import {onMount} from "svelte";

    // State management
    let allAnimals = [];
    let displayedAnimals = [];
    let loading = false;
    let error = null;

    // fetch animals from an API
    async function fetchAnimals() {
        try {
            loading = true;
            const response = await fetch('/api/animals');
            if (!response.ok) throw new Error('Failed to fetch animals');
            allAnimals = await response.json();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }


    let isExpanded = false;

    // Calculate which animals to display
    $: displayedAnimals = isExpanded ? allAnimals : allAnimals.slice(0, 3);

    onMount(fetchAnimals);
</script>


<section id="animals" class="py-16 px-4 bg-gray-50">
    <div class="max-w-7xl mx-auto">
        <h2 class="text-4xl font-display text-center text-jungle-brown mb-12">
            Meet Our Furry & Feathered Friends
        </h2>

        {#if loading}
            <div class="flex justify-center items-center h-64">
                <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
            </div>
        {:else if error}
            <div class="text-red-500 text-center p-4">
                {error}
            </div>
        {/if}

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
                            src={animal.imageUrl}
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