<script>
    import Nav from "@components/Nav.svelte";
    import WelcomeSection from "@components/WelcomeSection.svelte";
    import AnimalGallery from "@components/AnimalGallery.svelte";
    import InfoSection from "@components/InfoSection.svelte";
    import Footer from "@components/Footer.svelte";

    let animalGalleryComponent;

    function handleViewAnimalsClick() {
        if (animalGalleryComponent) {
            animalGalleryComponent.expand();
        }
    }
</script>


<div class="min-h-screen bg-white">
    <Nav onViewAnimalsClick={handleViewAnimalsClick} />
    <main>
        <WelcomeSection />
        <AnimalGallery bind:this={animalGalleryComponent} />
        <InfoSection />
    </main>
    <Footer />
</div>