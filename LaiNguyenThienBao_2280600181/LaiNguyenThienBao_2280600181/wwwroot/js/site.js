// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const navbar = document.querySelector(".navbar");
const navbarMobie = document.querySelector(".navbar-mobie");
const WIDTHWINDOW = window.innerWidth;
function handleScroll() {
    // Lấy vị trí cuộn trang
    var scrollTop = window.scrollY;
    const searchLogo = document.querySelector(".form-search-mobie-logo");
    const formSearch = document.querySelector(".form-search-mobie");
    const logo = document.querySelector(".mobie-logo");

    // Hiển thị thông báo nếu cuộn trang
    if (scrollTop > 0) {
        if (WIDTHWINDOW > 900) {
            navbar.style.position = "fixed";
            navbar.style.animation = "showNavbar 0.7s";
        } else {
            searchLogo.style.display = "flex";
            logo.style.display = "none";
            formSearch.style.display = "none";
            navbarMobie.style.position = "fixed";
            navbarMobie.style.animation = "showNavbar 0.7s";
        }
    } else {
        // người dùng cuộn lên đầu trang
        if (WIDTHWINDOW > 900) {
            navbar.style.position = "relative";
            navbar.style.animation = "";
        } else {
            searchLogo.style.display = "none";
            logo.style.display = "flex";
            formSearch.style.display = "flex";
            navbarMobie.style.position = "relative";
            navbarMobie.style.animation = "";
        }
    }
}
// Đăng ký sự kiện scroll
window.addEventListener("scroll", handleScroll);
