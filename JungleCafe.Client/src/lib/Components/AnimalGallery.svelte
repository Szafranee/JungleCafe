<script>
    export let animals = [
        {
            image: "/img/ara_araruna.jpeg",
            name: "Mary the Blue-and-yellow Macaw",
            alt: "Ara araruna"
        },
        {
            image: "/img/chameleon.jpg",
            name: "Max the Chameleon",
            alt: "Chameleon"
        },
        {
            image: "/img/hedgehog.jpeg",
            name: "Filip the Hedgehog",
            alt: "Hedgehog"
        }
    ];
</script>

<h2 class="text-heading">Our animals:</h2>
<div class="gallery">
    {#each animals as animal}
        <figure>
            <img alt={animal.alt} class="gallery__image" src={animal.image}>
            <figcaption class="text-subheading">{animal.name}</figcaption>
        </figure>
    {/each}
</div>
<h2 class="text-heading">...and many, many more!</h2>