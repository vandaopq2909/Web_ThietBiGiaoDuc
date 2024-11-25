let signin_form = document.querySelector('#signin-form');
let signin_btn = document.querySelector('#signin-btn');

validateEmail = (email) => {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

checkSignupInput = (input) => {
    let err_span = signin_form.querySelector(`span[data-err-for="${input.id}"]`);
    let val = input.value.trim();
    let form_group = input.parentElement;

    switch (input.getAttribute('type')) {
        case 'password':
            if (input.id === 'RetypePassword') {
                let password = signin_form.querySelector('#MatKhau').value.trim();
                if (val !== password) {
                    form_group.classList.add('err');
                    form_group.classList.remove('success');
                    err_span.innerHTML = 'Mật khẩu nhập lại không khớp.';
                } else {
                    form_group.classList.add('success');
                    form_group.classList.remove('err');
                    err_span.innerHTML = '';
                }
            } else {
                const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[A-Z]).{8,}$/;
                if (!passwordRegex.test(val)) {
                    form_group.classList.add('err');
                    form_group.classList.remove('success');
                    err_span.innerHTML = 'Mật khẩu cần ít nhất 8 kí tự, bao gồm chữ cái, số và ít nhất 1 chữ hoa.';
                } else {
                    form_group.classList.add('success');
                    form_group.classList.remove('err');
                    err_span.innerHTML = '';
                }
            }
            break;
        case 'text':
            if (input.id === 'TenDangNhap') {
                const usernameRegex = /^(?=.*[a-zA-Z])(?=.*\d).{6,}$/;
                if (!usernameRegex.test(val)) {
                    form_group.classList.add('err');
                    form_group.classList.remove('success');
                    err_span.innerHTML = 'Tên đăng nhập cần ít nhất 6 kí tự, bao gồm chữ và số.';
                } else {
                    form_group.classList.add('success');
                    form_group.classList.remove('err');
                    err_span.innerHTML = '';
                }
            } else {
                if (val.length === 0) {
                    form_group.classList.add('err');
                    form_group.classList.remove('success');
                    err_span.innerHTML = 'Trường này không được để trống.';
                } else {
                    form_group.classList.add('success');
                    form_group.classList.remove('err');
                    err_span.innerHTML = '';
                }
            }
            break;
        case 'email':
            if (val.length === 0 || !validateEmail(val)) {
                form_group.classList.add('err');
                form_group.classList.remove('success');
                err_span.innerHTML = 'Email không hợp lệ.';
            } else {
                form_group.classList.add('success');
                form_group.classList.remove('err');
                err_span.innerHTML = '';
            }
            break;
        default:
            if (val.length === 0) {
                form_group.classList.add('err');
                form_group.classList.remove('success');
                err_span.innerHTML = 'Trường này không được để trống.';
            } else {
                form_group.classList.add('success');
                form_group.classList.remove('err');
                err_span.innerHTML = '';
            }
    }
}

checkSignupForm = () => {
    let inputs = signin_form.querySelectorAll('.form-input');
    inputs.forEach(input => checkSignupInput(input));
}

signin_btn.onclick = (e) => {
    checkSignupForm();
    let hasError = signin_form.querySelectorAll('.err').length > 0;
    if (hasError) {
        e.preventDefault(); // Ngăn chặn gửi form nếu có lỗi
    }
}

let inputs = signin_form.querySelectorAll('.form-input');
inputs.forEach(input => {
    input.addEventListener('focusout', () => {
        checkSignupInput(input);
    });
});
