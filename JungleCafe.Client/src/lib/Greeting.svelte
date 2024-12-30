<!-- lib/Greeting.svelte -->
<script>
    export let name = 'Świat';  // export oznacza prop, można ustawić wartość domyślną
    export let color = 'black';
</script>

<h1 style="color: {color}">
    Witaj, {name}!
</h1>