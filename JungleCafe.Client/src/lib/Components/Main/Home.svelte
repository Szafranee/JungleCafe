<script>
    import WelcomeSection from './WelcomeSection.svelte';
    import AnimalGallery from './AnimalGallery.svelte';
    import InfoSection from './InfoSection.svelte';
    import Footer from './Footer.svelte';

    import { onMount } from 'svelte';

    onMount(() => {
        if (window.location.hash === '#animals') {
            document.getElementById('animals')?.scrollIntoView({behavior: 'smooth'});
        }
    });
</script>

<div class="min-h-screen bg-white">
    <main>
        <WelcomeSection />
        <AnimalGallery />
        <InfoSection />
    </main>
    <Footer />
</div>