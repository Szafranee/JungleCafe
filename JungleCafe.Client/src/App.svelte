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
    import EmployeePanel from "./lib/Components/Employee/EmployeePanel.svelte";
    import NotFound from "./lib/Components/NotFound/NotFound.svelte";

    export let url = "";
</script>

<Router {url}>
    <Nav />

    <main>
        <Route path="/" component={Home} />
        <Route path="/book" component={BookingPage} />
        <Route path="/menu" component={Menu} />
        <Route path="/events" component={Events} />
        <Route
                path="/events/:id"
                let:params
        >
            <EventDetails eventId={params.id} />
        </Route>
        <Route path="/employee" component={EmployeePanel} />
        <Route path="/login" component={Login} />
        <Route path="/register" component={Register} />
        <Route default component={NotFound} />  <!-- 404 page -->
    </main>
</Router>