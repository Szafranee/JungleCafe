import { writable } from 'svelte/store';

const createAuthStore = () => {
    // Safe way to get stored data
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
                isAuthenticated: !!token && !!user,
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

                let user = {
                    id: data.id,
                    name: data.name,
                    email: data.email,
                    role: data.role
                }

                console.log(data);

                if (data && data.token) {
                    localStorage.setItem('token', data.token);
                    localStorage.setItem('user', JSON.stringify(user || {}));

                    set({
                        user: user,
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

                let user = {
                    id: data.id,
                    name: data.name,
                    email: data.email,
                    role: data.role
                }

                // Save token and user data to localStorage
                localStorage.setItem('token', data.token);
                localStorage.setItem('user', JSON.stringify(user));

                // Update store
                set({
                    user: user,
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