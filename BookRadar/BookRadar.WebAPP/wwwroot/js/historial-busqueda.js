(function () {
    function scrollToResults() {
        const container = document.querySelector("#resultados-container");
        if (container) {
            const y = container.getBoundingClientRect().top + window.scrollY - 20;
            window.scrollTo({ top: y, behavior: "smooth" });
        }
    }

    async function loadAjax(url, push = true) {
        const res = await fetch(url, { headers: { "X-Requested-With": "XMLHttpRequest" } });
        const html = await res.text();
        document.querySelector("#resultados-container").innerHTML = html;
        if (push) window.history.pushState({ html }, "", url);
        scrollToResults();
    }

    document.addEventListener("click", function (e) {
        const a = e.target.closest("a.page-btn");
        if (a) {
            e.preventDefault();
            loadAjax(a.href);
        }
    });

    document.addEventListener("submit", function (e) {
        const form = e.target;
        if (form.matches("form.historial-page-size")) {
            e.preventDefault();
            const params = new URLSearchParams(new FormData(form)).toString();
            const base = form.getAttribute("action") || form.dataset.baseUrl;
            const url = `${base}?${params}`;
            loadAjax(url);
        }
    });

    window.addEventListener("popstate", function (e) {
        if (e.state && e.state.html) {
            document.querySelector("#resultados-container").innerHTML = e.state.html;
            scrollToResults();
        } else {
            loadAjax(location.href, false);
        }
    });
})();
