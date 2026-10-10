// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function getExchangeRate() {
    const myHeaders = new Headers();
    myHeaders.append("UserAgent", "curl/8.7.1");
    myHeaders.append("Accept", "*/*");

    const requestOptions = {
        method: "GET",
        headers: myHeaders,
        redirect: "follow"
    };

    fetch("https://api.hacienda.go.cr/indicadores/tc", requestOptions)
        .then((response) => response.text())
        .then((result) => {
            return result.json();
        })
        .catch((error) => console.error(error));
}

document.addEventListener("click", async function (event) {
    const link = event.target.closest("a[data-details]");
    if (!link) return;

    event.preventDefault();   // no navegar a otra pantalla

    const modalElement = document.getElementById("detailsModal");
    const title = document.getElementById("detailsModalLabel");
    const body = document.getElementById("detailsModalBody");

    title.textContent = link.dataset.details || "Details";
    body.innerHTML = '<div class="text-center py-5"><div class="spinner-border" role="status"></div></div>';
    bootstrap.Modal.getOrCreateInstance(modalElement).show();

    try {
        const response = await fetch(link.href);
        if (!response.ok) throw new Error(response.status);
        body.innerHTML = await response.text();
    } catch (error) {
        body.innerHTML = '<div class="alert alert-danger mb-0">The details could not be loaded.</div>';
    }
});