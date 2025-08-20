const API_URL = "https://localhost:7004/api";

function showPage(pageId) {
    document.querySelectorAll(".page").forEach(p => p.classList.remove("active"));
    document.getElementById(pageId).classList.add("active");
    clearMessages();
}

function showError(containerId, message) {
    const container = document.getElementById(containerId);
    container.innerHTML = `<div style="color: #dc2626; background: #fef2f2; border: 1px solid #fecaca; padding: 12px; border-radius: 6px; margin-bottom: 16px;">${message}</div>`;
}

function showSuccess(containerId, message) {
    const container = document.getElementById(containerId);
    container.innerHTML = `<div class="success-message">${message}</div>`;
    setTimeout(() => container.innerHTML = "", 3000);
}

function clearMessages() {
    ['login-error', 'register-error', 'books-error', 'books-success'].forEach(id => {
        const el = document.getElementById(id);
        if (el) el.innerHTML = '';
    });
}

function setLoading(elementId, isLoading) {
    const element = document.getElementById(elementId);
    if (isLoading) {
        element.classList.add('loading');
    } else {
        element.classList.remove('loading');
    }
}

function clearForm(prefix) {
    document.getElementById(`${prefix}-title`).value = '';
    document.getElementById(`${prefix}-author`).value = '';
    document.getElementById(`${prefix}-publisher`).value = '';
    document.getElementById(`${prefix}-year`).value = '';
}

function logout() {
    sessionStorage.removeItem("token");
    sessionStorage.removeItem("username");
    showPage("login-page");
    clearMessages();
}

// Event listeners for Enter key
document.addEventListener('DOMContentLoaded', function () {
    // Login form
    document.getElementById('login-username').addEventListener('keypress', function (e) {
        if (e.key === 'Enter') login();
    });
    document.getElementById('login-password').addEventListener('keypress', function (e) {
        if (e.key === 'Enter') login();
    });

    // Register form
    document.getElementById('register-username').addEventListener('keypress', function (e) {
        if (e.key === 'Enter') register();
    });
    document.getElementById('register-password').addEventListener('keypress', function (e) {
        if (e.key === 'Enter') register();
    });

    // Check if user is already logged in
    if (sessionStorage.getItem("token")) {
        showPage("books-page");
        getBooks();
    }
});