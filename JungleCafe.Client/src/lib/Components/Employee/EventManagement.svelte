<script>
    import {onMount} from 'svelte';
    import {authStore} from '../../stores/authStore';

    // State for events list and form
    let events = [];
    let editingEvent = null;
    let loading = false;
    let error = null;

    let errorElement;

    // Form state
    let eventForm = {
        title: '',
        description: '',
        startDate: '',
        endDate: '',
        maxParticipants: '',
        imageUrl: '',
        isPublic: true,
    };

    // Reset form and editing state
    function resetForm() {
        editingEvent = null;
        eventForm = {
            title: '',
            description: '',
            startDate: '',
            endDate: '',
            maxParticipants: '',
            imageUrl: '',
            isPublic: true,
        };
    }

    // Load events on component mount
    onMount(async () => {
        await fetchEvents();
    });

    // Fetch all events from API
    async function fetchEvents() {
        try {
            loading = true;
            const response = await fetch('/api/events', {
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to fetch events');
            events = await response.json();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    // Handle form submission (create/update)
    async function handleSubmit() {
        try {
            loading = true;
            const url = editingEvent
                ? `/api/events/${editingEvent}`
                : '/api/events';

            const method = editingEvent ? 'PUT' : 'POST';

            const response = await fetch(url, {
                method,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${$authStore.token}`
                },
                body: JSON.stringify(eventForm)
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                throw new Error(errorMessage);
            }

            await fetchEvents();
            resetForm();
        } catch (err) {
            error = err.message;
            // Scroll to error message
            setTimeout(() => {
                errorElement?.scrollIntoView({behavior: 'smooth', block: 'start'});
            }, 100);
        } finally {
            loading = false;
        }
    }

    // Handle event deletion
    async function handleDelete(id) {
        if (!confirm('Are you sure you want to delete this event?')) return;

        try {
            loading = true;
            const response = await fetch(`/api/events/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to delete event');
            await fetchEvents();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    let formElement;

    // Start editing an event
    function startEditing(event) {
        editingEvent = event.id;
        eventForm = {
            title: event.title,
            description: event.description,
            startDate: new Date(event.startDate).toISOString().slice(0, 16),
            endDate: new Date(event.endDate).toISOString().slice(0, 16),
            maxParticipants: event.maxParticipants,
            imageUrl: event.imageUrl,
            isPublic: event.isPublic,
        };

        // Scroll to form
        setTimeout(() => {
            formElement.scrollIntoView({behavior: 'smooth', block: 'start'});
        }, 50);
    }
</script>

<div>
    <div class="flex justify-between mb-6">
        <h2 class="text-2xl font-semibold">Events</h2>
        <button
                class="jungle-btn-primary"
                on:click={() => {
                    resetForm();
                    setTimeout(() => {
                        formElement.scrollIntoView({behavior: 'smooth', block: 'start'});
                    }, 50);
                }}
        >
            Add New Event
        </button>
    </div>

    <!-- Error display -->
    {#if error}
        <div
                bind:this={errorElement}
                class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4"
        >
            {error}
        </div>
    {/if}

    <!-- Events List -->
    {#if loading}
        <div class="flex justify-center py-8">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
        </div>
    {:else}
        <div class="mb-8 grid md:grid-cols-2 gap-6">
            {#each events as event}
                <div class="bg-white p-4 rounded-lg border border-gray-200 shadow-sm">
                    <div class="relative">
                        <!-- Buttons in top-right corner -->
                        <div class="absolute top-0 right-0 flex gap-4">
                            <button
                                    class="text-jungle-accent hover:text-jungle-secondary"
                                    on:click={() => startEditing(event)}
                            >
                                Edit
                            </button>
                            <button
                                    class="text-red-600 hover:text-red-800"
                                    on:click={() => handleDelete(event.id)}
                            >
                                Delete
                            </button>
                        </div>

                        <!-- Event content -->
                        <div class="pt-8">
                            <h3 class="font-medium text-lg">{event.title}</h3>
                            <p class="text-sm text-gray-500">
                                {new Date(event.startDate).toLocaleDateString()} -
                                {new Date(event.endDate).toLocaleDateString()}
                            </p>
                            {#if event.maxParticipants}
                                <p class="text-sm text-jungle-secondary">
                                    Max participants: {event.maxParticipants}
                                </p>
                            {/if}
                            <p class="text-sm text-gray-600 mt-2">{event.description}</p>

                            <!-- Creator info -->
                            <div class="border-t border-gray-200 pt-4 mt-4">
                                <div class="flex flex-col gap-1">
                                    <p class="text-sm text-gray-600">
                                        <span class="font-medium">Created by: {event.creator.firstName} {event.creator.lastName}</span>
                                    </p>
                                    <p class="text-sm text-gray-400">{event.creator.email}</p>
                                </div>
                            </div>
                        </div>

                        {#if event.imageUrl}
                            <img
                                    src={event.imageUrl}
                                    alt={event.title}
                                    class="mt-4 w-full h-48 object-cover rounded"
                            />
                        {/if}
                    </div>
                </div>
            {/each}
        </div>
    {/if}

    <!-- Event Form -->
    <form
            bind:this={formElement}
            on:submit|preventDefault={handleSubmit}
            class="space-y-6 bg-gray-50 p-6 rounded-lg border border-gray-200"
    >
        <h3 class="font-medium text-xl text-jungle-brown mb-4">
            {editingEvent ? 'Edit Event' : 'Add New Event'}
        </h3>

        <div class="space-y-8">
            <!-- Basic Info Section -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Basic Information</h4>
                <div class="grid grid-cols-1 gap-6">
                    <!-- Title field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="title" class="block text-sm font-medium text-gray-700 mb-2">Title</label>
                        <input
                                type="text"
                                id="title"
                                bind:value={eventForm.title}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        />
                    </div>

                    <!-- Description field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="description"
                               class="block text-sm font-medium text-gray-700 mb-2">Description</label>
                        <textarea
                                id="description"
                                bind:value={eventForm.description}
                                rows="3"
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                placeholder="Enter event description..."
                        ></textarea>
                    </div>
                </div>
            </div>

            <!-- Date and Time Section -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Date and Time</h4>
                <div class="grid grid-cols-2 gap-6">
                    <!-- Start Date field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="startDate" class="block text-sm font-medium text-gray-700 mb-2">Start Date</label>
                        <input
                                type="datetime-local"
                                id="startDate"
                                bind:value={eventForm.startDate}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        />
                    </div>

                    <!-- End Date field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="endDate" class="block text-sm font-medium text-gray-700 mb-2">End Date</label>
                        <input
                                type="datetime-local"
                                id="endDate"
                                bind:value={eventForm.endDate}
                                min={eventForm.startDate}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        />
                    </div>
                </div>
            </div>

            <!-- Additional Details Section -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Additional Details</h4>
                <div class="grid grid-cols-2 gap-6">
                    <!-- Max Participants field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="maxParticipants" class="block text-sm font-medium text-gray-700 mb-2">
                            Maximum Participants
                        </label>
                        <input
                                type="number"
                                id="maxParticipants"
                                bind:value={eventForm.maxParticipants}
                                min="1"
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                        />
                    </div>

                    <!-- Image URL field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="imageUrl" class="block text-sm font-medium text-gray-700 mb-2">Image URL</label>
                        <input
                                type="url"
                                id="imageUrl"
                                bind:value={eventForm.imageUrl}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                placeholder="Enter image URL..."
                        />
                    </div>
                </div>

                <!-- Public/Private toggle -->
                <div class="bg-white p-4 rounded border border-gray-200 mt-6">
                    <label class="flex items-center cursor-pointer">
                        <input
                                type="checkbox"
                                bind:checked={eventForm.isPublic}
                                class="rounded border-gray-300 text-jungle-accent focus:ring-jungle-accent mr-2 h-4 w-4"
                        />
                        <span class="text-sm font-medium text-gray-700">Make this event public</span>
                    </label>
                </div>
            </div>
        </div>

        <!-- Form buttons -->
        <div class="flex justify-end gap-4 pt-4 border-t border-gray-200 mt-8">
            {#if editingEvent}
                <button
                        type="button"
                        class="jungle-btn-outline"
                        on:click={resetForm}
                >
                    Cancel
                </button>
            {/if}
            <button
                    type="submit"
                    class="jungle-btn-primary"
                    disabled={loading}
            >
                {#if loading}
                    <div class="inline-block animate-spin h-4 w-4 border-2 border-white rounded-full border-t-transparent mr-2"></div>
                    Processing...
                {:else}
                    {editingEvent ? 'Update Event' : 'Add Event'}
                {/if}
            </button>
        </div>
    </form>
</div>