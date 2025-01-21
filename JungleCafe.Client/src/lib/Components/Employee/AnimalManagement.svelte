<script>
    import {onMount} from 'svelte';
    import {authStore} from '../../stores/authStore';

    // State for animals list and form
    let animals = [];
    let editingAnimal = null;
    let loading = false;
    let error = null;
    let formElement;

    // Form state
    let animalForm = {
        name: '',
        species: '',
        category: '',
        description: '',
        imageUrl: '',
        isActive: true,
        caretakerId: null
    };

    function resetForm() {
        editingAnimal = null;
        animalForm = {
            id: null,
            name: '',
            species: '',
            category: '',
            description: '',
            imageUrl: '',
            isActive: true,
            caretakerId: null
        };
    }

    // Available animal categories
    const categories = [
        'Reptile',
        'Bird',
        'Mammal'
    ];

    // List of caretakers (staff members)
    let caretakers = [];

    onMount(async () => {
        await Promise.all([
            fetchAnimals(),
            fetchCaretakers()
        ]);
    });

    async function fetchAnimals() {
        try {
            loading = true;
            const response = await fetch('/api/animals', {
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to fetch animals');
            animals = await response.json();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    async function fetchCaretakers() {
        try {
            const response = await fetch('/api/users/caretakers', {
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to fetch caretakers');
            caretakers = await response.json();
        } catch (err) {
            error = err.message;
        }
    }

    function startEditing(animal) {
        editingAnimal = animal.id;
        animalForm = {
            id: animal.id,
            name: animal.name,
            species: animal.species,
            category: animal.category,
            description: animal.description,
            imageUrl: animal.imageUrl,
            isActive: animal.isActive,
            caretakerId: animal.caretakerId
        };

        // Scroll to form with offset
        setTimeout(() => {
            formElement.scrollIntoView({behavior: 'smooth', block: 'start'});
        }, 50);
    }

    async function handleSubmit() {
        try {
            loading = true;
            const url = editingAnimal
                ? `/api/animals/${editingAnimal}`
                : '/api/animals';

            const method = editingAnimal ? 'PUT' : 'POST';

            const response = await fetch(url, {
                method,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${$authStore.token}`
                },
                body: JSON.stringify(animalForm)
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Failed to ${editingAnimal ? 'update' : 'create'} animal: ${errorText}`);
            }

            await fetchAnimals();
            resetForm();
        } catch (err) {
            error = err.message;
            console.error("Full error:", err);
        } finally {
            loading = false;
        }
    }

    async function handleDelete(id) {
        if (!confirm('Are you sure you want to delete this animal?')) return;

        try {
            loading = true;
            const response = await fetch(`/api/animals/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${$authStore.token}`
                }
            });

            if (!response.ok) throw new Error('Failed to delete animal');
            await fetchAnimals();
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    function getCaretakerName(id) {
        const caretaker = caretakers.find(c => c.id === id);
        return caretaker ? `${caretaker.firstName} ${caretaker.lastName}` : 'Unknown';
    }
</script>

<div class="mb-12">
    <div class="flex justify-between mb-6">
        <h2 class="text-2xl font-semibold">Animals</h2>
        <button
                class="jungle-btn-primary"
                on:click={resetForm}
        >
            Add New Animal
        </button>
    </div>

    <!-- Error display -->
    {#if error}
        <div class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
        </div>
    {/if}

    <!-- Animals List -->
    {#if loading}
        <div class="flex justify-center py-8">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-jungle-accent"></div>
        </div>
    {:else}
        <div class="mb-8 grid md:grid-cols-2 gap-6">
            {#each animals as animal}
                <div class="bg-white p-6 rounded-lg border border-gray-200 shadow-sm">
                    <div class="flex justify-between items-start">
                        <div>
                            <h3 class="font-medium text-lg text-jungle-brown">{animal.name}</h3>
                            <p class="text-sm text-gray-600 mt-1">
                                {animal.species}
                            </p>
                            <p class="text-sm text-jungle-secondary mt-1">
                                {animal.category}
                            </p>
                            <p class="text-sm text-gray-500 mt-2">
                                Caretaker: {getCaretakerName(animal.caretakerId)}
                            </p>
                            {#if animal.description}
                                <p class="mt-4 text-gray-600 text-sm">{animal.description}</p>
                            {/if}
                            {#if !animal.isActive}
                                <span class="inline-block mt-2 px-2 py-1 text-xs font-medium bg-red-100 text-red-800 rounded">
                                    Inactive
                                </span>
                            {/if}
                        </div>
                        <div class="flex gap-3">
                            <button
                                    class="text-jungle-accent hover:text-jungle-secondary transition-colors"
                                    on:click={() => startEditing(animal)}
                            >
                                Edit
                            </button>
                            <button
                                    class="text-red-600 hover:text-red-800 transition-colors"
                                    on:click={() => handleDelete(animal.id)}
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                    {#if animal.imageUrl}
                        <img
                                src={animal.imageUrl}
                                alt={animal.name}
                                class="mt-4 w-full h-48 object-cover rounded-lg"
                        />
                    {/if}
                </div>
            {/each}
        </div>
    {/if}

    <!-- Animal Form -->
    <div bind:this={formElement} class="bg-gray-50 p-6 rounded-lg border border-gray-200">
        <h3 class="font-medium text-xl text-jungle-brown mb-6">
            {editingAnimal ? 'Edit Animal' : 'Add New Animal'}
        </h3>

        <form on:submit|preventDefault={handleSubmit} class="space-y-8">
            <!-- Basic Information -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Basic Information</h4>
                <div class="grid grid-cols-2 gap-6">
                    <!-- Name field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="name" class="block text-sm font-medium text-gray-700 mb-2">Name</label>
                        <input
                                type="text"
                                id="name"
                                bind:value={animalForm.name}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                                placeholder="Enter animal name"
                        />
                    </div>

                    <!-- Species field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="species" class="block text-sm font-medium text-gray-700 mb-2">Species</label>
                        <input
                                type="text"
                                id="species"
                                bind:value={animalForm.species}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                                placeholder="Enter species name"
                        />
                    </div>
                </div>
            </div>

            <!-- Category and Caretaker -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Category & Care Information</h4>
                <div class="grid grid-cols-2 gap-6">
                    <!-- Category field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="category" class="block text-sm font-medium text-gray-700 mb-2">Category</label>
                        <select
                                id="category"
                                bind:value={animalForm.category}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        >
                            <option value="">Select category</option>
                            {#each categories as category}
                                <option value={category}>{category}</option>
                            {/each}
                        </select>
                    </div>

                    <!-- Caretaker field -->
                    <div class="bg-white p-4 rounded border border-gray-200">
                        <label for="caretakerId" class="block text-sm font-medium text-gray-700 mb-2">Caretaker</label>
                        <select
                                id="caretakerId"
                                bind:value={animalForm.caretakerId}
                                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                                required
                        >
                            <option value="">Select caretaker</option>
                            {#each caretakers as caretaker}
                                <option value={caretaker.id}>
                                    {caretaker.firstName} {caretaker.lastName}
                                </option>
                            {/each}
                        </select>
                    </div>
                </div>
            </div>

            <!-- Additional Details -->
            <div>
                <h4 class="font-medium text-gray-700 mb-4">Additional Details</h4>

                <!-- Description field -->
                <div class="bg-white p-4 rounded border border-gray-200 mb-6">
                    <label for="description" class="block text-sm font-medium text-gray-700 mb-2">Description</label>
                    <textarea
                            id="description"
                            bind:value={animalForm.description}
                            rows="3"
                            class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                            placeholder="Enter description..."
                    ></textarea>
                </div>

                <!-- Image URL field -->
                <div class="bg-white p-4 rounded border border-gray-200">
                    <label for="imageUrl" class="block text-sm font-medium text-gray-700 mb-2">Image URL</label>
                    <input
                            type="url"
                            id="imageUrl"
                            bind:value={animalForm.imageUrl}
                            class="block w-full rounded-md border-gray-300 shadow-sm focus:border-jungle-accent focus:ring-jungle-accent"
                            placeholder="Enter image URL..."
                    />
                </div>

                <!-- Active status -->
                <div class="bg-white p-4 rounded border border-gray-200 mt-6">
                    <label class="flex items-center cursor-pointer">
                        <input
                                type="checkbox"
                                bind:checked={animalForm.isActive}
                                class="rounded border-gray-300 text-jungle-accent focus:ring-jungle-accent mr-2 h-4 w-4"
                        />
                        <span class="text-sm font-medium text-gray-700">Animal is active and can be shown to visitors</span>
                    </label>
                </div>
            </div>

            <!-- Form buttons -->
            <div class="flex justify-end gap-4 pt-6 border-t border-gray-200">
                {#if editingAnimal}
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
                        {editingAnimal ? 'Update Animal' : 'Add Animal'}
                    {/if}
                </button>
            </div>
        </form>
    </div>
</div>