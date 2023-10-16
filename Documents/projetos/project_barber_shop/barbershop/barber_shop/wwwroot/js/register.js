function removeCaracteres() {
    const cpf = document.querySelector("#cpf");

    let value = cpf.value.replace(/[^0-9]/g, "");

    cpf.value = value;
}

function BuscaCep() {
    var zipCodeUser = document.querySelector('#zipCode').value;
    $.ajax({
        type: "GET",
        url: "GetAddressData",
        data: { zipCode: zipCodeUser },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.cep === null) {
                alert('CEP não encontrado');
            }
            $('#publicPlace').val(data.logradouro);
            $('#neighborhood').val(data.bairro);
            $('#locality').val(data.localidade);
        }
    })
}

function capturaValor(element) {
    $(document).ready(function () {
        $("#valorSelecionado").text(element).val();
    })
}

function AlterouForm() {
    var btn = document.getElementById("reagendar");
    btn.disabled = false;
    btn.style = "opacity: none";
}

function formatCpf() {
    const cpf = document.querySelector("#cpf");
    cpf.value = cpf.value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
}