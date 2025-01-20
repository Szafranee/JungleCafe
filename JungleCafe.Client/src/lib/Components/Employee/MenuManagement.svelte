<script>
    import { onMount } from 'svelte';
    import { authStore } from '../../stores/authStore';

    // State for menu items list and form
    let menuItems = [];
    let editingMenuItem = null;
    let loading = false;
    let error = null;

    // Form state
    let menuForm = {
        name: '',
        category: '',
        description: '',
        price: '',
        allergensInfo: '',
        isAvailable: true
    };

    // Menu categories
    const categories = [
        'Coffee',
        'Tea',
        'Snacks',
        'Desserts',
    ];

    // Fetch menu items on component mount
    onMount(async () => {
        await fetchMenuItems();
    });

    // Fetch all menu items from the API
    async function fetchMenuItems() {
        try {
            loading = true;
            const response = await fetch('/api/menu', {
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to fetch menu items');
            menuItems = await response.json();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    // Handle form submission (create or update)
    async function handleSubmit() {
        try {
            loading = true;
            const url = editingMenuItem
                ? `/api/menu/${editingMenuItem}`
                : '/api/menu';

            const method = editingMenuItem ? 'PUT' : 'POST';

            const response = await fetch(url, {
                method,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${$authStore.token}`
                },
                body: JSON.stringify(menuForm)
            });

            if (!response.ok) throw new Error(`Failed to ${editingMenuItem ? 'update' : 'create'} menu item`);

            // Refresh menu items list
            await fetchMenuItems();

            // Reset form
            resetForm();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    // Handle menu item deletion
    async function handleDelete(id) {
        if (!confirm('Are you sure you want to delete this item?')) return;

        try {
            loading = true;
            const response = await fetch(`/api/menu/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to delete menu item');

            await fetchMenuItems();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    // Reset form and editing state
    function resetForm() {
        editingMenuItem = null;
        menuForm = {
            name: '',
            category: '',
            description: '',
            price: '',
            allergensInfo: '',
            isAvailable: true
        };
    }

    // Start editing a menu item
    let formElement;

    function startEditing(item) {
        editingMenuItem = item.id;
        menuForm = { ...item };

        setTimeout(() => {
            formElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }, 100);
    }
</script>

<div>
    <div class="flex justify-between mb-6">
        <h2 class="text-2xl font-semibold">Menu Items</h2>
        <button
                class="jungle-btn-primary"
                on:click={resetForm}
        >
            Add New Item
        </button>
    </div>

    <!-- Error display -->
    {#if error}
        <div class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
        </div>
    {/if}

    <!-- Menu Items List -->
    {#if loading}
        <div class="flex justify-center py-8">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
        </div>
    {:else}
        <div class="mb-8">
            {#each menuItems as item}
                <div class="border-b border-gray-200 py-4 flex justify-between items-center">
                    <div>
                        <h3 class="font-medium">{item.name}</h3>
                        <p class="text-sm text-gray-500">
                            {item.category} - ${item.price}
                            {#if !item.isAvailable}
                                <span class="text-red-500 ml-2">(Unavailable)</span>
                            {/if}
                        </p>
                    </div>
                    <div class="flex gap-2">
                        <button
                                class="text-jungle-accent hover:text-jungle-secondary"
                                on:click={() => startEditing(item)}
                        >
                            Edit
                        </button>
                        <button
                                class="text-red-600 hover:text-red-800"
                                on:click={() => handleDelete(item.id)}
                        >
                            Delete
                        </button>
                    </div>
                </div>
            {/each}
        </div>
    {/if}

    <!-- Menu Form -->
    <form
            bind:this={formElement}
            on:submit|preventDefault={handleSubmit}
            class="space-y-6 bg-gray-50 p-6 rounded-lg border border-gray-200"
    >
        <h3 class="font-medium text-xl text-jungle-brown mb-4">
            {editingMenuItem ? 'Edit Menu Item' : 'Add New Menu Item'}
        </h3>

        <!-- Form content -->
        <div class="space-y-8"> <!-- Increased spacing between sections -->
            <!-- Basic Info Section -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Basic Information</h4>
                <div class="grid grid-cols-2 gap-6">
                    <!-- Name field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="name" class="block text-sm font-medium text-gray-700 mb-2">Name</label>
                        <input
                                type="text"
                                id="name"
                                bind:value={menuForm.name}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        />
                    </div>

                    <!-- Category field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="category" class="block text-sm font-medium text-gray-700 mb-2">Category</label>
                        <select
                                id="category"
                                bind:value={menuForm.category}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        >
                            <option value="">Select category</option>
                            {#each categories as category}
                                <option value={category}>{category}</option>
                            {/each}
                        </select>
                    </div>
                </div>
            </div>

            <!-- Price and Availability Section -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Price & Availability</h4>
                <div class="grid grid-cols-2 gap-6">
                    <!-- Price field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="price" class="block text-sm font-medium text-gray-700 mb-2">Price ($)</label>
                        <input
                                type="number"
                                id="price"
                                bind:value={menuForm.price}
                                step="0.01"
                                min="0"
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        />
                    </div>

                    <!-- Availability toggle -->
                    <div class="bg-white p-4 rounded border border-gray-200 flex items-center">
                        <label for="isAvailable" class="flex items-center cursor-pointer">
                            <input
                                    type="checkbox"
                                    id="isAvailable"
                                    bind:checked={menuForm.isAvailable}
                                    class="rounded border-gray-300 text-jungle-accent focus:ring-jungle-accent mr-2 h-4 w-4"
                            />
                            <span class="text-sm font-medium text-gray-700">Available for ordering</span>
                        </label>
                    </div>
                </div>
            </div>

            <!-- Details Section -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Additional Details</h4>
                <!-- Description field -->
                <div class="bg-white p-4 rounded border border-gray-200 mb-4">
                    <label for="description" class="block text-sm font-medium text-gray-700 mb-2">Description</label>
                    <textarea
                            id="description"
                            bind:value={menuForm.description}
                            rows="3"
                            class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                            placeholder="Enter item description..."
                    ></textarea>
                </div>

                <!-- Allergens field -->
                <div class="bg-white p-4 rounded border border-gray-200">
                    <label for="allergensInfo" class="block text-sm font-medium text-gray-700 mb-2">Allergens Information</label>
                    <input
                            type="text"
                            id="allergensInfo"
                            bind:value={menuForm.allergensInfo}
                            class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                            placeholder="e.g., Contains nuts, dairy, gluten..."
                    />
                </div>
            </div>
        </div>

        <!-- Form buttons -->
        <div class="flex justify-end gap-4 pt-4 border-t border-gray-200 mt-8">
            {#if editingMenuItem}
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
                    {editingMenuItem ? 'Update Item' : 'Add Item'}
                {/if}
            </button>
        </div>
    </form>
</div>