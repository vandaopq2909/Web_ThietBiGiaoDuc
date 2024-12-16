let signin_form = document.querySelector('#change-pass')

checkSigninInput = (input) => {
    let err_span = signin_form.querySelector(`span[data-err-for="${input.id}"]`)
    let val = input.value.trim()
    let form_group = input.parentElement

    switch (input.getAttribute('type')) {
        case 'password':
            if (input.id === 'XacNhanMatKhau') {
                let password = signin_form.querySelector('#MatKhauMoi').value.trim();
                if (val !== password) {
                    form_group.classList.add('err');
                    form_group.classList.remove('success');
                    err_span.innerHTML = 'Mật khẩu nhập lại không khớp.';
                } else {
                    form_group.classList.add('success');
                    form_group.classList.remove('err');
                    err_span.innerHTML = '';
                }
            }
            break;
    }
}

checkSigninForm = () => {
    let inputs = signin_form.querySelectorAll('.form-input')
    inputs.forEach(input => checkSigninInput(input))
}

signin_btn.onclick = () => {
    checkSigninForm()
}