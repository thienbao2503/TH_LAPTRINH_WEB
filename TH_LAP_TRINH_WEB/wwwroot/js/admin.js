document.querySelectorAll('.sidebar-content > ul > li > a').forEach(item => {
    item.addEventListener('click', function (event) {
        const submenu = this.nextElementSibling;
        const icon = this.querySelector('i.fi-rr-angle-small-down');

        // Đóng tất cả submenu khác
        document.querySelectorAll('.sidebar-content > ul > li > ul').forEach(otherSubmenu => {
            if (otherSubmenu !== submenu) {
                toggleSubmenu(otherSubmenu, false);
            }
        });

        if (submenu) {
            toggleSubmenu(submenu, !submenu.classList.contains('active'));
            const sidebar = document.getElementById('sidebar');
            sidebar.classList.remove('collapsed');
            const logo = document.getElementById('logo');
            logo.classList.remove('hidden');
            document.querySelectorAll('.menu-text').forEach(text => text.classList.remove('hidden'));
        }
    });
});

//document.getElementById('toggle-button').addEventListener('click', function () {
//    const sidebar = document.getElementById('sidebar');
//    const isCollapsed = sidebar.classList.contains('collapsed');

//    // Đóng tất cả submenu khi ẩn sidebar
//    if (!isCollapsed) {
//        document.querySelectorAll('.sidebar-content > ul > li > ul').forEach(submenu => {
//            toggleSubmenu(submenu, false);
//        });
//    }

//    sidebar.classList.toggle('collapsed');
//    const logo = document.getElementById('logo');
//    logo.classList.toggle('hidden');
//    document.querySelectorAll('.menu-text').forEach(text => text.classList.toggle('hidden'));
//});

document.getElementById('dropdown-button').addEventListener('click', function (event) {
    const menu = document.getElementById('dropdown-menu');
    const icon = document.querySelector('#dropdown-button i');
    const isVisible = menu.style.display === 'flex';

    // Thay đổi hiển thị menu và lớp biểu tượng
    menu.style.display = isVisible ? 'none' : 'flex';
    icon.classList.toggle('fi-rr-angle-small-down', isVisible);
    icon.classList.toggle('fi-rr-angle-small-up', !isVisible);

    event.stopPropagation();
});

// Ẩn menu khi nhấn ra ngoài
document.addEventListener('click', function () {
    const menu = document.getElementById('dropdown-menu');
    const icon = document.querySelector('#dropdown-button i');
    menu.style.display = 'none';
    icon.classList.replace('fi-rr-angle-small-up', 'fi-rr-angle-small-down');
});

// Hàm để thay đổi trạng thái submenu
function toggleSubmenu(submenu, isActive) {
    submenu.classList.toggle('active', isActive);
    const icon = submenu.previousElementSibling.querySelector('i.fi-rr-angle-small-down');
    if (icon) icon.classList.toggle('rotate', isActive);
}