document.addEventListener('DOMContentLoaded', () => {
    const themeToggle = document.getElementById('theme-toggle');
    const body = document.body;

    // Apply saved theme on load
    const savedTheme = localStorage.getItem('theme') || 'night';
    if (savedTheme === 'white') {
        body.setAttribute('data-theme', 'white');
        if (themeToggle) themeToggle.innerHTML = '🌙';
    } else {
        if (themeToggle) themeToggle.innerHTML = '☀️';
    }

    // Toggle theme on click
    if (themeToggle) {
        themeToggle.addEventListener('click', () => {
            if (body.getAttribute('data-theme') === 'white') {
                body.removeAttribute('data-theme');
                localStorage.setItem('theme', 'night');
                themeToggle.innerHTML = '☀️';
            } else {
                body.setAttribute('data-theme', 'white');
                localStorage.setItem('theme', 'white');
                themeToggle.innerHTML = '🌙';
            }
        });
    }
});
