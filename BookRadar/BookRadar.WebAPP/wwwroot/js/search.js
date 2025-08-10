function qs(sel) { return document.querySelector(sel); }

function scrollToResults() {
    const c = qs("#resultados-container");
    if (!c) return;
    const y = c.getBoundingClientRect().top + window.scrollY - 20;
    window.scrollTo({ top: y, behavior: "smooth" });
}

function setLoading(isLoading) {
    const c = qs("#resultados-container");
    if (!c) return;
    if (isLoading) {
        c.dataset.prevHtml = c.innerHTML;
        c.innerHTML = '<div class="text-center py-4"><div class="spinner-border" role="status" aria-label="Cargando"></div></div>';
    } else if (c.dataset.prevHtml) {
        delete c.dataset.prevHtml;
    }
}

async function loadPartial(url, pushState = true) {
    const c = qs("#resultados-container");
    if (!c) return;
    try {
        setLoading(true);
        const res = await fetch(url, { headers: { "X-Requested-With": "XMLHttpRequest" } });
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        const html = await res.text();
        c.innerHTML = html;
        if (pushState) window.history.pushState({ html }, "", url);
        scrollToResults();
    } catch (err) {
        if (c.dataset.prevHtml) c.innerHTML = c.dataset.prevHtml;
        console.error(err);
        c.insertAdjacentHTML("beforeend",
            '<div class="alert alert-danger mt-3">Ocurrió un error al cargar los resultados. Intenta de nuevo.</div>');
    } finally {
        setLoading(false);
    }
}

document.addEventListener("click", function (e) {
    const link = e.target.closest(".page-btn");
    if (!link) return;

    const li = link.closest(".page-item");
    if (li && (li.classList.contains("disabled") || li.classList.contains("active"))) {
        e.preventDefault();
        return;
    }

    e.preventDefault();
    const url = link.getAttribute("href");
    if (!url) return;
    loadPartial(url, true);
});

document.getElementById("search-form")?.addEventListener("submit", function (e) {
    e.preventDefault();
    const params = new URLSearchParams(new FormData(this)).toString();
    const url = `${window.location.pathname}?${params}`;
    loadPartial(url, true);
});

document.addEventListener("change", function (e) {
    const select = e.target;
    if (select.id === "pageSize" && select.closest("form.search-page-size")) {
        const form = select.closest("form.search-page-size");
        const params = new URLSearchParams(new FormData(form)).toString();
        const base = form.getAttribute("action") || form.dataset.baseUrl;
        const url = `${base}?${params}`;
        loadPartial(url, true);
    }
});

document.addEventListener("submit", function (e) {
    const form = e.target;
    if (form.matches("form.search-page-size")) {
        e.preventDefault();
        const params = new URLSearchParams(new FormData(form)).toString();
        const base = form.getAttribute("action") || form.dataset.baseUrl;
        const url = `${base}?${params}`;
        loadPartial(url, true);
    }
});

window.addEventListener("popstate", function (e) {
    const c = qs("#resultados-container");
    if (!c) return;

    if (e.state?.html) {
        c.innerHTML = e.state.html;
        scrollToResults();
    } else {
        const url = window.location.href;
        loadPartial(url, false);
    }
});