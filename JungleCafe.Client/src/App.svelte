<script>
    import { Router, Route } from "svelte-routing";
    import Nav from './lib/Components/Main/Nav.svelte';
    import Home from './lib/Components/Main/Home.svelte'
    import Login from './lib/Components/Auth/Login.svelte';
    import Register from './lib/Components/Auth/Register.svelte';
    import Menu from "./lib/Components/Menu/Menu.svelte";
    import Events from "./lib/Components/Events/Events.svelte";
    import EventDetails from "./lib/Components/Events/EventDetails.svelte";
    import BookingPage from "./lib/Components/Reservations/BookingPage.svelte";

    export let url = "";
</script>

<Router {url}>
    <Nav />

    <main>
        <Route path="/" component={Home} />
        <Route path="/book" component={BookingPage} />
        <Route path="/menu" component={Menu} />
        <Route path="/login" component={Login} />
        <Route path="/register" component={Register} />
        <Route path="/events" component={Events} />
        <Route
                path="/events/:id"
                let:params
        >
            <EventDetails eventId={params.id} />
        </Route>
    </main>
</Router>