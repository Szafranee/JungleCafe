<script>
    import { authStore } from '../lib/stores/authStore';
    import { navigate } from "svelte-routing";

    let isLoading = false;
    let error = '';

    async function handleSubmit(event) {
        if (event.target.checkValidity()) {
            isLoading = true;
            error = '';

            const formData = new FormData(event.target);
            const result = await authStore.register(
                formData.get('username'),
                formData.get('email'),
                formData.get('password')
            );

            if (result.success) {
                navigate('/');
            } else {
                error = result.error;
            }

            isLoading = false;
        }
    }
</script>



<div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8 bg-leaves-pattern bg-blend-overlay">
    <div class="max-w-md w-full space-y-8">
        <div>
            <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
                Create your account
            </h2>
            <p class="mt-2 text-center text-sm text-gray-600">
                Or
                <a href="/login" class="font-medium text-jungle-accent hover:text-jungle-secondary">
                    sign in to existing account
                </a>
            </p>
        </div>

        <form class="mt-8 space-y-6" on:submit|preventDefault={handleSubmit} novalidate>
            <div class="rounded-md shadow-sm -space-y-px">
                <div>
                    <label for="username" class="sr-only">Username</label>
                    <input
                            id="username"
                            name="username"
                            type="text"
                            required
                            minlength="3"
                            maxlength="20"
                            pattern="[a-zA-Z0-9]+"
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border
                   border-gray-300 placeholder-gray-500
                   text-gray-900 rounded-t-md focus:outline-none
                   focus:ring-jungle-accent focus:border-jungle-accent focus:z-10 sm:text-sm
                   [&:not(:placeholder-shown):not(:focus):invalid]:border-red-500
                   [&:not(:placeholder-shown):not(:focus):invalid]:focus:ring-red-500"
                            placeholder="Username"
                    />
                </div>
                <div>
                    <label for="email" class="sr-only">Email address</label>
                    <input
                            id="email"
                            name="email"
                            type="email"
                            autocomplete="email"
                            required
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border
                   border-gray-300 placeholder-gray-500
                   text-gray-900 focus:outline-none
                   focus:ring-jungle-accent focus:border-jungle-accent focus:z-10 sm:text-sm
                   [&:not(:placeholder-shown):not(:focus):invalid]:border-red-500
                   [&:not(:placeholder-shown):not(:focus):invalid]:focus:ring-red-500"
                            placeholder="Email address"
                    />
                </div>
                <div>
                    <label for="password" class="sr-only">Password</label>
                    <input
                            id="password"
                            name="password"
                            type="password"
                            required
                            minlength="6"
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border
                   border-gray-300 placeholder-gray-500
                   text-gray-900 rounded-b-md focus:outline-none
                   focus:ring-jungle-accent focus:border-jungle-accent focus:z-10 sm:text-sm
                   [&:not(:placeholder-shown):not(:focus):invalid]:border-red-500
                   [&:not(:placeholder-shown):not(:focus):invalid]:focus:ring-red-500"
                            placeholder="Password (min. 6 characters)"
                    />
                </div>
            </div>

            <div>
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
            </div>
        </form>
    </div>
</div>