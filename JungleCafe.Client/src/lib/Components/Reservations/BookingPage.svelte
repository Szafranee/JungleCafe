<script>
    import { onMount } from 'svelte';
    import TablesLayout from './TablesLayout.svelte';
    import { authStore } from '../../stores/authStore';

    let selectedDate = new Date().toISOString().split('T')[0];
    let selectedTime = "13:00";
    let selectedTable = null;
    let guestCount = 2;
    let specialRequests = "";
    let loading = false;
    let error = null;
    let success = false;

    $: dateTimeChanged = selectedDate && selectedTime;

    async function handleSubmit() {
        if (!$authStore.isAuthenticated) {
            window.location.href = '/login';
            return;
        }

        if (!selectedTable || !selectedDate || !selectedTime) {
            error = "Please select table, date and time";
            return;
        }

        loading = true;
        error = null;
        success = false;

        try {
            const response = await fetch('/api/reservations', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${$authStore.token}`
                },
                body: JSON.stringify({
                    tableId: selectedTable,
                    reservationDate: `${selectedDate}T${selectedTime}`,
                    guestCount,
                    specialRequests,
                    duration: 90
                })
            });

            const data = await response.json();

            console.log(data);

            if (!response.ok) {
                throw new Error(data.message || 'Reservation failed');
            }

            success = true;
            // Reset form
            selectedTable = null;
            specialRequests = "";
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    function handleTableSelect(tableId) {
        selectedTable = tableId;
    }
</script>

<div class="min-h-screen bg-gray-50 pt-20 pb-12 bg-leaves-pattern bg-blend-overlay">
    <div class="max-w-7xl mx-auto px-4">
        <h1 class="text-4xl font-display text-center mb-12 text-jungle-brown">
            Book a Table
        </h1>

        <div class="grid md:grid-cols-2 gap-8">
            <!-- Booking Form -->
            <div class="bg-white rounded-xl shadow-lg p-6">
                <h2 class="text-2xl font-semibold mb-6">Reservation Details</h2>

                <form on:submit|preventDefault={handleSubmit} class="space-y-6">
                    <!-- Date and Time Selection -->
                    <div class="grid grid-cols-2 gap-4">
                        <div>
                            <label for="date" class="block text-sm font-medium text-gray-700">Date</label>
                            <input
                                    type="date"
                                    id="date"
                                    bind:value={selectedDate}
                                    min={new Date().toISOString().split('T')[0]}
                                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                    required
                            >
                        </div>

                        <div>
                            <label for="time" class="block text-sm font-medium text-gray-700">Time</label>
                            <select
                                    id="time"
                                    bind:value={selectedTime}
                                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                    required
                            >
                                {#each Array.from({length: 13}, (_, i) => `${13 + Math.floor(i/2)}:${i % 2 ? '30' : '00'}`) as time}
                                    <option value={time}>{time}</option>
                                {/each}
                            </select>
                        </div>
                    </div>

                    <!-- Number of Guests -->
                    <div>
                        <label for="guests" class="block text-sm font-medium text-gray-700">Number of Guests</label>
                        <input
                                type="number"
                                id="guests"
                                bind:value={guestCount}
                                min="1"
                                max="8"
                                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        >
                    </div>

                    <!-- Special Requests -->
                    <div>
                        <label for="requests" class="block text-sm font-medium text-gray-700">Special Requests</label>
                        <textarea
                                id="requests"
                                bind:value={specialRequests}
                                rows="3"
                                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                placeholder="Any special requests or preferences?"
                        ></textarea>
                    </div>

                    {#if error}
                        <div class="text-red-500 text-sm">{error}</div>
                    {/if}

                    {#if success}
                        <div class="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded">
                            Reservation successful! Check your email for confirmation.
                        </div>
                    {/if}

                    <button
                            type="submit"
                            class="w-full jungle-btn-primary disabled:opacity-50"
                            disabled={loading || !selectedTable}
                    >
                        {#if loading}
                            <div class="inline-block animate-spin h-4 w-4 border-2 border-white rounded-full border-t-transparent mr-2"></div>
                            Processing...
                        {:else}
                            {selectedTable ? 'Confirm Reservation' : 'Please Select a Table'}
                        {/if}
                    </button>
                </form>
            </div>

            <!-- Table Layout -->
            <div>
                <h2 class="text-2xl font-semibold mb-6">Select a Table</h2>
                {#if dateTimeChanged}
                    <TablesLayout
                            onTableSelect={handleTableSelect}
                            selectedDateTime={`${selectedDate}T${selectedTime}`}
                    />
                {/if}
            </div>
        </div>
    </div>
</div>