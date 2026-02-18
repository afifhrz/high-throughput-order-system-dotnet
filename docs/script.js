// ============================================
// Navigation & Hamburger Menu
// ============================================
const hamburger = document.getElementById('hamburger');
const navMenu = document.getElementById('navMenu');
const navLinks = document.querySelectorAll('.nav-link');

hamburger.addEventListener('click', () => {
    hamburger.classList.toggle('active');
    navMenu.classList.toggle('active');
});

// Close menu when link is clicked
navLinks.forEach(link => {
    link.addEventListener('click', () => {
        hamburger.classList.remove('active');
        navMenu.classList.remove('active');
    });
});

// Close menu when clicking outside
document.addEventListener('click', (e) => {
    if (!e.target.closest('.navbar')) {
        hamburger.classList.remove('active');
        navMenu.classList.remove('active');
    }
});

// ============================================
// Smooth Scroll Enhancement
// ============================================
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// ============================================
// Scroll to Top Button
// ============================================
let scrollToTopBtn = document.querySelector('.scroll-to-top');

if (!scrollToTopBtn) {
    scrollToTopBtn = document.createElement('button');
    scrollToTopBtn.className = 'scroll-to-top';
    scrollToTopBtn.innerHTML = '↑';
    scrollToTopBtn.setAttribute('aria-label', 'Scroll to top');
    document.body.appendChild(scrollToTopBtn);
}

window.addEventListener('scroll', () => {
    if (window.scrollY > 300) {
        scrollToTopBtn.classList.add('visible');
    } else {
        scrollToTopBtn.classList.remove('visible');
    }
});

scrollToTopBtn.addEventListener('click', () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});

// ============================================
// Intersection Observer for Animations
// ============================================
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Observe cards and sections
document.querySelectorAll('.experience-card, .project-card, .skill-category').forEach(el => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(20px)';
    el.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
    observer.observe(el);
});

// ============================================
// Active Navigation Link
// ============================================
const sections = document.querySelectorAll('section[id]');
const navItems = document.querySelectorAll('.nav-link');

function updateActiveNav() {
    let current = '';

    sections.forEach(section => {
        const sectionTop = section.offsetTop;
        const sectionHeight = section.clientHeight;
        if (scrollY >= sectionTop - 200) {
            current = section.getAttribute('id');
        }
    });

    navItems.forEach(item => {
        item.classList.remove('active');
        if (item.getAttribute('href') === `#${current}`) {
            item.style.color = 'var(--accent-primary)';
        } else {
            item.style.color = '';
        }
    });
}

window.addEventListener('scroll', updateActiveNav);

// ============================================
// Navbar Appearance on Scroll
// ============================================
const navbar = document.querySelector('.navbar');
let lastScrollTop = 0;
let isNavbarVisible = true;

window.addEventListener('scroll', () => {
    const currentScroll = window.scrollY;

    if (currentScroll <= 100) {
        navbar.style.boxShadow = 'none';
    } else {
        navbar.style.boxShadow = '0 2px 8px rgba(0, 0, 0, 0.1)';
    }

    lastScrollTop = currentScroll <= 0 ? 0 : currentScroll;
});

// ============================================
// Lazy Loading for Images (Future Use)
// ============================================
if ('IntersectionObserver' in window) {
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                if (img.dataset.src) {
                    img.src = img.dataset.src;
                    img.removeAttribute('data-src');
                }
                imageObserver.unobserve(img);
            }
        });
    });

    document.querySelectorAll('img[data-src]').forEach(img => {
        imageObserver.observe(img);
    });
}

// ============================================
// Theme Detection (Optional Dark/Light)
// ============================================
function initTheme() {
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

    if (prefersDark) {
        document.documentElement.setAttribute('data-theme', 'dark');
    } else {
        document.documentElement.setAttribute('data-theme', 'light');
    }
}

// Listen for theme changes
window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
    if (e.matches) {
        document.documentElement.setAttribute('data-theme', 'dark');
    } else {
        document.documentElement.setAttribute('data-theme', 'light');
    }
});

initTheme();

// ============================================
// Form Handling (If Adding Contact Form Later)
// ============================================
function setupFormHandling() {
    const form = document.querySelector('form');
    if (!form) return;

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const formData = new FormData(form);
        const data = Object.fromEntries(formData);

        try {
            // Replace with your form submission endpoint
            // const response = await fetch('/api/contact', {
            //     method: 'POST',
            //     headers: { 'Content-Type': 'application/json' },
            //     body: JSON.stringify(data)
            // });

            console.log('Form data:', data);
            // Show success message to user
        } catch (error) {
            console.error('Form submission error:', error);
        }
    });
}

setupFormHandling();

// ============================================
// Utility: Debounce Function
// ============================================
function debounce(func, delay) {
    let timeoutId;
    return function (...args) {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => func(...args), delay);
    };
}

// ============================================
// Window Resize Handler
// ============================================
const handleResize = debounce(() => {
    // Handle responsive changes
    if (window.innerWidth > 768) {
        hamburger.classList.remove('active');
        navMenu.classList.remove('active');
    }
}, 250);

window.addEventListener('resize', handleResize);

// ============================================
// Performance: Prefetch Resources
// ============================================
function prefetchResources() {
    const links = [
        { rel: 'prefetch', href: 'styles.css' },
        { rel: 'prefetch', href: 'script.js' }
    ];

    links.forEach(({ rel, href }) => {
        const link = document.createElement('link');
        link.rel = rel;
        link.href = href;
        document.head.appendChild(link);
    });
}

// Call on load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', prefetchResources);
} else {
    prefetchResources();
}

// ============================================
// Analytics (Optional - Google Analytics or Similar)
// ============================================
function trackPageView(pageName) {
    if (window.gtag) {
        window.gtag('config', 'GA_MEASUREMENT_ID', {
            'page_path': pageName
        });
    }
}

// ============================================
// Keyboard Navigation
// ============================================
document.addEventListener('keydown', (e) => {
    // Skip to main content (Accessibility)
    if (e.ctrlKey && e.key === '.') {
        const mainContent = document.querySelector('main') || document.querySelector('section');
        if (mainContent) {
            mainContent.focus();
        }
    }
});

// ============================================
// Initialize on Page Load
// ============================================
document.addEventListener('DOMContentLoaded', () => {
    // Add any initialization code here
    updateActiveNav();
});

// ============================================
// Service Worker Registration (Optional PWA)
// ============================================
if ('serviceWorker' in navigator) {
    // Uncomment to enable offline support
    // navigator.serviceWorker.register('sw.js').catch(err => {
    //     console.log('Service Worker registration failed:', err);
    // });
}
