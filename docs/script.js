// Lightweight navigation behaviors used across pages.
(function () {
    function setupHamburger(hamburgerId, menuId) {
        var ham = document.getElementById(hamburgerId) || document.getElementById('hamburger');
        var menu = document.getElementById(menuId) || document.getElementById('nav-menu');
        if (!ham || !menu) return;
        ham.addEventListener('click', function () {
            ham.classList.toggle('active');
            menu.classList.toggle('show');
        });
        // close the menu when clicking outside (mobile)
        document.addEventListener('click', function (e) {
            if (!menu.contains(e.target) && !ham.contains(e.target)) {
                ham.classList.remove('active');
                menu.classList.remove('show');
            }
        });
    }

    // multiple nav instances across pages use these IDs
    setupHamburger('hamburger', 'nav-menu');
    setupHamburger('hamburger-arch', 'nav-menu-arch');
    setupHamburger('hamburger-trade', 'nav-menu-trade');
    setupHamburger('hamburger-scale', 'nav-menu-scale');
    setupHamburger('hamburger-fail', 'nav-menu-fail');
    setupHamburger('hamburger-fin', 'nav-menu-fin');

    // mark active link for readability
    try {
        var links = document.querySelectorAll('.nav-link');
        links.forEach(function (a) {
            if (a.href === location.href || a.getAttribute('href') === location.pathname.split('/').pop()) {
                a.classList.add('active');
            }
        });
    } catch (e) { /* ignore */ }
})();