<script>
    import Nav from "@components/Nav.svelte";
    import WelcomeSection from "@components/WelcomeSection.svelte";
    import AnimalGallery from "@components/AnimalGallery.svelte";
    import InfoSection from "@components/InfoSection.svelte";
    import Footer from "@components/Footer.svelte";
</script>

<div class="min-h-screen bg-white">
    <Nav />
    <main>
        <WelcomeSection />
        <AnimalGallery />
        <InfoSection />
    </main>
    <Footer />
</div>