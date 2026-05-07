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
// ── Plant Modal ──────────────────────────────────────
function openPlantModal(name, category, price, imageUrl, description) {
    document.getElementById('pm-name').textContent = name;
    document.getElementById('pm-full-desc').textContent = description || 'A beautiful plant perfect for your home.';
    document.getElementById('pm-desc').textContent = description || 'A beautiful plant perfect for your home.';

    var numPrice = parseFloat(price);
    var origPrice = Math.round(numPrice * 1.25);
    document.getElementById('pm-price').textContent = '₱' + numPrice.toLocaleString();
    document.getElementById('pm-orig-price').textContent = '₱' + origPrice.toLocaleString();

    document.getElementById('pm-spec-type').textContent = category || 'Tropical';

    // Set all thumbnails + main image to same plant image
    document.getElementById('pm-main-img').src = imageUrl;
    for (var i = 0; i < 4; i++) {
        var thumb = document.getElementById('pm-thumb-' + i);
        if (thumb) thumb.src = imageUrl;
    }

    // Reset qty
    document.getElementById('pm-qty').textContent = '1';

    // Reset tabs
    switchTab(document.querySelector('.pm-tab-active') || document.querySelector('.pm-tab'), 'pm-tab-desc');

    // Show modal
    var modal = document.getElementById('plantModal');
    modal.style.display = 'flex';
    document.body.style.overflow = 'hidden';
}

function closePlantModal() {
    document.getElementById('plantModal').style.display = 'none';
    document.body.style.overflow = '';
}

function changeQty(delta) {
    var el = document.getElementById('pm-qty');
    var val = parseInt(el.textContent) + delta;
    if (val < 1) val = 1;
    el.textContent = val;
}

function switchTab(btn, tabId) {
    document.querySelectorAll('.pm-tab').forEach(function (t) {
        t.style.color = '#888';
        t.style.fontWeight = '500';
        t.style.borderBottom = '2.5px solid transparent';
        t.classList.remove('pm-tab-active');
    });
    btn.style.color = '#2e7d32';
    btn.style.fontWeight = '600';
    btn.style.borderBottom = '2.5px solid #2e7d32';
    btn.classList.add('pm-tab-active');

    ['pm-tab-desc', 'pm-tab-care', 'pm-tab-spec', 'pm-tab-rev'].forEach(function (id) {
        var el = document.getElementById(id);
        if (el) el.style.display = 'none';
    });
    var active = document.getElementById(tabId);
    if (active) active.style.display = tabId === 'pm-tab-desc' ? 'flex' : 'block';
}

function switchThumb(thumbEl) {
    document.querySelectorAll('.pm-thumb').forEach(function (t) {
        t.style.border = '2px solid #e8e8e8';
    });
    thumbEl.style.border = '2px solid #2e7d32';
    var img = thumbEl.querySelector('img');
    if (img) document.getElementById('pm-main-img').src = img.src;
}

// Close modal kung i-click ang backdrop
document.getElementById('plantModal')?.addEventListener('click', function (e) {
    if (e.target === this) closePlantModal();
});
