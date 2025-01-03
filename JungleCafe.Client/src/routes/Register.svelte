<script>
    import { authStore } from '../lib/stores/authStore.js';
    import { navigate } from "svelte-routing";

    let formData = {
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        password: ''
    };

    let isLoading = false;
    let error = '';

    async function handleSubmit(event) {
        if (event.target.checkValidity()) {
            isLoading = true;
            error = '';

            const result = await authStore.register(formData);

            if (result.success) {
                navigate('/'); // Przekieruj do strony głównej po sukcesie
            } else {
                error = result.error;
            }

            isLoading = false;
        }
    }
</script>

<div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8
            bg-leaves-pattern bg-blend-overlay">
    <div class="max-w-md w-full space-y-8">
        <!-- Nagłówek formularza -->
        <div>
            <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
                Create your account
            </h2>
            <!-- ... -->
        </div>

        {#if error}
            <div class="rounded-md bg-red-50 p-4">
                <p class="text-sm text-red-700">{error}</p>
            </div>
        {/if}

        <form class="mt-8 space-y-6" on:submit|preventDefault={handleSubmit}>
            <div class="rounded-md shadow-sm -space-y-px">
                <div>
                    <label for="firstName" class="sr-only">First Name</label>
                    <input
                            bind:value={formData.firstName}
                            id="firstName"
                            name="firstName"
                            type="text"
                            required
                            class="appearance-none rounded-t-md relative block w-full px-3 py-2 border
                   border-gray-300 placeholder-gray-500
                   text-gray-900 focus:outline-none
                   focus:ring-jungle-accent focus:border-jungle-accent focus:z-10 sm:text-sm"
                            placeholder="First Name"
                    />
                </div>

                <div>
                    <label for="lastName" class="sr-only">Last Name</label>
                    <input
                            bind:value={formData.lastName}
                            id="lastName"
                            name="lastName"
                            type="text"
                            required
                            class="appearance-none relative block w-full px-3 py-2 border
                   border-gray-300 placeholder-gray-500
                   text-gray-900 focus:outline-none
                   focus:ring-jungle-accent focus:border-jungle-accent focus:z-10 sm:text-sm"
                            placeholder="Last Name"
                    />
                </div>

                <div>
                    <label for="email" class="sr-only">Email address</label>
                    <input
                            bind:value={formData.email}
                            id="email"
                            name="email"
                            type="email"
                            required
                            class="appearance-none relative block w-full px-3 py-2 border
                   border-gray-300 placeholder-gray-500
                   text-gray-900 focus:outline-none
                   focus:ring-jungle-accent focus:border-jungle-accent focus:z-10 sm:text-sm"
                            placeholder="Email address"
                    />
                </div>

                <div>
                    <label for="phoneNumber" class="sr-only">Phone Number</label>
                    <input
                            bind:value={formData.phoneNumber}
                            id="phoneNumber"
                            name="phoneNumber"
                            type="tel"
                            class="appearance-none relative block w-full px-3 py-2 border
                   border-gray-300 placeholder-gray-500
                   text-gray-900 focus:outline-none
                   focus:ring-jungle-accent focus:border-jungle-accent focus:z-10 sm:text-sm"
                            placeholder="Phone Number (optional)"
                    />
                </div>

                <div>
                    <label for="password" class="sr-only">Password</label>
                    <input
                            bind:value={formData.password}
                            id="password"
                            name="password"
                            type="password"
                            required
                            minlength="6"
                            class="appearance-none rounded-b-md relative block w-full px-3 py-2 border
                   border-gray-300 placeholder-gray-500
                   text-gray-900 focus:outline-none
                   focus:ring-jungle-accent focus:border-jungle-accent focus:z-10 sm:text-sm"
                            placeholder="Password (min. 6 characters)"
                    />
                </div>
            </div>

            <button
                    type="submit"
                    disabled={isLoading}
                    class="group relative w-full flex justify-center py-2 px-4 border border-transparent
               text-sm font-medium rounded-md text-white bg-jungle-accent hover:bg-jungle-secondary
               focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-jungle-accent
               disabled:opacity-50 disabled:cursor-not-allowed"
            >
                {#if isLoading}
                    <svg class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                {:else}
                    Create account
                {/if}
            </button>
        </form>
    </div>
</div>