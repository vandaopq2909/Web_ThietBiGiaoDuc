let signin_form = document.querySelector('#signin-form')

let signin_btn = document.querySelector('#signin-btn')

validateEmail = (email) => {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

    return re.test(String(email).toLowerCase())
}

checkSigninInput = (input) => {
    let err_span = signin_form.querySelector(`span[data-err-for="${input.id}"]`)
    let val = input.value.trim()
    let form_group = input.parentElement

    switch(input.getAttribute('type')) {
        case 'password':
            if (val.length < 6) {
                form_group.classList.add('err')
                form_group.classList.remove('success')
                err_span.innerHTML = 'Mật khẩu cần ít nhất 6 kí tự'
            } else {
                form_group.classList.add('success')
                form_group.classList.remove('err')
                err_span.innerHTML = ''
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
                form_group.classList.add('err')
                form_group.classList.remove('success')
                err_span.innerHTML = 'Mật khẩu không hợp lệ'
            } else {
                form_group.classList.add('success')
                form_group.classList.remove('err')
                err_span.innerHTML = ''
            }
    }
}

checkSigninForm = () => {
    let inputs = signin_form.querySelectorAll('.form-input')
    inputs.forEach(input => checkSigninInput(input))
}

signin_btn.onclick = () => {
    checkSigninForm()
}

let inputs = signin_form.querySelectorAll('.form-input')
inputs.forEach(input => {
    input.addEventListener('focusout', () => {
        checkSigninInput(input)
    })
})
