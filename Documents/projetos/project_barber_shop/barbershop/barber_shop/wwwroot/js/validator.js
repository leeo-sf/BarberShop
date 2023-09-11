
function redirecionarService() {
    var select = document.getElementById("selectActionService");
    var selectedValue = select.options[select.selectedIndex].value;

    window.location.href = "/" + selectedValue;
}

function redirecionarAdministrator(controller) {
    var select = document.getElementById("selectActionAdministrator");
    var selectedValue = select.options[select.selectedIndex].value;

    window.location.href = "/" + controller + "/" + selectedValue;
}