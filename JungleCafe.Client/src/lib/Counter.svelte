<script>
    let count = 0;
    $: doubled = count * 2;  // reaktywne obliczenie
    $: if (count > 20) {     // reaktywny blok kodu
        console.log('Licznik przekroczył 10!');
    }
</script>

<button on:click={() => count++}>
    Count: {count} (Podwójnie: {doubled})
</button>

<style>
    button {
        background: #4CAF50;
        color: white;
        padding: 8px 16px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }
</style>