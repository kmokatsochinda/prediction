document.addEventListener('DOMContentLoaded', () => {
    const themeToggle = document.getElementById('theme-toggle');
    const body = document.body;

    const savedTheme = localStorage.getItem('theme') || 'night';
    if (savedTheme === 'white') {
        body.setAttribute('data-theme', 'white');
        if (themeToggle) themeToggle.textContent = '☾';
    }

    if (themeToggle) {
        themeToggle.addEventListener('click', () => {
            if (body.getAttribute('data-theme') === 'white') {
                body.removeAttribute('data-theme');
                localStorage.setItem('theme', 'night');
                themeToggle.textContent = '☀';
            } else {
                body.setAttribute('data-theme', 'white');
                localStorage.setItem('theme', 'white');
                themeToggle.textContent = '☾';
            }
        });
    }
});
