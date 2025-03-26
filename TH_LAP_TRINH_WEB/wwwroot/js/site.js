﻿const navbar = document.querySelector('.navbar');
//const navbarMobie = document.querySelector('.navbar-mobie');
const backToTop = document.querySelector('.back-to-top');
const WIDTHWINDOW = window.innerWidth;
let isShowMenuV2 = false;


let nameIDClose = '';

if (document.querySelector('.section-slider')) {

    var swiper = new Swiper('.section-slider .mySwiper', {
        loop: true,
        navigation: {
            nextEl: '.section-slider .button-next',
            prevEl: '.section-slider .button-prev',
        },
    });
}




function BackToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth', // Sử dụng 'smooth' để có hiệu ứng cuộn mượt
    });
}
// bắt sự kiện khi ng dùng cuộn trang
function handleScroll() {
    // Lấy vị trí cuộn trang
    var scrollTop = window.scrollY;
    //const searchLogo = document.querySelector('.form-search-mobie-logo');
    //const formSearch = document.querySelector('.form-search-mobie');
    //const logo = document.querySelector('.mobie-logo');

    // Hiển thị thông báo nếu cuộn trang
    if (scrollTop > 0) {
        backToTop.style.display = 'flex';
        if (WIDTHWINDOW > 900) {
            navbar.style.position = 'fixed';
            navbar.style.animation = 'showNavbar 0.7s';
        } else {
            //searchLogo.style.display = 'flex';
            //logo.style.display = 'none';
            //formSearch.style.display = 'none';
            //navbarMobie.style.position = 'fixed';
            //navbarMobie.style.animation = 'showNavbar 0.7s';

        }
    } else {
        // người dùng cuộn lên đầu trang
        backToTop.style.display = 'none';
        if (WIDTHWINDOW > 900) {
            navbar.style.position = 'relative';
            navbar.style.animation = '';
        } else {
            //searchLogo.style.display = 'none';
            //logo.style.display = 'flex';
            //formSearch.style.display = 'flex';
            //navbarMobie.style.position = 'relative';
            //navbarMobie.style.animation = '';
        }
    }
}
//function defaultTextBtn() {
//    const textUser = document.querySelector(`#btn-user`);
//    const textShopping = document.querySelector(`#btn-shopping`);
//    textUser.innerHTML = '<i class="fi fi-rr-user"></i>'
//    textShopping.innerHTML = '<i class="fi fi-rr-shopping-cart">'
//}
//// Đống tất cả modal cùng cùng lúc
//function closeAllModal() {
//    const modal1 = document.querySelector(`#modal-search`);
//    const modal2 = document.querySelector(`#modal-user`);
//    const modal3 = document.querySelector(`#modal-cart`);
//    const modal4 = document.querySelector(`#modal-user-mobie`);
//    const modal5 = document.querySelector(`#modal-menu`);
//    const modal6 = document.querySelector(`#modal-shopping-mobie`);
//    const iconOpenMenu = document.querySelector('#icon-open-menu');


//    if (modal1) modal1.style.display = 'none';
//    if (modal2) modal2.style.display = 'none';
//    if (modal3) modal3.style.display = 'none';
//    if (modal4) modal4.style.display = 'none';
//    if (modal5) modal5.style.display = 'none';
//    if (modal6) modal6.style.display = 'none';
//    iconOpenMenu.classList.remove('btn-close-menu');

//    // nameIDClose = '';
//}

//// Mở modal truyền id của modal cần mở vào

//function handleShowModal(nameID, nameBtnID = '') {
//    const modal = document.querySelector(`#${nameID}`);
//    const BtnModal = nameBtnID ? document.querySelector(`#${nameBtnID}`) : '';
//    const iconOpenMenu = document.querySelector('#icon-open-menu');
//    closeAllModal();
//    defaultTextBtn()
//    if (nameIDClose == nameID) {
//        modal.style.animation = 'hiddenModal 0.4s';

