<script>
    import MenuManagement from './MenuManagement.svelte';
    import AnimalManagement from './AnimalManagement.svelte';
    import EventManagement from './EventManagement.svelte';
    import ReservationManagement from './ReservationManagement.svelte';
    import {authStore} from "../../stores/authStore.js";

    let userRole;
    $: userRole = $authStore.user.role;

    // Active tab state
    let activeTab;
    $: activeTab = userRole === "Caretaker" ? "animals" : "menu";
</script>

<div class="min-h-screen bg-gray-50 pt-20 pb-12 bg-leaves-pattern bg-blend-overlay">
    <div class="max-w-7xl mx-auto px-4">
        <h1 class="text-4xl font-display text-center mb-12 text-jungle-brown">
            Employee Panel
        </h1>

        <!-- Tabs -->
        <div class="border-b border-gray-200 mb-8">
            <nav class="flex -mb-px">
                {#if userRole === 'Employee' || userRole === 'Admin' || userRole === 'Manager'}
                    <button
                        class="mr-8 py-4 px-1 border-b-2 font-medium text-sm
                           {activeTab === 'menu' ?
                             'border-jungle-accent text-jungle-accent' :
                             'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'}"
                        on:click={() => activeTab = 'menu'}
                    >
                        Menu Management
                    </button>
                {/if}
                {#if userRole === 'Caretaker' || userRole === 'Admin' || userRole === 'Manager'}
                    <button
                        class="mr-8 py-4 px-1 border-b-2 font-medium text-sm
                           {activeTab === 'animals' ?
                             'border-jungle-accent text-jungle-accent' :
                             'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'}"
                        on:click={() => activeTab = 'animals'}
                    >
                        Animals Management
                    </button>
                {/if}
                {#if userRole === 'Employee' || userRole === 'Admin' || userRole === 'Manager'}
                    <button
                        class="mr-8 py-4 px-1 border-b-2 font-medium text-sm
                           {activeTab === 'events' ?
                             'border-jungle-accent text-jungle-accent' :
                             'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'}"
                        on:click={() => activeTab = 'events'}
                    >
                        Events Management
                    </button>
                    <button
                        class="mr-8 py-4 px-1 border-b-2 font-medium text-sm
                           {activeTab === 'reservations' ?
                             'border-jungle-accent text-jungle-accent' :
                             'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'}"
                        on:click={() => activeTab = 'reservations'}
                    >
                        Reservations
                    </button>
                {/if}
            </nav>
        </div>

        <!-- Content -->
        <div class="bg-white rounded-xl shadow-lg p-6">
            {#if activeTab === 'menu' && (userRole === 'Employee' || userRole === 'Admin' || userRole === 'Manager')}
                <MenuManagement />
            {:else if activeTab === 'animals' && (userRole === 'Caretaker' || userRole === 'Admin' || userRole === 'Manager')}
                <AnimalManagement />
            {:else if activeTab === 'events' && (userRole === 'Employee' || userRole === 'Admin' || userRole === 'Manager')}
                <EventManagement />
            {:else if activeTab === 'reservations' && (userRole === 'Employee' || userRole === 'Admin' || userRole === 'Manager')}
                <ReservationManagement />
            {/if}
        </div>
    </div>
</div>