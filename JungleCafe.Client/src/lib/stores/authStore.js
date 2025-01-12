import { writable } from 'svelte/store';

const createAuthStore = () => {
    // Bezpieczne pobieranie danych z localStorage
    const getStoredData = () => {
        try {
            const token = localStorage.getItem('token');
            let user = null;
            try {
                const storedUser = localStorage.getItem('user');
                if (storedUser) {
                    user = JSON.parse(storedUser);
                }
            } catch (e) {
                console.error('Error parsing user data, clearing localStorage');
                localStorage.removeItem('user');
                localStorage.removeItem('token');
            }

            return {
                token,
                user,
                isAuthenticated: !!token && !!user
            };
        } catch (e) {
            console.error('Error getting stored data:', e);
            return {
                token: null,
                user: null,
                isAuthenticated: false
            };
        }
    };

    const initialState = getStoredData();
    const { subscribe, set, update } = writable(initialState);

    return {
        subscribe,

        login: async (email, password) => {
            try {
                const response = await fetch('/api/auth/login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ email, password })
                });

                if (!response.ok) {
                    const error = await response.json();
                    throw new Error(error.message || 'Login failed');
                }

                const data = await response.json();

                // Bezpieczne zapisywanie danych
                if (data && data.token) {
                    localStorage.setItem('token', data.token);
                    localStorage.setItem('user', JSON.stringify(data.user || {}));

                    set({
                        user: data.user,
                        token: data.token,
                        isAuthenticated: true
                    });
                }

                return { success: true };
            } catch (error) {
                console.error('Login error:', error);
                return {
                    success: false,
                    error: error.message
                };
            }
        },

        logout: () => {
            localStorage.removeItem('token');
            localStorage.removeItem('user');

            set({
                user: null,
                token: null,
                isAuthenticated: false
            });
        }
    };
};

export const authStore = createAuthStore();