//        setTimeout(() => {
//            modal.style.display = 'none';
//        }, 400);
//        nameIDClose = '';
//    } else {
//        modal.style.display = 'flex';
//        BtnModal.innerHTML = '<i class="fi fi-rr-cross"></i>'
//        modal.style.animation = 'showModal 0.4s';
//        if (nameID === 'modal-menu') {
//            iconOpenMenu.classList.add('btn-close-menu');
//        }
//        nameIDClose = nameID;
//    }

//}

//function showMenuV2(nameID) {
//    const menuV2 = document.querySelector(`#${nameID}`);

//    if (isShowMenuV2) {
//        menuV2.style.animation = 'hiddenMenuV2 0.4s';

//        setTimeout(() => {
//            menuV2.style.display = 'none';
//        }, 400);

//        isShowMenuV2 = false;
//    } else {
//        menuV2.style.animation = 'showMenuV2 0.4s';

//        menuV2.style.display = 'flex';

//        isShowMenuV2 = true;
//    }
//}

// Đăng ký sự kiện scroll
window.addEventListener('scroll', handleScroll);

//document.addEventListener('click', function (event) {
//    var navbar = document.querySelector('.navbar');
//    if (!navbar.contains(event.target) && WIDTHWINDOW > 900) {
//        closeAllModal();
//    }
//});

function updateCartDisplay() {
    fetch('/Home/GetCartSummary')
        .then(response => response.json())
        .then(data => {
            const cartItems = document.querySelector('#modal-cart .mini-cart');
            const totalElement = document.querySelector('#modal-cart .total p:nth-child(2)');

            if (data.items.length === 0) {
                cartItems.innerHTML = '<i class="fi fi-rr-shopping-cart"></i><div>Hiện chưa có sản phẩm</div>';
            } else {
                let html = '';
                data.items.forEach(item => {
                    html += `
                        <div class="cart-item">
                            <img src="${item.imageUrl}" alt="${item.productName}" width="50">
                            <div class="item-details">
                                <p>${item.productName}</p>
                                <p>${item.price.toLocaleString('vi-VN')}đ x ${item.quantity}</p>
                                <button onclick="removeFromCart(${item.productId})">Xóa</button>
                            </div>
                        </div>
                    `;
                });
                cartItems.innerHTML = html;
            }

            totalElement.textContent = data.total.toLocaleString('vi-VN') + 'đ';
        });
}

document.addEventListener('DOMContentLoaded', function () {
    updateCartDisplay();

    document.querySelectorAll('.add-to-cart').forEach(button => {
        button.addEventListener('click', function () {
            const productId = parseInt(this.dataset.productId);
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            fetch('/Home/AddToCart', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({
                    productId: productId,
                    quantity: 1
                })
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.success) {
                        window.location.href = '/home/cart';
                    } else {
                        console.error('Error:', data.message);
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        });
    });

    // Quantity controls for cart page
    document.querySelectorAll('.quantity-controls').forEach(control => {
        const decrease = control.querySelector('.decrease');
        const increase = control.querySelector('.increase');
        const input = control.querySelector('.quantity-input');
        const productId = control.closest('.cart-item').dataset.productId;

        decrease?.addEventListener('click', () => {
            const newValue = Math.max(1, parseInt(input.value) - 1);
            input.value = newValue;
            updateCartItemQuantity(productId, newValue);
        });

        increase?.addEventListener('click', () => {
            const newValue = parseInt(input.value) + 1;
            input.value = newValue;
            updateCartItemQuantity(productId, newValue);
        });

        input?.addEventListener('change', () => {
            const newValue = Math.max(1, parseInt(input.value));
            input.value = newValue;
            updateCartItemQuantity(productId, newValue);
        });
    });
});

function updateCartItemQuantity(productId, quantity) {
    fetch('/Home/UpdateCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ productId, quantity })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                location.reload();
            }
        });
}

function removeFromCart(productId) {
    fetch('/Home/RemoveFromCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ productId: productId })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                updateCartDisplay();
            }
        });
}

