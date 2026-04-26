(function () {
    'use strict';

    const SECTION_IDS = ['hero', 'collection', 'about', 'sellers', 'cta'];
    const SECTION_TO_NAV = {
        hero: 'hero',
        collection: 'hero',
        about: 'about',
        sellers: 'sellers',
        cta: 'cta',
    };

    const navLinks = document.querySelectorAll('.nav-spy');

    function setActive(sectionId) {
        const targetKey = SECTION_TO_NAV[sectionId];
        navLinks.forEach(link => {
            link.classList.toggle('active', link.dataset.section === targetKey);
        });
    }

    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                setActive(entry.target.id);
            }
        });
    }, {
        root: null,
        threshold: 0.35,
    });

    SECTION_IDS.forEach(id => {
        const el = document.getElementById(id);
        if (el) observer.observe(el);
    });

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