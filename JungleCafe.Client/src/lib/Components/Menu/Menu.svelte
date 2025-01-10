<script>
    import { onMount } from 'svelte';
    import { menuStore } from '../../stores/menuStore';
    import MenuItem from './MenuItem.svelte';


    $: groupedItems = $menuStore.items.reduce((acc, item) => {
        if (!acc[item.category]) {
            acc[item.category] = [];
        }
        acc[item.category].push(item);
        return acc;
    }, {});

    onMount(() => {
        menuStore.fetchMenuItems();
    });
</script>

<div class="min-h-screen bg-gray-50 pt-20 bg-leaves-pattern bg-blend-overlay">
    <div class="max-w-7xl mx-auto px-4 py-8">
        <h1 class="text-4xl font-bold text-center mb-12">
            Our Menu
        </h1>


        {#if $menuStore.loading}
            <div class="flex justify-center items-center h-64">
                <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
            </div>
        {:else if $menuStore.error}
            <div class="text-red-500 text-center p-4">
                {$menuStore.error}
            </div>
        {:else}
            {#each Object.entries(groupedItems) as [category, items]}
                <div class="mb-16">
                    <h2 class="text-3xl font-semibold mb-8 capitalize">  <!-- Większy nagłówek i margin -->
                        {category}
                    </h2>
                    <div class="space-y-6">
                        {#each items as item (item.id)}
                            <MenuItem {item} />
                        {/each}
                    </div>
                </div>
            {/each}
        {/if}
    </div>
</div>