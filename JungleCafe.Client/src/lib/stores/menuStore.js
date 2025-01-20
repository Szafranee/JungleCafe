import { writable } from 'svelte/store';

function createMenuStore() {
    const { subscribe, set, update } = writable({
        items: [],
        loading: false,
        error: null
    });

    return {
        subscribe,
        fetchMenuItems: async () => {
            try {
                set({ items: [], loading: true, error: null });
                const response = await fetch('/api/menu');
                if (!response.ok) throw new Error('Failed to fetch menu items');
                const data = await response.json();
                set({ items: data, loading: false, error: null }); //idk what exactly changed, but we need data.value instead of just data like before
            } catch (error) {
                set({ items: [], loading: false, error: error.message });
            }
        }
    };
}

export const menuStore = createMenuStore();