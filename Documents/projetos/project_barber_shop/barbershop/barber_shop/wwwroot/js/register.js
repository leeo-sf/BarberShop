const cpf = document.querySelector("#cpf");

cpf.addEventListener("keyup", () => {
    let value = cpf.value.replace(/[^0-9]/g, "").replace(/^([\d]{3})([\d]{3})?([\d]{3})?([\d]{2})?/, "$1.$2.$3-$4");

    cpf.value = value;
});