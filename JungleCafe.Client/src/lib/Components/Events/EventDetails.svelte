<script>
    import {onMount} from 'svelte';
    import {authStore} from '../../stores/authStore';
    import {fade} from 'svelte/transition';

    export let eventId;

    let event = null;
    let loading = true;
    let error = null;
    let showSuccessAlert = false;
    let registrationInProgress = false;

    async function fetchEventDetails() {
        try {
            console.log('Fetching event details for ID:', eventId); // Debug log
            loading = true;
            const response = await fetch(`/api/events/${eventId}`);
            console.log('Response status:', response.status); // Debug log

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            console.log('Event data received:', data); // Debug log
            event = data;
        } catch (err) {
            console.error('Error fetching event:', err); // Debug log
            error = err.message;
        } finally {
            loading = false;
        }
    }

    console.log('Auth store state:', $authStore); // Debug log

    onMount(() => {
        console.log('Component mounted, event ID:', eventId); // Debug log
        fetchEventDetails();
    });

    async function handleRegistration() {
        console.log('Registration clicked, auth state:', $authStore);
        if (!$authStore.isAuthenticated) {
            console.log('User not authenticated, redirecting...');
            window.location.href = '/login';
            return;
        }

        console.log('Registering for event', eventId);

        try {
            registrationInProgress = true;
            console.log('Registering for event', eventId);
            const response = await fetch('/api/events/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${$authStore.token}`
                },
                body: JSON.stringify({
                    eventId: eventId
                })
            });

            if (!response.ok) throw new Error('Registration failed');

            showSuccessAlert = true;
            setTimeout(() => {
                showSuccessAlert = false;
            }, 5000);
        } catch (err) {
            error = err.message;
        } finally {
            registrationInProgress = false;
        }
    }

    function isEventPast(eventDate) {
        return new Date(eventDate) < new Date();
    }
</script>

<div class="min-h-screen bg-gray-50 pt-20 bg-leaves-pattern bg-blend-overlay">
    <div class="max-w-4xl mx-auto px-4 py-8">
        {#if showSuccessAlert}
            <div
                    class="mb-6 bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded relative"
                    transition:fade
            >
                <span class="block sm:inline">
                    Registration successful! Check your email for confirmation details.
                </span>
            </div>
        {/if}

        {#if loading}
            <div class="flex justify-center items-center h-64">
                <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
            </div>
        {:else if error}
            <div class="text-red-500 text-center p-4">
                {error}
            </div>
        {:else if event}
            <div class="bg-white rounded-xl shadow-lg overflow-hidden">
                {#if event.imageUrl}
                    <img
                            src={event.imageUrl}
                            alt={event.title}
                            class="w-full h-64 object-cover"
                    />
                {/if}

                <div class="p-8">
                    <h1 class="text-3xl font-bold text-jungle-brown mb-4">
                        {event.title}
                    </h1>

                    <div class="flex items-center text-gray-600 mb-6">
                        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                  d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"/>
                        </svg>
                        <div>
                            <div>
                                {new Date(event.startDate).toLocaleDateString('en-GB', {
                                    weekday: 'long',
                                    year: 'numeric',
                                    month: 'long',
                                    day: 'numeric'
                                })}
                            </div>
                            <div class="text-sm">
                                {new Date(event.startDate).toLocaleTimeString('en-GB', {
                                    hour: '2-digit',
                                    minute: '2-digit'
                                })} -
                                {new Date(event.endDate).toLocaleTimeString('en-GB', {
                                    hour: '2-digit',
                                    minute: '2-digit'
                                })}
                            </div>
                        </div>
                    </div>

                    <div class="prose max-w-none mb-8">
                        {event.description}
                    </div>

                    {#if event.maxParticipants}
                        <div class="bg-jungle-accent/10 text-jungle-accent rounded-lg p-4 mb-8">
                            <strong>Limited capacity:</strong> {event.maxParticipants} participants maximum
                        </div>
                    {/if}

                    {#if !isEventPast(event.startDate)}
                        <button
                                on:click={handleRegistration}
                                disabled={registrationInProgress}
                                class="w-full jungle-btn-primary disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            {#if registrationInProgress}
                                <div class="inline-block animate-spin h-4 w-4 border-2 border-white rounded-full border-t-transparent mr-2"></div>
                                Processing...
                            {:else}
                                {$authStore.isAuthenticated ? 'Register for Event' : 'Sign in to Register'}
                            {/if}
                        </button>
                    {:else}
                        <div class="text-gray-500 text-center p-4 bg-gray-100 rounded-lg">
                            This event has already taken place
                        </div>
                    {/if}
                </div>
            </div>
        {/if}
    </div>
</div>