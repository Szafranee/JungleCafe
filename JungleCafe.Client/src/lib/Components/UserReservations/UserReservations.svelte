<script>
    import { onMount } from 'svelte';
    import { authStore } from '../../stores/authStore.js';

    let reservations = [];
    let loading = false;
    let error = null;

    onMount(() => {
        fetchUserReservations();
    });

    async function fetchUserReservations() {
        try {
            loading = true;
            const response = await fetch('/api/reservations/user', {
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to fetch reservations');
            reservations = await response.json();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    async function handleCancelReservation(id) {
        const reason = prompt('Please enter cancellation reason (optional):');
        if (reason === null) return; // User clicked Cancel

        try {
            loading = true;
            const response = await fetch(`/api/reservations/${id}/cancel`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${$authStore.token}`
                },
                body: JSON.stringify({ reason })
            });

            if (!response.ok) throw new Error('Failed to cancel reservation');
            await fetchUserReservations();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }
</script>

<div class="min-h-screen bg-gray-50 pt-20 pb-12 bg-leaves-pattern bg-blend-overlay">
    <div class="max-w-7xl mx-auto px-4">
        <h1 class="text-4xl font-display text-center mb-12 text-jungle-brown">
            My Reservations
        </h1>

        {#if error}
            <div class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                {error}
            </div>
        {/if}

        {#if loading}
            <div class="flex justify-center py-8">
                <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
            </div>
        {:else if reservations.length === 0}
            <div class="text-center text-gray-600 py-8">
                You don't have any reservations yet.
            </div>
        {:else}
            <div class="bg-white rounded-lg shadow-lg overflow-hidden">
                <div class="overflow-x-auto">
                    <table class="min-w-full divide-y divide-gray-200">
                        <thead class="bg-gray-50">
                        <tr>
                            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Date & Time</th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Table</th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Guests</th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Notes</th>
                            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase w-52">Actions</th>
                        </tr>
                        </thead>
                        <tbody class="divide-y divide-gray-200">
                        {#each reservations as reservation}
                            <tr class="hover:bg-gray-50">
                                <td class="px-6 py-4 whitespace-nowrap text-sm">
                                    {new Date(reservation.reservationDate).toLocaleString('en-GB', {
                                        year: 'numeric',
                                        month: '2-digit',
                                        day: '2-digit',
                                        hour: '2-digit',
                                        minute: '2-digit'
                                    })}
                                </td>
                                <td class="px-6 py-4 whitespace-nowrap text-sm">
                                    {reservation.tableNumber}
                                </td>
                                <td class="px-6 py-4 whitespace-nowrap text-sm">
                                    {reservation.guestCount}
                                </td>
                                <td class="px-6 py-4 whitespace-nowrap">
                                        <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full
                                            {reservation.status === 'confirmed' ? 'bg-green-100 text-green-800' :
                                             reservation.status === 'cancelled' ? 'bg-red-100 text-red-800' :
                                             'bg-gray-100 text-gray-800'}">
                                            {reservation.status}
                                        </span>
                                </td>
                                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                    {reservation.notes || 'No notes'}
                                </td>
                                <td class="px-6 py-4 whitespace-nowrap text-sm">
                                    {#if reservation.status === 'confirmed'}
                                        <button
                                                class="inline-flex items-center justify-center px-2 py-1 bg-red-100 text-red-700 rounded-full text-xs font-medium hover:bg-red-200 transition-colors"
                                                on:click={() => handleCancelReservation(reservation.id)}
                                        >
                                            <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                                            </svg>
                                            <span class="ml-1">Cancel reservation</span>
                                        </button>
                                    {/if}
                                </td>
                            </tr>
                        {/each}
                        </tbody>
                    </table>
                </div>
            </div>
        {/if}
    </div>
</div>