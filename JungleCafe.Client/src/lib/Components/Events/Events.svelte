<script>
    import { onMount } from 'svelte';
    import { fly } from 'svelte/transition';
    import { backOut } from 'svelte/easing';

    // State management
    let events = [];
    let loading = true;
    let error = null;
    let filter = 'upcoming';

    // Pagination
    let currentPage = 1;
    let eventsPerPage = 6;

    // Fetch events from API
    async function fetchEvents() {
        try {
            loading = true;
            const response = await fetch('/api/events');
            if (!response.ok) throw new Error('Failed to fetch events');
            const data = await response.json();
            events = data;
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    // Filter events based on date
    $: filteredEvents = events.filter(event => {
        const eventDate = new Date(event.startDate);
        const now = new Date();
        return filter === 'upcoming' ? eventDate >= now : eventDate < now;
    });

    // Pagination calculations
    $: totalPages = Math.ceil(filteredEvents.length / eventsPerPage);
    $: paginatedEvents = filteredEvents.slice(
        (currentPage - 1) * eventsPerPage,
        currentPage * eventsPerPage
    );

    function goToPage(page) {
        if (page >= 1 && page <= totalPages) {
            currentPage = page;
        }
    }

    onMount(fetchEvents);
</script>

<div class="min-h-screen bg-gray-50 pt-20">
    <div class="max-w-7xl mx-auto px-4 py-8">
        <h1 class="text-4xl font-display text-center mb-12 text-jungle-brown">
            Jungle Café Events
        </h1>

        <!-- Filter Controls -->
        <div class="flex justify-center space-x-4 mb-8">
            <button
                    class="px-6 py-2 rounded-full font-medium transition-colors
                       {filter === 'upcoming' ?
                         'bg-jungle-accent text-white' :
                         'bg-white text-jungle-brown border border-jungle-accent'}"
                    on:click={() => {
                    filter = 'upcoming';
                    currentPage = 1;
                }}
            >
                Upcoming Events
            </button>
            <button
                    class="px-6 py-2 rounded-full font-medium transition-colors
                       {filter === 'past' ?
                         'bg-jungle-accent text-white' :
                         'bg-white text-jungle-brown border border-jungle-accent'}"
                    on:click={() => {
                    filter = 'past';
                    currentPage = 1;
                }}
            >
                Past Events
            </button>
        </div>

        {#if loading}
            <div class="flex justify-center items-center h-64">
                <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
            </div>
        {:else if error}
            <div class="text-red-500 text-center p-4">
                {error}
            </div>
        {:else if paginatedEvents.length === 0}
            <div class="text-center text-gray-600 p-8">
                No {filter} events found.
            </div>
        {:else}
            <!-- Events Grid -->
            <div class="grid md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
                {#each paginatedEvents as event, i}
                    <div
                            class="bg-white rounded-xl shadow-lg overflow-hidden transform
                               transition duration-300 hover:scale-105"
                            in:fly={{
                            y: 50,
                            duration: 600,
                            delay: i * 150,
                            opacity: 0,
                            easing: backOut
                        }}
                    >
                        {#if event.imageUrl}
                            <img
                                    src={event.imageUrl}
                                    alt={event.title}
                                    class="w-full h-48 object-cover"
                            />
                        {:else}
                            <div class="w-full h-48 bg-jungle-secondary/20 flex items-center justify-center">
                                <span class="text-jungle-secondary">No image available</span>
                            </div>
                        {/if}

                        <div class="p-6">
                            <h3 class="text-xl font-medium text-jungle-brown mb-2">
                                {event.title}
                            </h3>

                            <div class="flex items-center text-gray-600 mb-3">
                                <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                          d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                                </svg>
                                <span>
                                    {new Date(event.startDate).toLocaleDateString('en-GB', {
                                        weekday: 'long',
                                        year: 'numeric',
                                        month: 'long',
                                        day: 'numeric'
                                    })}
                                </span>
                            </div>

                            <p class="text-gray-600 mb-4">
                                {event.description ? event.description.slice(0, 100) : ''}
                                {event.description && event.description.length > 100 ? '...' : ''}
                            </p>

                            <a href={`/events/${event.id}`} class="block w-full text-center jungle-btn-primary">
                                Learn More
                            </a>
                        </div>
                    </div>
                {/each}
            </div>

            <!-- Pagination -->
            {#if totalPages > 1}
                <div class="flex justify-center space-x-2 mt-8">
                    <button
                            class="px-4 py-2 rounded-lg border {currentPage === 1 ? 'bg-gray-100 text-gray-400' : 'hover:bg-gray-50'}"
                            disabled={currentPage === 1}
                            on:click={() => goToPage(currentPage - 1)}
                    >
                        Previous
                    </button>

                    {#each Array(totalPages) as _, i}
                        <button
                                class="px-4 py-2 rounded-lg border {currentPage === i + 1 ? 'bg-jungle-accent text-white' : 'hover:bg-gray-50'}"
                                on:click={() => goToPage(i + 1)}
                        >
                            {i + 1}
                        </button>
                    {/each}

                    <button
                            class="px-4 py-2 rounded-lg border {currentPage === totalPages ? 'bg-gray-100 text-gray-400' : 'hover:bg-gray-50'}"
                            disabled={currentPage === totalPages}
                            on:click={() => goToPage(currentPage + 1)}
                    >
                        Next
                    </button>
                </div>
            {/if}
        {/if}
    </div>
</div>