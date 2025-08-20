async function login() {
    const username = document.getElementById("login-username").value.trim();
    const password = document.getElementById("login-password").value;

    if (!username || !password) {
        showError('login-error', 'Please fill in all fields.');
        return;
    }

    setLoading('login-page', true);
    clearMessages();

    try {
        const res = await fetch(`${API_URL}/auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        if (res.ok) {
            const data = await res.json();
            // Using session storage instead of localStorage for better security
            sessionStorage.setItem("token", data.content.token);
            sessionStorage.setItem("username", username);
            showPage("books-page");
            getBooks();
        } else {
            const errorData = await res.json().catch(() => ({}));
            showError('login-error', errorData.message || 'Login failed. Please check your credentials.');
        }
    } catch (error) {
        showError('login-error', 'Network error. Please try again.');
    } finally {
        setLoading('login-page', false);
    }
}

async function register() {
    const username = document.getElementById("register-username").value.trim();
    const password = document.getElementById("register-password").value;

    if (!username || !password) {
        showError('register-error', 'Please fill in all fields.');
        return;
    }

    if (password.length < 4) {
        showError('register-error', 'Password must be at least 4 characters long.');
        return;
    }

    setLoading('register-page', true);
    clearMessages();

    try {
        const res = await fetch(`${API_URL}/auth/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        if (res.ok) {
            showPage("login-page");
            showSuccess('login-error', 'Registration successful! Please sign in.');
            // Pre-fill username
            document.getElementById("login-username").value = username;
        } else {
            const errorData = await res.json().catch(() => ({}));
            showError('register-error', errorData.message || 'Registration failed. Username may already exist.');
        }
    } catch (error) {
        showError('register-error', 'Network error. Please try again.');
    } finally {
        setLoading('register-page', false);
    }
}