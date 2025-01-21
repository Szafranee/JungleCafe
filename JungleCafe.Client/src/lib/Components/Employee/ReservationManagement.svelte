<script>
    import {onMount} from 'svelte';
    import {authStore} from '../../stores/authStore';

    // State for reservations
    let reservations = [];
    let loading = false;
    let error = null;
    let editingReservation = null;
    let reservationForm = {
        reservationDate: '',
        guestCount: 2,
        notes: '',
    };
    let formElement;

    // Filters and sorting state
    let searchTerm = '';
    let statusFilter = 'all';
    let dateFilter = 'all';
    let sortBy = 'date';
    let sortDirection = 'desc';
    let cancellationReason = '';

    onMount(() => {
        fetchReservations();
    });

    // Fetch all reservations
    async function fetchReservations() {
        try {
            loading = true;
            const response = await fetch('/api/reservations/', {
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

    // Cancel reservation
    async function handleCancelReservation(id) {
        const cancellationReason = prompt('Please enter cancellation reason:');
        if (!cancellationReason) return;

        try {
            loading = true;
            const response = await fetch(`/api/reservations/${id}/cancel`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${$authStore.token}`
                },
                body: JSON.stringify({reason: cancellationReason})
            });

            if (!response.ok) throw new Error('Failed to cancel reservation');
            await fetchReservations();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    // Edit reservation
    function startEditing(reservation) {
        editingReservation = reservation.id;
        reservationForm = {
            reservationDate: new Date(reservation.reservationDate).toISOString().slice(0, 16),
            guestCount: reservation.guestCount,
            notes: reservation.notes || '',
        };

        // scroll to form
        setTimeout(() => {
            formElement.scrollIntoView({behavior: 'smooth', block: 'start'});
        }, 100);
    }

    function resetForm() {
        editingReservation = null;
        reservationForm = {
            reservationDate: '',
            guestCount: 0,
            notes: '',
        };
    }

    async function handleSubmitEdit(event) {
        event.preventDefault();

        console.log(reservationForm);
        try {
            loading = true;
            const response = await fetch(`/api/reservations/${editingReservation}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${$authStore.token}`
                },
                body: JSON.stringify(reservationForm)
            });

            if (!response.ok) throw new Error('Failed to update reservation');

            await fetchReservations();
            resetForm();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    // Complete reservation
    async function completeReservation(id) {
        try {
            loading = true;
            const response = await fetch(`/api/reservations/${id}/complete`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to complete reservation');
            await fetchReservations();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    // Filtered and sorted reservations
    $: filteredReservations = reservations
        .filter(res => {
            // Search filter
            const searchLower = searchTerm.toLowerCase();
            const matchesSearch =
                res.email?.toLowerCase().includes(searchLower) ||
                res.firstName.toLowerCase().includes(searchLower) ||
                res.lastName.toLowerCase().includes(searchLower) ||
                res.phoneNumber?.toLowerCase().includes(searchLower) ||
                res.tableNumber.toLowerCase().includes(searchLower) ||
                res.status.toLowerCase().includes(searchLower)

            // Status filter
            const matchesStatus = statusFilter === 'all' || res.status === statusFilter;

            // Date filter
            const date = new Date(res.reservationDate);
            const today = new Date();
            const isToday = date.toDateString() === today.toDateString();
            const isFuture = date > today;

            const matchesDate =
                dateFilter === 'all' ||
                (dateFilter === 'today' && isToday) ||
                (dateFilter === 'future' && isFuture) ||
                (dateFilter === 'past' && !isFuture && !isToday);

            return matchesSearch && matchesStatus && matchesDate;
        })
        .sort((a, b) => {
            let comparison = 0;
            switch (sortBy) {
                case 'date':
                    comparison = new Date(a.reservationDate).getTime() - new Date(b.reservationDate).getTime();
                    break;
                case 'table':
                    comparison = a.tableId - b.tableId;
                    break;
                case 'guest':
                    comparison = a.email.localeCompare(b.email);
                    break;
                case 'status':
                    comparison = a.status.localeCompare(b.status);
                    break;
            }
            return sortDirection === 'desc' ? -comparison : comparison;
        });

</script>

<div>
    <h2 class="text-2xl font-semibold mb-6">Reservations Management</h2>

    <!-- Error display -->
    {#if error}
        <div class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
        </div>
    {/if}

    <!-- Filters Section -->
    <div class="bg-white p-6 rounded-lg border border-gray-200 mb-6">
        <div class="grid md:grid-cols-2 lg:grid-cols-4 gap-4">
            <!-- Search filter -->
            <div>
                <label for="search" class="block text-sm font-medium text-gray-700 mb-2">Search</label>
                <input
                        type="text"
                        id="search"
                        placeholder="Search reservations..."
                        bind:value={searchTerm}
                        class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                />
            </div>

            <!-- Status filter -->
            <div>
                <label for="status" class="block text-sm font-medium text-gray-700 mb-2">Status</label>
                <select
                        id="status"
                        bind:value={statusFilter}
                        class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                >
                    <option value="all">All Statuses</option>
                    <option value="confirmed">Confirmed</option>
                    <option value="cancelled">Cancelled</option>
                    <option value="completed">Completed</option>
                </select>
            </div>

            <!-- Date filter -->
            <div>
                <label for="date" class="block text-sm font-medium text-gray-700 mb-2">Date</label>
                <select
                        id="date"
                        bind:value={dateFilter}
                        class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                >
                    <option value="all">All Dates</option>
                    <option value="today">Today</option>
                    <option value="future">Future</option>
                    <option value="past">Past</option>
                </select>
            </div>

            <!-- Sort -->
            <div>
                <label for="sort" class="block text-sm font-medium text-gray-700 mb-2">Sort by</label>
                <div class="flex gap-2">
                    <select
                            id="sort"
                            bind:value={sortBy}
                            class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                    >
                        <option value="date">Date</option>
                        <option value="table">Table</option>
                        <option value="guest">Guest</option>
                        <option value="status">Status</option>
                    </select>
                    <button
                            class="px-3 border rounded-md hover:bg-gray-50"
                            on:click={() => sortDirection = sortDirection === 'asc' ? 'desc' : 'asc'}
                    >
                        {sortDirection === 'asc' ? '↑' : '↓'}
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- Edit Form -->
    {#if editingReservation}
        <div bind:this={formElement} class="bg-gray-50 p-6 rounded-lg border border-gray-200 mb-6">
            <h3 class="font-medium text-xl text-jungle-brown mb-6">
                Edit Reservation
            </h3>

            <form on:submit|preventDefault={handleSubmitEdit} class="space-y-8">
                <!-- Date and Time Section -->
                <div>
                    <h4 class="font-medium text-gray-700 mb-4">Date and Time</h4>
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="reservationDate" class="block text-sm font-medium text-gray-700 mb-2">
                            Reservation Date and Time
                        </label>
                        <input
                                type="datetime-local"
                                id="reservationDate"
                                bind:value={reservationForm.reservationDate}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        />
                    </div>
                </div>

                <!-- Guest Count Section -->
                <div>
                    <h4 class="font-medium text-gray-700 mb-4">Guest Information</h4>
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="guestCount" class="block text-sm font-medium text-gray-700 mb-2">
                            Number of Guests
                        </label>
                        <input
                                type="number"
                                id="guestCount"
                                bind:value={reservationForm.guestCount}
                                min="1"
                                max="8"
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        />
                    </div>
                </div>

                <!-- Notes Section -->
                <div>
                    <h4 class="font-medium text-gray-700 mb-4">Additional Information</h4>
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="notes" class="block text-sm font-medium text-gray-700 mb-2">
                            Notes
                        </label>
                        <textarea
                                id="notes"
                                bind:value={reservationForm.notes}
                                rows="3"
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                placeholder="Any special requests or notes..."
                        ></textarea>
                    </div>
                </div>

                <!-- Form buttons -->
                <div class="flex justify-end gap-4 pt-4 border-t border-gray-200">
                    <button
                            type="button"
                            class="jungle-btn-outline"
                            on:click={resetForm}
                    >
                        Cancel
                    </button>
                    <button
                            type="submit"
                            class="jungle-btn-primary"
                            disabled={loading}
                    >
                        {#if loading}
                            <div class="inline-block animate-spin h-4 w-4 border-2 border-white rounded-full border-t-transparent mr-2"></div>
                            Processing...
                        {:else}
                            Update Reservation
                        {/if}
                    </button>
                </div>
            </form>
        </div>
    {/if}

    <!-- Reservations Table -->
    {#if loading}
        <div class="flex justify-center py-8">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
        </div>
    {:else}
        <div class="bg-white rounded-lg border border-gray-200 overflow-hidden">
            <div class="overflow-x-auto">
                <table class="min-w-full divide-y divide-gray-200">
                    <thead class="bg-gray-50">
                    <tr>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Date & Time
                        </th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Table
                        </th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Guests
                        </th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Reserved By
                        </th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Status
                        </th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Notes
                        </th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Cancellation reason
                        </th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Actions
                        </th>
                    </tr>
                    </thead>
                    <tbody class="bg-white divide-y divide-gray-200">
                    {#each filteredReservations as reservation}
                        <tr class="hover:bg-gray-50">
                            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                {new Date(reservation.reservationDate).toLocaleString('en-GB')}
                            </td>
                            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                {reservation.tableNumber}
                            </td>
                            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                {reservation.guestCount}
                            </td>
                            <td class="px-6 py-4 whitespace-nowrap">
                                <div class="text-sm text-gray-900">{reservation.firstName} {reservation.lastName}</div>
                                <div class="text-sm text-gray-500">{reservation.email}</div>
                                {#if reservation.phoneNumber}
                                    <div class="text-sm text-gray-500">{reservation.phoneNumber}</div>
                                {/if}
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
                                {#if reservation.notes}
                                    <div class="text-xs mt-1">Notes: {reservation.notes}</div>
                                {:else}
                                    <div class="text-xs mt-1 text-gray-500">No notes</div>
                                {/if}
                            </td>
                            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                {#if reservation.status === 'cancelled'}
                                    {reservation.cancellationReason}
                                {:else}
                                    <div class="text-xs mt-1 text-gray-500">Not cancelled</div>
                                {/if}
                            </td>
                            <td class="px-6 py-4 whitespace-nowrap text-base font-bold">
                                {#if reservation.status === 'confirmed'}
                                    <div class="mt-2">
                                        <button
                                                class="text-red-600 hover:text-red-900"
                                                on:click={() => handleCancelReservation(reservation.id)}
                                        >
                                            Cancel
                                        </button>
                                    </div>
                                    <div class="mt-2">
                                        <button
                                                class="text-jungle-accent hover:text-jungle-secondary"
                                                on:click={() => completeReservation(reservation.id)}
                                        >
                                            Complete
                                        </button>
                                    </div>
                                {/if}
                                <div class="mt-2">
                                    <button
                                            class="text-indigo-600 hover:text-indigo-900"
                                            on:click={() => startEditing(reservation)}
                                    >
                                        Edit
                                    </button>
                                </div>
                            </td>
                        </tr>
                    {/each}
                    </tbody>
                </table>
            </div>

            {#if filteredReservations.length === 0}
                <div class="text-center py-8 text-gray-500">
                    No reservations found matching your filters.
                </div>
            {/if}
        </div>
    {/if}
</div>