// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var member = document.getElementById("member");
var memberStatus = document.querySelector(".memberStatus");
var errormessage = document.getElementById('errormessage');
var OldPwCheckMessage = document.getElementById('OldPwCheck');
var agreeChecked = document.getElementById('AgreeChecked');
var Message = new bootstrap.Modal(document.getElementById('Message'));

//會員中心
function showMemberStatus() {
    memberStatus.style.display = "block";
    member.onclick = hideMemberStatus;
}
function hideMemberStatus() {
    memberStatus.style.display = "none";
    member.onclick = showMemberStatus;
}

//控制商品規格顯示
function showdata(element) {
    var $element = $(element);
    var $proContent = $element.closest("#proContent");
    // console.log($proContent);

    var price = $element.data('price');
    var quantity = $element.data('quantity');
    var psid = $element.data('psid');
    var img = $element.data('img');
    // console.log(img);
    // 更新顯示的價格和剩餘數量
    $proContent.find('#price').text(price);
    $proContent.find('#quantity').text(quantity);
    $proContent.find('#ProSpecID').val(psid);
    $proContent.find('#img').attr('src', `/prod/${img}`);

    // 處理按鈕的選中狀態
    $proContent.find('.spec-btn').removeClass('active');
    $element.addClass('active');

};


//檢查產品加入購物車的數量
function changeQ(element, e) {
    var addToCart = element.closest("#addToCart");
    var CartInput = addToCart.querySelector("input[type='number']");
    var quantity = document.getElementById("quantity").innerText;
    /*CartInput.style.fontSize = "2em";*/
    CartInput.value = parseInt(CartInput.value) + e;
    if (CartInput.value <= 0 ) {
        CartInput.value = 0;
    }
    if (CartInput.value > parseInt(quantity)) {
        /*alert("不可大於庫存數量");*/
        $('#Message .modal-body h4').text('不可大於庫存數量');
        Message.show();
        CartInput.value = parseInt(quantity);
    }
};

//產品加入購物車前的檢查
function checkspec(event) {
    
    var specValue = document.getElementById('ProSpecID').value;
    if (!specValue) {
        event.preventDefault();
        $('#Message .modal-body h4').text('請選擇規格');
        Message.show();
    } else {
        $('#Message .modal-body h4').text('加入成功');
        Message.show();
    }
}



//購物車刪除
function del(event, orderID, proSpecID, proID) {
    var Message = new bootstrap.Modal(document.getElementById('Message'));
        event.preventDefault();
        $('#Message .modal-body h4').text('確定要刪除嗎?');
        $('#Message input[name="OrderID"]').val(orderID);
        $('#Message input[name="ProSpecID"]').val(proSpecID);
        $('#Message input[name="ProID"]').val(proID);
        Message.show();

}

//計算購物車總金額
let totalSubtotal = 0;
document.querySelectorAll('input[name="product"]').forEach((productCheckbox) => {
    productCheckbox.addEventListener('change', function () {
        const subtotal = parseInt(this.getAttribute('data-subtotal'));
        if (this.checked) {
            totalSubtotal += subtotal;
        } else {
            totalSubtotal -= subtotal;
        }
        document.getElementById('totalSubtotal').innerText = "$" + totalSubtotal;
    });
});
function updateTotalSubtotal() {
    totalSubtotal = 0;
    document.querySelectorAll('input[name="product"]:checked').forEach((productCheckbox) => {
        const subtotal = parseInt(productCheckbox.getAttribute('data-subtotal'));
        if (this.unchecked) {
            totalSubtotal -= subtotal;
        } else {
            totalSubtotal += subtotal;
        }
    });
    document.getElementById('totalSubtotal').innerText = "$" + totalSubtotal;
}
//購物車只能選擇一個商店檢查
document.querySelectorAll('input[name="store"]').forEach((storeCheckbox) => {
    storeCheckbox.addEventListener('change', function () {
        const relatedProducts = this.closest('tbody').querySelectorAll('input[name="product"]');
        if (this.checked) {
            document.querySelectorAll('input[name="store"]').forEach((cb) => {
                if (cb !== this) cb.checked = false;
            });
        }

        relatedProducts.forEach((productCheckbox) => {
            productCheckbox.checked = this.checked;
        });

        updateTotalSubtotal();
    });
});


function minus(orderID, proSpecID, proID, qty) {
    $('#plusForm input[name="OrderID"]').val(orderID);
    $('#plusForm input[name="ProSpecID"]').val(proSpecID);
    $('#plusForm input[name="ProID"]').val(proID);
    $('#plusForm input[name="qty"]').val(qty);
    $('#plusForm input[name="operationType"]').val('minus');
    $('#plusForm').submit();
}

function checkqty(element, orderID, proSpecID, proID) {
    $('#plusForm input[name="OrderID"]').val(orderID);
    $('#plusForm input[name="ProSpecID"]').val(proSpecID);
    $('#plusForm input[name="ProID"]').val(proID);
    $('#plusForm input[name="qty"]').val(element.value);
    $('#plusForm input[name="operationType"]').val('checkqty');
    $('#plusForm').submit();
}

//去結帳
function checkout() {
    const selectedStore = document.querySelector('input[name="store"]:checked');
        if (!selectedStore) {
            alert("請勾選商店選擇結帳訂單");
            return false;
        }
    var oid = selectedStore.dataset.orderid;
    $('#checkout').attr('href', `/Orders/checkout/${oid}`);
}

//密碼檢查
function check() {
    var passwordValue = document.getElementById('Password').value;
    var checkPWValue = document.getElementById('checkPW').value;
    if (passwordValue != checkPWValue) {
        errormessage.innerText = '輸入的密碼不相符';
    } else {
        errormessage.innerText = '';
    }
}

//舊密碼檢查
function check_oldpw() {
    var OldPwValue = document.getElementById('OldPw').value;
    var checkOldPwValue = document.getElementById('OldPw_check').value;
    if (OldPwValue != checkOldPwValue) {
        OldPwCheckMessage.innerText = '錯誤的密碼';
    } else {
        OldPwCheckMessage.innerText = '';
    }
}

//評價
function selectStar(element, starCount) {
    var $element = $(element);

    // 找到当前评分容器中的 input 并设置值
    $element.closest('.rating-container').find('input[name="Star"]').val(starCount);

    // 重置该容器内所有星星的样式
    $element.closest('.rating-container').find('i[id^="star"]').each(function (index) {
        if (index < starCount) {
            $(this).removeClass('bi-star').addClass('bi-star-fill');
        } else {
            $(this).removeClass('bi-star-fill').addClass('bi-star');
        }
    });
}


//計算人氣指數
function clicks(pid) {
    $('#ClickForm input[name="ProID"]').val(pid);
    console.log($('#ClickForm input[name="ProID"]').val());
    $('#ClickForm').submit();
}


//搜尋
function search() {
    $('#search').submit();
}