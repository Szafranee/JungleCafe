<script>
    import {onMount} from 'svelte';

    let tables = [];
    let selectedTableId = null;
    let loading = true;
    let error = null;

    // Props
    export let onTableSelect = (tableId) => {
    };
    export let selectedDateTime = null;  // Dodajemy nowy prop

    onMount(async () => {
        fetchAvailableTables();
    });

    // Nasłuchuj zmian w selectedDateTime
    $: if (selectedDateTime) {
        fetchAvailableTables();
    }

    async function fetchAvailableTables() {
        try {
            loading = true;
            const response = await fetch(`/api/tables/available?dateTime=${selectedDateTime}`);
            if (!response.ok) throw new Error('Failed to fetch tables');
            tables = await response.json();
            loading = false;
        } catch (err) {
            error = err.message;
            loading = false;
        }
    }

    function handleTableSelection(number) {
        const table = getTableInfo(number);
        console.log('Selected table:', table); // Debug log
        if (table?.isAvailable) {
            selectedTableId = table.id;
            onTableSelect(table.id);
        }
    }

    function getTableInfo(number) {
        return tables.find(t => t.number === number);
    }
</script>

<div class="p-6 bg-white rounded-lg shadow-sm">
    {#if loading}
        <div class="flex justify-center items-center h-64">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
        </div>
    {:else if error}
        <div class="text-red-500 text-center p-4">{error}</div>
    {:else}
        <!-- Legend -->
        <div class="flex gap-4 mb-6">
            <div class="flex items-center">
                <div class="w-4 h-4 bg-green-500 rounded mr-2"></div>
                <span>Available</span>
            </div>
            <div class="flex items-center">
                <div class="w-4 h-4 bg-red-500 rounded mr-2"></div>
                <span>Occupied</span>
            </div>
            <div class="flex items-center">
                <div class="w-4 h-4 bg-blue-500 rounded mr-2"></div>
                <span>Selected</span>
            </div>
        </div>

        <div class="relative w-[900px] h-[350px] mx-auto">
            <!-- Left section with Main tables -->
            <div class="inline-block">
                <!-- Top row window tables -->
                <div class="flex gap-4 mb-16 ml-4">
                    {#each [1, 2, 3] as i}
                        {@const table = getTableInfo(`W${i}`)}
                        {@const isSelected = selectedTableId === table?.id}
                        <button
                                class="w-16 h-16 rounded flex flex-col items-center justify-center text-white
                                {isSelected ? 'bg-blue-500' : (table?.isAvailable ? 'bg-green-500' : 'bg-red-500')}
                                {table?.isAvailable ? 'hover:opacity-90' : 'cursor-not-allowed'}"
                                on:click={() => handleTableSelection(`W${i}`)}
                        >
                        {#if i < 10}
                                <span class="font-bold text-sm">Window 0{i}</span>
                            {:else}
                                <span class="font-bold text-sm">Window {i}</span>
                            {/if}
                            <span>{table?.capacity} seats</span>
                        </button>
                    {/each}
                </div>

                <!-- Main tables -->
                <div class="grid grid-cols-2 gap-4">
                    {#each Array(6) as _, i}
                        {@const table = getTableInfo(`M${i + 1}`)}
                        {@const isSelected = selectedTableId === table?.id}
                        <button
                                class="w-16 h-16 rounded flex flex-col items-center justify-center text-white
                                {isSelected ? 'bg-blue-500' : (table?.isAvailable ? 'bg-green-500' : 'bg-red-500')}
                                {table?.isAvailable ? 'hover:opacity-90' : 'cursor-not-allowed'}"
                                on:click={() => handleTableSelection(`M${i + 1}`)}
                        >
                        {#if i < 9}
                                <span class="font-bold text-sm">Main 0{i + 1}</span>
                            {:else}
                                <span class="font-bold text-sm">Main {i + 1}</span>
                            {/if}
                            <span>{table?.capacity} seats</span>
                        </button>
                    {/each}
                </div>
            </div>

            <!-- Right side window tables -->
            <div class="absolute top-0 right-96 left-64 flex flex-col gap-4">
                {#each [4, 5, 6] as i}
                    {@const table = getTableInfo(`W${i}`)}
                    {@const isSelected = selectedTableId === table?.id}
                    <button
                            class="w-16 h-16 rounded flex flex-col items-center justify-center text-white
                            {isSelected ? 'bg-blue-500' : (table?.isAvailable ? 'bg-green-500' : 'bg-red-500')}
                            {table?.isAvailable ? 'hover:opacity-90' : 'cursor-not-allowed'}"
                            on:click={() => handleTableSelection(`W${i}`)}
                    >
                        {#if i < 10}
                            <span class="font-bold text-sm">Window 0{i}</span>
                        {:else}
                            <span class="font-bold text-sm">Window {i}</span>
                        {/if}
                        <span>{table?.capacity} seats</span>
                    </button>
                {/each}
            </div>

            <!-- Terrace tables -->
            <div class="absolute top-0 right-72 left-96 grid grid-cols-2 gap-4">
                {#each Array(8) as _, i}
                    {@const table = getTableInfo(`T${i + 1}`)}
                    {@const isSelected = selectedTableId === table?.id}
                    <button
                            class="w-16 h-16 rounded flex flex-col items-center justify-center text-white
                            {isSelected ? 'bg-blue-500' : (table?.isAvailable ? 'bg-green-500' : 'bg-red-500')}
                            {table?.isAvailable ? 'hover:opacity-90' : 'cursor-not-allowed'}"
                            on:click={() => handleTableSelection(`T${i + 1}`)}
                    >
                        {#if i < 9}
                            <span class="font-bold text-sm">Terrace 0{i + 1}</span>
                        {:else}
                            <span class="font-bold text-sm">Terrace {i + 1}</span>
                        {/if}
                        <span>{table?.capacity} seats</span>
                    </button>
                {/each}
            </div>
        </div>
    {/if}
</div>