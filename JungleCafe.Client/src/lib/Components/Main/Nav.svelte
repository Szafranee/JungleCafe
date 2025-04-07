<script>
    import {Link, useLocation} from "svelte-routing";
    import {authStore} from "../../stores/authStore.js"
    import { onMount } from 'svelte';
    import { navigate } from "svelte-routing";

    export let activePage = 'home';

    const location = useLocation();
    
    // State for mobile menu
    let isMenuOpen = false;
    
    // Function to toggle mobile menu
    function toggleMenu() {
        isMenuOpen = !isMenuOpen;
    }
    
    // Close menu when navigating to a new page
    function closeMenu() {
        isMenuOpen = false;
    }
    
    // Close menu when window is resized to desktop size
    onMount(() => {
        const handleResize = () => {
            if (window.innerWidth > 768 && isMenuOpen) {
                isMenuOpen = false;
            }
        };
        
        // Watch for location changes to close menu
        const unsubscribe = location.subscribe(() => {
            if (isMenuOpen) {
                closeMenu();
            }
        });
        
        window.addEventListener('resize', handleResize);
        
        return () => {
            window.removeEventListener('resize', handleResize);
            unsubscribe();
        };
    });

    function handleAnimalsSectionClick(event) {
        event.preventDefault();
        if ($location.pathname !== '/') {
            window.location.href = '/#animals';
        } else {
            document.getElementById('animals')?.scrollIntoView({behavior: 'smooth'});
        }
        closeMenu();
    }
    
    // Helper function to handle navigation with menu closing
    function handleNavigation(path) {
        closeMenu();
        navigate(path);
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

            <!-- Desktop menu -->
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
            
            <!-- Mobile menu button - POWIĘKSZONE IKONY -->
            <div class="md:hidden flex items-center">
                <button
                    on:click={toggleMenu}
                    type="button"
                    class="inline-flex items-center justify-center p-2 rounded-md text-jungle-brown hover:text-jungle-primary focus:outline-none"
                    aria-controls="mobile-menu"
                    aria-expanded={isMenuOpen}
                >
                    <span class="sr-only">Open main menu</span>
                    <!-- Powiększona ikona hamburger -->
                    <svg
                        class={`${isMenuOpen ? 'hidden' : 'block'} h-8 w-8`}
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke="currentColor"
                        aria-hidden="true"
                    >
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                    </svg>
                    <!-- Powiększona ikona X -->
                    <svg
                        class={`${isMenuOpen ? 'block' : 'hidden'} h-8 w-8`}
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke="currentColor"
                        aria-hidden="true"
                    >
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                    </svg>
                </button>
            </div>
        </div>
    </div>
    
    <!-- Mobile menu dropdown -->
    {#if isMenuOpen}
        <div class="md:hidden bg-white/95 backdrop-blur-md shadow-lg" id="mobile-menu">
            <div class="px-2 pt-2 pb-3 space-y-2">
                <Link
                    to="/"
                    on:click={closeMenu}
                    class={`block px-3 py-2 rounded-md font-medium transition-colors ${
                        activePage === 'home' ? 'text-jungle-primary' : 'text-jungle-brown hover:text-jungle-primary'
                    }`}
                >
                    Home
                </Link>
                <Link
                    to="/book"
                    on:click={closeMenu}
                    class="block px-3 py-2 rounded-md text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Book a Table
                </Link>
                <Link
                    to="/menu"
                    on:click={closeMenu}
                    class="block px-3 py-2 rounded-md text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Menu
                </Link>
                <a
                    href="#animals"
                    on:click|preventDefault={handleAnimalsSectionClick}
                    class="block px-3 py-2 rounded-md text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Our Animals
                </a>
                <Link
                    to="/events"
                    on:click={closeMenu}
                    class="block px-3 py-2 rounded-md text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Events
                </Link>
                
                {#if $authStore.isAuthenticated}
                    <Link
                        to="/my-reservations"
                        on:click={closeMenu}
                        class="block px-3 py-2 rounded-md text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                    >
                        My Reservations
                    </Link>
                {/if}
                
                {#if $authStore.isAuthenticated && ($authStore.user.role === 'Admin' || $authStore.user.role === 'Employee' || $authStore.user.role === 'Manager' || $authStore.user.role === 'Caretaker')}
                    <Link
                        to="/employee"
                        on:click={closeMenu}
                        class="block px-3 py-2 rounded-md bg-jungle-primary text-white font-medium hover:bg-jungle-secondary transition-colors"
                    >
                        Employee Panel
                    </Link>
                {/if}
                
                {#if $authStore.isAuthenticated}
                    <button
                        on:click={() => {
                        authStore.logout();
                        window.location.href = '/';
                        closeMenu();
                    }}
                        class="w-full text-left px-3 py-2 rounded-md bg-jungle-accent text-white font-medium hover:bg-jungle-secondary transition-colors"
                    >
                        Log Out
                    </button>
                {:else}
                    <Link
                        to="/login"
                        on:click={closeMenu}
                        class="block px-3 py-2 rounded-md bg-jungle-accent text-white font-medium hover:bg-jungle-secondary transition-colors"
                    >
                        Sign In
                    </Link>
                {/if}
            </div>
        </div>
    {/if}
</nav>