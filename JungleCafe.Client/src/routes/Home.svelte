<!-- src/routes/Home.svelte -->
<script>
    import WelcomeSection from '../lib/components/WelcomeSection.svelte';
    import AnimalGallery from '../lib/components/AnimalGallery.svelte';
    import InfoSection from '../lib/components/InfoSection.svelte';
    import Footer from '../lib/components/Footer.svelte';

</script>

<div class="min-h-screen bg-white">
    <main>
        <WelcomeSection />
        <AnimalGallery />
        <InfoSection />
    </main>
    <Footer />
</div>