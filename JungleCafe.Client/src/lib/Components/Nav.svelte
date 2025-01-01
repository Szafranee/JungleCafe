<script>
    export let activePage = 'home';
</script>

<nav class="nav-main">
    <img alt="Big Logo" class="logo_big" src="/img/logo_big_no_bg.png">
    <a class:active={activePage === 'home'} href="/">Main page</a>
    <a class:active={activePage === 'menu'} href="/menu">Menu</a>
    <a class:active={activePage === 'reservation'} href="/reservation">Reservation</a>
    <a class:active={activePage === 'events'} href="/events">Events</a>
    <a class:active={activePage === 'login'} href="/login">Log in</a>
    <a class:active={activePage === 'register'} href="/register">Sign up</a>
</nav>