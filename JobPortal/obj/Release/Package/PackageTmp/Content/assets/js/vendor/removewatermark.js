function load() { document.querySelectorAll("a").forEach(function (e) { (e.innerText.includes("Hosted Windows") || e.innerText.includes("Somee")) && (e.innerText = "") }) } load(), window.addEventListener("load", function () {
    document.querydocument.querySelectorAll("a").forEach(function (e) {
        if (e.innerText.includes("Hosted Windows") || e.innerText.includes("Somee")) {
            e.style.background = "none"; // Remove background if there is any
            e.innerText = ""; // Remove text
        }
    });

    // Additionally, you might need to hide the entire element if it contains the watermark
    document.querySelectorAll("*").forEach(function (e) {
        if (e.innerText.includes("Hosted Windows") || e.innerText.includes("Somee")) {
            e.style.display = "none"; // Hide the element
        }
    });