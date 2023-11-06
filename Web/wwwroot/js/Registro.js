
const getDefaultValue = (element, style) => {
    return element.style.getPropertyValue(style);
};

const correo = document.getElementById('correo');
const confirCorreo = document.getElementById('confircorreo');

const contrasena = document.getElementById('contra');
const confirContrasena = document.getElementById('confircontra');

function verificarCorreos() {
    if (correo.value !== confirCorreo.value) {
        confirCorreo.style.borderColor = 'red';
        return;
    } else {
        confirCorreo.style.borderColor = getDefaultValue(confirCorreo, 'borderColor');
    }
}

function verificarContrasenas() {
    if (contrasena.value !== confirContrasena.value) {
        confirContrasena.style.borderColor = 'red';
        return;
    } else {
        confirContrasena.style.borderColor = getDefaultValue(confirContrasena, 'borderColor');
    }
}

correo.addEventListener('change', verificarCorreos);
confirCorreo.addEventListener('change', verificarCorreos);
contrasena.addEventListener('change', verificarContrasenas);
confirContrasena.addEventListener('change', verificarContrasenas);

function verificarFormulario() {
    if (correo.value !== confirCorreo.value || contrasena.value !== confirContrasena.value) {
        return false;
    } else {
        return true;
    }
}

document.querySelector('form').addEventListener('click', function (event) {
    if (!verificarFormulario()) {
        event.preventDefault();
    }
});