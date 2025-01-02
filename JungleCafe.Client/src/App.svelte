<script>
    import { Router, Route } from "svelte-routing";
    import Nav from './lib/components/Nav.svelte';
    import Home from './routes/Home.svelte';  // przeniesiona zawartość głównej strony
    import Login from './routes/Login.svelte';
    import Register from './routes/Register.svelte';

    export let url = "";
</script>

<Router {url}>
    <Nav />

    <main>
        <Route path="/" component={Home} />
        <Route path="/login" component={Login} />
        <Route path="/register" component={Register} />
    </main>
</Router>