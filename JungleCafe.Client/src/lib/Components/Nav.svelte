<script>
    export let activePage = 'home';
    export let onViewAnimalsClick;
</script>

<nav class="fixed w-full z-50 bg-white/80 backdrop-blur-md shadow-sm">
    <div class="max-w-7xl mx-auto px-4">
        <div class="flex items-center justify-between h-16">
            <div class="flex items-center">
                <img
                        src="/img/logo_big_no_bg.png"
                        alt="Jungle Cafe"
                        class="h-14 w-auto transform transition hover:scale-110"
                />
            </div>

            <div class="hidden md:flex items-center space-x-8">
                <a
                        href="/"
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                        class:text-jungle-primary={activePage === 'home'}
                >
                    Home
                </a>
                <a
                        href="/book"
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Book a Table
                </a>
                <a
                        href="/menu"
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Menu
                </a>
                <a
                        href="#animals"
                        on:click|preventDefault={() => {
                            document.getElementById('animals').scrollIntoView({ behavior: 'smooth' });
                            // Expand the animal gallery
                            onViewAnimalsClick();
                          }}
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Our Animals
                </a>
                <a
                        href="/events"
                        class="text-jungle-brown hover:text-jungle-primary transition-colors font-medium"
                >
                    Events
                </a>
                <a
                        href="/login"
                        class="bg-jungle-accent text-white px-6 py-2 rounded-full font-medium
                 hover:bg-jungle-secondary transition-colors shadow-sm"
                >
                    Sign In
                </a>
            </div>
        </div>
    </div>
</nav>