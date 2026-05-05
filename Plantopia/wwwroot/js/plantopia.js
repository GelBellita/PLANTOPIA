(function () {
    'use strict';

    // Jump to #collection instantly when a category tab was clicked
    if (window.location.search.includes('category=')) {
        const section = document.getElementById('collection');
        if (section) {
            section.scrollIntoView({ behavior: 'instant', block: 'start' });
        }
    }

    // Smooth scroll for all links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const selector = this.getAttribute('href');
            if (selector === '#') return;
            const target = document.querySelector(selector);
            if (target) {
                e.preventDefault();
                target.scrollIntoView({ behavior: 'smooth' });
            }
        });
    });

    // Navbar scroll effect
    const navbar = document.getElementById('mainNavbar');
    window.addEventListener('scroll', () => {
        navbar.classList.toggle('scrolled', window.scrollY > 40);
    });

    // Flash message fade
    setTimeout(() => {
        const flash = document.getElementById('flashMsg');
        if (flash) { flash.style.transition = 'opacity 0.6s'; flash.style.opacity = '0'; }
    }, 3000);

    // Scroll spy
    const sectionMap = {
        'hero':       'hero',
        'collection': 'hero',
        'about':      'about',
        'sellers':    'sellers',
        'cta':        'footer',
        'footer':     'footer',
    };

    function updateActiveNav() {
        let current = 'hero';
        document.querySelectorAll('section[id], footer[id]').forEach(sec => {
            if (window.scrollY >= sec.offsetTop - 120) current = sec.id;
        });

        document.querySelectorAll('.nav-link').forEach(l => l.classList.remove('active'));
        const targetSection = sectionMap[current];
        if (targetSection) {
            document.querySelector(`.nav-link[data-section="${targetSection}"]`)?.classList.add('active');
        }
    }

    window.addEventListener('scroll', updateActiveNav);
    updateActiveNav();

})();