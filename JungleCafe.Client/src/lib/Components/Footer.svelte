<script>
    import { SiFacebook, SiInstagram } from "@icons-pack/svelte-simple-icons";
</script>

<footer class="bg-jungle-primary text-white">
    <!-- Upper footer section with columns -->
    <div class="max-w-7xl mx-auto px-4 pt-16 pb-12">
        <div class="grid md:grid-cols-4 gap-8">
            <!-- About column -->
            <div class="space-y-4">
                <img
                        src="/img/logo_big_no_bg.png"
                        alt="Jungle Cafe"
                        class="h-24 w-auto mb-4"
                />
                <p class="text-gray-300">
                    Where exotic animals and perfect coffee create unforgettable moments
                </p>
                <div class="flex space-x-4">
                    <a href="https://www.facebook.com/" class="text-gray-300 hover:text-white transition p-1">
                        <SiFacebook />
                    </a>
                    <a href="https://www.instagram.com/" class="text-gray-300 hover:text-white transition p-1">
                        <SiInstagram />
                    </a>
                </div>
            </div>

            <!-- Links column -->
            <div>
                <h3 class="font-medium text-xl mb-4">Quick Links</h3>
                <ul class="space-y-2">
                    <li><a href="/menu" class="text-gray-300 hover:text-white transition">Menu</a></li>
                    <li><a href="/book" class="text-gray-300 hover:text-white transition">Book a Table</a></li>
                    <li><a href="/events" class="text-gray-300 hover:text-white transition">Events</a></li>
                    <li><a href="/about" class="text-gray-300 hover:text-white transition">About Us</a></li>
                </ul>
            </div>

            <!-- Opening Hours -->
            <div>
                <h3 class="font-medium text-xl mb-4">Opening Hours</h3>
                <ul class="space-y-2 text-gray-300">
                    <li>Monday - Friday</li>
                    <li class="font-medium text-white">13:00 - 19:00</li>
                    <li class="mt-4">Saturday - Sunday</li>
                    <li class="font-medium text-white">13:00 - 20:00</li>
                </ul>
            </div>

            <!-- Contact -->
            <div>
                <h3 class="font-medium text-xl mb-4">Contact Us</h3>
                <ul class="space-y-2 text-gray-300">
                    <li>Chmielna 69</li>
                    <li>00-801 Warszawa</li>
                    <li>Poland</li>
                    <li class="mt-4">
                        <a href="mailto:contact@junglecafe.com" class="hover:text-white transition">
                            contact@junglecafe.com
                        </a>
                    </li>
                    <li>
                        <a href="tel:+48123456789" class="hover:text-white transition">
                            +48 123 456 789
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </div>

    <!-- Bottom footer -->
    <div class="border-t border-gray-800">
        <div class="max-w-7xl mx-auto px-4 py-6">
            <div class="flex justify-between items-center">
                <p class="text-gray-400 text-sm">
                    © 2024 Jungle Café. All rights reserved.
                </p>
                <div class="flex space-x-6 text-sm">
                    <a href="/privacy" class="text-gray-400 hover:text-white transition">
                        Privacy Policy
                    </a>
                    <a href="/terms" class="text-gray-400 hover:text-white transition">
                        Terms of Service
                    </a>
                </div>
            </div>
        </div>
    </div>
</footer>