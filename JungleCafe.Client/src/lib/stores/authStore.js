// src/lib/stores/authStore.js
import { writable } from 'svelte/store';

const createAuthStore = () => {
    // Initial state - check if user is already logged in (from localStorage)
    const storedToken = localStorage.getItem('token');
    const storedUser = localStorage.getItem('user');

    const { subscribe, set, update } = writable({
        user: storedUser ? JSON.parse(storedUser) : null,
        token: storedToken || null,
        isAuthenticated: !!storedToken
    });

    return {
        subscribe,

        // Login
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

                // Save token and user data to localStorage
                localStorage.setItem('token', data.token);
                localStorage.setItem('user', JSON.stringify(data.user));

                // Update store
                set({
                    user: data.user,
                    token: data.token,
                    isAuthenticated: true
                });

                return { success: true };
            } catch (error) {
                return {
                    success: false,
                    error: error.message
                };
            }
        },

        // Register
        register: async (userData) => {
            try {
                const response = await fetch('/api/auth/register', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        email: userData.email,
                        password: userData.password,
                        firstName: userData.firstName,
                        lastName: userData.lastName,
                        phoneNumber: userData.phoneNumber || null
                    })
                });

                if (!response.ok) {
                    const error = await response.json();
                    throw new Error(error.message || 'Registration failed');
                }

                const data = await response.json();

                // Save token and user data to localStorage
                localStorage.setItem('token', data.token);
                localStorage.setItem('user', JSON.stringify(data.user));

                // Update store
                set({
                    user: data.user,
                    token: data.token,
                    isAuthenticated: true
                });

                return { success: true };
            } catch (error) {
                return {
                    success: false,
                    error: error.message
                };
            }
        },

        // Logout
        logout: () => {
            // Remove token and user data from localStorage
            localStorage.removeItem('token');
            localStorage.removeItem('user');

            // Reset store
            set({
                user: null,
                token: null,
                isAuthenticated: false
            });
        },

        // Update user data
        updateUser: (userData) => {
            update(state => ({
                ...state,
                user: { ...state.user, ...userData }
            }));
            localStorage.setItem('user', JSON.stringify(userData));
        },

        // Check if user is authenticated
        checkAuth: async () => {
            const token = localStorage.getItem('token');
            if (!token) return false;

            try {
                const response = await fetch('/api/auth/verify', {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

                return response.ok;
            } catch {
                return false;
            }
        }
    };
};

export const authStore = createAuthStore();