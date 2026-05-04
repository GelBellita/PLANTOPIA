(function () {
    'use strict';

    // Jump to #collection instantly when a category tab was clicked
    if (window.location.search.includes('category=')) {
        const section = document.getElementById('collection');
        if (section) {
            section.scrollIntoView({ behavior: 'instant', block: 'start' });
        }
    }

    // Smooth scroll for all links (Our Story, Meet Farmers, etc.)
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

})();