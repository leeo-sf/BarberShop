function redirecionarService() {
    var select = document.getElementById("selectActionService");
    var selectedValue = select.options[select.selectedIndex].value;

    window.location.href = "/" + selectedValue;
}

function redirecionarAdministrator() {
    var select = document.getElementById("selectActionAdministrator");
    var selectedValue = select.options[select.selectedIndex].value;

    window.location.href = "/" + selectedValue;
}

function redirecionarScheduling() {
    var select = document.getElementById("selectActionScheduling");
    var selectedValue = select.options[select.selectedIndex].value;

    window.location.href = "/" + selectedValue;
}