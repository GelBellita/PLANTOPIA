(function () {
    'use strict';

    if (window.location.search.includes('category=')) {
        const section = document.getElementById('collection');
        if (section) {
            section.scrollIntoView({ behavior: 'instant', block: 'start' });
        }
    }

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

    const navbar = document.getElementById('mainNavbar');
    if (navbar) {
        window.addEventListener('scroll', () => {
            navbar.classList.toggle('scrolled', window.scrollY > 40);
        });
    }

    setTimeout(() => {
        const flash = document.getElementById('flashMsg');
        if (flash) { flash.style.transition = 'opacity 0.6s'; flash.style.opacity = '0'; }
    }, 3000);

    const sectionMap = {
        'hero': 'hero',
        'collection': 'hero',
        'about': 'about',
        'sellers': 'sellers',
        'cta': 'footer',
        'footer': 'footer',
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

// ── Wishlist Toggle ──────────────────────────────────
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.wishlist-btn').forEach(btn => {
        btn.addEventListener('click', function () {
            const plantId = this.dataset.plantId;

            fetch('/Account/ToggleWishlist', {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                body: `plantId=${plantId}`
            })
                .then(res => res.json())
                .then(data => {
                    if (!data.success) {
                        window.location.href = '/Auth/Login';
                        return;
                    }

                    if (data.wishlisted) {
                        this.classList.add('wishlisted');
                        const svg = this.querySelector('svg');
                        if (svg) svg.setAttribute('fill', 'currentColor');
                        if (this.classList.contains('wishlist-heart-btn')) this.textContent = '❤️';
                    } else {
                        this.classList.remove('wishlisted');
                        const svg = this.querySelector('svg');
                        if (svg) svg.setAttribute('fill', 'none');
                        if (this.classList.contains('wishlist-heart-btn')) this.textContent = '🤍';

                        const card = document.getElementById('wishlist-card-' + plantId);
                        if (card) card.remove();
                    }
                });
        });
    });
});