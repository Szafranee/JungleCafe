<script>
    import {Link, useLocation} from "svelte-routing";
    import {authStore} from "../../stores/authStore.js"

    export let activePage = 'home';

    const location = useLocation();

    function handleAnimalsSectionClick(event) {
        event.preventDefault();
        if ($location.pathname !== '/') {
            window.location.href = '/#animals';
        } else {
            document.getElementById('animals')?.scrollIntoView({behavior: 'smooth'});
        }
    }
</script>

<nav class="fixed w-full z-50 bg-white/80 backdrop-blur-md shadow-sm">
    <div class="max-w-7xl mx-auto px-4">
        <div class="flex items-center justify-between h-16">
            <div class="flex items-center">
                <Link to="/">
                    <img
                            src="/img/logo_big_no_bg.png"
                            alt="Jungle Cafe"
                            class="h-14 w-auto transform transition hover:scale-110"
                    />
                </Link>
            </div>

            <div class="hidden md:flex items-center space-x-8">
                <Link
                        to="/"
                        class={`text-jungle-brown hover:text-jungle-primary transition-colors font-medium ${
                            activePage === 'home' ? 'text-jungle-primary' : ''
                        }`}
                >
                    Home
                </Link>
                <Link
                        to="/book"
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Book a Table
                </Link>
                <Link
                        to="/menu"
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Menu
                </Link>

                <a
                        href="#animals"
                        on:click|preventDefault={handleAnimalsSectionClick}
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Our Animals
                </a>

                <Link
                        to="/events"
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Events
                </Link>

                {#if $authStore.isAuthenticated}
                    <Link
                            to="/my-reservations"
                            class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                    >
                        My Reservations
                    </Link>
                {/if}

                {#if $authStore.isAuthenticated && ($authStore.user.role === 'Admin' || $authStore.user.role === 'Employee' || $authStore.user.role === 'Manager' || $authStore.user.role === 'Caretaker')}
                    <button
                            on:click={() => {
                            window.location.href = '/employee';
                        }}
                            class="bg-jungle-primary text-white px-6 py-2 rounded-full font-medium
                               hover:bg-jungle-secondary transition-colors shadow-sm"
                    >
                        Employee Panel
                    </button>
                {/if}

                {#if $authStore.isAuthenticated}
                    <button
                            on:click={() => {
                            authStore.logout();
                            window.location.href = '/'; // Return to home page
                        }}
                            class="bg-jungle-accent text-white px-6 py-2 rounded-full font-medium
                               hover:bg-jungle-secondary transition-colors shadow-sm"
                    >
                        Log Out
                    </button>
                {:else}
                    <Link
                            to="/login"
                            class="bg-jungle-accent text-white px-6 py-2 rounded-full font-medium
                               hover:bg-jungle-secondary transition-colors shadow-sm"
                    >
                        Sign In
                    </Link>
                {/if}

            </div>
        </div>
    </div>
</nav>