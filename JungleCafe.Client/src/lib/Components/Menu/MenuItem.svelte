<script>
    import { slide, fade } from 'svelte/transition';
    import { cubicOut } from 'svelte/easing';
    import { marked } from 'marked';

    export let item;
    let isExpanded = false;

    function toggleDetails() {
        isExpanded = !isExpanded;
    }

    $: formattedDescription = item.description ? marked(item.description) : '';
</script>

<div
        class="bg-white rounded-lg shadow-lg overflow-hidden transition-all duration-500 ease-out
           hover:shadow-xl {isExpanded ? 'ring-1 ring-gray-200' : ''} cursor-pointer"
        transition:fade={{ duration: 200 }}
        on:click={toggleDetails}
        on:keydown={(e) => e.key === 'Enter' && toggleDetails()}
        role="button"
        tabindex="0"
>
    <div class="flex">
        <!-- Image -->
        {#if item.imageUrl}
            <div class="w-64 overflow-hidden">
                <img
                        src={item.imageUrl}
                        alt={item.name}
                        class="w-full h-36 object-cover transition-transform duration-500
                          {isExpanded ? 'scale-105' : ''}"
                />
            </div>
        {/if}

        <!-- Content -->
        <div class="flex-1 p-6">
            <div class="flex justify-between items-start mb-2">
                <div>
                    <h3 class="text-xl font-semibold text-gray-900">
                        {item.name}
                    </h3>
                    <p class="text-sm text-gray-500 mt-1">
                        Click for {isExpanded ? 'less' : 'more'} information
                    </p>
                </div>
                <span class="text-lg font-bold text-jungle-accent">
                    {item.price.toFixed(2)} €
                </span>
            </div>

            {#if isExpanded}
                <div
                        class="space-y-2"
                        transition:slide={{ duration: 300, easing: cubicOut }}
                >
                    <div class="prose prose-gray">
                        {@html formattedDescription}
                    </div>

                    {#if item.allergensInfo}
                        <div class="text-sm text-gray-600 pt-2 border-t">
                            <span class="font-medium">Allergens:</span> {item.allergensInfo}
                        </div>
                    {/if}

                    {#if !item.isAvailable}
                        <div class="text-sm text-red-500 font-medium mt-2">
                            ⚠️ This item is currently unavailable
                        </div>
                    {/if}
                </div>
            {/if}
        </div>
    </div>
</div>

<style>
    :global(.prose) {
        max-width: none;
    }
    :global(.prose p) {
        margin: 0.5em 0;
    }
    :global(.prose ul) {
        margin: 0.8em 0;
        padding-left: 1.5em;
        list-style-type: disc;
    }
    :global(.prose li) {
        margin: 0.3em 0;
    }
    :global(.prose a) {
        color: #2563eb;
        text-decoration: underline;
        transition: color 0.2s;
    }
    :global(.prose a:hover) {
        color: #1d4ed8;
    }
</style>