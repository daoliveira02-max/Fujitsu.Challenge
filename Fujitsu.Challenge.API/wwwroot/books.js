async function getBooks() {
    const token = sessionStorage.getItem("token");
    if (!token) {
        logout();
        return;
    }

    setLoading('books-page', true);
    clearMessages();

    try {
        const res = await fetch(`${API_URL}/books`, {
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (res.ok) {
            const books = await res.json();
            renderBooks(books.content || []);
        } else if (res.status === 401) {
            logout();
        } else {
            showError('books-error', 'Failed to load books. Please try again.');
        }
    } catch (error) {
        showError('books-error', 'Network error. Please try again.');
    } finally {
        setLoading('books-page', false);
    }
}

function renderBooks(books) {
    const list = document.getElementById("books-list");
    const emptyState = document.getElementById("empty-books");

    list.innerHTML = "";

    if (books.length === 0) {
        emptyState.style.display = "block";
        return;
    }

    emptyState.style.display = "none";

    books.forEach(book => {
        const li = document.createElement("li");
        li.className = "book-item";
        li.innerHTML = `
                        <div class="book-info">
                            <div class="book-title">${escapeHtml(book.title)}</div>
                            <div class="book-details">
                                by ${escapeHtml(book.author)}
                                ${book.publisher ? ` • ${escapeHtml(book.publisher)}` : ''}
                                ${book.year ? ` • ${book.year}` : ''}
                            </div>
                        </div>
                        <div class="book-actions">
                            <button class="btn btn-edit" onclick="showUpdateForm(${book.id}, '${escapeForJs(book.title)}', '${escapeForJs(book.author)}', '${escapeForJs(book.publisher || '')}', ${book.year || ''})">
                                Edit
                            </button>
                            <button class="btn btn-danger" onclick="deleteBook(${book.id})">
                                Delete
                            </button>
                        </div>
                    `;
        list.appendChild(li);
    });
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

function escapeForJs(text) {
    return text.replace(/'/g, "\\'").replace(/"/g, '\\"').replace(/\\/g, '\\\\');
}

async function addBook() {
    const token = sessionStorage.getItem("token");
    if (!token) {
        logout();
        return;
    }

    const title = document.getElementById("book-title").value.trim();
    const author = document.getElementById("book-author").value.trim();
    const publisher = document.getElementById("book-publisher").value.trim();
    const year = document.getElementById("book-year").value;

    if (!title || !author) {
        showError('books-error', 'Title and Author are required fields.');
        return;
    }

    const book = {
        title,
        author,
        publisher: publisher || null,
        year: year ? parseInt(year) : null
    };

    setLoading('books-page', true);

    try {
        const res = await fetch(`${API_URL}/books`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(book)
        });

        if (res.ok) {
            showSuccess('books-success', 'Book added successfully!');
            clearForm('book');
            getBooks();
        } else if (res.status === 401) {
            logout();
        } else {
            const errorData = await res.json().catch(() => ({}));
            showError('books-error', errorData.message || 'Failed to add book. Please try again.');
        }
    } catch (error) {
        showError('books-error', 'Network error. Please try again.');
    } finally {
        setLoading('books-page', false);
    }
}

async function deleteBook(id) {
    if (!confirm('Are you sure you want to delete this book?')) {
        return;
    }

    const token = sessionStorage.getItem("token");
    if (!token) {
        logout();
        return;
    }

    setLoading('books-page', true);

    try {
        const res = await fetch(`${API_URL}/books/${id}`, {
            method: "DELETE",
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (res.ok) {
            showSuccess('books-success', 'Book deleted successfully!');
            getBooks();
        } else if (res.status === 401) {
            logout();
        } else {
            showError('books-error', 'Failed to delete book. Please try again.');
        }
    } catch (error) {
        showError('books-error', 'Network error. Please try again.');
    } finally {
        setLoading('books-page', false);
    }
}

function showUpdateForm(id, title, author, publisher, year) {
    document.getElementById("update-id").value = id;
    document.getElementById("update-title").value = title;
    document.getElementById("update-author").value = author;
    document.getElementById("update-publisher").value = publisher;
    document.getElementById("update-year").value = year;

    document.getElementById("add-form").style.display = "none";
    document.getElementById("update-form").style.display = "block";

    // Scroll to update form
    document.getElementById("update-form").scrollIntoView({ behavior: 'smooth' });
}

function cancelUpdate() {
    document.getElementById("update-form").style.display = "none";
    document.getElementById("add-form").style.display = "block";
    clearMessages();
}

async function submitUpdate() {
    const token = sessionStorage.getItem("token");
    if (!token) {
        logout();
        return;
    }

    const id = document.getElementById("update-id").value;
    const title = document.getElementById("update-title").value.trim();
    const author = document.getElementById("update-author").value.trim();
    const publisher = document.getElementById("update-publisher").value.trim();
    const year = document.getElementById("update-year").value;

    if (!title || !author) {
        showError('books-error', 'Title and Author are required fields.');
        return;
    }

    const book = {
        title,
        author,
        publisher: publisher || null,
        year: year ? parseInt(year) : null
    };

    setLoading('books-page', true);

    try {
        const res = await fetch(`${API_URL}/books/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(book)
        });

        if (res.ok) {
            showSuccess('books-success', 'Book updated successfully!');
            cancelUpdate();
            getBooks();
        } else if (res.status === 401) {
            logout();
        } else {
            const errorData = await res.json().catch(() => ({}));
            showError('books-error', errorData.message || 'Failed to update book. Please try again.');
        }
    } catch (error) {
        showError('books-error', 'Network error. Please try again.');
    } finally {
        setLoading('books-page', false);
    }
}